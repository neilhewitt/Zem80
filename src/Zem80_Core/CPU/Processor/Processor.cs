using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;
using Zem80.Core.InputOutput;
using Zem80.Core.Memory;
using Stack = Zem80.Core.CPU.Stack;
using System.Runtime.InteropServices;

namespace Zem80.Core.CPU
{
    public partial class Processor : IDisposable
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
        public const float DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ = 4;

        private bool _running;
        private bool _halted;
        private bool _suspended;
        
        private HaltReason _reasonForLastHalt;
        private int _waitCyclesAdded;

        private DateTime _lastStart;
        private DateTime _lastEnd;

        private Thread _instructionCycle;
        private InstructionDecoder _instructionDecoder;
        
        private bool _runExtraNOP;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; init; }
        public Stack Stack { get; init; }
        public Ports Ports { get; init; }
        public IO IO { get; init; }
        public Interrupts Interrupts { get; init; }
        public IMachineCycleTiming Timing { get; init; }
        
        public IMemoryBank Memory { get; init; }
        public IClock Clock { get; init; }

        public IReadOnlyFlags Flags => new Flags(Registers.F, true);

        public int PendingWaitCycles { get; private set; }

        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 

        public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
        public event EventHandler<ExecutionResult> AfterExecuteInstruction;
        public event EventHandler<int> BeforeInsertWaitCycles;
        public event EventHandler AfterInitialise;
        public event EventHandler OnStop;
        public event EventHandler OnSuspend;
        public event EventHandler OnResume;
        public event EventHandler<HaltReason> OnHalt;

        internal bool InterruptsWereEnabledBeforeNMI { get; private set; }

        public void Dispose()
        {
            _running = false;
            _instructionCycle?.Interrupt(); // just in case
        }

        public void Start(ushort address = 0x0000, bool endOnHalt = false, InterruptMode interruptMode = InterruptMode.IM0)
        {
            EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
            Interrupts.SetMode(interruptMode);
            Interrupts.Disable();
            Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
            AfterInitialise?.Invoke(null, null);

            _running = true;

            IO.Clear();
            _instructionCycle = new Thread(InstructionCycle);
            _instructionCycle.IsBackground = true;
            _instructionCycle.Start();

            _lastStart = DateTime.Now;
        }

        public void Stop()
        {
            _running = false;
            _halted = false;
            
            Clock.Stop();
            _lastEnd = DateTime.Now;  
            
            OnStop?.Invoke(null, null);
        }

        public void Suspend()
        {
            _suspended = true;
            OnSuspend?.Invoke(null, null);
        }

        public void Resume()
        {
            if (_halted)
            {
                _halted = false;
                // if coming back from a HALT instruction (at next interrupt or by API override here), move the Program Counter on to step over the HALT instruction
                // otherwise we'll HALT forever in a loop
                if (_reasonForLastHalt == HaltReason.HaltInstruction) Registers.PC++;
            }

            _suspended = false;
            OnResume?.Invoke(null, null);
        }

        public void RunUntilStopped()
        {
            while (_running) Thread.Sleep(1); // main thread can sleep while instruction thread does its thing
        }

        public void ResetAndClearMemory(bool restartAfterReset = true, ushort startAddress = 0, InterruptMode interruptMode = InterruptMode.IM0)
        {
            IO.SetResetState();
            Stop();
            Memory.Clear();
            Registers.Clear();
            IO.Clear();
            Registers.SP = Stack.Top;
            if (restartAfterReset)
            {
                Start(startAddress, this.EndOnHalt, interruptMode);
            }
        }

        public void AddWaitCycles(int waitCycles)
        {
            // Will add *waitCycles* wait states at the next insertion point.
            // Waits are only actually inserted at certain points in the instruction cycle.
            // If some waits are already pending, the new waits will be added to that total.
            PendingWaitCycles += waitCycles;
        }

        public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
        {
            if (!_halted)
            {
                _halted = true;
                _reasonForLastHalt = reason;
                OnHalt?.Invoke(null, reason);

                if (EndOnHalt)
                {
                    Stop();
                }
            }
        }

        private void InstructionCycle()
        {
            Clock.Start();

            while (_running)
            {
                if (_suspended)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    // instructon decoder handle decode timing; if HALTed, we run NOP until resumed
                    DecodeResult result;

                    if (_halted)
                    {
                        result = _instructionDecoder.DecodeNOPAt(Registers.PC); // don't advance PC, resume will do that
                    }
                    else
                    {
                        result = _instructionDecoder.DecodeInstructionAt(Registers.PC);
                        Registers.PC += result.InstructionSizeInBytes;
                    }

                    Run(result);
                }
            }

            void Run(DecodeResult result)
            {
                ExecuteInstruction(result.InstructionPackage);
                if (!result.OpcodeError) Interrupts.HandleAll(result.InstructionPackage, ExecuteInstruction);
                RefreshMemory();

                if (result.SkipNextByte) // special case - opcode error for 0xED prefix
                {
                    // run another NOP to cover the next byte
                    result = _instructionDecoder.DecodeNOPAt(Registers.PC);
                    Run(result);
                }
            }
        }

        private ExecutionResult ExecuteInstruction(InstructionPackage package)
        {
            BeforeExecuteInstruction?.Invoke(this, package);

            // check for breakpoints
            if (_breakpoints != null && _breakpoints.Contains(package.InstructionAddress))
            {
                _onBreakpoint?.Invoke(this, package);
            }

            // set the internal WZ register to an initial value based on whether this is an indexed instruction or not; the instruction that runs may alter/set WZ itself
            // the value in WZ (sometimes known as MEMPTR in Z80 enthusiast circles) is only ever used to control the behavior of the BIT instruction
            Registers.WZ = (ushort)(Registers[package.Instruction.IndexedRegister] + package.Data.Argument1);

            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);
            if (result.Flags != null) Registers.F = result.Flags.Value;
            result.WaitCyclesAdded = _waitCyclesAdded;
            AfterExecuteInstruction?.Invoke(this, result);

            return result;
        }

        private void RefreshMemory()
        {
            Registers.R = (byte)(((Registers.R + 1) & 0x7F) | (Registers.R & 0x80)); // bits 0-6 of R are incremented as part of the memory refresh - bit 7 is preserved 
        }

        public Processor(IMemoryBank memory = null, IMemoryMap map = null, IClock clock = null, ushort topOfStackAddress = 0xFFFF)
        {
            // Default clock is the FastClock which, well, isn't really a clock. It'll run as fast as possible on the hardware and in .NET
            // but it'll *say* that it's running at 4MHz. It's a lying liar that lies. You may want a different clock - luckily there are several.
            // Clocks and timing are a thing, too much to go into here, so check the docs (one day, there will be docs!).
            Clock = clock ?? ClockMaker.FastClock(DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ);
            Clock.Initialise(this);

            Timing = new MachineCycleTiming(this);
            Registers = new Registers();
            Ports = new Ports(Timing);
            IO = new IO(this);
            Interrupts = new Interrupts(this);

            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));
            
            // stack pointer defaults to 0xFFFF - this is undocumented but verified behaviour of the Z80
            Stack = new Stack(topOfStackAddress, this);
            Registers.SP = Stack.Top;
            
            // The Z80 instruction set needs to be built (all Instruction objects are created, bound to the microcode instances, and indexed into a hashtable - undocumented 'overloads' are built here too)
            InstructionSet.Build();
            _instructionDecoder = new InstructionDecoder(this);
        }
    }
}

using System;
using System.Threading;
using Zem80.Core.InputOutput;
using Zem80.Core.Memory;
using Stack = Zem80.Core.CPU.Stack;

namespace Zem80.Core.CPU
{
    public class Processor : IDisposable
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
        public const float DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ = 4;

        private bool _running;
        private bool _halted;
        private bool _suspended;

        private HaltReason _reasonForLastHalt;

        private Thread _instructionCycle;
        private InstructionDecoder _instructionDecoder;

        public bool EndOnHalt { get; private set; }

        public IRegisters Registers { get; init; }
        public IStack Stack { get; init; }
        public IPorts Ports { get; init; }
        public IIO IO { get; init; }
        public IInterrupts Interrupts { get; init; }
        public IProcessorTiming Timing { get; init; }
        public IDebugProcessor Debug { get; init; }
        public IMemoryBank Memory { get; init; }
        public IClock Clock { get; init; }

        public IReadOnlyFlags Flags => new Flags(Registers.F, true);

        public bool Running => _running;
        public bool Halted => _halted;
        public bool Suspended => _suspended;

        public DateTime LastStarted { get; private set; }
        public DateTime LastStopped { get; private set; }
        public TimeSpan LastRunTime { get; private set; }

        public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
        public event EventHandler<ExecutionResult> AfterExecuteInstruction;
        public event EventHandler<int> BeforeInsertWaitCycles;
        public event EventHandler AfterInitialise;
        public event EventHandler OnStop;
        public event EventHandler OnSuspend;
        public event EventHandler OnResume;
        public event EventHandler<HaltReason> OnHalt;

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

            LastStarted = DateTime.Now;
        }

        public void Stop()
        {
            _running = false;
            _halted = false;

            Clock.Stop();
            LastStopped = DateTime.Now;
            LastRunTime = (LastStopped - LastStarted);

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
                Start(startAddress, EndOnHalt, interruptMode);
            }
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
                if (!_suspended)
                {
                    bool skipNextByte = false;
                    do
                    {
                        ushort address = Registers.PC;
                        // when the Z80 is *halted*, it doesn't stop running. It continuously executes NOP instructions until an interrupt occurs;
                        // if halted (or skipping because of an instruction set error) we provide 4 zero-bytes; otherwise, the 4 bytes at the Program Counter
                        // note that instructions can be 1-4 bytes long but we always send the next 4 bytes to the decoder
                        byte[] instructionBytes = (_halted || skipNextByte) ? new byte[4] : Memory.ReadBytesAt(address, 4);

                        // decode the bytes
                        InstructionPackage package = _instructionDecoder.DecodeInstruction(instructionBytes, address, out skipNextByte, out bool opcodeErrorNOP);

                        // on the real Z80, during instruction decode, memory timing for the opcode fetch and operand reads is happening
                        // but here we will simulate the timing based on the instruction package received
                        // (all the rest of the timing happens during the execution of the instruction microcode, and the microcode is responsible for it)
                        Timing.OpcodeFetchTiming(package.Instruction, address);
                        Timing.OperandReadTiming(package.Instruction, address, package.Data.Argument1, package.Data.Argument2);

                        Registers.PC += (ushort)package.Instruction.SizeInBytes;
                        ExecuteInstruction(package);

                        if (!opcodeErrorNOP)
                        {
                            Interrupts.HandleAll();
                        }
                        
                        RefreshMemory();
                    }
                    while (skipNextByte); // usually false, but if true, the cycle will run again with a NOP and skip over the next byte in RAM
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void ExecuteInstruction(InstructionPackage package)
        {
            BeforeExecuteInstruction?.Invoke(this, package);

            // check for breakpoints
            Debug.EvaluateAndRunBreakpoint(package.InstructionAddress, package);

            // set the internal WZ register to an initial value based on whether this is an indexed instruction or not; the instruction that runs may alter/set WZ itself
            // the value in WZ (sometimes known as MEMPTR in Z80 enthusiast circles) is only ever used to control the behavior of the BIT instruction
            Registers.WZ = (ushort)(package.Instruction.IsIndexed ? (Registers[package.Instruction.IndexedRegister] + package.Data.Argument1) : 0);

            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);
            if (result.Flags != null) Registers.F = result.Flags.Value;
            
            AfterExecuteInstruction?.Invoke(this, result);
        }

        private void RefreshMemory()
        {
            Registers.R = (byte)(Registers.R + 1 & 0x7F | Registers.R & 0x80); // bits 0-6 of R are incremented as part of the memory refresh - bit 7 is preserved 
        }

        public Processor(IMemoryBank memory = null, IMemoryMap map = null, IStack stack = null, IClock clock = null, IRegisters registers = null, IPorts ports = null,
            IProcessorTiming cycleTiming = null, IIO io = null, IInterrupts interrupts = null, IDebugProcessor debug = null, ushort topOfStackAddress = 0xFFFF)
        {
            // Default clock is the fast DefaultClock which, well, isn't really a clock. It'll run as fast as possible on the hardware and in .NET
            // but it'll *say* that it's running at 4MHz. It's a lying liar that lies. You may want a different clock - luckily there are several.
            // Clocks and timing are a thing, too much to go into here, so check the docs (one day, there will be docs!).
            Clock = clock ?? ClockMaker.DefaultClock(DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ);
            Clock.Initialise(this);

            Timing = cycleTiming ?? new ProcessorTiming(this);
            Registers = registers ?? new Registers();
            Ports = ports ?? new Ports(Timing);
            IO = io ?? new IO(this);
            Interrupts = interrupts ?? new Interrupts(this, ExecuteInstruction);
            Debug = debug ?? new DebugProcessor(this, ExecuteInstruction);

            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));

            // stack pointer defaults to 0xFFFF - this is undocumented but verified behaviour of the Z80
            Stack = stack ?? new Stack(topOfStackAddress, this);
            Registers.SP = Stack.Top;

            // The Z80 instruction set needs to be built (all Instruction objects are created, bound to the microcode instances, and indexed into a hashtable - undocumented 'overloads' are built here too)
            InstructionSet.Build();
            _instructionDecoder = new InstructionDecoder(this);
        }
    }
}

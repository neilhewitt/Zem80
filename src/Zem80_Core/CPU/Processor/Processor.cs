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

        public bool EndOnHalt { get; private set; }

        public IRegisters Registers { get; private set; }
        public IStack Stack { get; private set; }
        public IPorts Ports { get; private set; }
        public IIO IO { get; private set; }
        public IInterrupts Interrupts { get; private set; }
        public IProcessorTiming Timing { get; private set; }
        public IMemoryBank Memory { get; private set; }
        public IClock Clock { get; private set; }

        public DebugProcessor Debug { get; private set; }

        public IReadOnlyFlags Flags => new Flags(Registers.F, true);

        public bool Running => _running;
        public bool Halted => _halted;
        public bool Suspended => _suspended;

        public DateTime LastStarted { get; private set; }
        public DateTime LastStopped { get; private set; }
        public TimeSpan LastRunTime { get; private set; }

        public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
        public event EventHandler<ExecutionState> OnMachineCycleExecution;
        public event EventHandler<ExecutionResult> AfterExecuteInstruction;
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
                    InstructionPackage package = null;
                    ushort address = Registers.PC;

                    // when the Z80 is *halted*, it doesn't stop running. It continuously executes NOP instructions until an interrupt occurs;
                    // if not halted we provide the 4 bytes at the Program Counter to the decoder
                    // note that instructions can be 1-4 bytes long, but we always send the next 4 bytes to the decoder
                    if (_halted)
                    {
                        package = new InstructionPackage(InstructionSet.NOP, new InstructionData(), address);
                    }
                    else
                    {
                        byte[] instructionBytes = Memory.ReadBytesAt(address, 4);
                        package = InstructionDecoder.DecodeInstruction(instructionBytes, address);
                    }

                    // on the real Z80, during instruction decode, memory timing for the opcode fetch and operand reads is happening
                    // but here we will simulate the timing based on the instruction package received
                    // (all the rest of the timing happens during the execution of the instruction microcode, and the microcode is responsible for it)
                    Timing.AddOpcodeFetchTiming(package.Instruction, address, NotifyMachineCycle);
                    Timing.AddOperandReadTiming(package.Instruction, address, NotifyMachineCycle, package.Data.Argument1, package.Data.Argument2);

                    // advance the Program Counter to the start of the next instruction - but the current instruction may still change it
                    // if we're halted, we don't advance the PC here as we want to stay on the HALT instruction
                    if (!_halted) Registers.PC += (ushort)package.Instruction.SizeInBytes;
                    ExecuteInstruction(package);

                    Interrupts.HandleAll();
                    RefreshMemory();
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        private void ExecuteInstruction(InstructionPackage package)
        {
            Debug.NotifyExecute(package); // gets first crack before events
            BeforeExecuteInstruction?.Invoke(this, package);

            // set the internal WZ register to an initial value based on whether this is an indexed instruction or not; the instruction that runs may still alter/set WZ itself
            // the value in WZ (sometimes known as MEMPTR in Z80 enthusiast circles) is only ever used to control the behavior of the BIT instruction
            Registers.WZ = (ushort)(package.Instruction.IsIndexed ? (Registers[package.Instruction.IndexedRegister] + package.Data.Argument1) : 0);

            ExecutionResult result = package.Instruction.Microcode.Execute(this, package, NotifyMachineCycle);
            if (result.Flags != null) Registers.F = result.Flags.Value;
            if (Registers.PC != package.InstructionAddress + package.Instruction.SizeInBytes) result.ProgramCounterWasModified = true;

            AfterExecuteInstruction?.Invoke(this, result);
        }

        private void NotifyMachineCycle(ExecutionState state)
        {
            OnMachineCycleExecution?.Invoke(this, state);
        }

        private void RefreshMemory()
        {
            Registers.R = (byte)(Registers.R + 1 & 0x7F | Registers.R & 0x80); // bits 0-6 of R are incremented as part of the memory refresh - bit 7 is preserved 
        }

        public Processor()
            : this(null, null, null, null, null, null, null, null, null)
        {
        }

        public Processor(IMemoryBank memory = null, IMemoryMap map = null, IStack stack = null, IClock clock = null, IRegisters registers = null, IPorts ports = null,
            IProcessorTiming cycleTiming = null, IIO io = null, IInterrupts interrupts = null, ushort topOfStackAddress = 0xFFFF)
        {
            Debug = new DebugProcessor(this, ExecuteInstruction);

            // default clock is the RealTimeClock at 4MHz (which is the normal Z80 clock speed) - this will attempt to run as close to real time
            // as possible on a non-deterministic platform like .NET, but this only works if the host platform has a high-resolution timer
            // otherwise this clock will just run as fast as the host platform can manage and timing will not be accurate
            Clock = clock ?? ClockMaker.RealTimeClock(DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ);
            Clock.Initialise(this);

            Timing = cycleTiming ?? new ProcessorTiming(this);
            Registers = registers ?? new Registers();
            Ports = ports ?? new Ports(Timing);
            IO = io ?? new IO(this);
            Interrupts = interrupts ?? new Interrupts(this, ExecuteInstruction);

            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(Timing, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));

            // stack pointer defaults to 0xFFFF - this is undocumented but verified behaviour of the Z80
            Stack = stack ?? new Stack(topOfStackAddress, this);
            Registers.SP = Stack.Top;

            // The Z80 instruction set needs to be built (all Instruction objects are created, bound to the microcode instances, and indexed into a hashtable - undocumented 'overloads' are built here too)
            InstructionSet.Build();
        }
    }
}

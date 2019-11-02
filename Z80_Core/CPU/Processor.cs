using System;
using System.Threading.Tasks;
using System.Timers;

namespace Z80.Core
{
    public class Processor
    {
        private static object _padlock = new object();

        private bool _running;
        private bool _halted;
        private bool _pendingINT;
        private Action _interruptCallback;
        private bool _pendingNMI;
        private Task _instructionCycle;

        private InstructionDecoder _decoder = new InstructionDecoder();

        public IRegisters Registers { get; private set; }
        public IMemory Memory { get; private set; }
        public IPorts Ports { get; private set; }
        public IStack Stack { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; }
        public double SpeedInMhz { get; private set; }

        public long Ticks { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; // yay for tri-states!

        public event EventHandler<InstructionPackage> BeforeExecute;
        public event EventHandler<ExecutionResult> AfterExecute;

        public void Start()
        {
            if (!_running)
            {
                _running = true;

                _instructionCycle = new Task(InstructionCycle, TaskCreationOptions.None);
                _instructionCycle.Start();
            }
        }

        public void Stop()
        {
            _running = false;
            _halted = false;
        }

        public void Halt()
        {
            _halted = true;
        }

        public void Resume()
        {
            _halted = false;
        }

        public void Reset(bool stopAfterReset = false)
        {
            Stop();
            Memory.Clear();
            Registers.Clear();
            Stack.Reset();
            if (!stopAfterReset) Start();
        }

        public void SetInterruptMode(InterruptMode mode)
        {
            InterruptMode = mode;
        }

        public void RaiseInterrupt(Action callback)
        {
            if (InterruptsEnabled)
            {
                _pendingINT = true;
                _interruptCallback = callback;
            }
        }

        public void RaiseNonMasktableInterrupt()
        {
            _pendingNMI = true;
        }

        public void SetAddressBus(ushort value)
        {
            AddressBus = value;
        }

        public void SetAddressBus(byte low, byte high)
        {
            AddressBus = (ushort)((high * 256) + low);
        }

        public void SetDataBus(byte value)
        {
            DataBus = value;
        }

        public void DisableInterrupts()
        {
            InterruptsEnabled = false;
        }

        public void EnableInterrupts()
        {
            InterruptsEnabled = true;
        }

        private void InstructionCycle()
        {
            while (_running)
            {
                if (!_halted)
                {
                    byte[] instruction = Memory.ReadBytesAt(Registers.PC, 4);
                    InstructionPackage package = _decoder.Decode(instruction);
                    if (package == null) Stop(); // only happens if instruction buffer is short (end of memory reached) and corrupt (not a valid instruction)

                    ExecutionResult result = ExecuteInstructionPackage(package);
                    if (!result.ProgramCounterUpdated)
                    {
                        if (Registers.PC + package.Instruction.SizeInBytes >= Memory.SizeInBytes) Stop(); // PC overflow
                        Registers.PC += package.Instruction.SizeInBytes;
                    }
                }

                if (_pendingNMI)
                {
                    Stack.Push(Registers.PC);
                    Registers.PC = 0x0066;
                    _pendingNMI = false;
                    _halted = false;
                }

                if (_pendingINT && InterruptsEnabled)
                {
                    if (_interruptCallback == null && InterruptMode != InterruptMode.IM1)
                    {
                        throw new Z80Exception("Interrupt mode is " + InterruptMode.ToString() + " which requires a callback for reading data from the device. Callback was null.");
                    }

                    switch (InterruptMode)
                    {
                        case InterruptMode.IM0: // read instruction data from data bus in 1-4 cycles and execute resulting instruction - flags are set but PC is unaffected
                            InstructionPackage package = _decoder.DecodeInterrupt(() =>
                                {
                                    _interruptCallback(); // each time callback is called, device will set data bus with next instruction byte
                                    return DataBus;
                                });
                            ExecuteInstructionPackage(package); // instruction is *usually* RST which diverts execution, but can be any valid instruction
                            break;

                        case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                            Stack.Push(Registers.PC);
                            Registers.PC = 0x0038;
                            break;

                        case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                            if (_interruptCallback != null) _interruptCallback(); // device must populate data bus with low byte of address
                            Stack.Push(Registers.PC);
                            Registers.PC = (ushort)((Registers.I * 256) + DataBus);
                            break;
                    }

                    _pendingINT = false;
                    _halted = false;
                }

                Ticks++;
            }
        }

        private ExecutionResult ExecuteInstructionPackage(InstructionPackage package)
        {
            BeforeExecute?.Invoke(this, package);
            ExecutionResult result = package.Instruction.Implementation.Execute(this, package);
            AfterExecute?.Invoke(this, result);
            Registers.SetFlags(result.Flags);

            return result;
        }

        internal Processor(IRegisters registers, IMemory memory, IStack stack, IPorts ports, double speedInMHz)
        {
            Registers = registers;
            Memory = memory;
            Stack = stack;
            Ports = ports;
            SpeedInMhz = speedInMHz;

            Registers.SP = stack.StartAddress;
            Memory.Initialise(this); // creates circular reference that cannot be created at constructor-time
        }
    }
}

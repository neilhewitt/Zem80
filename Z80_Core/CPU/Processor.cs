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
        private bool _pendingNMI;
        private Task _instructionCycle;

        private InstructionDecoder _decoder = new InstructionDecoder();

        public IRegisters Registers { get; private set; }
        public IMemory Memory { get; private set; }
        public IPorts Ports { get; private set; }
        public IStack Stack { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.Zero;
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
        }

        public void Halt()
        {
            _halted = true;
        }

        public void Resume()
        {
            _halted = false;
        }

        public void SetInterruptMode(InterruptMode mode)
        {
            InterruptMode = mode;
            // handle interrupt stuff!
        }

        public void RaiseMaskableInterrupt()
        {
            _pendingINT = true;
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
                    InstructionPackage package = _decoder.Decode(instruction, Registers.PC);
                    if (package == null) Stop(); // only happens if instruction buffer is short (end of memory reached) and corrupt (not a valid instruction)

                    BeforeExecute?.Invoke(this, package);
                    ExecutionResult result = package.Instruction.Implementation.Execute(this, package);
                    AfterExecute?.Invoke(this, result);

                    Registers.SetFlags(result.Flags);
                    if (!result.PCWasSet)
                    {
                        if (Registers.PC + package.Instruction.SizeInBytes >= Memory.SizeInBytes) Stop(); // PC overflow
                        Registers.PC += package.Instruction.SizeInBytes;
                    }

                    if (_pendingNMI)
                    {
                        Stack.Push(Registers.PC);
                        Registers.PC = 0x0066;
                        _pendingNMI = false;
                    }

                    if (_pendingINT && InterruptsEnabled)
                    {
                        HandleINT();
                    }

                    Ticks++;
                }
            }
        }

        private void HandleINT()
        {
            switch (InterruptMode)
            {
                case InterruptMode.Zero:
                    break;

                case InterruptMode.One:
                    break;

                case InterruptMode.Two:
                    break;
            }

            _pendingINT = false;
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

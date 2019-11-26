using System;
using System.Threading.Tasks;
using System.Timers;

namespace Z80.Core
{
    public class Processor : IDebugProcessor
    {
        private static object _padlock = new object();

        private bool _running;
        private bool _halted;
        private bool _pendingINT;
        private Action _interruptCallback;
        private bool _pendingNMI;
        private Task _instructionCycle;

        private EventHandler<InstructionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler _onHalt;

        private InstructionDecoder _decoder = new InstructionDecoder();

        public IRegisters Registers { get; private set; }
        public Memory Memory { get; private set; }
        public IPorts Ports { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; } = true;
        public double SpeedInMhz { get; private set; }

        public long InstructionTicks { get; private set; }
        public long ClockCycles { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; // yay for tri-states!

        // why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        event EventHandler<InstructionPackage> IDebugProcessor.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugProcessor.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler IDebugProcessor.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugProcessor.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler IDebugProcessor.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }

        public void Start(bool synchronous = false)
        {
            if (!_running)
            {
                _beforeStart?.Invoke(null, null);
                _running = true;

                if (!synchronous) // run the CPU on a thread and return to the calling code immediately
                {
                    _instructionCycle = new Task(InstructionCycle, TaskCreationOptions.None);
                    _instructionCycle.Start();
                }
                else
                {
                    InstructionCycle(); // run the CPU as a synchronous task until stopped
                }
            }
        }

        public void Stop()
        {
            _running = false;
            _halted = false;
            _onStop?.Invoke(null, null);
        }

        public void Halt()
        {
            _halted = true;
            _onHalt?.Invoke(null, null);
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
            Memory.Stack.Reset();
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

                    ClockCycles += result.ClockCycles;
                }
                else
                {
                    ClockCycles++; // while halted we should be running NOP continuously (but no Program Counter movement), so just add a cycle each time
                }

                if (_pendingNMI)
                {
                    Memory.Stack.Push(Registers.PC);
                    Registers.PC = 0x0066;
                    _pendingNMI = false;
                    _halted = false;
                }

                if (_pendingINT && InterruptsEnabled)
                {
                    if (_interruptCallback == null && InterruptMode != InterruptMode.IM1)
                    {
                        throw new Z80Exception("Interrupt mode is " + InterruptMode.ToString() + " which requires a callback for reading data from the interrupting device. Callback was null.");
                    }

                    switch (InterruptMode)
                    {
                        case InterruptMode.IM0: // read instruction data from data bus in 1-4 cycles and execute resulting instruction - flags are set but PC is unaffected
                            InstructionPackage package = _decoder.DecodeInterrupt(() =>
                                {
                                    _interruptCallback(); // each time callback is called, device will set data bus with next instruction byte
                                    return DataBus;
                                });
                            ExecutionResult result = ExecuteInstructionPackage(package); // instruction is *usually* RST which diverts execution, but can be any valid instruction
                            break;

                        case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                            Memory.Stack.Push(Registers.PC);
                            Registers.PC = 0x0038;
                            break;

                        case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                            _interruptCallback(); // device must populate data bus with low byte of address
                            Memory.Stack.Push(Registers.PC);
                            Registers.PC = (ushort)((Registers.I * 256) + DataBus);
                            break;
                    }

                    _pendingINT = false;
                    _halted = false;
                }

                InstructionTicks++;
            }
        }

        ExecutionResult IDebugProcessor.ExecuteDirect(Instruction instruction, InstructionData data)
        {
            return ExecuteInstructionPackage(new InstructionPackage(instruction, data));
        }

        private ExecutionResult ExecuteInstructionPackage(InstructionPackage package)
        {
            _beforeExecute?.Invoke(this, package);
            ExecutionResult result = package.Instruction.Implementation.Execute(this, package);
            _afterExecute?.Invoke(this, result);
            if (result.Flags != null) Registers.SetFlags(result.Flags);

            return result;
        }

        internal Processor(IRegisters registers, IMemoryMap memoryMap, IStack stack, IPorts ports, double speedInMHz)
        {
            Registers = registers;
            Ports = ports;
            SpeedInMhz = speedInMHz;

            Memory = new Memory(memoryMap, stack);
            Memory.Initialise(this);

            Registers.SP = stack.StartAddress;
            stack.Initialise(this);
        }
    }
}

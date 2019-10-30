using System;
using System.Timers;

namespace Z80.Core
{
    public class Processor
    {
        private bool _running;
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

        public event EventHandler<InstructionPackage> BeforeExecute;
        public event EventHandler<ExecutionResult> AfterExecute;

        public void Start()
        {
            _running = true;

            while (_running)
            {
                InstructionCycle();
            }
        }

        public void Stop()
        {
            _running = false;
        }

        public void SetInterruptMode(InterruptMode mode)
        {
            InterruptMode = mode;
            // handle interrupt stuff!
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

        public void HandleInterrupts()
        {
            // nothign to see here yet...
        }

        private void InstructionCycle()
        {
            try
            {
                byte[] instruction = Memory.ReadBytesAt(Registers.PC, 4);
                InstructionPackage package = _decoder.Decode(instruction, Registers.PC);
                if (package == null) Stop(); // only happens if instruction buffer is short (memory overflow) and corrupt (not valid instruction)

                BeforeExecute?.Invoke(this, package);
                ExecutionResult result = package.Instruction.Implementation.Execute(this, package);
                AfterExecute?.Invoke(this, result);
                
                Registers.SetFlags(result.Flags);
                if (!result.PCWasSet)
                {
                    if (Registers.PC + package.Instruction.SizeInBytes >= Memory.SizeInBytes) Stop(); // PC overflow... HALT here 
                    Registers.PC += package.Instruction.SizeInBytes;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Stop(); // clean-up here
            }
        }

        internal Processor(IRegisters registers, IMemory memory, IStack stack, IPorts ports, double speedInMHz)
        {
            Registers = registers;
            Memory = memory;
            Stack = stack;
            Ports = ports;
            SpeedInMhz = speedInMHz;

            Registers.SP = stack.StartAddress;
        }
    }
}

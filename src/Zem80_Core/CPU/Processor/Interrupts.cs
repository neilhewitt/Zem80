using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;

namespace Zem80.Core.CPU
{
    public class Interrupts : IInterrupts
    {
        private Processor _cpu;
        private Func<byte> _interruptCallback;

        public InterruptMode Mode { get; private set; }
        public bool Enabled { get; private set; }
        public bool IFF1 { get; private set; }
        public bool IFF2 { get; private set; }

        public event EventHandler<long> OnMaskableInterrupt;
        public event EventHandler<long> OnNonMaskableInterrupt;

        public void SetMode(InterruptMode mode)
        {
            Mode = mode;
        }

        public void RaiseMaskable(Func<byte> callback = null)
        {
            _cpu.IO.SetInterruptState();
            _interruptCallback = callback;
        }

        public void RaiseNonMaskable()
        {
            _cpu.IO.SetNMIState();
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void RestoreAfterNMI()
        {
            IFF1 = IFF2;
            IFF2 = false;
        }

        public void HandleAll(InstructionPackage package, Action<InstructionPackage> IM0_ExecuteInstruction)
        {
            if (!HandleNMI())
            {
                HandleMaskableInterrupts(package, IM0_ExecuteInstruction);
            }
        }

        private bool HandleNMI()
        {
            bool handledNMI = false;

            if (_cpu.IO.NMI)
            {
                _cpu.Resume(); // in case we're halted

                IFF2 = Enabled; // save IFF1 state ready for RETN
                Enabled = false; // disable maskable interrupts until RETN

                _cpu.Timing.BeginInterruptRequestAcknowledgeCycle(ProcessorTiming.NMI_INTERRUPT_ACKNOWLEDGE_TSTATES);

                _cpu.Stack.Push(WordRegister.PC);
                _cpu.Registers.PC = 0x0066;
                _cpu.Registers.WZ = _cpu.Registers.PC;

                _cpu.Timing.EndInterruptRequestAcknowledgeCycle();

                handledNMI = true;
            }

            OnNonMaskableInterrupt?.Invoke(this, _cpu.Clock.Ticks);
            _cpu.IO.EndNMIState();

            return handledNMI;
        }

        private void HandleMaskableInterrupts(InstructionPackage executingPackage, Action<InstructionPackage> IM0_ExecuteInstruction)
        {
            if (_cpu.IO.INT && Enabled)
            {
                if (_interruptCallback == null && Mode == InterruptMode.IM0)
                {
                    throw new InterruptException("Interrupt mode is IM0 which requires a callback for reading data from the interrupting device. Callback was null.");
                }

                _cpu.Resume(); // in case we're halted

                switch (Mode)
                {
                    case InterruptMode.IM0:

                        // In this mode, we read up to 4 bytes that form an instruction which is then executed. Usually, this is an RST but it can in theory be 
                        // any Z80 instruction. The instruction bytes are read from the data bus, whose value is set by a hardware device during 4 clock cycles.

                        // To emulate a device functioning via IM0, your device code must call RaiseInterrupt on the CPU when it is ready to interrupt the system, supplying a
                        // Func<byte> that returns the opcode bytes of the instruction to run one by one as it is called 4 times (it will *always* be called 4 times). Trailing
                        // 0x00s will be ignored, but you must return 4 bytes even if the instruction is shorter than that. See DecodeIM0Interrupt below for details of how this works.

                        // The decoded instruction is executed in the current execution context with registers and program counter where they were when the interrupt was triggered.
                        // The program counter is pushed on the stack before executing the instruction and restored afterwards (but *not* popped), so instructions like JR, JP and CALL will have no effect. 

                        // NOTE: I have not been able to test this mode extensively based on real-world examples. It may well be buggy compared to the real Z80. 
                        // TODO: verify the behaviour of the real Z80 and fix the code if necessary

                        _cpu.Timing.BeginInterruptRequestAcknowledgeCycle(ProcessorTiming.IM0_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        InstructionPackage IM0Package = DecodeIM0Interrupt();
                        ushort pc = _cpu.Registers.PC;
                        _cpu.Stack.Push(WordRegister.PC);
                        IM0_ExecuteInstruction(IM0Package);
                        _cpu.Registers.PC = pc;
                        _cpu.Registers.WZ = _cpu.Registers.PC;
                        break;

                    case InterruptMode.IM1:

                        // This mode is simple. When IM1 is set (this is the default mode) then a jump to 0x0038 is performed when 
                        // the interrupt occurs. 

                        _cpu.Timing.BeginInterruptRequestAcknowledgeCycle(ProcessorTiming.IM1_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        _cpu.Stack.Push(WordRegister.PC);
                        _cpu.Registers.PC = 0x0038;
                        _cpu.Registers.WZ = _cpu.Registers.PC;
                        break;

                    case InterruptMode.IM2:

                        // In this mode, we synthesize an address from the contents of register I as the high byte and the 
                        // value on the data bus as the low byte. This address is a pointer into a table in RAM containing the *actual* interrupt
                        // routine addresses. We read the word at the address we calculated, and then jump to *that* address.

                        // When emulating a hardware device that uses IM2 to jump into its service routine/s, you must trigger the interrupt
                        // by calling RaiseInterrupt and supplying a Func<byte> that returns the data bus value to use when called.
                        // If the callback is null then the data bus value is assumed to be 0. 

                        // It's actually quite common on some platforms (eg ZX Spectrum) to use IM2 this way to call a routine that needs to be synchronised 
                        // with the hardware (on the Spectrum, an interrupt is raised by the system after each display refresh, and setting IM2 allows 
                        // the programmer to divert that interrupt to a routine of their choice and then call down to the ROM routine [which handles the
                        // keyboard, sound etc] afterwards).

                        _cpu.Timing.BeginInterruptRequestAcknowledgeCycle(ProcessorTiming.IM2_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        _cpu.Stack.Push(WordRegister.PC);
                        _cpu.IO.SetDataBusValue(_interruptCallback?.Invoke() ?? 0);
                        ushort address = (_cpu.IO.DATA_BUS, _cpu.Registers.I).ToWord();
                        _cpu.Registers.PC = _cpu.Memory.ReadWordAt(address, ProcessorTiming.MEMORY_READ_NORMAL_TSTATES);
                        _cpu.Registers.WZ = _cpu.Registers.PC;
                        break;
                }

                // handling an interrupt always results in two extra wait cycles being added, so we'll add those here
                _cpu.Clock.WaitForClockTicks(2);

                _interruptCallback = null;
                OnMaskableInterrupt?.Invoke(this, _cpu.Clock.Ticks);
                _cpu.Timing.EndInterruptRequestAcknowledgeCycle();
            }
        }

        private InstructionPackage DecodeIM0Interrupt()
        {
            // NOTE: this probably doesn't work properly. Certainly, it doesn't generate the right timing. One to fix later.

            // In IM0, when an interrupt is generated, the CPU will ask the device to supply four bytes one at a time via
            // a callback method, which are then decoded into an instruction to be executed.

            byte[] opcode = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                // The callback will be called 4 times; it should return the opcode bytes of the instruction to run in sequence.
                // If there are fewer than 4 bytes in the opcode, return 0x00 for the 'extra' bytes
                opcode[i] = _interruptCallback();
            }

            InstructionDecoder decoder = new InstructionDecoder(_cpu);
            InstructionPackage package = decoder.DecodeInstruction(opcode, _cpu.Registers.PC, out bool _, out bool _);

            return package;
        }

        public Interrupts(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

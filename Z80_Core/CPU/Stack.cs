using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Stack : IStack
    {
        private IRegisters _registers;
        private Memory _memory;
        private bool _initialised;

        public ushort StartAddress { get; private set; }

        public void Push(ushort value)
        {
            if (!_initialised) throw new StackNotInitialisedException();

            _registers.SP -= 2;
            _memory.WriteWordAt(_registers.SP, value);
        }

        public ushort Pop()
        {
            if (!_initialised) throw new StackNotInitialisedException();

            ushort output = _memory.ReadWordAt(_registers.SP);
            _registers.SP += 2;
            return output;
        }

        public void Reset()
        {
            _registers.SP = StartAddress;
        }

        public void Initialise(Processor cpu)
        {
            _memory = cpu.Memory;
            _initialised = true;
        }

        public Stack(IRegisters registers, ushort startAddress)
        {
            StartAddress = startAddress;
            _registers = registers;
            _registers.SP = StartAddress;
        }
    }
}

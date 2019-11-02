using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Stack : IStack
    {
        private IRegisters _registers;
        private IMemory _memory;

        public ushort StartAddress { get; private set; }

        public void Push(ushort value)
        {
            _registers.SP -= 2;
            _memory.WriteWordAt(_registers.SP, value);
        }

        public ushort Pop()
        {
            ushort output = _memory.ReadWordAt(_registers.SP);
            _registers.SP += 2;
            return output;
        }

        public void Reset()
        {
            _registers.SP = StartAddress;
        }

        public Stack(IRegisters registers, IMemory memory, ushort startAddress)
        {
            StartAddress = startAddress;
            _registers = registers;
            _registers.SP = StartAddress;
            _memory = memory;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Stack
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

        public Stack(Processor cpu, ushort startAddress)
        {
            StartAddress = startAddress;
            _registers = cpu.Registers;
            _memory = cpu.Memory;
        }
    }
}

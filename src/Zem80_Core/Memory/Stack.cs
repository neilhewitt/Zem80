using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core.Memory
{
    public class Stack : IDebugStack
    {
        private Processor _cpu;

        public ushort Top { get; init; }
        public ushort Pointer => _cpu.Registers.SP;

        public IDebugStack Debug => this;

        public void Push(WordRegister register)
        {
            ushort value = _cpu.Registers[register];
            _cpu.Registers.SP--;
            _cpu.Timing.BeginStackWriteCycle(true, value.HighByte());
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.HighByte());
            _cpu.Timing.EndStackWriteCycle();

            _cpu.Registers.SP--;
            _cpu.Timing.BeginStackWriteCycle(false, value.LowByte());
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.LowByte());
            _cpu.Timing.EndStackWriteCycle();
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            _cpu.Timing.BeginStackReadCycle();
            low = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Timing.EndStackReadCycle(false, low);
            _cpu.Registers.SP++;

            _cpu.Timing.BeginStackReadCycle();
            high = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Timing.EndStackReadCycle(true, high);
            _cpu.Registers.SP++;

            ushort value = (low, high).ToWord();
            _cpu.Registers[register] = value;
        }

        void IDebugStack.PushStackDirect(ushort value)
        {
            _cpu.Registers.SP--;
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.HighByte());

            _cpu.Registers.SP--;
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.LowByte());
        }

        ushort IDebugStack.PopStackDirect()
        {
            byte high, low;

            low = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Registers.SP++;

            high = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Registers.SP++;

            return (low, high).ToWord();
        }

        ushort IDebugStack.PeekStack()
        {
            return _cpu.Memory.Untimed.ReadWordAt(_cpu.Registers.SP);
        }

        public Stack(ushort topOfStackAddress, Processor cpu)
        {
            Top = topOfStackAddress;
            _cpu = cpu;
        }
    }
}

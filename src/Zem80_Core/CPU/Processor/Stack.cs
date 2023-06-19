using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core.CPU
{
    public class Stack : IDebugStack, IStack
    {
        private Processor _cpu;

        public ushort Pointer => _cpu.Registers.SP;
        public ushort Top { get; init; }

        public IDebugStack Debug => this;

        public void Push(WordRegister register)
        {
            ushort value = _cpu.Registers[register];
            _cpu.Registers.SP--;
            _cpu.Timing.BeginStackWriteCycle(value.HighByte());
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.HighByte());
            _cpu.Timing.EndStackWriteCycle();

            _cpu.Registers.SP--;
            _cpu.Timing.BeginStackWriteCycle(value.LowByte());
            _cpu.Memory.Untimed.WriteByteAt(_cpu.Registers.SP, value.LowByte());
            _cpu.Timing.EndStackWriteCycle();
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            _cpu.Timing.BeginStackReadCycle();
            low = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Timing.EndStackReadCycle(low);
            _cpu.Registers.SP++;

            _cpu.Timing.BeginStackReadCycle();
            high = _cpu.Memory.Untimed.ReadByteAt(_cpu.Registers.SP);
            _cpu.Timing.EndStackReadCycle(high);
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
            return Debug.PeekStack(0);
        }

        ushort IDebugStack.PeekStack(int wordsFromTop)
        {
            return _cpu.Memory.Untimed.ReadWordAt((ushort)(_cpu.Registers.SP - wordsFromTop * 2));
        }

        public Stack(ushort topOfStackAddress, Processor cpu)
        {
            Top = topOfStackAddress;
            _cpu = cpu;
        }
    }
}

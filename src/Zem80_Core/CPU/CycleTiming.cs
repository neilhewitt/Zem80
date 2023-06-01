using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.CPU;
using Zem80.Core.CPU;

namespace Zem80.Core.CPU
{
    public class CycleTiming : ICycleTiming
    {
        private Processor _cpu;

        public int WaitCyclesAdded { get; private set; }

        void ICycleTiming.OpcodeFetchCycle(ushort address, byte opcode, byte extraTStates)
        {
            _cpu.IO.SetOpcodeFetchState(address);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.AddOpcodeFetchData(opcode);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndOpcodeFetchState();
            _cpu.IO.SetAddressBusValue(_cpu.Registers.IR);
            _cpu.IO.SetDataBusValue(0x00);

            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);
        }

        void ICycleTiming.MemoryReadCycle(ushort address, byte data, byte extraTStates)
        {
            _cpu.IO.SetMemoryReadState(address);
            _cpu.Clock.WaitForNextClockTick();

            _cpu.IO.AddMemoryData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryReadState();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);
        }

        void ICycleTiming.MemoryWriteCycle(ushort address, byte data, byte extraTStates)
        {
            _cpu.IO.SetMemoryWriteState(address, data);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryWriteState();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);
        }

        void ICycleTiming.BeginStackReadCycle()
        {
            _cpu.IO.SetMemoryReadState(_cpu.Registers.SP);
            _cpu.Clock.WaitForNextClockTick();
        }


        void ICycleTiming.EndStackReadCycle(bool highByte, byte data)
        {
            _cpu.IO.AddMemoryData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryReadState();
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginStackWriteCycle(bool highByte, byte data)
        {
            _cpu.IO.SetMemoryWriteState(_cpu.Registers.SP, data);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();
        }

        void ICycleTiming.EndStackWriteCycle()
        {
            _cpu.IO.EndMemoryWriteState();
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (_cpu.Registers.C, _cpu.Registers.B).ToWord() : (n, _cpu.Registers.A).ToWord();

            _cpu.IO.SetPortReadState(address);
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.EndPortReadCycle(byte data)
        {
            _cpu.IO.AddPortReadData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.EndPortReadState();
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (_cpu.Registers.C, _cpu.Registers.B).ToWord() : (n, _cpu.Registers.A).ToWord();

            _cpu.IO.SetPortWriteState(address, data);
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.EndPortWriteCycle()
        {
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.EndPortWriteState();
            _cpu.Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            _cpu.IO.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                _cpu.Clock.WaitForNextClockTick();
            }
        }

        void ICycleTiming.EndInterruptRequestAcknowledgeCycle()
        {
            _cpu.IO.EndInterruptState();
        }

        void ICycleTiming.InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                _cpu.Clock.WaitForNextClockTick();
            }
        }

        private void InsertWaitCycles()
        {
            int cyclesToAdd = _cpu.PendingWaitCycles;
            if (cyclesToAdd > 0)
            {
                _cpu.Clock.WaitForClockTicks(cyclesToAdd);
                WaitCyclesAdded = cyclesToAdd;
            }
        }

        public CycleTiming(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

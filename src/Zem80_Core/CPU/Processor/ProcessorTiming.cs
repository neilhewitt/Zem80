using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public partial class Processor : IInstructionTiming
    {
        public IInstructionTiming Timing => this;

        void IInstructionTiming.OpcodeFetchCycle(ushort address, byte data)
        {
            IO.SetOpcodeFetchState(address);
            WaitForNextClockTick();
            IO.AddOpcodeFetchData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndOpcodeFetchState();
            IO.SetAddressBusValue(Registers.IR);
            IO.SetDataBusValue(0x00);

            WaitForNextClockTick();
            WaitForNextClockTick();
        }

        void IInstructionTiming.MemoryReadCycle(ushort address, byte data)
        {
            IO.SetMemoryReadState(address);
            WaitForNextClockTick();

            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasProlongedMemoryRead)
                {
                    WaitForNextClockTick();
                }
            }
        }

        void IInstructionTiming.MemoryWriteCycle(ushort address, byte data)
        {
            IO.SetMemoryWriteState(address, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryWriteState();
            WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasProlongedMemoryWrite)
                {
                    WaitForNextClockTick();
                    WaitForNextClockTick();
                }
            }
        }

        void IInstructionTiming.BeginStackReadCycle()
        {
            IO.SetMemoryReadState(Registers.SP);
            WaitForNextClockTick();
        }


        void IInstructionTiming.EndStackReadCycle(bool highByte, byte data)
        {
            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginStackWriteCycle(bool highByte, byte data)
        {
            IO.SetMemoryWriteState(Registers.SP, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();
        }

        void IInstructionTiming.EndStackWriteCycle()
        {
            IO.EndMemoryWriteState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortReadState(address);
            WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortReadCycle(byte data)
        {
            IO.AddPortReadData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortReadState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortWriteState(address, data);
            WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortWriteCycle()
        {
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortWriteState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            IO.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }

        void IInstructionTiming.EndInterruptRequestAcknowledgeCycle()
        {
            IO.EndInterruptState();
        }

        void IInstructionTiming.InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }
    }
}

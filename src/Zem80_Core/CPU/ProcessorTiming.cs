using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    // IInstructionTiming contains methods to execute the different types of machine cycle that the Z80 supports.
    // These will be called mostly by the instruction decoder, stack operations and interrupt handlers, but some instruction
    // microcode uses these methods directly to generate timing (eg IN/OUT) or add 'internal operation' ticks. 
    // I segregated these onto an interface just to keep them logically partioned from the main API but without moving them out to a class.
    // Calling code can get at these methods using the Processor.Timing property (or by casting to the interface type, but don't do that, it's ugly).

    public partial class Processor : ICycleTiming
    {
        public ICycleTiming Timing => this;

        void ICycleTiming.OpcodeFetchCycle(ushort address, byte data)
        {
            Buses.SetOpcodeFetchState(address);
            Clock.WaitForNextClockTick();
            Buses.AddOpcodeFetchData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Buses.EndOpcodeFetchState();
            Buses.SetAddressBusValue(Registers.IR);
            Buses.SetDataBusValue(0x00);

            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.MemoryReadCycle(ushort address, byte data)
        {
            Buses.SetMemoryReadState(address);
            Clock.WaitForNextClockTick();

            Buses.AddMemoryData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Buses.EndMemoryReadState();
            Clock.WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasProlongedMemoryRead)
                {
                    Clock.WaitForNextClockTick();
                }
            }
        }

        void ICycleTiming.MemoryWriteCycle(ushort address, byte data)
        {
            Buses.SetMemoryWriteState(address, data);
            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Buses.EndMemoryWriteState();
            Clock.WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasProlongedMemoryWrite)
                {
                    Clock.WaitForNextClockTick();
                    Clock.WaitForNextClockTick();
                }
            }
        }

        void ICycleTiming.BeginStackReadCycle()
        {
            Buses.SetMemoryReadState(Registers.SP);
            Clock.WaitForNextClockTick();
        }


        void ICycleTiming.EndStackReadCycle(bool highByte, byte data)
        {
            Buses.AddMemoryData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Buses.EndMemoryReadState();
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginStackWriteCycle(bool highByte, byte data)
        {
            Buses.SetMemoryWriteState(Registers.SP, data);
            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
            InsertWaitCycles();
        }

        void ICycleTiming.EndStackWriteCycle()
        {
            Buses.EndMemoryWriteState();
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            Buses.SetPortReadState(address);
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.EndPortReadCycle(byte data)
        {
            Buses.AddPortReadData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Clock.WaitForNextClockTick();
            Buses.EndPortReadState();
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            Buses.SetPortWriteState(address, data);
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.EndPortWriteCycle()
        {
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Clock.WaitForNextClockTick();
            Buses.EndPortWriteState();
            Clock.WaitForNextClockTick();
        }

        void ICycleTiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            Buses.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                Clock.WaitForNextClockTick();
            }
        }

        void ICycleTiming.EndInterruptRequestAcknowledgeCycle()
        {
            Buses.EndInterruptState();
        }

        void ICycleTiming.InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                Clock.WaitForNextClockTick();
            }
        }

        private void InsertWaitCycles()
        {
            int cyclesToAdd = _pendingWaitCycles;

            if (cyclesToAdd > 0)
            {
                BeforeInsertWaitCycles?.Invoke(this, cyclesToAdd);
            }

            Clock.WaitForClockTicks(cyclesToAdd);

            _waitCyclesAdded = _pendingWaitCycles;
            _pendingWaitCycles = 0;
        }
    }
}

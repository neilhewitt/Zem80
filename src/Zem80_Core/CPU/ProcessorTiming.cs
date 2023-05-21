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

    public partial class Processor : IInstructionTiming
    {
        public IInstructionTiming Timing => this;

        void IInstructionTiming.OpcodeFetchCycle(ushort address, byte data)
        {
            Interface.SetOpcodeFetchState(address);
            Clock.WaitForNextClockTick();
            Interface.AddOpcodeFetchData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Interface.EndOpcodeFetchState();
            Interface.SetAddressBusValue(Registers.IR);
            Interface.SetDataBusValue(0x00);

            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.MemoryReadCycle(ushort address, byte data)
        {
            Interface.SetMemoryReadState(address);
            Clock.WaitForNextClockTick();

            Interface.AddMemoryData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Interface.EndMemoryReadState();
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

        void IInstructionTiming.MemoryWriteCycle(ushort address, byte data)
        {
            Interface.SetMemoryWriteState(address, data);
            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Interface.EndMemoryWriteState();
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

        void IInstructionTiming.BeginStackReadCycle()
        {
            Interface.SetMemoryReadState(Registers.SP);
            Clock.WaitForNextClockTick();
        }


        void IInstructionTiming.EndStackReadCycle(bool highByte, byte data)
        {
            Interface.AddMemoryData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Interface.EndMemoryReadState();
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.BeginStackWriteCycle(bool highByte, byte data)
        {
            Interface.SetMemoryWriteState(Registers.SP, data);
            Clock.WaitForNextClockTick();
            Clock.WaitForNextClockTick();
            InsertWaitCycles();
        }

        void IInstructionTiming.EndStackWriteCycle()
        {
            Interface.EndMemoryWriteState();
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            Interface.SetPortReadState(address);
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortReadCycle(byte data)
        {
            Interface.AddPortReadData(data);
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Clock.WaitForNextClockTick();
            Interface.EndPortReadState();
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            Interface.SetPortWriteState(address, data);
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortWriteCycle()
        {
            Clock.WaitForNextClockTick();
            InsertWaitCycles();

            Clock.WaitForNextClockTick();
            Interface.EndPortWriteState();
            Clock.WaitForNextClockTick();
        }

        void IInstructionTiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            Interface.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                Clock.WaitForNextClockTick();
            }
        }

        void IInstructionTiming.EndInterruptRequestAcknowledgeCycle()
        {
            Interface.EndInterruptState();
        }

        void IInstructionTiming.InternalOperationCycle(int tStates)
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

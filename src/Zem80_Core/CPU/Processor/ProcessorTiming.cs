using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;

namespace Zem80.Core.CPU
{
    public class PortTiming
    {
        public byte PortNumber { get; init; }
        public bool AddressFromBC { get; init; }
        public Func<byte> PortReadCallback { get; init; }
    }

    public class ProcessorTiming
    {
        public const byte OPCODE_FETCH_NORMAL_TSTATES = 4;
        public const byte MEMORY_READ_NORMAL_TSTATES = 3;
        public const byte MEMORY_WRITE_NORMAL_TSTATES = 3;
        public const byte NMI_INTERRUPT_ACKNOWLEDGE_TSTATES = 5;
        public const byte IM0_INTERRUPT_ACKNOWLEDGE_TSTATES = 6;
        public const byte IM1_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;
        public const byte IM2_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;

        private Processor _cpu;
        private int _waitCyclesPending;

        public int WaitCyclesAdded { get; private set; }

        public void AddWaitCycles(int waitCycles)
        {
            _waitCyclesPending += waitCycles;
        }

        public void OpcodeFetchTiming(Instruction instruction, ushort address)
        {
            int opcodeByteIndex = 0;
            foreach(MachineCycle machineCycle in instruction.MachineCycles.OpcodeFetches)
            {
                OpcodeFetchCycle(address, instruction.OpcodeBytes[opcodeByteIndex++], machineCycle.TStates);
            }

            void OpcodeFetchCycle(ushort address, byte opcode, byte tStates)
            {
                byte extraTStates = (byte)(tStates - OPCODE_FETCH_NORMAL_TSTATES);

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
        }

        public void OperandReadTiming(Instruction instruction, ushort address, params byte[] operands)
        {
            // move on to the operand bytes
            address += (ushort)instruction.MachineCycles.OpcodeFetches.Count();

            // either single operand byte read, or operand low / high reads
            int operandByteIndex = 0;
            foreach (MachineCycle machineCycle in instruction.MachineCycles.OperandReads)
            {
                MemoryReadCycle(address, operands[operandByteIndex++], machineCycle.TStates);
            }
        }

        public void MemoryReadCycle(ushort address, byte data, byte tStates)
        {
            byte extraTStates = (byte)(tStates - MEMORY_READ_NORMAL_TSTATES);

            _cpu.IO.SetMemoryReadState(address);
            _cpu.Clock.WaitForNextClockTick();

            _cpu.IO.AddMemoryData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryReadState();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);
        }

        public void MemoryWriteCycle(ushort address, byte data, byte tStates)
        {
            byte extraTStates = (byte)(tStates - MEMORY_WRITE_NORMAL_TSTATES);

            _cpu.IO.SetMemoryWriteState(address, data);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryWriteState();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);
        }

        public void BeginStackReadCycle()
        {
            _cpu.IO.SetMemoryReadState(_cpu.Registers.SP);
            _cpu.Clock.WaitForNextClockTick();
        }

        public void EndStackReadCycle(byte data)
        {
            _cpu.IO.AddMemoryData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndMemoryReadState();
            _cpu.Clock.WaitForNextClockTick();
        }

        public void BeginStackWriteCycle(byte data)
        {
            _cpu.IO.SetMemoryWriteState(_cpu.Registers.SP, data);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();
        }

        public void EndStackWriteCycle()
        {
            _cpu.IO.EndMemoryWriteState();
            _cpu.Clock.WaitForNextClockTick();
        }

        public void BeginPortReadCycle(byte port, bool addressFromBC)
        {
            ushort address = addressFromBC ? (_cpu.Registers.C, _cpu.Registers.B).ToWord() : (port, _cpu.Registers.A).ToWord();

            _cpu.IO.SetPortReadState(address);
            _cpu.Clock.WaitForNextClockTick();
        }

        public void EndPortReadCycle(byte data)
        {
            _cpu.IO.AddPortReadData(data);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.EndPortReadState();
            _cpu.Clock.WaitForNextClockTick();
        }

        public void BeginPortWriteCycle(byte data, byte port, bool addressFromBC)
        {
            ushort address = addressFromBC ? (_cpu.Registers.C, _cpu.Registers.B).ToWord() : (port, _cpu.Registers.A).ToWord();

            _cpu.IO.SetPortWriteState(address, data);
            _cpu.Clock.WaitForNextClockTick();
        }

        public void EndPortWriteCycle()
        {
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.EndPortWriteState();
            _cpu.Clock.WaitForNextClockTick();
        }

        public void BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            _cpu.IO.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                _cpu.Clock.WaitForNextClockTick();
            }
        }

        public void EndInterruptRequestAcknowledgeCycle()
        {
            _cpu.IO.EndInterruptState();
        }

        public void InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                _cpu.Clock.WaitForNextClockTick();
            }
        }

        private void InsertWaitCycles()
        {
            int cyclesToAdd = _waitCyclesPending;
            _waitCyclesPending = 0;
            if (cyclesToAdd > 0)
            {
                _cpu.Clock.WaitForClockTicks(cyclesToAdd);
                WaitCyclesAdded = cyclesToAdd;
            }
        }

        public ProcessorTiming(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

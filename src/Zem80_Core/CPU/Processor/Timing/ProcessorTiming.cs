using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;

namespace Zem80.Core.CPU
{
    public class ProcessorTiming : IProcessorTiming
    {
        public const byte OPCODE_FETCH_NORMAL_TSTATES = 4;
        public const byte MEMORY_READ_NORMAL_TSTATES = 3;
        public const byte MEMORY_WRITE_NORMAL_TSTATES = 3;
        public const byte STACK_READ_NORMAL_TSTATES = 3;
        public const byte STACK_WRITE_NORMAL_TSTATES = 3;
        public const byte PORT_READ_NORMAL_TSTATES = 4;
        public const byte PORT_WRITE_NORMAL_TSTATES = 4;
        public const byte NMI_INTERRUPT_ACKNOWLEDGE_TSTATES = 5;
        public const byte IM0_INTERRUPT_ACKNOWLEDGE_TSTATES = 6;
        public const byte IM1_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;
        public const byte IM2_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;

        private Processor _cpu;
        private int _waitCyclesPending;
        private byte _currentStackData;
        private byte _currentPort;
        private bool _currentPortAddressFromBC;
        private byte _currentPortData;

        public event EventHandler<int> BeforeInsertWaitCycles;
        public event EventHandler<MemoryReadWriteCycleInfo> OnOpcodeFetch;
        public event EventHandler<MemoryReadWriteCycleInfo> OnMemoryRead;
        public event EventHandler<MemoryReadWriteCycleInfo> OnMemoryWrite;
        public event EventHandler<StackReadWriteCycleInfo> OnStackRead;
        public event EventHandler<StackReadWriteCycleInfo> OnStackWrite;
        public event EventHandler<PortReadWriteCycleInfo> OnPortRead;
        public event EventHandler<PortReadWriteCycleInfo> OnPortWrite;
        public event EventHandler OnInterruptAcknowledge;
        public event EventHandler<int> OnInternalOperation;

        public void AddWaitCycles(int waitCycles)
        {
            _waitCyclesPending += waitCycles;
        }

        public void AddOpcodeFetchTiming(Instruction instruction, ushort address)
        {
            int opcodeByteIndex = 0;
            foreach (MachineCycle machineCycle in instruction.MachineCycles.OpcodeFetches)
            {
                OpcodeFetchCycle(address, instruction.OpcodeBytes[opcodeByteIndex++], machineCycle.TStates);
            }
        }

        public void AddOperandReadTiming(Instruction instruction, ushort address, params byte[] operands)
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

        public void OpcodeFetchCycle(ushort address, byte opcode, byte tStates)
        {
            byte extraTStates = (byte)(tStates - OPCODE_FETCH_NORMAL_TSTATES);

            _cpu.IO.SetOpcodeFetchState(address);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.IO.AddOpcodeFetchData(opcode);
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();

            _cpu.IO.EndOpcodeFetchState();
            _cpu.IO.SetAddressBusValue(_cpu.Registers.IR);
            _cpu.IO.ResetDataBusValue();

            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();

            if (extraTStates > 0) _cpu.Clock.WaitForClockTicks(extraTStates);

            OnOpcodeFetch?.Invoke(this, new MemoryReadWriteCycleInfo { Address = address, Data = opcode, TStates = tStates });
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

            OnMemoryRead?.Invoke(this, new MemoryReadWriteCycleInfo() {
                Address = address,
                Data = data,
                TStates = tStates 
                }
            );
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

            OnMemoryWrite?.Invoke(this, new MemoryReadWriteCycleInfo() { 
                Address = address, 
                Data = data, 
                TStates = tStates 
                }
            );
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

            OnStackRead?.Invoke(this, new StackReadWriteCycleInfo() { 
                Data = data, 
                StackPointer = _cpu.Registers.SP, 
                TStates = STACK_READ_NORMAL_TSTATES 
                }
            );
        }

        public void BeginStackWriteCycle(byte data)
        {
            _currentStackData = data;
            _cpu.IO.SetMemoryWriteState(_cpu.Registers.SP, data);
            _cpu.Clock.WaitForNextClockTick();
            _cpu.Clock.WaitForNextClockTick();
            InsertWaitCycles();
        }

        public void EndStackWriteCycle()
        {
            _cpu.IO.EndMemoryWriteState();
            _cpu.Clock.WaitForNextClockTick();

            OnStackWrite?.Invoke(this, new StackReadWriteCycleInfo() { 
                Data = _currentStackData, 
                StackPointer = _cpu.Registers.SP, 
                TStates = STACK_WRITE_NORMAL_TSTATES 
                }
            );
        }

        public void BeginPortReadCycle(byte port, bool addressFromBC)
        {
            _currentPort = port;
            _currentPortAddressFromBC = addressFromBC;
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

            OnPortRead?.Invoke(this, new PortReadWriteCycleInfo() { 
                Port = _currentPort, Data = data, 
                AddressFromBC = _currentPortAddressFromBC, 
                TStates = PORT_READ_NORMAL_TSTATES 
                }
            );
        }

        public void BeginPortWriteCycle(byte data, byte port, bool addressFromBC)
        {
            _currentPort = port;
            _currentPortAddressFromBC = addressFromBC;
            _currentPortData = data;
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

            OnPortWrite?.Invoke(this, new PortReadWriteCycleInfo() { 
                Port = _currentPort, 
                Data = _currentPortData, 
                AddressFromBC = _currentPortAddressFromBC, 
                TStates = PORT_WRITE_NORMAL_TSTATES }
            );
        }

        public void BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            _cpu.IO.SetInterruptState();
            _cpu.Clock.WaitForClockTicks(tStates);
        }

        public void EndInterruptRequestAcknowledgeCycle()
        {
            _cpu.IO.EndInterruptState();
            OnInterruptAcknowledge?.Invoke(this, EventArgs.Empty);
        }

        public void InternalOperationCycle(int tStates)
        {
            _cpu.Clock.WaitForClockTicks(tStates);
            OnInternalOperation?.Invoke(this, tStates);
        }

        public void InternalOperationCycles(params int[] tStates)
        {
            foreach(int t in tStates)
            {
                _cpu.Clock.WaitForClockTicks(t);
            }

            OnInternalOperation?.Invoke(this, tStates.Sum());
        }

        private void InsertWaitCycles()
        {
            BeforeInsertWaitCycles?.Invoke(this, _waitCyclesPending);

            int cyclesToAdd = _waitCyclesPending;
            _waitCyclesPending = 0;
            if (cyclesToAdd > 0)
            {
                _cpu.Clock.WaitForClockTicks(cyclesToAdd);
            }
        }

        public ProcessorTiming(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

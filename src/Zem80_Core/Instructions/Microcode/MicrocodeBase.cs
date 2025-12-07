using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zem80.Core.CPU
{
    public abstract class MicrocodeBase
    {
        protected Processor _cpu;
        protected InstructionPackage _package;
        protected Action<ExecutionState> _onMachineCycle;
        protected IList<MachineCycle> _machineCycles;

        public InstructionMachineCycles MachineCycle { get; private set; }

        public void Setup(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            _cpu = cpu;
            _package = package;
            _onMachineCycle = onMachineCycle;

            MachineCycle = package.Instruction.MachineCycles;
            //_machineCycles = new List<MachineCycle>(MachineCycle.Cycles); // want a realised countable list
        }

        public abstract ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle);

        protected byte GetSourceByteAndAddTimingAndEvents()
        {
            // this fetches a byte operand value for the instruction given, adjusting how it is fetched based on the addressing of the instruction

            IRegisters r = _cpu.Registers;
            Instruction instruction = _package.Instruction;
            InstructionData data = _package.Data;
            ushort address = 0x0000;

            byte value;
            ByteRegister source = instruction.Source.AsByteRegister();
            if (source != ByteRegister.None)
            {
                // operand comes from another byte register directly (eg LD A,B)
                // no additional timing needed
                value = r[source];
            }
            else
            {
                if (instruction.Argument1 == InstructionElement.ByteValue)
                {
                    // operand is supplied as an argument (eg LD A,n)
                    // timing and events are already handled at decode time
                    value = data.Argument1;
                }
                else if (instruction.Source == InstructionElement.None)
                {
                    // operand is fetched from a memory location but the source and target are the same (eg INC (HL) or INC (IX+d))
                    address = instruction.Target.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = _cpu.Memory.ReadByteAt(address);
                    _cpu.Timing.MemoryReadCycle(address, value, MachineCycle.MemoryRead.TStates);
                    NotifyMachineCycle(MachineCycle.MemoryRead, value);
                }
                else
                {
                    // operand is fetched from a memory location and assigned elsewhere (eg LD A,(HL) or LD B,(IX+d))
                    address = instruction.Source.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    // indexed addressing adds an internal operation cycle
                    if (instruction.IsIndexed)
                    {
                        _cpu.Timing.InternalOperationCycle(MachineCycle.InternalOperation1.TStates);
                        NotifyMachineCycle(MachineCycle.InternalOperation1);
                    }

                    // normal timing
                    value = _cpu.Memory.ReadByteAt(address);
                    _cpu.Timing.MemoryReadCycle(address, value, MachineCycle.MemoryRead.TStates);
                    NotifyMachineCycle(MachineCycle.MemoryRead, value);
                }
            }

            return value;
        }

        protected void InternalOperation1()
        {
            MachineCycle cycle = MachineCycle.InternalOperation1;
            if (cycle != null)
            {
                _cpu.Timing.InternalOperationCycle(cycle.TStates);
                NotifyMachineCycle(cycle);
            }
        }

        protected void InternalOperation2()
        {
            MachineCycle cycle = MachineCycle.InternalOperation2;
            if (cycle != null)
            {
                _cpu.Timing.InternalOperationCycle(cycle.TStates);
                NotifyMachineCycle(cycle);
            }
        }

        protected void NotifyMachineCycle(MachineCycle cycle, byte? arg1 = null, byte? arg2 = null)
        {
            if (_onMachineCycle != null)
            {
                if (cycle != null)
                {
                    ExecutionState state = new ExecutionState(_package.Instruction, arg1, arg2, _cpu.Flags.Clone(), _cpu.Registers.Snapshot(), cycle);
                    _onMachineCycle(state);
                }
            }
        }
    }
}

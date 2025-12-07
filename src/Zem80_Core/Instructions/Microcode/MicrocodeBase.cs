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

        protected InstructionMachineCycles MachineCycle { get; private set; }

        public void Setup(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            _cpu = cpu;
            _package = package;
            _onMachineCycle = onMachineCycle;

            MachineCycle = package.Instruction.MachineCycles;
            _machineCycles = new List<MachineCycle>(MachineCycle.Cycles); // want a realised countable list
        }

        public void NotifyMachineCycle(MachineCycle cycle, byte? arg1 = null, byte? arg2 = null)
        {
            if (_onMachineCycle != null)
            {
                if (cycle !=null)
                {
                    ExecutionState state = new ExecutionState(_package.Instruction, arg1, arg2, _cpu.Flags.Clone(), _cpu.Registers.Snapshot(), cycle);
                    _onMachineCycle(state);
                }
            }
        }

        public void NotifyMachineCycle(int cycleIndex)
        {
            NotifyMachineCycle(cycleIndex, null, null);
        }

        public void NotifyMachineCycle(int cycleIndex, byte arg1)
        {
            NotifyMachineCycle(cycleIndex, arg1, null);
        }

        public void NotifyMachineCycle(int cycleIndex, byte arg1, byte arg2)
        {
            NotifyMachineCycle(cycleIndex, arg1, arg2);
        }

        public void NotifyMachineCycle(int cycleIndex, ushort args)
        {
            NotifyMachineCycle(cycleIndex, args.LowByte(), args.HighByte());
        }   

        private void NotifyMachineCycle(int cycleIndex, byte? arg1, byte? arg2)
        {
            if (_onMachineCycle != null)
            {
                MachineCycle machineCycle = _machineCycles.Count >= cycleIndex ? _machineCycles[cycleIndex - 1] : null;
                if (machineCycle != null)
                {
                    ExecutionState state = new ExecutionState(_package.Instruction, arg1, arg2, _cpu.Flags.Clone(), _cpu.Registers.Snapshot(), machineCycle);
                    _onMachineCycle(state);
                }
            }
        }
    }
}

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
            _machineCycles = new List<MachineCycle>(MachineCycle.Cycles); // want a realised countable list
        }

        public void NotifyMachineCycle(MachineCycle cycle, byte? arg1 = null, byte? arg2 = null)
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

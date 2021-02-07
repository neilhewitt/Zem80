using System;
using System.Collections.Generic;
using System.Linq;

namespace Zem80.Core.Instructions
{
    public class InstructionTiming
    {
        public IEnumerable<MachineCycle> MachineCycles { get; private set; }
        public TimingExceptions Exceptions { get; private set; }

        public byte TStates => (byte)MachineCycles.Sum(x => x.TStates);
        
        public IEnumerable<MachineCycle> ByType(MachineCycleType type)
        {
            return MachineCycles.Where(x => x.Type == type);
        }

        public InstructionTiming(Instruction instruction, IEnumerable<MachineCycle> machineCycles)
        {
            MachineCycles = machineCycles;
            Exceptions = new TimingExceptions(instruction, this);
        }
    }
}

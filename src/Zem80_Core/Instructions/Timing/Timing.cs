using System;
using System.Collections.Generic;
using System.Linq;

namespace Zem80.Core.Instructions
{
    public class Timing
    {
        public IEnumerable<MachineCycle> MachineCycles { get; private set; }
        public TimingExceptions Exceptions { get; private set; }
        
        public IEnumerable<MachineCycle> ByType(MachineCycleType type)
        {
            return MachineCycles.Where(x => x.Type == type);
        }

        public Timing(Instruction instruction, IEnumerable<MachineCycle> machineCycles)
        {
            MachineCycles = machineCycles;
            Exceptions = new TimingExceptions(instruction, this);
        }
    }
}

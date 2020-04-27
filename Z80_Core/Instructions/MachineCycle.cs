using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class MachineCycle
    {
        public MachineCycleType Type { get; private set; }
        public int ClockCycles { get; private set; }
        public int? ClockCyclesAlternate { get; private set; }
        public bool IsConditional { get; private set; }

        public MachineCycle(MachineCycleType type, int clockCycles, bool conditional, int? clockCyclesAlternate = null)
        {
            Type = type;
            ClockCycles = clockCycles;
            IsConditional = conditional;
            ClockCyclesAlternate = clockCyclesAlternate;
        }
    }
}

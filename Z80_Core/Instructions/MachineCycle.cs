using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class MachineCycle
    {
        public const int OPCODE_FETCH_STANDARD_CYCLES = 4;
        public const int NMI_INTA_CYCLES = 5;
        public const int IM0_INTA_CYCLES = 6;
        public const int IM1_INTA_CYCLES = 7;
        public const int IM2_INTA_CYCLES = 7;

        public MachineCycleType Type { get; private set; }
        public int ClockCycles { get; private set; }
        public bool RunsOnlyIfConditionTrue { get; private set; }

        public MachineCycle(MachineCycleType type, int clockCycles, bool runsOnlyIfConditionTrue)
        {
            Type = type;
            ClockCycles = clockCycles;
            RunsOnlyIfConditionTrue = runsOnlyIfConditionTrue;
        }
    }
}

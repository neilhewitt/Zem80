using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class MachineCycle
    {
        public MachineCycleType Type { get; private set; }
        public int TStates { get; private set; }
        public bool RunsOnlyIfConditionTrue { get; private set; }

        public MachineCycle(MachineCycleType type, int tStates, bool runsOnlyIfConditionTrue)
        {
            Type = type;
            TStates = tStates;
            RunsOnlyIfConditionTrue = runsOnlyIfConditionTrue;
        }
    }
}

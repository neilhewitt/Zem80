using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
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

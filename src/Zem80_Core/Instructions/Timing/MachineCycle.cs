using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zem80.Core.CPU
{
    public class MachineCycle
    {
        public MachineCycleType Type { get; private set; }
        public byte TStates { get; private set; }
        public bool RunsOnlyIfConditionTrue { get; private set; }

        public MachineCycle(MachineCycleType type, byte tStates, bool runsOnlyIfConditionTrue)
        {
            Type = type;
            TStates = tStates;
            RunsOnlyIfConditionTrue = runsOnlyIfConditionTrue;
        }
    }

    public static class MachineCycleExtensions
    {
        public static IEnumerable<MachineCycle> ByType(this IEnumerable<MachineCycle> cycles, MachineCycleType type)
        {
            return cycles.Where(x => x.Type == type);
        }

        public static IEnumerable<MachineCycle> ByType(this IEnumerable<MachineCycle> cycles, params MachineCycleType[] types)
        {
            return cycles.Where(x => types.Contains(x.Type));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public static class UtilityExtensions
    {
        public static MachineCycle IOCycle(this IEnumerable<MachineCycle> machineCycles)
        {
            return machineCycles.Single(x => x.Type == MachineCycleType.InternalOperation); // there can be only one :-)
        }
    }
}

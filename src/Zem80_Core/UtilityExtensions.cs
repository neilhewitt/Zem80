using System.Collections.Generic;
using System.Linq;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public static class UtilityExtensions
    {
        public static IEnumerable<MachineCycle> CyclesByType(this IEnumerable<MachineCycle> machineCycles, params MachineCycleType[] cycleTypes)
        {
            IList<MachineCycleType> types = cycleTypes.ToList();
            return machineCycles.Where(x => types.Contains(x.Type));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public static class Bootstrapper
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;

        public static Processor BuildCPU(IMemoryMap map = null, ushort? topOfStackAddress = null, double speedInMHz = 4.00, bool enableFlagPrecalculation = true)
        {
            return new Processor(
                map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true),
                topOfStackAddress ?? (MAX_MEMORY_SIZE_IN_BYTES - 3),
                speedInMHz,
                enableFlagPrecalculation
                );
        }
    }
}

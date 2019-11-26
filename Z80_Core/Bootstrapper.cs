using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public static class Bootstrapper
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
        
        public static Processor BuildDefaultCPU()
        {
            Registers registers = new Registers();
            MemoryMap map = new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES);
            map.Map(new RAM(0, MAX_MEMORY_SIZE_IN_BYTES), 0);
            Ports ports = new Ports();

            Processor z80 = new Processor(registers, map, ports, MAX_MEMORY_SIZE_IN_BYTES - 3, 4.00);
            return z80;
        }

        public static IDebugProcessor BuildDebugCPU()
        {
            return (IDebugProcessor)BuildDefaultCPU();
        }
    }
}

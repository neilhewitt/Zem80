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
            IRegisters registers = new Registers();
            IMemoryMap map = new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES);
            RAM ram = new RAM(0, MAX_MEMORY_SIZE_IN_BYTES);
            map.Map(ram, 0);
            Memory memory = new Memory(map);
            IStack stack = new Stack(registers, (ushort)(MAX_MEMORY_SIZE_IN_BYTES - 3)); // leave two bytes at stack top
            IPorts ports = new Ports();

            Processor z80 = new Processor(registers, map, stack, ports, 4.00);
            return z80;
        }

        public static IDebugProcessor BuildDebugCPU()
        {
            return (IDebugProcessor)BuildDefaultCPU();
        }
    }
}

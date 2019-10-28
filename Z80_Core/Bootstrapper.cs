using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public static class Bootstrapper
    {
        public const uint MAX_MEMORY_SIZE_IN_BYTES = 65536;
        
        public static Processor BuildProcessor()
        {
            IRegisters registers = new Registers();
            IMemoryMap map = new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES);
            RAM ram = new RAM(0, MAX_MEMORY_SIZE_IN_BYTES);
            map.Map(ram);
            IMemory memory = new Memory(map);

            Processor z80 = new Processor(registers, memory, 65533, 4.00); // 65533 == leave two bytes at stack top
            return z80;
        }
    }
}

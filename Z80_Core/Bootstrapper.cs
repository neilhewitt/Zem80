﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public static class Bootstrapper
    {
        public const int MAX_CONTIGUOUS_MEMORY_SIZE_IN_KILOBYTES = 16;
        
        public static Processor BuildProcessor()
        {
            IRegisters registers = new Registers();
            IMemoryMap map = new MemoryMap(MAX_CONTIGUOUS_MEMORY_SIZE_IN_KILOBYTES);
            RAM ram = new RAM(0, MAX_CONTIGUOUS_MEMORY_SIZE_IN_KILOBYTES);
            map.Map(ram);
            IMemory memory = new Memory(map);

            Processor z80 = new Processor(registers, memory);
            return z80;
        }
    }
}

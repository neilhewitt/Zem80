using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class MemoryWriter
    {
        private MemoryMap _map;

        public void WriteByteAt(uint address, byte value)
        {
            IMemoryLocation memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly"); // TODO: custom exception type
            }

            memory.WriteByteAt(address, value);
        }

        public void WriteWordAt(uint address, ushort value)
        {
            IMemoryLocation memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly"); // TODO: custom exception type
            }

            memory.WriteWordAt(address, value); 
        }

        public MemoryWriter(MemoryMap map)
        {
            _map = map;
        }
    }
}

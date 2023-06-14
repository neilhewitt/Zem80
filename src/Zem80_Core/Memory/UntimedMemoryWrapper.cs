using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core.Memory
{
    public class UntimedMemoryWrapper : IMemory
    {
        private IMemoryBank _wrappedMemory;

        public byte ReadByteAt(ushort address)
        {
            return _wrappedMemory.ReadByteAt(address, 0);
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return _wrappedMemory.ReadBytesAt(address, numberOfBytes, 0);
        }

        public ushort ReadWordAt(ushort address)
        {
            return _wrappedMemory.ReadWordAt(address, 0);
        }

        public void WriteByteAt(ushort address, byte value)
        {
            _wrappedMemory.WriteByteAt(address, value, 0);
        }

        public void WriteBytesAt(ushort address, byte[] bytes)
        {
            _wrappedMemory.WriteBytesAt(address, bytes, 0);
        }

        public void WriteWordAt(ushort address, ushort value)
        {
           _wrappedMemory.WriteWordAt(address, value, 0);
        }

        public UntimedMemoryWrapper(IMemoryBank memoryToWrap)
        {
            _wrappedMemory = memoryToWrap;
        }
    }
}

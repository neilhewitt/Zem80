using System;
using System.Runtime.Serialization;

namespace Zem80.Core
{
    public class MemoryNotWritableException : MemoryException
    {
        public MemoryNotWritableException()
        {
        }

        public MemoryNotWritableException(string message) : base(message)
        {
        }

        public MemoryNotWritableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

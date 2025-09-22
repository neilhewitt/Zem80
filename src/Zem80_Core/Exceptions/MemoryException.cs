using System;
using System.Runtime.Serialization;

namespace Zem80.Core
{
    public class MemoryException : Z80Exception
    {
        public MemoryException()
        {
        }

        public MemoryException(string message) : base(message)
        {
        }

        public MemoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace Zem80.Core
{
    public class MemorySegmentException : MemoryException
    {
        public MemorySegmentException()
        {
        }

        public MemorySegmentException(string message) : base(message)
        {
        }

        public MemorySegmentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

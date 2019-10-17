using System;
using System.Runtime.Serialization;

namespace Z80.Core
{
    public class MemorySegmentException : Z80Exception
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

        protected MemorySegmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

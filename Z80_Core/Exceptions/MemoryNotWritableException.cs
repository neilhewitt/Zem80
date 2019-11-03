using System;
using System.Runtime.Serialization;

namespace Z80.Core
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

        protected MemoryNotWritableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

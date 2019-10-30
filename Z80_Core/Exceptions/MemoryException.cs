using System;
using System.Runtime.Serialization;

namespace Z80.Core
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

        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

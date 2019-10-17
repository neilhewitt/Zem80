using System;
using System.Runtime.Serialization;

namespace Z80.Core
{
    public class MemoryMapException : Z80Exception
    {
        public MemoryMapException()
        {
        }

        public MemoryMapException(string message) : base(message)
        {
        }

        public MemoryMapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemoryMapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

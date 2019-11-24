using System;
using System.Runtime.Serialization;

namespace Z80.Core
{
    public class MemoryNotInitialisedException : MemoryException
    {
        public MemoryNotInitialisedException()
        {
        }

        public MemoryNotInitialisedException(string message) : base(message)
        {
        }

        public MemoryNotInitialisedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemoryNotInitialisedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

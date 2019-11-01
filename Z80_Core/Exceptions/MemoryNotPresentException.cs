using System;
using System.Runtime.Serialization;

namespace Z80.Core
{
    public class MemoryNotPresentException : MemoryException
    {
        public MemoryNotPresentException()
        {
        }

        public MemoryNotPresentException(string message) : base(message)
        {
        }

        public MemoryNotPresentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MemoryNotPresentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

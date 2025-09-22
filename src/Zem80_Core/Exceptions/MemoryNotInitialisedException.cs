using System;
using System.Runtime.Serialization;

namespace Zem80.Core
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
    }
}

using System;
using System.Runtime.Serialization;

namespace Zem80.Core
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
    }
}

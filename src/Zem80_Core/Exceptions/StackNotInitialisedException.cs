using System;
using System.Runtime.Serialization;

namespace Zem80.Core
{
    public class StackNotInitialisedException : Z80Exception
    {
        public StackNotInitialisedException()
        {
        }

        public StackNotInitialisedException(string message) : base(message)
        {
        }

        public StackNotInitialisedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

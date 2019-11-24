using System;
using System.Runtime.Serialization;

namespace Z80.Core
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

        protected StackNotInitialisedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

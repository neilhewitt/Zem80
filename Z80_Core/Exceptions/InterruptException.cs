using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Z80.Core
{
    public class InterruptException : Exception
    {
        public InterruptException()
        {
        }

        public InterruptException(string message) : base(message)
        {
        }

        public InterruptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InterruptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

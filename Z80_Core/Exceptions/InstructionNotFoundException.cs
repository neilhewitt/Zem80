using System;
using System.Runtime.Serialization;

namespace Zem80.Core
{
    public class InstructionNotFoundException : Z80Exception
    {
        public InstructionNotFoundException()
        {
        }

        public InstructionNotFoundException(string message) : base(message)
        {
        }

        public InstructionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstructionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

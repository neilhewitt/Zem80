using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Zem80.Core
{
    public class InstructionDecoderException : Exception
    {
        public InstructionDecoderException()
        {
        }

        public InstructionDecoderException(string message) : base(message)
        {
        }

        public InstructionDecoderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

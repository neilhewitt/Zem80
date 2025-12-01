using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Zem80.Core
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
    }
}

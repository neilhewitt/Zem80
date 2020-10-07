using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Zem80.Core
{
    public class Z80Exception : Exception
    {
        public Z80Exception()
        {
        }

        public Z80Exception(string message) : base(message)
        {
        }

        public Z80Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Z80Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

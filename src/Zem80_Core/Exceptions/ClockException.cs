using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core
{
    public class ClockException : Z80Exception
    {
        public ClockException()
        {
        }

        public ClockException(string message) : base(message)
        {
        }

        public ClockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

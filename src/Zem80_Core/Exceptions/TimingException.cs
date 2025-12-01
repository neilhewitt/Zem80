using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core
{
    public class TimingException : Z80Exception
    {
        public TimingException()
        {
        }

        public TimingException(string message) : base(message)
        {
        }

        public TimingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

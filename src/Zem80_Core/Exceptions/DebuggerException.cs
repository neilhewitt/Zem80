using System;

namespace Zem80.Core
{
    public class DebuggerException : Z80Exception
    {
        public DebuggerException()
        {
        }
        public DebuggerException(string message) : base(message)
        {
        }
        public DebuggerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

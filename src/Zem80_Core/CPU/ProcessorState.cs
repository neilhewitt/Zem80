using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    [Flags]
    public enum ProcessorState
    {
        Running,
        Suspended,
        Halted,
        Stopped
    }
}

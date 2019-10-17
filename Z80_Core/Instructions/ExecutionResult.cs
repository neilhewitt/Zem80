using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ExecutionResult
    {
        public IFlags Flags { get; }
        public long CLRTicks { get; }

        public ExecutionResult(IFlags flags, long ticks)
        {
            Flags = flags;
            CLRTicks = ticks;
        }
    }
}
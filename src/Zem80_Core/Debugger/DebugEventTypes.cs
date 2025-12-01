using System;
using Zem80.Core.CPU;

namespace Zem80.Core.Debugger
{
    [Flags]
    public enum DebugEventTypes : byte
    {
        BreakpointReached = 1 << 0,
        BeforeInstructionExecution = 1 << 1,
        BeforeMachineCycle = 1 << 2,
        AfterMachineCycle = 1 << 3,
        AfterInstructionExecution = 1 << 4,
        OnInterruptAcknowledge = 1 << 5,
        BeforeMaskableInterrupt = 1 << 6,
        BeforeNonMaskableInterrupt = 1 << 7,
        All = byte.MaxValue
    }
}

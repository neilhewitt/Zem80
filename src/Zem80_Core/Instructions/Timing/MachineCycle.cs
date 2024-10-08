﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zem80.Core.CPU
{
    public class MachineCycle
    {
        public MachineCycleType Type { get; private set; }
        public byte TStates { get; private set; }
        public bool RunsOnlyIfConditionTrue { get; private set; }
        public bool HasMemoryAccess { get; private set; }
        public bool HasIO { get; private set; }

        public MachineCycle(MachineCycleType type, byte tStates, bool runsOnlyIfConditionTrue)
        {
            Type = type;
            TStates = tStates;
            RunsOnlyIfConditionTrue = runsOnlyIfConditionTrue;
            HasMemoryAccess = type < MachineCycleType.InternalOperation;
            HasIO = type == MachineCycleType.PortRead || type == MachineCycleType.PortWrite;
        }
    }
}

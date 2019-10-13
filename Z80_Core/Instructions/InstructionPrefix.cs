using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum InstructionPrefix
    {
        Unprefixed,
        CB,
        ED,
        DD,
        FD,
        DDCB,
        FDCB
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum InstructionPrefix
    {
        Unprefixed = 0x00,
        CB = 0xCB,
        ED = 0xED,
        DD = 0xDD, 
        FD = 0xFD,
        DDCB = 0xDDCB,
        FDCB = 0xFDCB,
        PseudoInstruction = 0xFFFF
    }
}

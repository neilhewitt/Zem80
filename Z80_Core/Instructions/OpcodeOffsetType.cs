using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum OpcodeOffsetType
    {
        Register,
        IXHigher,
        IXLower,
        IYHigher,
        IYLower
    }
}

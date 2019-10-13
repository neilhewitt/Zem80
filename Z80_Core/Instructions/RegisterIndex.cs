using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum RegisterIndex : uint
    {
        B = 0,
        C = 1,
        D = 2,
        E = 3,
        H = 4,
        L = 5,
        A = 7,
        IXh = 4,
        IXl = 5,
        IYh = 4,
        IYl = 5
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public enum ByteRegister
    {
        // NOTE: the numeric values correspond to the index of the register value in the underlying storage array
        None = 26,
        A = 0,
        F = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        H = 6,
        L = 7,
        IXh = 16,
        IXl = 17,
        IYh = 18,
        IYl = 19,
        I = 22,
        R = 23
    }
}

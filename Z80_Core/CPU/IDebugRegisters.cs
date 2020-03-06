using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public interface IDebugRegisters : IRegisters
    {
        new ushort AF { get; set; }
    }
}

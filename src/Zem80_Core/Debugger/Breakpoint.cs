using System;

namespace Zem80.Core.Debugger
{
    public class Breakpoint
    {
        public ushort Address { get; }

        public Breakpoint(ushort address)
        {
            Address = address;
        }
    }
}

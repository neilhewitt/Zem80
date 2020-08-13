using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Z80.Core
{
    public static class ALUOperations
    {
        public static (byte Result, Flags Flags) Add(byte left, byte right, bool carry)
        {
            Flags flags = FlagLookup.ByteArithmeticFlags(left, right, carry, false);
            return ((byte)(left + right + (carry ? 1 : 0)), flags);
        }

        public static (byte Result, Flags Flags) Subtract(byte left, byte right, bool carry)
        {
            Flags flags = FlagLookup.ByteArithmeticFlags(left, right, carry, true);
            return ((byte)(left - right - (carry ? 1 : 0)), flags);
        }

        public static (ushort Result, Flags Flags) Add(ushort left, ushort right, bool carry, bool setSZPV, Flags currentFlags)
        {
            Flags flags = FlagLookup.GetWordArithmeticFlags(currentFlags, left, right, carry, setSZPV, false);
            return ((ushort)(left + right + (carry ? 1 : 0)), flags);
        }
        public static (ushort Result, Flags Flags) Subtract(ushort left, ushort right, bool carry, bool setSZPV, Flags currentFlags)
        {
            Flags flags = FlagLookup.GetWordArithmeticFlags(currentFlags, left, right, carry, setSZPV, true);
            return ((ushort)(left - right - (carry ? 1 : 0)), flags);
        }
    }
}

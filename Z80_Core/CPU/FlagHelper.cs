using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagHelper
    {
        public static void SetFlagsFromArithmeticOperation(Flags flags, byte startingValue, byte addOrSubtractValue, int resultingValue, bool subtracts = false, IEnumerable<Flag> flagsToPreserve = null)
        {
            if (NotPreserved(flagsToPreserve, Flag.Zero)) flags.Zero = (resultingValue == 0);
            if (NotPreserved(flagsToPreserve, Flag.Carry)) flags.Carry = (resultingValue > 0xFF);
            if (NotPreserved(flagsToPreserve, Flag.Sign)) flags.Sign = ((sbyte)resultingValue < 0);
            if (NotPreserved(flagsToPreserve, Flag.ParityOverflow)) flags.ParityOverflow = (resultingValue > 0x7F || resultingValue < -0x80);
            if (NotPreserved(flagsToPreserve, Flag.HalfCarry)) flags.HalfCarry = (startingValue.HalfCarryWhenAdding(addOrSubtractValue));
            if (NotPreserved(flagsToPreserve, Flag.Subtract)) flags.Subtract = subtracts;
        }

        public static void SetFlagsFromArithmeticOperation(Flags flags, ushort startingValue, ushort addOrSubtractValue, int resultingValue, bool subtracts = false, IEnumerable<Flag> flagsToPreserve = null) 
        {
            if (NotPreserved(flagsToPreserve, Flag.Zero)) flags.Zero = (resultingValue == 0);
            if (NotPreserved(flagsToPreserve, Flag.Carry)) flags.Carry = (resultingValue > 0xFFFF);
            if (NotPreserved(flagsToPreserve, Flag.Sign)) flags.Sign = ((short)resultingValue < 0);
            if (NotPreserved(flagsToPreserve, Flag.ParityOverflow)) flags.ParityOverflow = (resultingValue > 0x7FFF || resultingValue < -0x8000);
            if (NotPreserved(flagsToPreserve, Flag.HalfCarry)) flags.HalfCarry = (startingValue.HalfCarryWhenAdding(addOrSubtractValue));
            if (NotPreserved(flagsToPreserve, Flag.Subtract)) flags.Subtract = subtracts;
        }

        public static void SetFlagsFromLogicalOperation(Flags flags, byte startingValue, byte addOrSubtractValue, int resultingValue, bool carry = false, IEnumerable<Flag> flagsToPreserve = null)
        {
            if (NotPreserved(flagsToPreserve, Flag.Zero)) flags.Zero = (resultingValue == 0x00);
            if (NotPreserved(flagsToPreserve, Flag.Sign)) flags.Sign = (((sbyte)resultingValue) < 0);
            if (NotPreserved(flagsToPreserve, Flag.ParityOverflow)) flags.ParityOverflow = (((byte)resultingValue).CountBits(true) % 2 == 0);
            if (NotPreserved(flagsToPreserve, Flag.HalfCarry)) flags.HalfCarry = (startingValue.HalfCarryWhenAdding(addOrSubtractValue));
            if (NotPreserved(flagsToPreserve, Flag.Subtract)) flags.Subtract = false;
            if (NotPreserved(flagsToPreserve, Flag.Carry)) flags.Carry = carry;
        }

        private static bool NotPreserved(IEnumerable<Flag> preserve, Flag flag) => preserve == null ? true : !preserve.Contains(flag);
    }
}
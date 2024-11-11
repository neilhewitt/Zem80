using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.CPU
{
    public static class Arithmetic
    {
        public static (byte Result, Flags Flags) Add(byte left, byte right, bool carry)
        {
            return ByteArithmetic(left, right, carry, false);
        }

        public static (byte Result, Flags Flags) Subtract(byte left, byte right, bool carry)
        {
            return ByteArithmetic(left, right, carry, true);
        }

        public static (ushort Result, Flags Flags) Add(ushort left, ushort right, bool carry, bool setSZPV, Flags currentFlags)
        {
            return WordArithmetic(currentFlags, left, right, false, carry, setSZPV);
        }
        public static (ushort Result, Flags Flags) Subtract(ushort left, ushort right, bool carry, bool setSZPV, Flags currentFlags)
        {
            return WordArithmetic(currentFlags, left, right, true, carry, setSZPV);
        }

        private static (byte, Flags) ByteArithmetic(byte startingValue, byte addOrSubtractValue, bool carry, bool subtract)
        {
            Flags flags = new Flags();
            int carryValue = carry ? 1 : 0;

            int result = subtract ? startingValue - addOrSubtractValue - carryValue :
                                    startingValue + addOrSubtractValue + carryValue;

            // we need this value - (sbyte)result won't work here because we need the *arithmetic* to be signed
            int signed = subtract ? ((sbyte)startingValue - (sbyte)addOrSubtractValue - carryValue) :
                                    ((sbyte)startingValue + (sbyte)addOrSubtractValue + carryValue);

            flags.Zero = ((byte)result == 0);
            flags.Carry = ((result & 0x100) != 0);
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = (signed >= 0x80 || signed <= -0x81);
            flags.HalfCarry = ((startingValue ^ (byte)(result & 0xFF) ^ addOrSubtractValue) & 0x10) != 0;
            flags.Subtract = subtract; // don't forget to override
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5

            return ((byte)result, flags);
        }

        private static (ushort, Flags) WordArithmetic(Flags currentFlags, ushort left, ushort right, bool subtract, bool carry, bool setSZPV)
        {
            Flags flags = new Flags(currentFlags?.Value ?? 0);
            int carryValue = carry ? 1 : 0;

            int result = subtract ? left - right - carryValue :
                        left + right + carryValue;

            flags.Carry = ((result & 0x10000) != 0);
            flags.HalfCarry = ((left ^ (short)(result & 0xFFFF) ^ right) & 0x1000) != 0;
            flags.Subtract = subtract;

            byte r = ((ushort)result).HighByte();
            flags.X = (r & 0x08) > 0; // copy bit 3
            flags.Y = (r & 0x20) > 0; // copy bit 5

            // some 16-bit arithmetic operations preserve flags from the
            // last instruction - if not, then set the remaining flags here
            if (setSZPV)
            {
                flags.Zero = result == 0;

                // to check for overflow, we must perform the arithmetic again but with the inputs as signed values - casting won't work
                int signed = (subtract ? ((short)left - (short)right - carryValue) : ((short)left + (short)right + carryValue));
                flags.ParityOverflow = (signed >= 0x8000 || signed < -0x8001);

                flags.Sign = ((short)result < 0);
            }

            return ((ushort)result, flags);
        }
    }
}

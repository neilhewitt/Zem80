using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Reflection;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public static class FlagLookup
    {
        // the state space of 16 x 16 bit numbers is too large to pre-calculate the flags in a reasonable time
        // TODO: run the code to completion and store output in a file?
        public static Flags WordArithmeticFlags(Flags currentFlags, ushort startingValue, int addOrSubtractValue, bool carry, bool setSignZeroParityOverflow, bool subtract) 
        {
            Flags flags = new Flags(currentFlags?.Value ?? 0);
            
            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : 
                                    startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Carry = ((result & 0x10000) != 0);
            flags.HalfCarry = HalfCarry(startingValue, (ushort)addOrSubtractValue, carry, subtract);
            flags.Subtract = subtract;
            
            byte r = ((ushort)result).HighByte();
            flags.X = (r & 0x08) > 0; // copy bit 3
            flags.Y = (r & 0x20) > 0; // copy bit 5

            // some 16-bit arithmetic operations preserve flags from the
            // last instruction - if not, then set the remaining flags here
            if (setSignZeroParityOverflow)
            {
                flags.Zero = ((ushort)result == 0);
                flags.ParityOverflow = Overflows(startingValue, (ushort)addOrSubtractValue, carry, subtract);
                flags.Sign = ((short)result < 0);
            }

            return flags;
        }

        public static Flags ByteArithmeticFlags(byte startingValue, byte addOrSubtractValue, bool carry, bool subtract)
        {
            Flags flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) :
                                    startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = ((byte)result == 0);
            flags.Carry = ((result & 0x100) != 0);
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = Overflows(startingValue, addOrSubtractValue, carry, subtract);
            flags.HalfCarry = HalfCarry(startingValue, addOrSubtractValue, carry, subtract);
            flags.Subtract = subtract; // don't forget to override
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5
            return flags;
        }

        public static Flags LogicalFlags(byte first, byte second, LogicalOperation operation)
        {
            Flags flags = new Flags();

            int result = operation switch
            {
                LogicalOperation.And => first & second,
                LogicalOperation.Or => first | second,
                LogicalOperation.Xor => first ^ second,
                _ => 0x00
            };

            flags.Zero = ((byte)result == 0x00);
            flags.Carry = false;
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = ((byte)result).EvenParity();
            flags.HalfCarry = operation == LogicalOperation.And;
            flags.Subtract = false;
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5
            return flags;
        }

        public static Flags BitwiseFlags(byte value, BitwiseOperation operation, bool previousCarry)
        {
            Flags flags = new Flags();

            int result = operation switch
            {
                BitwiseOperation.ShiftLeft => value << 1,
                BitwiseOperation.ShiftLeftSetBit0 => (value << 1) + 1,
                BitwiseOperation.ShiftRight => value >> 1,
                BitwiseOperation.ShiftRightPreserveBit7 => ((byte)(value >> 1)).SetBit(7, value.GetBit(7)),
                BitwiseOperation.RotateLeft => ((byte)(value << 1)).SetBit(0, value.GetBit(7)),
                BitwiseOperation.RotateRight => ((byte)(value >> 1)).SetBit(7, value.GetBit(0)),
                BitwiseOperation.RotateLeftThroughCarry => ((byte)(value << 1)).SetBit(0, previousCarry),
                BitwiseOperation.RotateRightThroughCarry => ((byte)(value >> 1)).SetBit(7, previousCarry),
                _ => value
            };

            flags.Carry = operation switch
            {
                BitwiseOperation.RotateLeftThroughCarry => value.GetBit(7),
                BitwiseOperation.RotateRightThroughCarry => value.GetBit(0),
                _ => flags.Carry
            };

            flags.Zero = ((byte)result == 0x00);
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = ((byte)result).EvenParity();
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5
            return flags;
        }

        private static bool HalfCarry(byte first, byte second, bool carry, bool isSubtraction)
        {
            int c = carry ? 1 : 0;
            int result = (isSubtraction ? (first - second - c) : (first + second + c));
            byte r = (byte)(result & 0xFF);

            return ((first ^ r ^ second) & 0x10) != 0;
        }

        private static bool HalfCarry(ushort first, ushort second, bool carry, bool isSubtraction)
        {
            int c = carry ? 1 : 0;
            int result = (isSubtraction ? (first - second - c) : (first + second + c));
            short r = (short)(result & 0xFFFF);

            return ((first ^ r ^ second) & 0x1000) != 0;
        }

        private static bool Overflows(byte first, byte second, bool carry, bool isSubtraction)
        {
            int c = (carry ? 1 : 0);
            int signed = (isSubtraction ? ((sbyte)first - (sbyte)second - c) : ((sbyte)first + (sbyte)second + c));
            return (signed >= 0x80 || signed <= -0x81);
        }

        private static bool Overflows(ushort first, ushort second, bool carry, bool isSubtraction)
        {
            int c = (carry ? 1 : 0);
            int signed = (isSubtraction ? ((short)first - (short)second - c) : ((short)first + (short)second + c));
            return (signed >= 0x8000 || signed < -0x8000);
        }
    }
}
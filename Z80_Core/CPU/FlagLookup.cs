using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagLookup
    {
        public static bool EnablePrecalculation { get; set; } = false;

        private static bool _initialised;
        private static byte[,,] _addFlags8;
        private static byte[,,] _subFlags8;
        private static byte[,,] _logicalFlags;
        private static byte[,] _bitwiseFlags;

        public static void BuildFlagLookupTables()
        {
            if (!_initialised && EnablePrecalculation)
            {
                _addFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _addFlags8[i, j, 0] = GetByteArithmeticFlags((byte)i, (byte)j, false, false).Value;
                        _addFlags8[i, j, 1] = GetByteArithmeticFlags((byte)i, (byte)j, true, false).Value;
                    }
                }

                _subFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _subFlags8[i, j, 0] = GetByteArithmeticFlags((byte)i, (byte)j, false, true).Value;
                        _subFlags8[i, j, 1] = GetByteArithmeticFlags((byte)i, (byte)j, true, true).Value;
                    }
                }

                _logicalFlags = new byte[256, 256, 3];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _logicalFlags[i, j, 0] = GetLogicalFlags((byte)i, (byte)j, LogicalOperation.Or).Value;
                        _logicalFlags[i, j, 1] = GetLogicalFlags((byte)i, (byte)j, LogicalOperation.And).Value;
                        _logicalFlags[i, j, 2] = GetLogicalFlags((byte)i, (byte)j, LogicalOperation.Xor).Value;
                    }
                }

                _bitwiseFlags = new byte[256, 4];
                for (int i = 0; i < 256; i++)
                {
                    Flags flags = new Flags();
                    _bitwiseFlags[i, 0] = GetBitwiseFlags((byte)i, BitwiseOperation.ShiftLeft).Value;
                    _bitwiseFlags[i, 1] = GetBitwiseFlags((byte)i, BitwiseOperation.ShiftRight).Value;
                    _bitwiseFlags[i, 2] = GetBitwiseFlags((byte)i, BitwiseOperation.RotateLeft).Value;
                    _bitwiseFlags[i, 3] = GetBitwiseFlags((byte)i, BitwiseOperation.RotateRight).Value;
                }

                _initialised = true;
            }            
        }

        public static Flags ByteArithmeticFlags(byte startingValue, int addOrSubtractValue, bool carry, bool subtract)
        {
            if (EnablePrecalculation)
            {
                return new Flags((subtract ?
                                    _subFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0] :
                                    _addFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0]
                                    ));
            }
            else
            {
                return GetByteArithmeticFlags(startingValue, (byte)addOrSubtractValue, carry, subtract);
            }
        }

        public static Flags LogicalFlags(byte first, byte second, LogicalOperation operation)
        {
            if (EnablePrecalculation)
            {
                return new Flags(_logicalFlags[first, second, (int)operation]);
            }
            else
            {
                return GetLogicalFlags(first, second, operation);
            }
        }

        public static Flags BitwiseFlags(byte value, BitwiseOperation operation)
        {
            if (EnablePrecalculation)
            {
                return new Flags(_bitwiseFlags[value, (int)operation]);
            }
            else
            {
                return GetBitwiseFlags(value, operation);
            }
        }

        // the state space of 16 x 16 bit numbers is too large to pre-calculate the flags in a reasonable time
        // TODO: run the code to completion and store output in a file?
        public static Flags WordArithmeticFlags(Flags flags, ushort startingValue, int addOrSubtractValue, bool carry, bool setSignZeroParityOverflow, bool subtract) 
        {
            if (flags == null) flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : 
                                    startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Carry = result > 0xFFFF;
            flags.HalfCarry = HalfCarry(startingValue, (ushort)addOrSubtractValue, subtract);
            flags.Subtract = subtract;

            // some 16-bit arithmetic operations preserve flags from the
            // last instruction - if not, then set the remaining flags here
            if (setSignZeroParityOverflow)
            {
                flags.Zero = ((byte)result == 0);
                flags.ParityOverflow = (Overflows(startingValue, (ushort)addOrSubtractValue));
                flags.Sign = ((short)result < 0);
            }

            return flags;
        }

        private static Flags GetByteArithmeticFlags(byte startingValue, byte addOrSubtractValue, bool carry, bool subtract)
        {
            Flags flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : 
                                    startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = ((byte)result == 0);
            flags.Carry = result > 0xFF;
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = (Overflows(startingValue, addOrSubtractValue));
            flags.HalfCarry = HalfCarry(startingValue, addOrSubtractValue, subtract);
            flags.Subtract = subtract; // don't forget to override
            return flags;
        }

        private static Flags GetLogicalFlags(byte first, byte second, LogicalOperation operation)
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
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
            flags.HalfCarry = operation == LogicalOperation.And;
            flags.Subtract = false;
            flags.Carry = false;
            return flags;
        }

        private static Flags GetBitwiseFlags(byte value, BitwiseOperation operation)
        {
            Flags flags = new Flags();

            int result = operation switch
            {
                BitwiseOperation.ShiftLeft => value << 1,
                BitwiseOperation.ShiftRight => value >> 1,
                BitwiseOperation.RotateLeft => ((byte)(value << 1)).SetBit(0, value.GetBit(7)),
                BitwiseOperation.RotateRight => ((byte)(value >> 1)).SetBit(7, value.GetBit(0)),
                _ => 0x00
            };

            flags.Zero = ((byte)result == 0x00);
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.Carry = ((byte)value).GetBit(7);
            return flags;
        }

        private static bool HalfCarry(byte first, byte second, bool isSubtraction)
        {
            if (isSubtraction)
            {
                return (((first & 0x0F) - (second & 0x0F)) < 0);
            }
            else
            {
                return (((first & 0x0F) + (second & 0x0F)) & 0x10) == 0x10;
            }
        }

        private static bool HalfCarry(ushort first, ushort second, bool isSubtraction)
        {
            return HalfCarry(first.HighByte(), second.HighByte(), isSubtraction);
        }

        private static bool Overflows(byte first, byte second)
        {
            int result = ((sbyte)first + (sbyte)second);
            return (result >= 0x80 || result <= -0x80);
        }

        private static bool Overflows(ushort first, ushort second)
        {
            int result = ((short)first + (short)second);
            return (result >= 0x8000 || result <= -0x8000);
        }
    }
}
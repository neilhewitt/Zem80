using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagLookup
    {
        private const bool FLAG_PRECALCULATION_ENABLED = false;

        private static bool _initialised;
        private static byte[,,] _addFlags8;
        private static byte[,,] _subFlags8;
        private static byte[,,] _logicalFlags;
        private static byte[,] _bitwiseFlags;

        public static void BuildFlagLookupTables()
        {
            if (!_initialised && FLAG_PRECALCULATION_ENABLED)
            {
                _addFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _addFlags8[i, j, 0] = CalulateArithmeticFlags((byte)i, (byte)j, false, false).Value;
                        _addFlags8[i, j, 1] = CalulateArithmeticFlags((byte)i, (byte)j, true, false).Value;
                    }
                }

                _subFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _subFlags8[i, j, 0] = CalulateArithmeticFlags((byte)i, (byte)j, false, true).Value;
                        _subFlags8[i, j, 1] = CalulateArithmeticFlags((byte)i, (byte)j, true, true).Value;
                    }
                }

                _logicalFlags = new byte[256, 256, 3];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _logicalFlags[i, j, 0] = CalculateLogicalFlags((byte)i, (byte)j, LogicalOperation.Or).Value;
                        _logicalFlags[i, j, 1] = CalculateLogicalFlags((byte)i, (byte)j, LogicalOperation.And).Value;
                        _logicalFlags[i, j, 2] = CalculateLogicalFlags((byte)i, (byte)j, LogicalOperation.Xor).Value;
                    }
                }

                _bitwiseFlags = new byte[256, 4];
                for (int i = 0; i < 256; i++)
                {
                    Flags flags = new Flags();
                    _bitwiseFlags[i, 0] = CalculateBitwiseFlags((byte)i, BitwiseOperation.ShiftLeft).Value;
                    _bitwiseFlags[i, 1] = CalculateBitwiseFlags((byte)i, BitwiseOperation.ShiftRight).Value;
                    _bitwiseFlags[i, 2] = CalculateBitwiseFlags((byte)i, BitwiseOperation.RotateLeft).Value;
                    _bitwiseFlags[i, 3] = CalculateBitwiseFlags((byte)i, BitwiseOperation.RotateRight).Value;
                }

                _initialised = true;
            }            
        }

        public static Flags ByteArithmeticFlags(byte startingValue, int addOrSubtractValue, bool carry, bool subtract)
        {
            if (FLAG_PRECALCULATION_ENABLED)
            {
                return new Flags((subtract ?
                                    _subFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0] :
                                    _addFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0]
                                    ));
            }
            else
            {
                return CalulateArithmeticFlags(startingValue, (byte)addOrSubtractValue, carry, subtract);
            }
        }

        public static Flags LogicalFlags(byte first, byte second, LogicalOperation operation)
        {
            if (FLAG_PRECALCULATION_ENABLED)
            {
                return new Flags(_logicalFlags[first, second, (int)operation]);
            }
            else
            {
                return CalculateLogicalFlags(first, second, operation);
            }
        }

        public static Flags BitwiseFlags(byte value, BitwiseOperation operation)
        {
            if (FLAG_PRECALCULATION_ENABLED)
            {
                return new Flags(_bitwiseFlags[value, (int)operation]);
            }
            else
            {
                return CalculateBitwiseFlags(value, operation);
            }
        }

        // the state space of 16 x 16 bit numbers is too large to pre-calculate the flags in a reasonable time
        // TODO: run the code to completion and store output in a file?
        public static Flags WordArithmeticFlags(Flags flags, ushort startingValue, int addOrSubtractValue, bool carry, bool setSignZeroParityOverflow, bool subtract) 
        {
            if (flags == null) flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = setSignZeroParityOverflow && ((byte)result == 0);
            flags.Carry = result > 0xFFFF;
            flags.Sign = setSignZeroParityOverflow && ((short)result < 0);
            flags.ParityOverflow = setSignZeroParityOverflow && (startingValue.OverflowsWhenAdding((ushort)addOrSubtractValue));
            flags.HalfCarry = subtract ? (startingValue.HalfCarryWhenSubtracting((ushort)addOrSubtractValue)) : (startingValue.HalfCarryWhenAdding((ushort)addOrSubtractValue));
            flags.Subtract = subtract;

            return flags;
        }

        private static Flags CalulateArithmeticFlags(byte startingValue, byte addOrSubtractValue, bool carry, bool subtract)
        {
            Flags flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = ((byte)result == 0);
            flags.Carry = result > 0xFF;
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = (startingValue.OverflowsWhenAddingOrSubtracting(addOrSubtractValue));
            flags.HalfCarry = subtract ? (startingValue.HalfCarryWhenSubtracting(addOrSubtractValue)) : (startingValue.HalfCarryWhenAdding(addOrSubtractValue));
            flags.Subtract = subtract; // don't forget to override
            return flags;
        }

        private static Flags CalculateLogicalFlags(byte first, byte second, LogicalOperation operation)
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

        private static Flags CalculateBitwiseFlags(byte value, BitwiseOperation operation)
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
    }
}
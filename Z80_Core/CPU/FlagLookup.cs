using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagLookup
    {
        private static bool USE_LOOKUPS = false;

        private static bool _initialised;
        private static byte[,] _addFlags8;
        private static byte[,] _subFlags8;
        private static byte[,,] _logicalFlags;
        private static byte[,] _bitwiseFlags;

        public static void BuildFlagLookupTables()
        {
            if (!_initialised)
            {
                _addFlags8 = new byte[256, 256];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        Flags flags = new Flags();
                        setArithmeticFlags(flags, (byte)i, (byte)j, i + j, false);
                        _addFlags8[i, j] = flags.Value;
                    }
                }

                _subFlags8 = new byte[256, 256];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        Flags flags = new Flags();
                        setArithmeticFlags(flags, (byte)i, (byte)j, i - j, true);
                        _subFlags8[i, j] = flags.Value;
                    }
                }

                _logicalFlags = new byte[256, 256, 3];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        Flags flags = new Flags();
                        setLogicalFlags(flags, (byte)i, (byte)j, i | j, LogicalOperation.Or);
                        _logicalFlags[i, j, 0] = flags.Value;
                        setLogicalFlags(flags, (byte)i, (byte)j, i & j, LogicalOperation.And);
                        _logicalFlags[i, j, 1] = flags.Value;
                        setLogicalFlags(flags, (byte)i, (byte)j, i ^ j, LogicalOperation.Xor);
                        _logicalFlags[i, j, 2] = flags.Value;
                    }
                }

                _bitwiseFlags = new byte[256, 4];
                for (int i = 0; i < 256; i++)
                {
                    Flags flags = new Flags();
                    setBitwiseFlags(flags, i, i << 1, BitwiseOperation.ShiftLeft);
                    _bitwiseFlags[i, 0] = flags.Value;
                    setBitwiseFlags(flags, i, i >> 1, BitwiseOperation.ShiftRight);
                    _bitwiseFlags[i, 1] = flags.Value;

                    byte rotated = ((byte)(i << 1)).SetBit(0, ((byte)i).GetBit(7));
                    setBitwiseFlags(flags, i, rotated, BitwiseOperation.RotateLeft);
                    _bitwiseFlags[i, 2] = flags.Value;

                    rotated = ((byte)(i >> 1)).SetBit(7, ((byte)i).GetBit(0));
                    setBitwiseFlags(flags, i, rotated, BitwiseOperation.RotateRight);
                    _bitwiseFlags[i, 3] = flags.Value;
                }

                _initialised = true;
            }

            void setArithmeticFlags(Flags flags, byte first, byte second, int result, bool subtract)
            {
                flags.Zero = ((byte)result == 0);
                flags.Carry = ((byte)result > 0xFF);
                flags.Sign = ((sbyte)result < 0);
                flags.ParityOverflow = (first.OverflowsWhenAdding(second));
                flags.HalfCarry = (first.HalfCarryWhenAdding(second));
                flags.Subtract = subtract; // don't forget to override
            }

            void setLogicalFlags(Flags flags, byte first, byte second, int result, LogicalOperation operation)
            {
                flags.Zero = ((byte)result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = operation == LogicalOperation.And; 
                flags.Subtract = false;
                flags.Carry = false;
            }

            void setBitwiseFlags(Flags flags, int value, int result, BitwiseOperation operation)
            {
                flags.Zero = ((byte)result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = false;
                flags.Subtract = false;
                flags.Carry = ((byte)value).GetBit(7);
            }
        }

        public static Flags FlagsFromArithmeticOperation(byte startingValue, int addOrSubtractValue, bool carry, bool subtract)
        {
            if (USE_LOOKUPS)
            {
                return new Flags((subtract ? _subFlags8[startingValue, (byte)addOrSubtractValue] : _addFlags8[startingValue, (byte)addOrSubtractValue]));
            }

            Flags flags = new Flags();
            
            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1: 0) : startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = ((byte)result == 0);
            flags.Carry = result > 0xFF;
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = (startingValue.OverflowsWhenAdding((byte)addOrSubtractValue));
            flags.HalfCarry = (startingValue.HalfCarryWhenAdding((byte)addOrSubtractValue));
            flags.Subtract = subtract; // don't forget to override
            return flags;
        }

        public static Flags FlagsFromArithmeticOperation16Bit(Flags flags, ushort startingValue, int addOrSubtractValue, bool carry, bool setSignZeroParityOverflow, bool subtract) 
        {
            if (flags == null) flags = new Flags();

            int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : startingValue + addOrSubtractValue + (carry ? 1 : 0);

            flags.Zero = setSignZeroParityOverflow && ((byte)result == 0);
            flags.Carry = result > 0xFFFF;
            flags.Sign = setSignZeroParityOverflow && ((short)result < 0);
            flags.ParityOverflow = setSignZeroParityOverflow && (startingValue.OverflowsWhenAdding((ushort)addOrSubtractValue));
            flags.HalfCarry = (startingValue.HalfCarryWhenAdding((ushort)addOrSubtractValue));
            flags.Subtract = subtract;

            return flags;
        }

        public static Flags FlagsFromLogicalOperation(byte first, byte second, LogicalOperation operation)
        {
            if (USE_LOOKUPS)
            {
                byte flagValue = _logicalFlags[first, second, (int)operation];
                return new Flags(flagValue);
            }

            Flags flags = new Flags();
            int result = operation switch
            {
                LogicalOperation.And => first & second,
                LogicalOperation.Or => first | second,
                LogicalOperation.Xor => first ^ second,
                _ => 0
            };
            flags.Zero = ((byte)result == 0x00);
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
            flags.HalfCarry = operation == LogicalOperation.And;
            flags.Subtract = false;
            flags.Carry = false;
            return flags;
        }

        public static Flags FlagsFromBitwiseOperation(byte value, BitwiseOperation operation)
        {
            if (USE_LOOKUPS)
            {
                return new Flags(_bitwiseFlags[value, (int)operation]);
            }

            Flags flags = new Flags();
            int result = operation switch
            {
                BitwiseOperation.ShiftLeft => value << 1,
                BitwiseOperation.ShiftRight => value >> 1,
                BitwiseOperation.RotateLeft => ((byte)(value << 1)).SetBit(0, ((byte)value).GetBit(7)),
                BitwiseOperation.RotateRight => ((byte)(value >> 1)).SetBit(7, ((byte)value).GetBit(0)),
                _ => 0
            };
            flags.Zero = (result == 0x00);
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.Carry = ((byte)value).GetBit(7);
            return flags;
        }
    }
}
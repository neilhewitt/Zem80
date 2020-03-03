using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagLookup
    {
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
                flags.Zero = (result == 0);
                flags.Carry = ((byte)result > 0xFF);
                flags.Sign = ((sbyte)result < 0);
                flags.ParityOverflow = (first.OverflowsWhenAdding(second));
                flags.HalfCarry = (first.HalfCarryWhenAdding(second));
                flags.Subtract = subtract; // don't forget to override
            }

            void setLogicalFlags(Flags flags, byte first, byte second, int result, LogicalOperation operation)
            {
                flags.Zero = (result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = operation == LogicalOperation.And; 
                flags.Subtract = false;
                flags.Carry = false;
            }

            void setBitwiseFlags(Flags flags, int value, int result, BitwiseOperation operation)
            {
                flags.Zero = (result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = false;
                flags.Subtract = false;
                flags.Carry = ((byte)value).GetBit(7);
            }
        }

        public static Flags FlagsFromArithmeticOperation(byte startingValue, byte addOrSubtractValue, bool subtracts)
        {
            return new Flags((subtracts ? _subFlags8[startingValue, addOrSubtractValue] : _addFlags8[startingValue, addOrSubtractValue]));
        }

        public static Flags FlagsFromArithmeticOperation16Bit(Flags flags, ushort startingValue, ushort addOrSubtractValue, int result, bool setSignZeroParityOverflow, bool subtract) 
        {
            if (flags == null) flags = new Flags();

            flags.Zero = setSignZeroParityOverflow && (result == 0);
            flags.Carry = ((ushort)result > 0xFFff);
            flags.Sign = setSignZeroParityOverflow && ((short)result < 0);
            flags.ParityOverflow = setSignZeroParityOverflow && (startingValue.OverflowsWhenAdding(addOrSubtractValue));
            flags.HalfCarry = (startingValue.HalfCarryWhenAdding(addOrSubtractValue));
            flags.Subtract = subtract;

            return flags;
        }

        public static Flags FlagsFromLogicalOperation(byte first, byte second, LogicalOperation operation)
        {
            byte flagValue = _logicalFlags[first, second, (int)operation];
            return new Flags(flagValue);
        }

        public static Flags FlagsFromBitwiseOperation(byte value, BitwiseOperation operation)
        {
            return new Flags(_bitwiseFlags[value, (int)operation]);
        }
    }
}
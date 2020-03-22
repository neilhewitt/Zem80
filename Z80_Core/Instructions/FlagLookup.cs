using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public static class FlagLookup
    {
        private static bool _initialised;
        private static byte[,,] _addFlags8;
        private static byte[,,] _subFlags8;
        private static byte[,,] _logicalFlags;
        private static byte[,] _bitwiseFlags;

        public static void BuildFlagLookupTables()
        {
            if (!_initialised)
            {
                _addFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _addFlags8[i, j, 0] = arithmeticFlags((byte)i, (byte)j, false, false).Value;
                        _addFlags8[i, j, 1] = arithmeticFlags((byte)i, (byte)j, true, false).Value;
                    }
                }

                _subFlags8 = new byte[256, 256, 2];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _subFlags8[i, j, 0] = arithmeticFlags((byte)i, (byte)j, false, true).Value;
                        _subFlags8[i, j, 1] = arithmeticFlags((byte)i, (byte)j, true, true).Value;
                    }
                }

                _logicalFlags = new byte[256, 256, 3];
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        _logicalFlags[i, j, 0] = logicalFlags((byte)i, (byte)j, i | j, LogicalOperation.Or).Value;
                        _logicalFlags[i, j, 1] = logicalFlags((byte)i, (byte)j, i & j, LogicalOperation.And).Value;
                        _logicalFlags[i, j, 2] = logicalFlags((byte)i, (byte)j, i ^ j, LogicalOperation.Xor).Value;
                    }
                }

                _bitwiseFlags = new byte[256, 4];
                for (int i = 0; i < 256; i++)
                {
                    Flags flags = new Flags();
                    _bitwiseFlags[i, 0] = bitwiseFlags(i, i << 1, BitwiseOperation.ShiftLeft).Value;
                    _bitwiseFlags[i, 1] = bitwiseFlags(i, i >> 1, BitwiseOperation.ShiftRight).Value;

                    byte rotated = ((byte)(i << 1)).SetBit(0, ((byte)i).GetBit(7));
                    _bitwiseFlags[i, 2] = bitwiseFlags(i, rotated, BitwiseOperation.RotateLeft).Value;

                    rotated = ((byte)(i >> 1)).SetBit(7, ((byte)i).GetBit(0));
                    _bitwiseFlags[i, 3] = bitwiseFlags(i, rotated, BitwiseOperation.RotateRight).Value;
                }

                _initialised = true;
            }

            Flags arithmeticFlags(byte startingValue, byte addOrSubtractValue, bool carry, bool subtract)
            {
                Flags flags = new Flags();

                int result = subtract ? startingValue - addOrSubtractValue - (carry ? 1 : 0) : startingValue + addOrSubtractValue + (carry ? 1 : 0);

                flags.Zero = ((byte)result == 0);
                flags.Carry = result > 0xFF;
                flags.Sign = ((sbyte)result < 0);
                flags.ParityOverflow = (startingValue.OverflowsWhenAdding((byte)addOrSubtractValue));
                flags.HalfCarry = (startingValue.HalfCarryWhenAdding((byte)addOrSubtractValue));
                flags.Subtract = subtract; // don't forget to override
                return flags;
            }

            Flags logicalFlags(byte first, byte second, int result, LogicalOperation operation)
            {
                Flags flags = new Flags();
                flags.Zero = ((byte)result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = operation == LogicalOperation.And; 
                flags.Subtract = false;
                flags.Carry = false;
                return flags;
            }

            Flags bitwiseFlags(int value, int result, BitwiseOperation operation)
            {
                Flags flags = new Flags();
                flags.Zero = ((byte)result == 0x00);
                flags.Sign = (((sbyte)result) < 0);
                flags.ParityOverflow = (((byte)result).CountBits(true) % 2 == 0);
                flags.HalfCarry = false;
                flags.Subtract = false;
                flags.Carry = ((byte)value).GetBit(7);
                return flags;
            }
        }

        public static Flags FlagsFromArithmeticOperation8(byte startingValue, int addOrSubtractValue, bool carry, bool subtract)
        {
            return new Flags((subtract ? 
                                _subFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0] : 
                                _addFlags8[startingValue, addOrSubtractValue, carry ? 1 : 0]
                                ));
        }
        public static Flags FlagsFromLogicalOperation(byte first, byte second, LogicalOperation operation)
        {
            return new Flags(_logicalFlags[first, second, (int)operation]);
        }

        public static Flags FlagsFromBitwiseOperation(byte value, BitwiseOperation operation)
        {
            return new Flags(_bitwiseFlags[value, (int)operation]);
        }

        // the state space of 16 x 16 bit numbers is too large to pre-calculate the flags in a reasonable time
        // TODO: run the code to completion and store output in a file?
        public static Flags FlagsFromArithmeticOperation16(Flags flags, ushort startingValue, int addOrSubtractValue, bool carry, bool setSignZeroParityOverflow, bool subtract) 
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

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z80.Core;

namespace FlagSetMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            FlagLookup.BuildFlagLookupTables();

            #region done
            //ProcessInstruction<byte>("ADC A,n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation(i, j, carry, false).State,
            //    (i, j, carry) => (byte)(i + j + (carry ? 1 : 0)),
            //    true, false);

            ////ProcessInstruction("ADD A,n",
            ////    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation(i, j, false).State,
            ////    (i, j, carry) => (byte)(i + j),
            ////    false, false);

            //ProcessArithmetic8("SBC A,n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation(i, j, carry, true).State,
            //    (i, j, carry) => (byte)(i - j - (carry ? 1 : 0)),
            //    true, true);

            //ProcessArithmetic16("SBC HL,rr",
            //    (flags, i, j, carry) => FlagLookup.FlagsFromArithmeticOperation16Bit(flags, i, j, carry, true, true).State,
            //    (i, j, carry) => (ushort)(i - j - (carry ? 1 : 0)),
            //    true, true);

            //ProcessArithmetic16("ADC HL,rr",
            //    (flags, i, j, carry) => FlagLookup.FlagsFromArithmeticOperation16Bit(flags, i, j, carry, true, false).State,
            //    (i, j, carry) => (ushort)(i + j + (carry ? 1 : 0)),
            //    true);

            //ProcessInstruction("SUB A,n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation(i, j, true).State,
            //    (i, j, carry) => (byte)(i - j),
            //    false, true);

            //Process8("CP n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation8(i, (byte)j, false, true).State,
            //    (i, j, carry) => (byte)(i - j),
            //    false);

            //Process8("DEC n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation8(i, (byte)1, false, true).State,
            //    (i, j, carry) => (byte)(i - 1),
            //    false);

            //Process8("AND n",
            //    (i, j, carry) => FlagLookup.FlagsFromLogicalOperation(i, (byte)j, LogicalOperation.And).State,
            //    (i, j, carry) => (byte)(i & j),
            //    false);

            //Process8("OR n",
            //    (i, j, carry) => FlagLookup.FlagsFromLogicalOperation(i, (byte)j, LogicalOperation.Or).State,
            //    (i, j, carry) => (byte)(i | j),
            //    false);

            //Process8("XOR n",
            //    (i, j, carry) => FlagLookup.FlagsFromLogicalOperation(i, (byte)j, LogicalOperation.Xor).State,
            //    (i, j, carry) => (byte)(i ^ j),
            //    false);

            //Process8("INC n",
            //    (i, j, carry) => FlagLookup.FlagsFromArithmeticOperation8(i, (byte)1, false, false).State,
            //    (i, j, carry) => (byte)(i + 1),
            //    false);

            //Process8("RL",
            //    (i, j, carry) => FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.RotateLeft).State,
            //    (i, j, carry) => ((byte)(i << 1)).SetBit(0, carry),
            //    true);

            //Process8("RR",
            //    (i, j, carry) => FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.RotateRight).State,
            //    (i, j, carry) => ((byte)(i >> 1)).SetBit(7, carry),
            //    true);

            //Process8("RLC",
            //    (i, j, carry) => {
            //        Flags flags = FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.RotateLeft);
            //        flags.HalfCarry = false;
            //        flags.Subtract = false;
            //        return flags.State;
            //        },
            //    (i, j, carry) => ((byte)(i << 1)).SetBit(0, i.GetBit(7)),
            //    true);

            //Process8("RRC",
            //    (i, j, carry) => {
            //        Flags flags = FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.RotateRight);
            //        flags.HalfCarry = false;
            //        flags.Subtract = false;
            //        return flags.State;
            //    },
            //    (i, j, carry) => ((byte)(i >> 1)).SetBit(7, i.GetBit(0)),
            //    true);

            Process8("SLA",
                (i, j, carry) =>
                {
                    Flags flags = FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.ShiftLeft);
                    flags.HalfCarry = false;
                    flags.Subtract = false;
                    return flags;
                },
                (i, j, carry, flags) => { byte result = ((byte)(i << 1)); flags.Carry = result.GetBit(7); return result; },
                false, false, true);

            Process8("SRA",
                (i, j, carry) =>
                {
                    Flags flags = FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.ShiftRight);
                    flags.HalfCarry = false;
                    flags.Subtract = false;
                    return flags;
                },
                (i, j, carry, flags) => { byte result = ((byte)(i >> 1)).SetBit(7, i.GetBit(7)); flags.Carry = result.GetBit(0); return result; },
                false, false, true);

            Process8("SRL",
                (i, j, carry) =>
                {
                    Flags flags = FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.ShiftRight);
                    flags.HalfCarry = false;
                    flags.Subtract = false;
                    return flags;
                },
                (i, j, carry, flags) => { byte result = ((byte)(i >> 1)).SetBit(7, false); flags.Carry = result.GetBit(0); return result; },
                false, false, true);

            #endregion
        }

        static void Process8(string instruction, Func<byte, int, bool, Flags> getFlags, Func<byte, int, bool, Flags, int> getResult, bool useCarry, bool includeOperand = true, bool includeResult = true)
        {
            List<string> output = new List<string>();

            IDictionary<FlagState, (IList<string>, IList<string>)> log = new Dictionary<FlagState, (IList<string>, IList<string>)>();
            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 255; j++)
                {
                    foreach (bool carry in new bool[] { true, false })
                    {
                        if ((carry && useCarry) || !carry)
                        {
                            Flags flags = getFlags((byte)i, j, carry);
                            int result = getResult((byte)i, j, carry, flags);

                            if (!log.ContainsKey(flags.State))
                            {
                                log.Add(flags.State, (new List<string>(), new List<string>()));
                            }
                            IList<string> listToAddTo = carry ? log[flags.State].Item1 : log[flags.State].Item2;
                            listToAddTo.Add($"0x{i.ToString("X2")}, {(includeOperand ? $"0x{j.ToString("X2")}, " : "")}{(useCarry ? (carry.ToString().ToLower() + ", ") : "")}{(includeResult? $"0x{ result.ToString("X2")}, " : "")}FlagState.{flags.State.ToString().Replace(",", " | FlagState.")}");
                        }
                    }
                }
            }

            ProcessLog(instruction, log, output);
        }

        static void Process16(string instruction, Func<Flags, ushort, int, bool, FlagState> getFlags, Func<ushort, int, bool, int> getResult, bool useCarry)
        {
            List<string> output = new List<string>();

            IDictionary<FlagState, (IList<string>, IList<string>)> log = new Dictionary<FlagState, (IList<string>, IList<string>)>();
            for (int i = 0; i <= 65535; i+= 256)
            {
                for (int j = 0; j <= 65535; j++)
                {
                    foreach (bool carry in new bool[] { true, false })
                    {
                        if ((carry && useCarry) || !carry)
                        {
                            FlagState state = getFlags(new Flags(), (ushort)i, j, carry);

                            if (!log.ContainsKey(state))
                            {
                                log.Add(state, (new List<string>(), new List<string>()));
                                IList<string> listToAddTo = carry ? log[state].Item1 : log[state].Item2;
                                var result = getResult((ushort)i, j, carry);
                                listToAddTo.Add($"0x{i.ToString("X4")}, 0x{j.ToString("X4")}, {(useCarry ? (carry.ToString().ToLower() + ", ") : "")}0x{result.ToString("X4")}, FlagState.{state.ToString().Replace(",", " | FlagState.")}");
                            }
                        }
                    }
                }
            }

            ProcessLog(instruction, log, output);
        }

        private static void ProcessLog(string instruction, IDictionary<FlagState, (IList<string>, IList<string>)> log, List<string> output)
        {
            foreach (FlagState key in log.Keys.OrderBy(x => x))
            {
                (IList<string> @true, IList<string> @false) = log[key];
                logFirstEntryFromList(@true);
                logFirstEntryFromList(@false);

                void logFirstEntryFromList(IList<string> list)
                {
                    if (list.Count > 0)
                    {
                        string entry = $"[TestCase({list.First()})]";
                        Console.WriteLine(entry);
                        output.Add(entry);
                    }
                }
            }

            File.WriteAllLines("..\\..\\..\\" + instruction + ".txt", output);
        }
    }
}

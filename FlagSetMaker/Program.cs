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
            
            //Process8("RR",
            //    (i, j, carry) => FlagLookup.FlagsFromBitwiseOperation(i, BitwiseOperation.RotateRight).State,
            //    (i, j, carry) => ((byte)(i >> 1)).SetBit(7, carry),
            //    true);

            //Process16("ADC HL,rr",
            //    (flags, i, j, carry) => FlagLookup.WordArithmeticFlags(flags, i, j, carry, true, false).State,
            //    (i, j, carry) => (ushort)(i + j + (carry ? 1 : 0)),
            //    true);

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

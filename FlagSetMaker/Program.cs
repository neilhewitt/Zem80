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

            //ProcessInstruction("AND n",
            //    (i, j, carry) => FlagLookup.FlagsFromLogicalOperation(i, j, LogicalOperation.And).State,
            //    (i, j, carry) => (byte)(i & j),
            //    false, true);
            #endregion


        }

        static void ProcessArithmetic8(string instruction, Func<byte, int, bool, FlagState> getFlags, Func<byte, int, bool, int> getResult, bool useCarry)
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
                            FlagState state = getFlags((byte)i, j, carry);

                            if (!log.ContainsKey(state))
                            {
                                log.Add(state, (new List<string>(), new List<string>()));
                            }
                            IList<string> listToAddTo = carry ? log[state].Item1 : log[state].Item2;
                            listToAddTo.Add($"0x{i.ToString("X2")}, 0x{j.ToString("X2")}, {(useCarry ? (carry.ToString().ToLower() + ", ") : "")}0x{getResult((byte)i, j, carry).ToString("X2")}, FlagState.{state.ToString().Replace(",", " | FlagState.")}");
                        }
                    }
                }
            }

            ProcessLog(instruction, log, output);
        }

        static void ProcessArithmetic16(string instruction, Func<Flags, ushort, int, bool, FlagState> getFlags, Func<ushort, int, bool, int> getResult, bool useCarry)
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
                            if (i == 256 && j == 32512) i = i;
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

        static ExecutionResult ExecuteInstruction(IDebugProcessor processor, string mnemonic, byte? arg1 = null, byte? arg2 = null)
        {
            Instruction instruction = Instruction.FindByMnemonic(mnemonic);
            InstructionData data = new InstructionData()
            {
                Argument1 = arg1 ?? 0,
                Argument2 = arg2 ?? 0
            };

            ExecutionResult result = processor.Execute(new ExecutionPackage(instruction, data)); // only available on IDebugProcessor debug interface - sets flags but does not advance PC
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using Z80.Core;
using System.Collections;
using System.IO;

namespace ZexallCSharp
{
    public class ZexTest
    {
        protected Dictionary<string, TestDescriptor> _tests;
        protected byte[][] _crcTable;
        protected Func<TestVector, TestState> _executeTest;

        public IEnumerable<TestDescriptor> Tests => _tests.Values;

        public IReadOnlyDictionary<string, bool> RunAll()
        {
            IDictionary<string, bool> results = new Dictionary<string, bool>();
            foreach (TestDescriptor test in _tests.Values)
            {
                results.Add(test.Message, RunTest(test));
            }

            return results as IReadOnlyDictionary<string, bool>;
        }

        public bool RunTest(TestDescriptor descriptor)
        {
            // start with base case
            // apply change for each 1 bit in increment vector
            // for each change above, apply change for each 1 bit in shift vector
            // this is the total test case set for this test

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(descriptor.Name.PadRight(10));

            byte[] crc = new byte[4] { 255, 255, 255, 255 };

            InstructionDecoder decoder = new InstructionDecoder();

            var tests = MakeTestSet(descriptor);
            int testsDone = 0;
            foreach (var test in tests)
            {
                var package = decoder.Decode(test.Instruction.AsByteArray());
                TestState state = _executeTest(test);
                if (state != null)
                {
                    state.F = (byte)(state.F & descriptor.Mask);

                    foreach (byte b in new byte[] { test.MemOp.LowByte(), test.MemOp.HighByte() }.Union(state.Bytes))
                    {
                        byte xor = (byte)(crc[3] ^ b);
                        byte[] lookupCRC = _crcTable[xor];

                        crc[0] = (byte)(lookupCRC[0] ^ 0);
                        crc[1] = (byte)(lookupCRC[1] ^ crc[0]);
                        crc[2] = (byte)(lookupCRC[2] ^ crc[1]);
                        crc[3] = (byte)(lookupCRC[3] ^ crc[2]);
                    }

                    testsDone++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" INVALID");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false; // fail out immediately
                }

                Console.SetCursorPosition(0, 2);
                Console.WriteLine(descriptor.Message + " ----------->");
                Console.WriteLine("Instruction: " + test.Instruction.First + " " + test.Instruction.Second + " ("
                    + package.Instruction.Mnemonic + ") " + package.Data.Argument1.ToString("X2") + " " + package.Data.Argument2.ToString("X2"));
                Console.WriteLine("MemOp: " + test.MemOp.ToString("X4"));
                Console.WriteLine("IY: " + test.IY.ToString("X4"));
                Console.WriteLine("IX: " + test.IX.ToString("X4"));
                Console.WriteLine("HL: " + test.HL.ToString("X4"));
                Console.WriteLine("DE: " + test.DE.ToString("X4"));
                Console.WriteLine("BC: " + test.BC.ToString("X4"));
                Console.WriteLine("F: " + test.F.ToString("X2"));
                Console.WriteLine("A: " + test.A.ToString("X2"));
                Console.WriteLine("SP: " + test.SP.ToString("X4"));
                Console.WriteLine();
                Console.WriteLine("Done: " + testsDone);

                #region console-output-for-test
                //Console.SetCursorPosition(0, 2);
                //Console.WriteLine(descriptor.Message + " ----------->");
                //Console.WriteLine("Instruction: " + test.Instruction.First + " " + test.Instruction.Second + " ("
                //    + package.Instruction.Mnemonic + ") " + package.Data.Argument1.ToString("X2") + " " + package.Data.Argument2.ToString("X2"));
                //Console.WriteLine("MemOp: " + test.MemOp.ToString("X4"));
                //Console.WriteLine("IY: " + test.IY.ToString("X4"));
                //Console.WriteLine("IX: " + test.IX.ToString("X4"));
                //Console.WriteLine("HL: " + test.HL.ToString("X4"));
                //Console.WriteLine("DE: " + test.DE.ToString("X4"));
                //Console.WriteLine("BC: " + test.BC.ToString("X4"));
                //Console.WriteLine("F: " + test.F.ToString("X2"));
                //Console.WriteLine("A: " + test.A.ToString("X2"));
                //Console.WriteLine("SP: " + test.SP.ToString("X4"));
                //Console.WriteLine();
                //Console.WriteLine("Done: " + ++index);
                #endregion
            }

            (byte one, byte two, byte three, byte four) expectedCRC = descriptor.CRC;
            (byte one, byte two, byte three, byte four) testCRC = (crc[0], crc[1], crc[2], crc[3]);

            if (expectedCRC != testCRC)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" CRC CHECK FAILED");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" CRC CHECK OK");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return false;
        }

        private void LoadRegisters(TestVector test)
        {
            throw new NotImplementedException();
        }

        private IList<TestVector> MakeTestSet(TestDescriptor descriptor)
        {
            var baseCase = descriptor.Base;
            var increment = descriptor.Increment;
            var shift = descriptor.Shift;

            List<TestVector> incrementCases = new List<TestVector>();
            Increment(incrementCases, GetIncrementPermutations(baseCase.Bytes[0], increment.Bytes[0]), 0, baseCase, increment);
            
            List<TestVector> shiftCases = new List<TestVector>();
            Shift(shiftCases, incrementCases, shift, descriptor.Mask, 0);

            return shiftCases;
        }

        private void Increment(IList<TestVector> cases, IList<byte> permutations, int index, TestVector baseCase, TestVector increment)
        {
            if (increment.Bytes.All(b => b == 0))
            {
                cases.Add(baseCase);
                return;
            }

            int lastIndexNotZero = 0;
            for (int i = 0; i < 20; i++)
            {
                if (increment.Bytes[i] != 0)
                {
                    lastIndexNotZero = i;
                }
            }

            if (index < 20)
            {
                foreach (byte permutation in permutations)
                {
                    TestVector newCase = null;
                    newCase = new TestVector(baseCase);
                    newCase.Bytes[index] = (byte)(baseCase.Bytes[index] ^ permutation);
                    baseCase = newCase;
                    if (index == lastIndexNotZero)
                    {
                        cases.Add(newCase);
                    }
                    if (index < 19) Increment(cases, GetIncrementPermutations(baseCase.Bytes[index + 1], increment.Bytes[index + 1]), index + 1, newCase, increment);
                }

                if (permutations.Count == 0)
                {
                    if (index < 19) Increment(cases, GetIncrementPermutations(baseCase.Bytes[index + 1], increment.Bytes[index + 1]), index + 1, baseCase, increment);
                }
            }
        }

        private IList<byte> GetIncrementPermutations(byte baseValue, byte incrementValue)
        {
            List<byte> permutations = new List<byte>();

            byte current = 0;

            for (int i = 0; i <= incrementValue; i++)
            {
                byte and = (byte)(incrementValue & i);
                if (and != current)
                {
                    current = and;
                    byte currentPermutation = (byte)(current);
                    permutations.Add(currentPermutation);
                }
            }

            if (permutations.Count() > 0) permutations.Add(0);
            return permutations.Distinct().ToList();
        }

        private void Shift(List<TestVector> shiftCases, IList<TestVector> incrementCases, TestVector shift, byte mask, int valueIndex)
        {
            if (shift.Bytes.All(x => x == 0))
            {
                shiftCases.AddRange(incrementCases);
                return;
            }

            if (valueIndex > 19) return;

            foreach (TestVector incrementCase in incrementCases)
            {
                dynamic baseValue = incrementCase.GetValue(valueIndex);
                dynamic shiftValue = shift.GetValue(valueIndex);

                int bitIndex = (shiftValue is byte) ? 7 : 15;
                while (shiftValue > 0)
                {
                    if (shiftValue is byte)
                    {
                        byte s = (byte)shiftValue;
                        if (s.GetBit(bitIndex))
                        {
                            TestVector newVector = new TestVector(incrementCase);
                            newVector.SetValue(valueIndex, baseValue ^ shiftValue);
                            shiftCases.Add(newVector);
                            baseValue = shiftValue;
                        }
                        s = s.SetBit(bitIndex--, false);
                        shiftValue = s;
                    }
                    else if (shiftValue is ushort)
                    {
                        ushort s = (ushort)shiftValue;
                        if (s == 65535 && shift.MemOp == 0 && shift.F != 0) s = 255;
                        if (s.GetBit(bitIndex))
                        {
                            TestVector newVector = new TestVector(incrementCase);
                            newVector.SetValue(valueIndex, baseValue ^ shiftValue);
                            shiftCases.Add(newVector);
                        }
                        s = s.SetBit(bitIndex--, false);
                        shiftValue = s;
                    }
                }
            }

            Shift(shiftCases, incrementCases, shift, mask, valueIndex + 1);
        }

        public ZexTest(IZexTestSource testSource, Func<TestVector, TestState> executeTest)
        {
            _tests = new Dictionary<string, TestDescriptor>(testSource.Tests);
            _crcTable = testSource.CRCTable;
            _executeTest = executeTest;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public static class TestCases
    {
        public static IEnumerable<RegisterPair> GetRegisterPairs()
        {
            // get all register pairs that are assignable with LD
            return new List<RegisterPair>()
            {
                RegisterPair.BC,
                RegisterPair.DE,
                RegisterPair.HL,
                RegisterPair.IX,
                RegisterPair.IY,
                RegisterPair.SP
            };
        }

        public static IEnumerable<RegisterPair> GetRegisterPairs_PUSH_POP() // special case
        {
            // get all register pairs that can be PUSHed or POPped
            return new List<RegisterPair>()
            {
                RegisterPair.AF,
                RegisterPair.BC,
                RegisterPair.DE,
                RegisterPair.HL,
                RegisterPair.IX,
                RegisterPair.IY
            };
        }


        public static IEnumerable<Register> GetRegisters()
        {
            // generate a list of registers excluding F (note 6 == RegisterIndex.None and has to be omitted)
            IList<Register> cases = new System.Collections.Generic.List<Register>();
            for (int i = 0; i <= 7; i++)
            {
                Register register = (Register)i;
                if (i != 6)
                {
                    cases.Add(register);
                }
            }

            return cases;
        }

        public static IEnumerable<object[]> GetRegistersAndBits()
        {
            // generate a matrix of each register (A-H without F) plus each bit index (0-6)
            // for bit setting operations
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                if (i != 6)
                {
                    Register register = (Register)i;
                    for (int j = 0; j <= 6; j++)
                    {
                        cases.Add(new object[] { register, j });
                    }
                }
            }

            return cases;
        }

        public static IEnumerable<object[]> GetRegisterPairings()
        {
            // generate a matrix of each register (A-H without F) versus each register (same)
            // for LD register, register operations like LD B,H et
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                Register left = (Register)i;
                for (int j = 0; j <= 7; j++)
                {
                    Register right = (Register)j;
                    if (i != 6 && j != 6)
                    {
                        cases.Add(new object[] { left, right });
                    }
                }
            }

            return cases;
        }

        public static IEnumerable<object[]> GetRegisterAndIndexPairings()
        {
            // get a list of pairs of registers + one of IX and IY, for LD A,IX or LD D,(IX+offset) etc
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                Register register = (Register)i;
                if (i != 6)
                {
                    cases.Add(new object[] { register, RegisterPair.IX });
                    cases.Add(new object[] { register, RegisterPair.IY });
                }
            }

            return cases;
        }

        public static IEnumerable<RegisterPair> GetIndexRegisters()
        {
            // get the Index register names (for indexed operations)
            return new List<RegisterPair>() { RegisterPair.IX, RegisterPair.IY };
        }

        public static IEnumerable<(string, Func<IFlags>, Func<IFlags, bool>)> GetConditions()
        {
            yield return ("Z", () => new Flags() { Zero = true }, (IFlags flags) => flags.Zero);
            yield return ("NZ", () => new Flags() { Zero = false }, (IFlags flags) => !flags.Zero);
            yield return ("C", () => new Flags() { Carry = true }, (IFlags flags) => flags.Carry);
            yield return ("NC", () => new Flags() { Carry = false }, (IFlags flags) => !flags.Carry);
            yield return ("PE", () => new Flags() { ParityOverflow = true }, (IFlags flags) => flags.ParityOverflow);
            yield return ("PO", () => new Flags() { ParityOverflow = false }, (IFlags flags) => !flags.ParityOverflow);
            yield return ("M", () => new Flags() { Sign = true }, (IFlags flags) => flags.Sign);
            yield return ("P", () => new Flags() { Sign = false }, (IFlags flags) => !flags.Sign);
        }

        public static IEnumerable<(string, Func<IFlags>, Func<IFlags, bool>)> GetZeroAndCarryConditions()
        {
            yield return ("Z", () => new Flags() { Zero = true }, (IFlags flags) => flags.Zero);
            yield return ("NZ", () => new Flags() { Zero = false }, (IFlags flags) => !flags.Zero);
            yield return ("C", () => new Flags() { Carry = true }, (IFlags flags) => flags.Carry);
            yield return ("NC", () => new Flags() { Carry = false }, (IFlags flags) => !flags.Carry);
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public static class LD_TestCases
    {
        public static IEnumerable<RegisterPairName> GetRegisterPairs()
        {
            // get all register pairs that are assignable with LD
            return new List<RegisterPairName>()
            {
                RegisterPairName.BC,
                RegisterPairName.DE,
                RegisterPairName.HL,
                RegisterPairName.IX,
                RegisterPairName.IY,
                RegisterPairName.SP
            };
        }

        public static IEnumerable<RegisterPairName> GetRegisterPairs_PUSH_POP() // special case
        {
            // get all register pairs that can be PUSHed or POPped
            return new List<RegisterPairName>()
            {
                RegisterPairName.AF,
                RegisterPairName.BC,
                RegisterPairName.DE,
                RegisterPairName.HL,
                RegisterPairName.IX,
                RegisterPairName.IY
            };
        }


        public static IEnumerable<RegisterName> GetRegisters()
        {
            // generate a list of registers excluding F (note 6 == RegisterIndex.None and has to be omitted)
            IList<RegisterName> cases = new System.Collections.Generic.List<RegisterName>();
            for (int i = 0; i <= 7; i++)
            {
                RegisterName register = (RegisterName)i;
                if (i != 6)
                {
                    cases.Add(register);
                }
            }

            return cases;
        }

        public static IEnumerable<RegisterName> Plus(this IEnumerable<RegisterName> registers, RegisterName name) => registers.Append(name);

        public static IEnumerable<object[]> GetRegistersAndBits()
        {
            // generate a matrix of each register (A-H without F) plus each bit index (0-6)
            // for bit setting operations
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                if (i != 6)
                {
                    RegisterName register = (RegisterName)i;
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
                RegisterName left = (RegisterName)i;
                for (int j = 0; j <= 7; j++)
                {
                    RegisterName right = (RegisterName)j;
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
                RegisterName register = (RegisterName)i;
                if (i != 6)
                {
                    cases.Add(new object[] { register, RegisterPairName.IX });
                    cases.Add(new object[] { register, RegisterPairName.IY });
                }
            }

            return cases;
        }

        public static IEnumerable<RegisterPairName> GetIndexRegisters()
        {
            // get the Index register names (for indexed operations)
            return new List<RegisterPairName>() { RegisterPairName.IX, RegisterPairName.IY };
        }

        public static IEnumerable<(string, Func<Flags>, Func<Flags, bool>)> GetConditions()
        {
            yield return ("Z", () => new Flags() { Zero = true }, (Flags flags) => flags.Zero);
            yield return ("NZ", () => new Flags() { Zero = false }, (Flags flags) => !flags.Zero);
            yield return ("C", () => new Flags() { Carry = true }, (Flags flags) => flags.Carry);
            yield return ("NC", () => new Flags() { Carry = false }, (Flags flags) => !flags.Carry);
            yield return ("PE", () => new Flags() { ParityOverflow = true }, (Flags flags) => flags.ParityOverflow);
            yield return ("PO", () => new Flags() { ParityOverflow = false }, (Flags flags) => !flags.ParityOverflow);
            yield return ("M", () => new Flags() { Sign = true }, (Flags flags) => flags.Sign);
            yield return ("P", () => new Flags() { Sign = false }, (Flags flags) => !flags.Sign);
        }

        public static IEnumerable<(string, Func<Flags>, Func<Flags, bool>)> GetZeroAndCarryConditions()
        {
            yield return ("Z", () => new Flags() { Zero = true }, (Flags flags) => flags.Zero);
            yield return ("NZ", () => new Flags() { Zero = false }, (Flags flags) => !flags.Zero);
            yield return ("C", () => new Flags() { Carry = true }, (Flags flags) => flags.Carry);
            yield return ("NC", () => new Flags() { Carry = false }, (Flags flags) => !flags.Carry);
        }
    }
}

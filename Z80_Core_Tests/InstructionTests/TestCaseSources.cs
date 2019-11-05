using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public static class TestCases
    {
        public static IEnumerable<RegisterPairIndex> GetRegisterPairs()
        {
            // get all register pairs that are assignable with LD
            return new List<RegisterPairIndex>()
            {
                RegisterPairIndex.BC,
                RegisterPairIndex.DE,
                RegisterPairIndex.HL,
                RegisterPairIndex.IX,
                RegisterPairIndex.IY,
                RegisterPairIndex.SP
            };
        }

        public static IEnumerable<RegisterIndex> GetRegisters()
        {
            // generate a list of registers excluding F (note 6 == RegisterIndex.None and has to be omitted)
            IList<RegisterIndex> cases = new System.Collections.Generic.List<RegisterIndex>();
            for (int i = 0; i <= 7; i++)
            {
                RegisterIndex index = (RegisterIndex)i;
                if (i != 6)
                {
                    cases.Add(index);
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
                RegisterIndex left = (RegisterIndex)i;
                for (int j = 0; j <= 7; j++)
                {
                    RegisterIndex right = (RegisterIndex)j;
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
                RegisterIndex index = (RegisterIndex)i;
                if (i != 6)
                {
                    cases.Add(new object[] { index, RegisterPairIndex.IX });
                    cases.Add(new object[] { index, RegisterPairIndex.IY });
                }
            }

            return cases;
        }

        public static IEnumerable<RegisterPairIndex> GetIndexRegisters()
        {
            // get the Index register names (for indexed operations)
            return new List<RegisterPairIndex>() { RegisterPairIndex.IX, RegisterPairIndex.IY };
        }
    }
}

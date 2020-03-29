using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public static class LD_TestCases
    {
        public static IEnumerable<WordRegister> GetWordRegisters()
        {
            // get all register pairs that are assignable with LD
            return new List<WordRegister>()
            {
                WordRegister.BC,
                WordRegister.DE,
                WordRegister.HL,
                WordRegister.IX,
                WordRegister.IY,
                WordRegister.SP
            };
        }

        public static IEnumerable<ByteRegister> GetByteRegisters()
        {
            // generate a list of registers excluding F (note 6 == RegisterIndex.None and has to be omitted)
            IList<ByteRegister> cases = new System.Collections.Generic.List<ByteRegister>();
            for (int i = 0; i <= 7; i++)
            {
                ByteRegister register = (ByteRegister)i;
                if (i != 6)
                {
                    cases.Add(register);
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
                ByteRegister left = (ByteRegister)i;
                for (int j = 0; j <= 7; j++)
                {
                    ByteRegister right = (ByteRegister)j;
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
                ByteRegister register = (ByteRegister)i;
                if (i != 6)
                {
                    cases.Add(new object[] { register, WordRegister.IX });
                    cases.Add(new object[] { register, WordRegister.IY });
                }
            }

            return cases;
        }

        public static IEnumerable<WordRegister> GetIndexRegisters()
        {
            // get the Index register names (for indexed operations)
            return new List<WordRegister>() { WordRegister.IX, WordRegister.IY };
        }
    }
}

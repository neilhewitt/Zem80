using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_IND_INI_INDR_INI : InstructionTestBase
    {
        [TestCase("IND")]
        [TestCase("INI")]
        [TestCase("INDR")]
        [TestCase("INIR")]
        public void IND_INI_INDR_INIR(string instruction)
        {
            bool repeats = instruction == "INIR" || instruction == "INDR";

            PortSignal signal;
            byte count = repeats ? (byte)(RandomByte(254) + 1) : (byte)1; // it's always at least 1
            byte[] data = Enumerable.Range(0, count).Select(x => RandomByte()).ToArray();
            ushort address = RandomWord((ushort)(65535 - count));

            int index = 0;
            Func<byte> reader = () => data[index++];
            Action<byte> writer = null;
            Action<PortSignal> signaller = (s) => { signal = s; };
            _cpu.Ports[0].Connect(reader, writer, signaller);

            Registers.HL = address;
            Registers.C = 0;
            Registers.B = count;

            while (Registers.B > 0)
            {
                Execute(instruction);
            }

            bool addressCorrect = Registers.HL == (repeats ? (instruction == "INIR" ? address + count : address - count) :
                                                             (instruction == "INI" ? address + 1 : address - 1));

            // address is 1 byte offset from the data now, so put it back
            if (instruction.StartsWith("INI")) Registers.HL--; else Registers.HL++;

            byte[] checkData = _cpu.Memory.ReadBytesAt(Registers.HL, count).Reverse().ToArray(); // bytes are in reverse order in RAM vs input

            Assert.That(addressCorrect && Registers.B == 0 && data.SequenceEqual(checkData) &&
                repeats ? TestFlags(zero: true, subtract: true) : true);
        }
    }
}
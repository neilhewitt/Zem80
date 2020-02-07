using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_OUTD_OUTI_OTDR_OTIR : InstructionTestBase
    {
        [TestCase("OUTD")]
        [TestCase("OUTI")]
        [TestCase("OTDR")]
        [TestCase("OTIR")]
        public void OUTD_OUTI_OTDR_OTIR(string instruction)
        {
            bool repeats = instruction == "OTIR" || instruction == "OTDR";

            PortSignal signal;
            byte count = repeats ? (byte)(RandomByte(254) + 1) : (byte)1; // it's always at least 1
            byte[] data = new byte[count]; // port will store data here for checking
            ushort address = RandomWord((ushort)(65535 - count));

            int index = 0;
            Func<byte> reader = () => 0;
            Action<byte> writer = (value) => data[index++] = value;
            Action<PortSignal> signaller = (s) => { signal = s; };
            _cpu.Ports[0].Connect(reader, writer, signaller);

            Registers.HL = address;
            Registers.C = 0;
            Registers.B = count;

            while (Registers.B > 0)
            {
                Execute(instruction);
            }

            bool addressCorrect = Registers.HL == (repeats ? (instruction == "OTIR" ? address + count : address - count) :
                                                             (instruction == "OUTI" ? address + 1 : address - 1));

            // address is 1 byte offset from the data now, so put it back
            if (instruction.StartsWith("OUTI")) Registers.HL--; else Registers.HL++;

            byte[] checkData = _cpu.Memory.ReadBytesAt(Registers.HL, count).Reverse().ToArray(); // bytes are in reverse order in RAM vs output

            Assert.That(addressCorrect && Registers.B == 0 && data.SequenceEqual(checkData) &&
                repeats ? TestFlags(zero: true, subtract: true) : true);
        }
    }
}
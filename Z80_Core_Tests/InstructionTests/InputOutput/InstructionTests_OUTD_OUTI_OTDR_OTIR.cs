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
        private int _index;

        private void SetupPort(byte portNumber, byte[] data, ushort bufferAddress)
        {
            _index = 0;
            Func<byte> reader = () => 0;
            Action<byte> writer = (value) => data[_index++] = value;
            Action signaller = () => { };
            CPU.Ports[0x00].Connect(reader, writer, signaller, signaller);
        }

        private void SetupBuffer(ushort bufferAddress, byte bufferLength, int offset)
        {
            Registers.HL = bufferAddress; // location of the buffer
            Registers.C = 0x00; // we'll use Port 0
            Registers.B = bufferLength; // size of the buffer
            CPU.Memory.WriteBytesAt((ushort)(bufferAddress + offset), RandomBytes(bufferLength));
        }

        private (byte[] data, ushort expectedValueInHL) GetDataOutputAndExpectedValueInHL(int bufferSize, ushort bufferAddress, WriteDirection direction, WriteRepeats repeats)
        {
            ushort expectedValueInHL = repeats == WriteRepeats.Repeating ?
                                        (direction == WriteDirection.Incrementing ? (ushort)(bufferAddress + bufferSize) : (ushort)(bufferAddress - bufferSize)) :
                                        (direction == WriteDirection.Incrementing ? (ushort)(bufferAddress + 1) : (ushort)(bufferAddress - 1));

            // where is the data? 
            // OUTI - at the initial buffer address
            // OUTD - at the initial buffer address
            // OTIR - at the initial buffer address
            // OTDR - at initial buffer address - buffer length
            ushort checkAddress = (direction, repeats) switch
            {
                (WriteDirection.Decrementing, WriteRepeats.Repeating) => (ushort)(bufferAddress - bufferSize + 1),
                _ => bufferAddress
            };

            return (
                CPU.Memory.ReadBytesAt(checkAddress, (ushort)bufferSize),
                expectedValueInHL
                );
        }

        [Test]
        public void OUTI()
        {
            byte[] data = new byte[0x01];
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0x01, 0x00);

            ExecutionResult executionResult = ExecuteInstruction("OUTI");
            (byte[] checkData, ushort expectedValueInHL) = GetDataOutputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Incrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void OUTD()
        {
            byte[] data = new byte[0x01];
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0x01, 0x00);

            ExecutionResult executionResult = ExecuteInstruction("OUTD");
            (byte[] checkData, ushort expectedValueInHL) = GetDataOutputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Decrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void OTIR()
        {
            byte[] data = new byte[0xFF];
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0xFF, 0x00);

            while (Registers.B > 0) ExecuteInstruction("OTIR");
            (byte[] checkData, ushort expectedValueInHL) = GetDataOutputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Incrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void OTDR()
        {
            byte[] data = new byte[0xFF];
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0xFF, -0xFF); // offset the address by -0xFF because bytes are read in reverse order

            while (Registers.B > 0) ExecuteInstruction("OTDR");
            (byte[] checkData, ushort expectedValueInHL) = GetDataOutputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Decrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data.Reverse()));
        }

        //[TestCase("OUTD")]
        //[TestCase("OUTI")]
        //[TestCase("OTDR")]
        //[TestCase("OTIR")]
        //public void OUTD_OUTI_OTDR_OTIR(string instruction)
        //{
        //    bool repeats = instruction == "OTIR" || instruction == "OTDR";

        //    PortSignal signal;
        //    byte count = repeats ? (byte)(RandomByte(254) + 1) : (byte)1; // it's always at least 1
        //    byte[] data = new byte[count]; // port will store data here for checking
        //    ushort address = RandomWord((ushort)(65535 - count));

        //    int index = 0;
        //    Func<byte> reader = () => 0;
        //    Action<byte> writer = (value) => data[index++] = value;
        //    Action<PortSignal> signaller = (s) => { signal = s; };
        //    CPU.Ports[0].Connect(reader, writer, signaller);

        //    Registers.HL = address;
        //    Registers.C = 0;
        //    Registers.B = count;

        //    while (Registers.B > 0)
        //    {
        //        ExecutionResult executionResult = ExecuteInstruction(instruction);
        //    }

        //    bool addressCorrect = Registers.HL == (repeats ? (instruction == "OTIR" ? address + count : address - count) :
        //                                                     (instruction == "OUTI" ? address + 1 : address - 1));

        //    // address is 1 byte offset from the data now, so put it back
        //    if (instruction.StartsWith("OUTI")) Registers.HL--; else Registers.HL++;

        //    byte[] checkData = CPU.Memory.ReadBytesAt(Registers.HL, count).Reverse().ToArray(); // bytes are in reverse order in RAM vs output

        //    Assert.That(addressCorrect && Registers.B == 0 && data.SequenceEqual(checkData) &&
        //        repeats ? CompareWithCPUFlags(zero: true, subtract: true) : true);
        //}
    }
}
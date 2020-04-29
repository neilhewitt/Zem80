using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_IND_INI_INDR_INIR : InstructionTestBase
    {
        private int _index;

        private void SetupPort(byte portNumber, byte[] data, ushort bufferAddress)
        {
            _index = 0;
            Func<byte> reader = () => data[_index++];
            Action<byte> writer = null;
            Action signaller = () => { };
            CPU.Ports[0x00].Connect(reader, writer, signaller, signaller);
        }

        private void SetupBuffer(ushort bufferAddress, byte bufferLength)
        {
            Registers.HL = bufferAddress; // location of the buffer
            Registers.C = 0x00; // we'll use Port 0
            Registers.B = bufferLength; // size of the buffer
        }

        private (byte[] data, ushort expectedValueInHL) GetDataInputAndExpectedValueInHL(int bufferSize, ushort bufferAddress, WriteDirection direction, WriteRepeats repeats)
        {
            ushort expectedValueInHL = repeats == WriteRepeats.Repeating ?
                                        (direction == WriteDirection.Incrementing ? (ushort)(bufferAddress + bufferSize) : (ushort)(bufferAddress - bufferSize)) :
                                        (direction == WriteDirection.Incrementing ? (ushort)(bufferAddress + 1) : (ushort)(bufferAddress - 1));

            // where is the data? 
            // INI - at the initial buffer address
            // IND - at the initial buffer address
            // INIR - at the initial buffer address
            // INDR - at initial buffer address - buffer length
            ushort checkAddress = (direction, repeats) switch
            {
                (WriteDirection.Decrementing, WriteRepeats.Repeating) => (ushort)(bufferAddress - bufferSize + 1),
                _ => bufferAddress
            };

            byte[] data = CPU.Memory.ReadBytesAt(checkAddress, (ushort)bufferSize, true);

            return (
                data,
                expectedValueInHL
                );
        }

        [Test]
        public void INI()
        {
            byte[] data = new byte[1] { 0x7F };
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0x01);

            ExecutionResult executionResult = ExecuteInstruction("INI");
            (byte[] checkData, ushort expectedValueInHL) = GetDataInputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Incrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void IND()
        {
            byte[] data = new byte[1] { 0x7F };
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0x01);

            ExecutionResult executionResult = ExecuteInstruction("IND");
            (byte[] checkData, ushort expectedValueInHL) = GetDataInputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Decrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void INIR()
        {
            byte[] data = RandomBytes(0xFF);
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0xFF);

            while (Registers.B > 0) ExecuteInstruction("INIR");
            (byte[] checkData, ushort expectedValueInHL) = GetDataInputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Incrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void INDR()
        {
            byte[] data = RandomBytes(0xFF);
            ushort bufferAddress = 0x5000;
            SetupPort(0, data, bufferAddress);
            SetupBuffer(bufferAddress, 0xFF);

            while (Registers.B > 0) ExecuteInstruction("INDR");
            (byte[] checkData, ushort expectedValueInHL) = GetDataInputAndExpectedValueInHL(data.Length, bufferAddress, WriteDirection.Decrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(checkData, Is.EqualTo(data.Reverse()));
        }
    }
}
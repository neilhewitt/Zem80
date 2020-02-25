using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_LDD_LDI_LDDR_LDIR : InstructionTestBase
    {
        private void SetupAndCopy(ushort startAddress, ushort destinationAddress, byte[] data, bool offsetDestination = false)
        {
            Registers.HL = startAddress; // location of the data
            Registers.BC = (ushort)data.Length; // amount of data
            Registers.DE = destinationAddress; // destination address
            if (offsetDestination) startAddress = (ushort)(startAddress - data.Length + 1); // if LDDR, we will write from (destination - data length) forwards
            CPU.Memory.WriteBytesAt(startAddress, data); // write the data to the start address prior to copy
        }

        private (byte[] data, ushort expectedValueInHL, ushort expectedValueInDE) GetDataCopiedAndExpectedValueInHLAndDE(int byteCount, ushort fromAddress, ushort toAddress, WriteDirection direction, WriteRepeats repeats)
        {
            ushort expectedValueInHL = repeats == WriteRepeats.Repeating ?
                                        (direction == WriteDirection.Incrementing ? (ushort)(fromAddress + byteCount) : (ushort)(fromAddress - byteCount)) :
                                        (direction == WriteDirection.Incrementing ? (ushort)(fromAddress + 1) : (ushort)(fromAddress - 1));

            ushort expectedValueInDE = repeats == WriteRepeats.Repeating ?
                                        (direction == WriteDirection.Incrementing ? (ushort)(toAddress + byteCount) : (ushort)(toAddress - byteCount)) :
                                        (direction == WriteDirection.Incrementing ? (ushort)(toAddress + 1) : (ushort)(toAddress - 1));


            // where is the data? 
            // LDI - at the destination address
            // LDD - at the destination address
            // LDIR - at the destination address
            // LDDDR - at destination address - byte count + 1 (because bytes are written backwards)
            ushort checkAddress = (direction, repeats) switch
            {
                (WriteDirection.Decrementing, WriteRepeats.Repeating) => (ushort)(toAddress - byteCount + 1),
                _ => toAddress
            };

            byte[] data = CPU.Memory.ReadBytesAt(checkAddress, (ushort)byteCount);

            return (
                data,
                expectedValueInHL,
                expectedValueInDE
                );
        }

        [Test]
        public void LDI()
        {
            byte[] data = new byte[1] { 0x7F };
            ushort startAddress = 0x5000;
            ushort destinationAddress = 0x8000;
            SetupAndCopy(startAddress, destinationAddress, data);

            ExecutionResult executionResult = ExecuteInstruction("LDI");
            (byte[] checkData, ushort expectedValueInHL, ushort expectedValueInDE) = GetDataCopiedAndExpectedValueInHLAndDE(data.Length, startAddress, destinationAddress, WriteDirection.Incrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(Registers.DE, Is.EqualTo(expectedValueInDE));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void LDD()
        {
            byte[] data = new byte[1] { 0x7F };
            ushort startAddress = 0x5000;
            ushort destinationAddress = 0x8000;
            SetupAndCopy(startAddress, destinationAddress, data);

            ExecutionResult executionResult = ExecuteInstruction("LDD");
            (byte[] checkData, ushort expectedValueInHL, ushort expectedValueInDE) = GetDataCopiedAndExpectedValueInHLAndDE(data.Length, startAddress, destinationAddress, WriteDirection.Decrementing, WriteRepeats.NotRepeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(Registers.DE, Is.EqualTo(expectedValueInDE));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void LDIR()
        {
            byte[] data = RandomBytes(0xFF);
            ushort startAddress = 0x5000;
            ushort destinationAddress = 0x8000;
            SetupAndCopy(startAddress, destinationAddress, data);

            while (Registers.BC > 0) ExecuteInstruction("LDIR");
            (byte[] checkData, ushort expectedValueInHL, ushort expectedValueInDE) = GetDataCopiedAndExpectedValueInHLAndDE(data.Length, startAddress, destinationAddress, WriteDirection.Incrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(Registers.DE, Is.EqualTo(expectedValueInDE));
            Assert.That(checkData, Is.EqualTo(data));
        }

        [Test]
        public void LDDR()
        {
            byte[] data = RandomBytes(0xFF);
            ushort startAddress = 0x5000;
            ushort destinationAddress = 0x8000;
            SetupAndCopy(startAddress, destinationAddress, data, true); // note extra param to have buffer written in the right place

            while (Registers.BC > 0) ExecuteInstruction("LDDR");
            (byte[] checkData, ushort expectedValueInHL, ushort expectedValueInDE) = GetDataCopiedAndExpectedValueInHLAndDE(data.Length, startAddress, destinationAddress, WriteDirection.Decrementing, WriteRepeats.Repeating);

            Assert.That(Registers.HL, Is.EqualTo(expectedValueInHL));
            Assert.That(Registers.DE, Is.EqualTo(expectedValueInDE));
            Assert.That(checkData, Is.EqualTo(data));
        }
    }
}
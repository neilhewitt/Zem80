using NUnit.Framework;
using NSubstitute;
using Zem80.Core.Memory;
using Zem80.Core.CPU;

namespace Zem80.Core.Tests.MemoryTests
{
    [TestFixture]
    public class MemoryBankTests
    {
        private MemoryBank _bank;
        private IMemorySegment _segment;
        private IMemoryMap _map;
        private IProcessorTiming _timing;

        [SetUp]
        public void Setup()
        {
            _segment = Substitute.For<IMemorySegment>();
            _segment.StartAddress.Returns((ushort)0x0000);
            _segment.SizeInBytes.Returns((uint)0x400);
            _segment.ReadOnly.Returns(false);

            _map = Substitute.For<IMemoryMap>();
            _map.SizeInBytes.Returns((uint)0x400);
            _map.SegmentFor(Arg.Any<ushort>()).Returns(call =>
            {
                ushort address = call.Arg<ushort>();
                if (address >= _segment.StartAddress && address < _segment.StartAddress + _segment.SizeInBytes)
                    return _segment;
                return null;
            });

            _timing = Substitute.For<IProcessorTiming>();

            _bank = new MemoryBank();
            _bank.Initialise(_timing, _map);
        }

        [Test]
        public void Clear_CallsMapClearAllWritableMemory()
        {
            _bank.Clear();
            _map.Received(1).ClearAllWritableMemory();
        }

        [Test]
        public void ReadByteAt_ThrowsIfNotInitialised()
        {
            var bank = new MemoryBank();
            Assert.Throws<MemoryNotInitialisedException>(() => bank.ReadByteAt(0x0000, null));
        }

        [Test]
        public void ReadByteAt_ReturnsSegmentByte_AndCallsTiming()
        {
            _segment.ReadByteAt(0).Returns((byte)0x42);

            var result = _bank.ReadByteAt(0x0000, 4);

            Assert.That(result, Is.EqualTo(0x42));
            _timing.Received(1).MemoryReadCycle(0x0000, 0x42, 4);
        }

        [Test]
        public void ReadByteAt_UnmappedAddress_ReturnsZero()
        {
            var result = _bank.ReadByteAt(0x1234, null);
            Assert.That(result, Is.EqualTo(0x00));
        }

        [Test]
        public void WriteByteAt_WritesToSegment_AndCallsTiming()
        {
            _segment.ReadOnly.Returns(false);

            _bank.WriteByteAt(0x0000, 0x99, 5);

            _segment.Received(1).WriteByteAt(0, 0x99);
            _timing.Received(1).MemoryWriteCycle(0x0000, 0x99, 5);
        }

        [Test]
        public void WriteByteAt_DoesNotWriteIfReadOnly()
        {
            _segment.ReadOnly.Returns(true);

            _bank.WriteByteAt(0x0000, 0x99, 5);

            _segment.DidNotReceive().WriteByteAt(Arg.Any<ushort>(), Arg.Any<byte>());
            _timing.Received(1).MemoryWriteCycle(0x0000, 0x99, 5);
        }

        [Test]
        public void ReadWordAt_ReadsTwoBytes()
        {
            _segment.ReadByteAt(0).Returns((byte)0x34);
            _segment.ReadByteAt(1).Returns((byte)0x12);

            var value = _bank.ReadWordAt(0x0000, 3);

            Assert.That(value, Is.EqualTo(0x1234));
        }

        [Test]
        public void WriteWordAt_WritesTwoBytes()
        {
            _segment.ReadOnly.Returns(false);

            _bank.WriteWordAt(0x0004, 0x1234, 2);

            _segment.Received(1).WriteByteAt(4, 0x34);
            _segment.Received(1).WriteByteAt(5, 0x12);
        }

        [Test]
        public void ReadBytesAt_ReadsCorrectNumberOfBytes()
        {
            _segment.ReadByteAt(3).Returns((byte)0xA1);
            _segment.ReadByteAt(4).Returns((byte)0xB2);

            var bytes = _bank.ReadBytesAt(0x0003, 2, null);

            Assert.That(bytes, Is.EqualTo(new byte[] { 0xA1, 0xB2 }));
        }

        [Test]
        public void WriteBytesAt_WritesAllBytes()
        {
            _segment.ReadOnly.Returns(false);

            var data = new byte[] { 0x11, 0x22, 0x33 };
            _bank.WriteBytesAt(0x0005, data, null);

            _segment.Received(1).WriteByteAt(5, 0x11);
            _segment.Received(1).WriteByteAt(6, 0x22);
            _segment.Received(1).WriteByteAt(7, 0x33);
        }

        [Test]
        public void ReadBytesAt_Overflow_ReturnsZeroes()
        {
            _map.SizeInBytes.Returns((uint)0x400);
            _segment.ReadByteAt(0x3FE).Returns((byte)0xFE);
            _segment.ReadByteAt(0x3FF).Returns((byte)0xFF);

            // read 4 bytes starting from 0x3FF
            // this overflows the memory map, and bytes beyond 0x400 should be zero
            var bytes = _bank.ReadBytesAt(0x3FE, 4, null);

            Assert.That(bytes.Length, Is.EqualTo(4));
            Assert.That(bytes[0], Is.EqualTo(0xFE));
            Assert.That(bytes[1], Is.EqualTo(0xFF));
            Assert.That(bytes[2], Is.EqualTo(0x00));
            Assert.That(bytes[3], Is.EqualTo(0x00));
        }
    }
}
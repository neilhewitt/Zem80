using NUnit.Framework;
using System;
using Zem80.Core.Memory;

namespace Zem80.Core.Tests.MemoryTests
{
    [TestFixture]
    public class MemorySegmentTests
    {
        private MemorySegment _writeableSegment;
        private ReadOnlyMemorySegment _readOnlySegment;

        [SetUp]
        public void Setup()
        {
            _writeableSegment = new MemorySegment(0x400);
            _writeableSegment.MapAt(0x0000);

            _readOnlySegment = new ReadOnlyMemorySegment(new byte[0x400]);
            _readOnlySegment.MapAt(0x0400);
        }

        [Test]
        public void StartAddress_And_SizeInBytes_AreSetCorrectly()
        {
            Assert.That(_writeableSegment.StartAddress, Is.EqualTo(0x0000));
            Assert.That(_writeableSegment.SizeInBytes, Is.EqualTo(0x400));
        }

        [Test]
        public void WriteByteAt_And_ReadByteAt_Work()
        {
            _writeableSegment.WriteByteAt(10, 0xAB);
            var value = _writeableSegment.ReadByteAt(10);
            Assert.That(value, Is.EqualTo(0xAB));
        }

        [Test]
        public void WriteByteAt_DoesNothing_WhenReadOnly()
        {
            _readOnlySegment.WriteByteAt(5, 0x55);
            var value = _readOnlySegment.ReadByteAt(5);
            Assert.That(value, Is.EqualTo(0x00));
        }

        [Test]
        public void WriteByteAt_Throws_IfOffsetOutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _writeableSegment.WriteByteAt(1024, 0x12));
        }

        [Test]
        public void ReadByteAt_Throws_IfOffsetOutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _writeableSegment.ReadByteAt(1024));
        }

        [Test]
        public void Clear_SetsAllBytesToZero()
        {
            _writeableSegment.WriteByteAt(0, 0xFF);
            _writeableSegment.WriteByteAt(100, 0xAA);
            _writeableSegment.Clear();
            Assert.That(_writeableSegment.ReadByteAt(0), Is.EqualTo(0x00));
            Assert.That(_writeableSegment.ReadByteAt(100), Is.EqualTo(0x00));
        }
    }
}
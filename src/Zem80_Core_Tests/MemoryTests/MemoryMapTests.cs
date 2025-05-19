using NUnit.Framework;
using NSubstitute;
using Zem80.Core.Memory;

namespace Zem80.Core.Tests.MemoryTests
{
    [TestFixture]
    public class MemoryMapTests
    {
        private IMemorySegment _segment1;
        private IMemorySegment _segment2;
        private MemoryMap _map;

        [SetUp]
        public void Setup()
        {
            _segment1 = Substitute.For<IMemorySegment>();
            _segment1.StartAddress.Returns((ushort)0x0000);
            _segment1.SizeInBytes.Returns((uint)0x400);
            _segment1.ReadOnly.Returns(false);

            _segment2 = Substitute.For<IMemorySegment>();
            _segment2.StartAddress.Returns((ushort)0x0400);
            _segment2.SizeInBytes.Returns((uint)0x400);
            _segment2.ReadOnly.Returns(true);

            _map = new MemoryMap(0x800);
            _map.Map(_segment1, _segment1.StartAddress);
            _map.Map(_segment2, _segment2.StartAddress);
        }

        [Test]
        public void SizeInBytes_ReturnsTotalMappedSize()
        {
            Assert.That(_map.SizeInBytes, Is.EqualTo(0x800));
        }

        [Test]
        public void SegmentFor_ReturnsCorrectSegment()
        {
            var segment1 = _map.SegmentFor(0x0000);
            var segment2 = _map.SegmentFor(0x0400);
            var nullSegment = _map.SegmentFor(0x0800);

            Assert.That(segment1, Is.EqualTo(_segment1));
            Assert.That(segment2, Is.EqualTo(_segment2));
            Assert.That(nullSegment, Is.Null);
        }

        [Test]
        public void Map_OverwritesSegment_WhenOverwriteIsTrue()
        {
            var newSegment = Substitute.For<IMemorySegment>();
            newSegment.StartAddress.Returns((ushort)0x0000);
            newSegment.SizeInBytes.Returns((uint)0x400);

            _map.Map(newSegment, 0x0000, overwriteMappedPages: true);

            var segment = _map.SegmentFor(0x0000);
            Assert.That(segment, Is.EqualTo(newSegment));
        }

        [Test]
        public void Map_DoesNotOverwriteSegment_WhenOverwriteIsFalse()
        {
            var newSegment = Substitute.For<IMemorySegment>();
            newSegment.StartAddress.Returns((ushort)0x0000);
            newSegment.SizeInBytes.Returns((uint)0x400);

            Assert.Throws<MemoryMapException>(
                () => _map.Map(newSegment, 0x0000, overwriteMappedPages: false)
                );
        }
    }
}
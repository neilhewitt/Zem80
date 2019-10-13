using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Z80.Core;
using Z80.SimpleVM;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionDecoderTests
    {
        [Test]
        public void TestDecode()
        {
            MemoryMap map = new MemoryMap(); // ordinarily you'd get this via the Processor
            RAM ram = new RAM(0, 64);
            map.Map(ram);
            MemoryReader reader = new MemoryReader(map);
            MemoryWriter writer = new MemoryWriter(map);
            writer.WriteBytesAt(0, 0x8E, 0xDD, 0x8E, 0xFF, 0xDD, 0x8C, 0xDD, 0xDD, 0xDD, 0xCB, 0xDD, 0x46);

            InstructionDecoder decoder = new InstructionDecoder();
            Instruction instruction = decoder.Decode(reader, 0, out InstructionData data);
            instruction = decoder.Decode(reader, 1, out data);
            instruction = decoder.Decode(reader, 4, out data);
            instruction = decoder.Decode(reader, 6, out data);
        }
    }
}

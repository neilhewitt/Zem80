using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionDecoderTests
    {
        [Test]
        public void TestDecode()
        {
            Processor cpu = Bootstrapper.BuildProcessor();
            cpu.Memory.WriteBytesAt(0, 0x8E, 0xDD, 0x8E, 0xFF, 0xDD, 0x8C, 0xDD, 0xDD, 0xDD, 0xCB, 0xDD, 0x46);
            cpu.OnBeforeExecute += CheckInstruction;
            cpu.Start();
        }

        private void CheckInstruction(object sender, InstructionPackage package)
        {
            int sizeInBytes = package.Info.SizeInBytes;
        }
    }
}

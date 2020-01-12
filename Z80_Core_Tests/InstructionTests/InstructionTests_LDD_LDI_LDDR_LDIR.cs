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
        [TestCase("LDD")]
        [TestCase("LDI")]
        [TestCase("LDDR")]
        [TestCase("LDIR")]
        public void LDD_LDI_LDDR_LDIR(string instruction)
        {
            // LDDR and LDIR are block instructions which repeat until terminated
            bool repeats = instruction == "LDDR" || instruction == "LDIR";

            ushort startAddress = RandomWord(0x9194); 
            ushort destinationAddress = RandomWord(); 
            ushort count = repeats ? (ushort)RandomByte() : (ushort)1;
            
            byte[] data = new byte[count];
            // fill block with random data and keep copy for comparison
            for (int i = 0; i < count; i++)
            {
                data[i] = RandomByte();
                WriteByteAt((ushort)(startAddress + i), data[i]);
            }

            // if LDDR, we start at the *end* of the block, so increase HL to suit
            Registers.HL = (instruction == "LDDR" ? (ushort)(startAddress + count - 1) : startAddress);
            Registers.BC = count;
            Registers.DE = destinationAddress;
            while (Registers.BC > 0)
            {
                Execute(instruction);
            }

            // we need to offset the original start address depending on instruction, so we can 
            // check HL points at the right address at the end of the operation
            // (Note LDDR adjust is -1 because the address was pushed to the end of the block at the start)
            int adjust = instruction switch { "LDD" => -1, "LDI" => 1, "LDIR" => count, "LDDR" => -1, _ => 0 };
            bool addressCorrect = Registers.HL == startAddress + adjust;

            byte[] checkData = _cpu.Memory.ReadBytesAt(startAddress, count).ToArray();

            Assert.That(addressCorrect && Registers.BC == 0 && data.SequenceEqual(checkData));
        }
    }
}
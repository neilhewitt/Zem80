using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    public class InstructionTests_RST : InstructionTestBase
    {
        [Test]
        public void RST([Values(0x00, 0x10, 0x18, 0x20, 0x28, 0x30, 0x38)] byte resetCode)
        {
            ushort address = 0x5000; // simulate being in the middle of a code block
            SetProgramCounter(address);
            
            string resetHexCode = resetCode == 0 ? "0" : (resetCode.ToString("X2").Replace("0x", "") + "H");
            ExecutionResult executionResult = ExecuteInstruction($"RST { resetHexCode  }");

            byte t_index = executionResult.Instruction.Opcode.GetByteFromBits(3, 3);
            ushort rst_address = (ushort)(t_index * 8);

            Assert.That(Registers.PC, Is.EqualTo(rst_address)); // PC should now point to reset table entry
            Assert.That(CPU.Peek(), Is.EqualTo(address + 1)); // stack should have previous PC address + 1 byte for instruction size
        }
    }
}

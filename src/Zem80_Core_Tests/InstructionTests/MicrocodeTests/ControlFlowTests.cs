using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Zem80.Core.Instructions;

namespace Zem80.Core.Tests.MicrocodeTests
{
    [TestFixture]
    public class ControlFlowTests : MicrocodeTestBase
    {
        [Test]
        // JP nn - jump to absolute address nn
        public void JP()
        {
            ExecuteInstruction("JP nn", 0x24, 0x05);
            Assert.That(Registers.PC == 0x0524);
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        // JP condition, nn - jump to absolute address if condition is satisfied by flags
        // we test both the not case (condition is not satisfied, program counter set to start of next instruction)
        // and the true case (condition is satisfied, program counter set to absolute address nn) for each condition
        public void JP(Condition condition)
        {
            SetCPUFlagsFromCondition(condition, true); // set flag condition according to test case value

            Registers.PC = 0;
            ExecuteInstruction("JP " + condition.ToString() + ",nn", 0x10, 0x30);
            Assert.That(Registers.PC == 3); // condition was not true so PC = instruction address (0) + instruction length in bytes (3) so PC should == 3

            SetCPUFlagsFromCondition(condition, false);
            ExecuteInstruction("JP " + condition.ToString() + ",nn", 0x10, 0x30); // condition was true so PC should == 0x3010
            Assert.That(Registers.PC == 0x3010);
        }

        [TestCase(0x4000, 0x10, 0x4012)] // +16 bytes, +2 bytes for instruction = jump 18 bytes to 0x4012
        [TestCase(0x4000, 0x80, 0x3F82)] // -128 bytes, +2 bytes for instruction = jump -126 bytes to 0x3F82
        // JR o - jump forward / back relative to this instruction address
        // o is a signed byte (-128 to +127) but the jump address is adjusted by +2 bytes to accomodate for the JR instruction length itself
        // so the actual jump range is -126 to +129, and we need to test this behaviour
        public void JR(int address, int displacement, int expectedAddress)
        {
            Registers.PC = (ushort)address;
            ExecuteInstruction("JR o", (byte)displacement);
            Assert.That(Registers.PC == (ushort)expectedAddress);
        }

        [Test]
        public void CALL()
        {
            Registers.PC = 0x4000;
            ExecuteInstruction("CALL nn", 0x24, 0x05);
            CPU.Pop(WordRegister.HL); // instruction address + 3 bytes (instruction length + 2 byte argument) gets pushed to the stack during CALL operation, so retrieve this value to check it
            Assert.That(Registers.PC == 0x0524 && Registers.HL == 0x4003);
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        // CALL condition, nn - jump to absolute address if condition is satisfied by flags
        // we test both the not case (condition is not satisfied, program counter set to start of next instruction)
        // and the true case (condition is satisfied, program counter set to absolute address nn) for each condition
        public void CALL(Condition condition)
        {
            SetCPUFlagsFromCondition(condition, true); // set flag condition according to test case value

            // FAILING CASE

            // initialise addresses
            Registers.PC = 0x4000; // instruction starts here
            Registers.HL = 0x5000; // check address
            
            CPU.Push(WordRegister.HL); // push the check address to the stack: since the following instruction will fail, PC should *not* get pushed to the stack so this value should still be at the top
            ExecuteInstruction("CALL " + condition.ToString() + ",nn", 0x10, 0x30);
            CPU.Pop(WordRegister.DE); // pop the stack value into DE so we can check it is the check address (0x5000)
            
            Assert.That(Registers.PC == 0x4003 && Registers.DE == 0x5000); // condition was not true so PC = instruction address (0x4000) + instruction length in bytes (3) so PC should == 0x4003

            // PASSING CASE

            Registers.PC = 0x4000; // instruction starts here
            Registers.HL = 0x5000; // check address

            SetCPUFlagsFromCondition(condition, false);
            CPU.Push(WordRegister.HL); // this time the condition should pass, so PC should get pushed to the stack *after* this value and so the check value should *not* be at the top
            ExecuteInstruction("CALL " + condition.ToString() + ",nn", 0x10, 0x30); // condition was true so PC should == 0x3010
            CPU.Pop(WordRegister.DE); // pop the value at the top of the stack - this time it should be the instruction address + 3 (0x4003) as PC got pushed to the stack before the CALL
            Assert.That(Registers.PC == 0x3010 && Registers.DE == 0x4003);
        }

        [Test]
        public void RET()
        {
            ushort origin = 0x4000;
            Registers.HL = origin;
            CPU.Push(WordRegister.HL);

            Registers.PC = 0;
            ExecuteInstruction("RET");

            Assert.That(Registers.PC == origin);
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        public void RET(Condition condition)
        {
            ushort origin = 0x4000;

            Registers.PC = 0;
            Registers.HL = origin;
            CPU.Push(WordRegister.HL);
            SetCPUFlagsFromCondition(condition, true); // set flag condition according to test case value
            ExecuteInstruction("RET " + condition.ToString());
            Assert.That(Registers.PC == 1); // condition was not true so PC = instruction address (0) + instruction length in bytes (1) so PC should == 1

            Registers.PC = 0;
            Registers.HL = origin;
            CPU.Push(WordRegister.HL);
            SetCPUFlagsFromCondition(condition, false);
            ExecuteInstruction("RET " + condition.ToString()); // condition was true so PC should == 0x4000
            Assert.That(Registers.PC == origin);
        }

        [Test]
        public void RETN()
        {
            ushort origin = 0x4000;
            Registers.HL = origin;
            CPU.Push(WordRegister.HL);

            Registers.PC = 0;
            ExecuteInstruction("RETN");

            Assert.That(Registers.PC == origin);
        }

        [Test]
        public void RETI()
        {
            ushort origin = 0x4000;
            Registers.HL = origin;
            CPU.Push(WordRegister.HL);

            Registers.PC = 0;
            ExecuteInstruction("RETI");

            Assert.That(Registers.PC == origin);
        }

        [TestCase(0x4000, 0x10, 0x4012)] // +16 bytes, +2 bytes for instruction = jump 18 bytes to 0x4012
        [TestCase(0x4000, 0x80, 0x3F82)] // -128 bytes, +2 bytes for instruction = jump -126 bytes to 0x3F82
        public void DJNZ(int address, int displacement, int expectedAddress)
        {
            Registers.PC = (ushort)address;
            Registers.B = 0x01;
            ExecuteInstruction("DJNZ o", (byte)displacement);
            Assert.That(Registers.PC == address + 2); // condition was not true so PC = instruction address (0) + instruction length in bytes (2) so PC should == 2

            Registers.PC = (ushort)address;
            Registers.B = 0x00;
            ExecuteInstruction("DJNZ o", (byte)displacement); // condition was true so PC should == 0x3010
            Assert.That(Registers.PC == expectedAddress);
        }

        [TestCase(0x00)]
        [TestCase(0x08)]
        [TestCase(0x10)]
        [TestCase(0x18)]
        [TestCase(0x20)]
        [TestCase(0x28)]
        [TestCase(0x30)]
        [TestCase(0x38)]
        public void RST(int target)
        {
            Registers.PC = 0x4000;
            ExecuteInstruction($"RST { target.ToString("X2") }H");
            CPU.Pop(WordRegister.HL);
            Assert.That(Registers.PC == (ushort)target && Registers.HL == 0x4001); // PC will have moved +1 before being PUSHed, so it's *0x4001*
        }
    }
}

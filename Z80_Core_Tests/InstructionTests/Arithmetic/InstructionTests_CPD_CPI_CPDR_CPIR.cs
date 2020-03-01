using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CPD_CPI_CPDR_CPIR : InstructionTestBase
    {
        private Flags GetExpectedFlags(byte input, byte compareTo, ushort byteCount)
        {
            Flags flags = new Flags();
            int result = input - compareTo;
            FlagHelper.SetFlagsFromArithmeticOperation(flags, input, compareTo, result, true);
            flags.ParityOverflow = (byteCount - 2 != 0);

            return flags;
        }

        [Test]
        public void CPD([Values(0x01, 0x7F, 0xFF)] byte input, [Values(0x7F, 0x0F, 0x01)] byte byteCount)
        {
            // CPD - compare value in A with value at memory address pointed to by HL
            // then decrement the value in HL and BC 

            byte compareTo = 0x7F; // will only match one of the input values
            ushort address = 0x5000;

            Registers.A = input;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt(Registers.HL, compareTo);
            
            Flags expectedFlags = GetExpectedFlags(input, compareTo, byteCount);
            ExecutionResult executionResult = ExecuteInstruction($"CPD");

            Assert.That(Registers.HL, Is.EqualTo(address - 1));
            Assert.That(Registers.BC, Is.EqualTo(byteCount - 1));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void CPI([Values(0x01, 0x7F, 0xFF)] byte input, [Values(0x7F, 0x0F, 0x01)] byte byteCount)
        {
            // CPI - compare value in A with value at memory address pointed to by HL
            // then increment the value in HL and decrement the value in BC 

            byte compareTo = 0x7F; // will only match one of the input values
            ushort address = 0x5000;

            Registers.A = input;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt(Registers.HL, compareTo);

            Flags expectedFlags = GetExpectedFlags(input, compareTo, byteCount);
            ExecutionResult executionResult = ExecuteInstruction($"CPI");

            Assert.That(Registers.HL, Is.EqualTo(address + 1));
            Assert.That(Registers.BC, Is.EqualTo(byteCount - 1));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void CPDR([Values(false, true)] bool inputValueIsInMemory)
        {
            // CPDR - compare value in A with value at memory address pointed to by HL
            // then decrement the value in HL and BC, and set the Program Counter back to create a loop
            // until either Z goes true (compare is successful) or P/V goes false (BC goes to zero)

            byte input = 0x7F;
            byte[] buffer = new byte[0x7F];
            if (inputValueIsInMemory) buffer[0x01] = input;
            ushort address = 0x5000;
            byte byteCount = 0x7F;

            Registers.A = input;
            Registers.HL = address; 
            Registers.BC = byteCount;
            CPU.Memory.WriteBytesAt((ushort)(address - byteCount), buffer);

            // loop until either BC == 0 or comparison is true (Flags.Zero is true)
            Flags expectedFlags = new Flags();
            while (Registers.Flags.Zero == false && Registers.BC > 0)
            {
                // note we need to call GetExpected flags *before* Execute Instruction, because
                // otherwise at BC == 2 -> 1, this will trigger invalid flag evaluation
                expectedFlags = GetExpectedFlags(Registers.A, ReadByteAt(Registers.HL), Registers.BC);
                ExecutionResult executionResult = ExecuteInstruction($"CPDR");

                Assert.That(Registers.HL, Is.EqualTo(address - 1));
                Assert.That(Registers.BC, Is.EqualTo(byteCount - 1));
                Assert.That(Registers.Flags, Is.EqualTo(expectedFlags));

                address--;
                byteCount--;
            }

            // at exit, either Flags.Zero is true (value was found) or Flags.ParityOverflow is false (byte count went to zero first)
            if (inputValueIsInMemory)
            {
                // should be found, Flags.Zero must be true
                Assert.That(Registers.Flags.Zero, Is.EqualTo(expectedFlags.Zero));
            }
            else
            {
                // should not be found and BC should have gone to zero
                Assert.That(Registers.Flags.ParityOverflow, Is.EqualTo(expectedFlags.ParityOverflow));
                Assert.That(Registers.BC, Is.EqualTo(0));
            }
        }

        [Test]
        public void CPIR([Values(false, true)] bool inputValueIsInMemory)
        {
            // CPIR - compare value in A with value at memory address pointed to by HL
            // then increment the value in HL and decrement the value in BC, and set the Program Counter back to create a loop
            // until either Z goes true (compare is successful) or P/V goes false (BC goes to zero)

            byte input = 0x7F;
            byte[] buffer = new byte[0x7F];
            if (inputValueIsInMemory) buffer[0x01] = input;
            ushort address = 0x5000;
            byte byteCount = 0x7F;

            Registers.A = input;
            Registers.HL = address;
            Registers.BC = byteCount;
            CPU.Memory.WriteBytesAt(address, buffer);

            // loop until either BC == 0 or comparison is true (Flags.Zero is true)
            Flags expectedFlags = new Flags();
            while (Registers.Flags.Zero == false && Registers.BC > 0)
            {
                expectedFlags = GetExpectedFlags(Registers.A, ReadByteAt(Registers.HL), Registers.BC);
                ExecutionResult executionResult = ExecuteInstruction($"CPIR");

                Assert.That(Registers.HL, Is.EqualTo(address + 1));
                Assert.That(Registers.BC, Is.EqualTo(byteCount - 1));
                Assert.That(Registers.Flags, Is.EqualTo(expectedFlags));

                address++;
                byteCount--;
            }

            // at exit, either Flags.Zero is true (value was found) or Flags.ParityOverflow is false (byte count went to zero first)
            if (inputValueIsInMemory)
            {
                // should be found, Flags.Zero must be true
                Assert.That(Registers.Flags.Zero, Is.EqualTo(expectedFlags.Zero));
            }
            else
            {
                // should not be found and BC should have gone to zero
                Assert.That(Registers.Flags.ParityOverflow, Is.EqualTo(expectedFlags.ParityOverflow));
                Assert.That(Registers.BC, Is.EqualTo(0));
            }
        }
    }
}

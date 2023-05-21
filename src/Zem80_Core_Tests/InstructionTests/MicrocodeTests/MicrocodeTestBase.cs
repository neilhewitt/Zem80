using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.Tests.MicrocodeTests
{
    public abstract class MicrocodeTestBase
    {
        public Processor CPU { get; private set; }
        public Registers Registers => CPU.Registers;
        public IReadOnlyFlags Flags => CPU.Flags;

        [OneTimeSetUp]
        public void Setup()
        {
            CPU = new Processor();
        }

        [SetUp]
        public void Init()
        {
            CPU.ResetAndClearMemory(false);
        }

        public ExecutionResult ExecuteInstruction(string mnemonic, byte? arg1 = null, byte? arg2 = null)
        {
            // only available on Processor debug interface - sets flags but does not advance PC 
            // (but if PC is assigned in the instruction, that value is preserved)
            ExecutionResult result = CPU.Debug.ExecuteDirect(mnemonic, arg1, arg2);
            return result;
        }

        public void SetCPUFlagsFromCondition(Condition condition, bool conditionIsTrue)
        {
            Flags flags = new Flags();

            switch (condition)
            {
                case Condition.Z: flags.Zero = true; break;
                case Condition.NZ: flags.Zero = false; break;
                case Condition.C: flags.Carry = true; break;
                case Condition.NC: flags.Carry = false; break;
                case Condition.PE: flags.ParityOverflow = true; break;
                case Condition.PO: flags.ParityOverflow = false; break;
                case Condition.M: flags.Sign = true; break;
                case Condition.P: flags.Sign = false; break;
            }

            CPU.Registers.F = conditionIsTrue ? flags.Value : flags.Value.Invert();
        }
    }
}

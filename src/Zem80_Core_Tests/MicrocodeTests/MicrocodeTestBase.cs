using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Zem80.Core.Instructions;

namespace Zem80.Core.Tests.MicrocodeTests
{
    public abstract class MicrocodeTestBase
    {
        protected Random _random;

        public Processor CPU { get; private set; }
        public Registers Registers => CPU.Registers;
        public Flags Flags => CPU.Registers.Flags;

        [OneTimeSetUp]
        public void Setup()
        {
            try
            {
                CPU = new Processor();
                _random = new Random(DateTime.Now.Millisecond);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [SetUp]
        public void Init()
        {
            CPU.ResetAndClearMemory(false);
        }

        public ExecutionResult ExecuteInstruction(string mnemonic, byte? arg1 = null, byte? arg2 = null)
        {
            ExecutionResult result = CPU.Debug.ExecuteDirect(mnemonic, arg1, arg2); // only available on Processor debug interface - sets flags but does not advance PC
            return result;
        }

        public void SetCPUFlagsFromCondition(Condition condition, bool invert)
        {
            Flags flags = CPU.Registers.Flags;
            flags.Reset(); // start with a blank slate

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

            if (invert) CPU.Registers.Debug.F = flags.Value.Invert(); // excludes the condition rather than including it
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class FlagsTests
    {
        // this seems like a fairly convoluted way to avoid lots of repeated code...
        // but it works :-)
        // plus... local functions!

        [TestCase("Sign", 0x7F)]
        [TestCase("Zero", 0xBF)]
        [TestCase("Five", 0xDF)]
        [TestCase("HalfCarry", 0xEF)]
        [TestCase("Three", 0xF7)]
        [TestCase("Parity", 0xFB)]
        [TestCase("Subtract", 0xFD)]
        [TestCase("Carry", 0xFE)]

        public void CanSetAndReadFlag(string flagName, byte initialFlagsValue)
        {
            Registers registers = new Registers();
            registers.F = initialFlagsValue; // all flags set except Sign
            Flags flags = new Flags(registers);
            
            var property = flags.GetType().GetProperty(flagName);
            var getter = property.GetMethod;
            var setter = property.SetMethod;

            Assert.That(FlagValue() == false);
            SetFlag();
            Assert.That(FlagValue() == true && registers.F == 0xFF);
            
            bool FlagValue()
            {
                return (bool)getter.Invoke(flags, null);
            }

            void SetFlag()
            {
                setter.Invoke(flags, new object[] { true });
            }
        }
    }
}

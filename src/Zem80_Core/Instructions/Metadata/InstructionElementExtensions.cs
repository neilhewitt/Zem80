using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using Zem80.Core.CPU;

namespace Zem80.Core.CPU
{
    public static class InstructionElementExtensions
    {
        public static WordRegister AsWordRegister(this InstructionElement argument)
        {
            argument = argument switch
            {
                InstructionElement.AddressFromIXAndOffset => InstructionElement.IX,
                InstructionElement.AddressFromIYAndOffset => InstructionElement.IY,
                var a when (a >= InstructionElement.AddressFromHL && a <= InstructionElement.AddressFromSP) => (InstructionElement)(a - 6),
                _ => argument
            };

            // this looks clunky but it's *much* faster than using Enum.ToString()
            WordRegister register = argument switch
            {
                InstructionElement.AF => WordRegister.AF,
                InstructionElement.BC => WordRegister.BC,
                InstructionElement.DE => WordRegister.DE,
                InstructionElement.HL => WordRegister.HL,
                InstructionElement.IX => WordRegister.IX,
                InstructionElement.IY => WordRegister.IY,
                InstructionElement.SP => WordRegister.SP,
                _ => WordRegister.None
            };

            return register;
        }

        public static ByteRegister AsByteRegister(this InstructionElement argument)
        {
            ByteRegister register = argument switch
            {
                InstructionElement.A => ByteRegister.A,
                InstructionElement.B => ByteRegister.B,
                InstructionElement.C => ByteRegister.C,
                InstructionElement.D => ByteRegister.D,
                InstructionElement.E => ByteRegister.E,
                InstructionElement.F => ByteRegister.F,
                InstructionElement.H => ByteRegister.H,
                InstructionElement.L => ByteRegister.L,
                InstructionElement.IXh => ByteRegister.IXh,
                InstructionElement.IYh => ByteRegister.IYh,
                InstructionElement.IXl => ByteRegister.IXl,
                InstructionElement.IYl => ByteRegister.IYl,
                InstructionElement.I => ByteRegister.I,
                InstructionElement.R => ByteRegister.R,
                _ => ByteRegister.None
            };

            return register;
        }

        public static bool IsAddressFromIndexAndOffset(this InstructionElement argument)
        {
            return argument == InstructionElement.AddressFromIXAndOffset || argument == InstructionElement.AddressFromIYAndOffset;
        }
    }
}
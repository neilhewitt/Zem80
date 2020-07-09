using System;
using System.Diagnostics.SymbolStore;

namespace Z80.Core
{
    public static class InstructionElementExtensions
    {
        public static WordRegister AsWordRegister(this InstructionElement argument)
        {
            WordRegister register;
            argument = argument switch
            {
                InstructionElement.AddressFromIXAndOffset => InstructionElement.IX,
                InstructionElement.AddressFromIYAndOffset => InstructionElement.IY,
                var a when (a >= InstructionElement.AddressFromHL && a <= InstructionElement.AddressFromSP) => (InstructionElement)(a - 6),
                _ => argument
            };

            string name = argument.ToString();
            if (!Enum.TryParse<WordRegister>(name, out register))
            {
                register = WordRegister.None;
            }

            return register;
        }

        public static ByteRegister AsByteRegister(this InstructionElement argument)
        {
            ByteRegister register;
            if (!Enum.TryParse<ByteRegister>(argument.ToString(), out register))
            {
                register = ByteRegister.None;
            }

            return register;
        }

        public static bool IsByteRegister(this InstructionElement argument)
        {
            return argument.AsByteRegister() != ByteRegister.None;
        }

        public static bool IsWordRegister(this InstructionElement argument)
        {
            return argument.AsWordRegister() != WordRegister.None;
        }

        public static bool IsAddressFromWordRegister(this InstructionElement argument)
        {
            return argument >= InstructionElement.AddressFromHL && argument <= InstructionElement.AddressFromSP;
        }

        public static bool IsAddressFromImmediateWord(this InstructionElement argument)
        {
            return argument == InstructionElement.AddressFromWordValue;
        }

        public static bool IsAddressFromIndexAndOffset(this InstructionElement argument)
        {
            return argument == InstructionElement.AddressFromIXAndOffset || argument == InstructionElement.AddressFromIYAndOffset;
        }

        public static bool IsAddress(this InstructionElement argument)
        {
            return argument.IsAddressFromImmediateWord() || argument.IsAddressFromWordRegister() || argument.IsAddressFromIndexAndOffset();
        }

        public static bool Is8Bit(this InstructionElement argument)
        {
            return argument == InstructionElement.ByteValue || argument == InstructionElement.DisplacementValue ||
                argument == InstructionElement.PortNumberFromByteValue || argument == InstructionElement.BitIndex ||
                (argument >= InstructionElement.A && argument <= InstructionElement.IYl) ||
                argument == InstructionElement.PortNumberFromC;
        }

        public static bool Is16Bit(this InstructionElement argument)
        {
            return argument == InstructionElement.WordValue || 
                (argument >= InstructionElement.AF && argument <= InstructionElement.AddressFromIYAndOffset);
        }
    }
}
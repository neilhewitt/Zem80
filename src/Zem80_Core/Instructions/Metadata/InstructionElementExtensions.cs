using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;

namespace Zem80.Core.Instructions
{
    public static class InstructionElementExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WordRegister AsWordRegister(this InstructionElement argument)
        {
            argument = argument switch
            {
                InstructionElement.AddressFromIXAndOffset => InstructionElement.IX,
                InstructionElement.AddressFromIYAndOffset => InstructionElement.IY,
                var a when (a >= InstructionElement.AddressFromHL && a <= InstructionElement.AddressFromSP) => (InstructionElement)(a - 6),
                _ => argument
            };

            string name = argument.ToString();
            if (!Enum.TryParse<WordRegister>(name, out WordRegister register))
            {
                register = WordRegister.None;
            }

            return register;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ByteRegister AsByteRegister(this InstructionElement argument)
        {
            if (!Enum.TryParse<ByteRegister>(argument.ToString(), out ByteRegister register))
            {
                register = ByteRegister.None;
            }

            return register;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressFromIndexAndOffset(this InstructionElement argument)
        {
            return argument == InstructionElement.AddressFromIXAndOffset || argument == InstructionElement.AddressFromIYAndOffset;
        }
    }
}
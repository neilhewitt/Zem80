namespace Z80.Core
{
    public static class OpcodeHelper
    {
        public static byte GetBitIndex(this Instruction instruction)
        {
            return instruction.Opcode.GetByteFromBits(3, 3);
        }

        //public static ByteRegister GetDestinationByteRegister(this Instruction instruction)
        //{
        //    return (ByteRegister)instruction.Opcode.GetByteFromBits(3, 3);
        //}

        public static ByteRegister GetIOByteRegister(this Instruction instruction)
        {
            return (ByteRegister)instruction.Opcode.GetByteFromBits(3, 3);
        }

        public static ByteRegister GetByteRegister(this Instruction instruction)
        {
            ByteRegister register = (ByteRegister)instruction.Opcode.GetByteFromBits(0, 3);
            return register switch
            {
                var r when (instruction.Prefix == InstructionPrefix.DD && r == ByteRegister.H) => ByteRegister.IXh,
                var r when (instruction.Prefix == InstructionPrefix.DD && r == ByteRegister.L) => ByteRegister.IXl,
                var r when (instruction.Prefix == InstructionPrefix.FD && r == ByteRegister.H) => ByteRegister.IYh,
                var r when (instruction.Prefix == InstructionPrefix.FD && r == ByteRegister.L) => ByteRegister.IYl,
                _ => register
            };
        }

        public static WordRegister GetWordRegister(this Instruction instruction)
        {
            WordRegister register = instruction.Opcode.GetByteFromBits(4, 2) switch
            {
                0x00 => WordRegister.BC,
                0x01 => WordRegister.DE,
                0x02 => WordRegister.HL,
                0x03 => WordRegister.SP,
                _ => WordRegister.None
            };

            // special cases
            if ((instruction.Microcode is PUSH || instruction.Microcode is POP) && register == WordRegister.SP)
            {
                return WordRegister.AF;
            }

            if (instruction.HLIX && register == WordRegister.HL) return WordRegister.IX;
            if (instruction.HLIY && register == WordRegister.HL) return WordRegister.IY;

            return register;
        }

        public static WordRegister HLOrIXOrIY(this Instruction instruction)
        {
            return instruction.HLIX ? WordRegister.IX : instruction.HLIY ? WordRegister.IY : WordRegister.HL;
        }
    }
}

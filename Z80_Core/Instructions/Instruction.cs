using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Instruction
    {
        public static Instruction NOP
        {
            get
            {
                Instruction nop = new Instruction(0x00, 0x00, "NOP", 1, 1);
                return nop;
            }
        }

        public static Instruction Build(byte opcode, InstructionInfo info)
        {
            bool indexed = (info.Prefix == InstructionPrefix.DD || info.Prefix == InstructionPrefix.FD);
            bool extendedIndexed = (info.Prefix == InstructionPrefix.DDCB || info.Prefix == InstructionPrefix.FDCB);

            Instruction instruction = new Instruction(
                info.Prefix,
                opcode,
                info.Mnemonic,
                info.Size,
                byte.Parse(info.Timing.Split('/')[0])
                );

            if (info.Timing.Contains('/'))
            {
                instruction.ClockCyclesOnTrue = byte.Parse(info.Timing.Split('/')[1]);
            }

            if (indexed || extendedIndexed)
            {
                instruction.IndexingMode = ((info.Prefix == InstructionPrefix.DDCB || info.Prefix == InstructionPrefix.DD) ? IndexingMode.IX : IndexingMode.IY);
            }

            return instruction;
        }

        public InstructionPrefix Prefix { get; private set; }
        public byte Opcode { get; private set; }
        public string Mnemonic { get; private set; }
        public byte SizeInBytes { get; private set; }
        public byte ClockCycles { get; private set; }
        public byte? ClockCyclesOnTrue { get; private set; }
        public IndexingMode IndexingMode { get; set; }

        public Instruction(InstructionPrefix prefix, byte opcode, string mnemonic, byte sizeInBytes, byte clockCycles)
        {
            Prefix = prefix;
            Opcode = opcode;
            Mnemonic = mnemonic;
            SizeInBytes = sizeInBytes;
            ClockCycles = clockCycles;
            IndexingMode = IndexingMode.None;
        }
    }
}

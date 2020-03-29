using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Z80.Core
{
    public class Instruction
    {
        public static Instruction NOP => InstructionSet.Instructions[InstructionPrefix.Unprefixed][0x00];

        public static Instruction Find(byte opcode, InstructionPrefix prefix)
        {
            try
            {
                return InstructionSet.Instructions[prefix][opcode];
            }
            catch (KeyNotFoundException)
            {
                throw new InstructionNotFoundException($"Instruction not found ({ prefix.ToString() }::{opcode}.)");
            }
        }

        public static Instruction FindByMnemonic(string mnemonic)
        {
            try
            {
                return InstructionSet.Instructions.Values.SelectMany(x => x.Values).Single(x => x.Mnemonic == (mnemonic));
            }
            catch (InvalidOperationException)
            {
                throw new InstructionNotFoundException($"Instruction not found ({mnemonic})");
            }
        }

        public InstructionPrefix Prefix { get; private set; }
        public byte Opcode { get; private set; }
        public string Mnemonic { get; private set; }
        public ArgumentType Argument1 { get; private set; }
        public ArgumentType Argument2 { get; private set; }
        public ModifierType Modifier { get; private set; }
        public byte SizeInBytes { get; private set; }
        public byte ClockCycles { get; private set; }
        public byte? ClockCyclesConditional { get; private set; }
        public byte? BitIndex { get; private set; }
        public ByteRegister OperandByteRegister { get; private set; }
        public bool ReplacesHLWithIX => Prefix == InstructionPrefix.DDCB;
        public bool ReplacesHLWithIY => Prefix == InstructionPrefix.FDCB;
        internal IMicrocode Microcode { get; private set; }

        internal Instruction(InstructionPrefix prefix, byte opcode, string mnemonic, ArgumentType argument1, ArgumentType argument2, ModifierType modifier, byte size, byte clockCycles, 
            byte? clockCyclesConditional, IMicrocode implementation = null)
        {
            Prefix = prefix;
            Opcode = opcode;
            Mnemonic = mnemonic;
            Argument1 = argument1;
            Argument2 = argument2;
            Modifier = modifier;
            SizeInBytes = size;
            ClockCycles = clockCycles;
            ClockCyclesConditional = clockCyclesConditional;
            BitIndex = Modifier switch
            {
                var m when (m == ModifierType.Bit || m == ModifierType.BitAndRegister) => opcode.GetByteFromBits(3, 3),
                _ => null
            };
            OperandByteRegister = Modifier switch
            {
                var m when (m == ModifierType.Bit || m == ModifierType.WordRegister || m == ModifierType.None) => ByteRegister.None,
                ModifierType.InputRegister => (ByteRegister)opcode.GetByteFromBits(3, 3),
                _ => (ByteRegister)opcode.GetByteFromBits(0, 3)
            };

            if (implementation != null)
            {
                Microcode = implementation;
            }
            else
            {
                // this is expensive, but only done once at startup; binds the Instruction directly to the method instance implementing it
                Type microcodeType = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(x => x.Name == mnemonic.Split(' ')[0]);
                Microcode = (IMicrocode)Activator.CreateInstance(microcodeType);
            }
        }
    }
}

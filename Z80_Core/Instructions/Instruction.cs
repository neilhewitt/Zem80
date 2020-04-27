using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;

namespace Z80.Core
{
    public class Instruction
    {
        public InstructionPrefix Prefix { get; private set; }
        public byte Opcode { get; private set; }
        public int FullOpcode { get; private set; }
        public string Mnemonic { get; private set; }
        public ArgumentType Argument1 { get; private set; }
        public ArgumentType Argument2 { get; private set; }
        public ModifierType Modifier { get; private set; }
        public Condition? Condition { get; private set; }
        public byte SizeInBytes { get; private set; }
        public IReadOnlyList<MachineCycle> MachineCycles { get; private set; }
        public byte ClockCycles { get; private set; }
        public byte? ClockCyclesConditional { get; private set; }
        public byte? BitIndex { get; private set; }
        public ByteRegister OperandRegister { get; private set; }
        public bool HLIX => Prefix == InstructionPrefix.DDCB;
        public bool HLIY => Prefix == InstructionPrefix.FDCB;
        public bool HasOperandDataReadHigh4 { get; private set; }
        public bool HasMemoryRead4 { get; private set; }
        public bool HasMemoryWrite5 { get; private set; }
        public IMicrocode Microcode { get; private set; }

        public Instruction(string opcode, string mnemonic, ArgumentType argument1, ArgumentType argument2, ModifierType modifier, byte sizeInBytes, MachineCycle[] machineCycles, IMicrocode microcode = null)
        {
            FullOpcode = int.Parse(opcode, NumberStyles.HexNumber);
            Prefix = opcode.Length == 2 ? InstructionPrefix.Unprefixed : (InstructionPrefix)Enum.Parse(typeof(InstructionPrefix), opcode[..^2], true);
            Opcode = byte.Parse(opcode[^2..], NumberStyles.HexNumber); ;
            Mnemonic = mnemonic;
            if (mnemonic.Contains(' '))
            {
                string right = mnemonic.Split(' ',',')[1];
                if (Enum.TryParse<Condition>(right, out Condition condition))
                {
                    Condition = condition;
                }
            }

            Argument1 = argument1;
            Argument2 = argument2;
            Modifier = modifier;
            SizeInBytes = sizeInBytes;

            BitIndex = Modifier switch
            {
                var m when (m == ModifierType.Bit || m == ModifierType.BitAndRegister) => Opcode.GetByteFromBits(3, 3),
                _ => null
            };

            OperandRegister = Modifier switch
            {
                var m when (m == ModifierType.Bit || m == ModifierType.WordRegister || m == ModifierType.None) => ByteRegister.None,
                ModifierType.InputRegister => (ByteRegister)Opcode.GetByteFromBits(3, 3),
                _ => (ByteRegister)Opcode.GetByteFromBits(0, 3)
            };

            MachineCycles = new List<MachineCycle>(machineCycles);
            ClockCycles = (byte)MachineCycles.Where(x => x.IsConditional == false).Sum(x => x.ClockCycles); // sum of machine cycles (except conditional ones)
            if (MachineCycles.Any(x => x.IsConditional == true || x.ClockCyclesAlternate.HasValue))
            {
                ClockCyclesConditional = (byte)MachineCycles.Sum(x => x.ClockCyclesAlternate.HasValue ? x.ClockCyclesAlternate : x.ClockCycles); // sum of all machine cycles (including conditional)
                HasOperandDataReadHigh4 = true;
            }
            if (MachineCycles.Any(x => x.Type == MachineCycleType.MemoryRead && x.ClockCycles == 4)) HasMemoryRead4 = true;
            if (MachineCycles.Any(x => x.Type == MachineCycleType.MemoryWrite && x.ClockCycles == 5)) HasMemoryWrite5 = true;

            // this is expensive, but only done once at startup; binds the Instruction directly to the method instance implementing it
            if (microcode == null)
            {
                Type microcodeType = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(x => x.Name == mnemonic.Split(' ')[0]);
                Microcode = (IMicrocode)Activator.CreateInstance(microcodeType);
            }
            else
            {
                Microcode = microcode;
            }
        }
    }
}

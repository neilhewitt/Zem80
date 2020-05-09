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
        public IReadOnlyList<MachineCycle> Timing { get; private set; }
        public TimingExceptions TimingExceptions { get; private set; }
        public byte? BitIndex { get; private set; }
        public ByteRegister OperandRegister { get; private set; }
        public bool HLIX => Prefix == InstructionPrefix.DDCB;
        public bool HLIY => Prefix == InstructionPrefix.FDCB;
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

            Timing = new List<MachineCycle>(machineCycles);
            bool odh4 = false, mr4 = false, mw5 = false;
            if (Mnemonic.StartsWith("CALL"))
            {
                // specifically for CALL instructions, the high byte operand read is 4 clock cycles rather than 3 *if* the condition is true (or there is no condition)
                odh4 = true;
            }
            if (Timing.Any(x => x.Type == MachineCycleType.MemoryRead && x.ClockCycles == 4)) mr4 = true;
            if (Timing.Any(x => x.Type == MachineCycleType.MemoryWrite && x.ClockCycles == 5)) mw5 = true;
            TimingExceptions = new TimingExceptions(odh4, mr4, mw5);

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

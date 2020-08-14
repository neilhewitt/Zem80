using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;

namespace Z80.Core
{
    public class Instruction
    {
        private bool HLIX => Prefix == InstructionPrefix.DD || Prefix == InstructionPrefix.DDCB;
        private bool HLIY => Prefix == InstructionPrefix.FD || Prefix == InstructionPrefix.FDCB;

        public InstructionPrefix Prefix { get; private set; }
        public byte Opcode { get; private set; }
        public string Mnemonic { get; private set; }
        public Condition Condition { get; private set; }
        public byte SizeInBytes { get; private set; }
        public IReadOnlyList<MachineCycle> Timing { get; private set; }
        public TimingExceptions TimingExceptions { get; private set; }
        public bool IsIndexed => HLIX || HLIY;
        public bool IsConditional => Condition != Condition.None;
        public IMicrocode Microcode { get; private set; }
        public InstructionElement Target { get; private set; }
        public InstructionElement Source { get; private set; }
        public InstructionElement Argument1 { get; private set; }
        public InstructionElement Argument2 { get; private set; }
        public bool TargetsByteRegister => Target >= InstructionElement.A && Target <= InstructionElement.IYl;
        public bool TargetsWordRegister => Target >= InstructionElement.AF && Target <= InstructionElement.SP;
        public bool TargetsByteInMemory => (Target >= InstructionElement.AddressFromHL && Target <= InstructionElement.AddressFromIYAndOffset);
        public string FullOpcode { get; private set; }
        public byte[] FullOpcodeBytes { get; private set; }
        public ByteRegister? CopyResultTo { get; private set; }

        internal int OpcodeAsInt => int.Parse(FullOpcode, NumberStyles.HexNumber);

        public Instruction(string opcode, string mnemonic, Condition condition, InstructionElement target, InstructionElement source, InstructionElement arg1, InstructionElement arg2, byte sizeInBytes, MachineCycle[] machineCycles, ByteRegister? copyResultTo = ByteRegister.None, IMicrocode microcode = null)
        {
            FullOpcode = opcode;
            FullOpcodeBytes = new byte[FullOpcode.Length / 2];
            FullOpcodeBytes[0] = byte.Parse(opcode.Substring(0,2), NumberStyles.HexNumber);
            if (opcode.Length == 4) FullOpcodeBytes[1] = byte.Parse(opcode.Substring(2,2), NumberStyles.HexNumber);
            if (opcode.Length == 6) FullOpcodeBytes[2] = byte.Parse(opcode.Substring(4,2), NumberStyles.HexNumber);
            CopyResultTo = copyResultTo;

            Prefix = opcode.Length == 2 ? InstructionPrefix.Unprefixed : (InstructionPrefix)Enum.Parse(typeof(InstructionPrefix), opcode[..^2], true);
            Opcode = byte.Parse(opcode[^2..], NumberStyles.HexNumber);
            
            Mnemonic = mnemonic;
            SizeInBytes = sizeInBytes;
            Target = target;
            Source = source;
            Argument1 = arg1;
            Argument2 = arg2;
            Condition = condition;

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

            // deal with timing + any exceptions
            Timing = new List<MachineCycle>(machineCycles);
            bool odh4 = false, mr4 = false, mw5 = false;
            if (Microcode is CALL)
            {
                // specifically for CALL instructions, the high byte operand read is 4 clock cycles rather than 3 *if* the condition is true (or there is no condition)
                odh4 = true;
            }
            if (Timing.Any(x => x.Type == MachineCycleType.MemoryRead && x.TStates == 4)) mr4 = true;
            if (Timing.Any(x => x.Type == MachineCycleType.MemoryWrite && x.TStates == 5)) mw5 = true;
            TimingExceptions = new TimingExceptions(odh4, mr4, mw5);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Zem80.Core.CPU;
using System.Buffers.Binary;

namespace Zem80.Core.CPU
{
    public class Instruction
    {
        public int Prefix { get; private set; }
        public byte LastOpcodeByte { get; private set; }
        public string OpcodeString { get; private set; }
        public byte[] OpcodeBytes { get; private set; }
        public string Mnemonic { get; private set; }
        public Condition Condition { get; private set; }
        public byte SizeInBytes { get; private set; }
        public InstructionMachineCycles MachineCycles { get; private set; }
        public bool IsIndexed { get; private set; }
        public bool IsConditional { get; private set; }
        public bool HasIntermediateDisplacementByte { get; private set; }
        public bool IsLoopingInstruction { get; private set; }
        public IMicrocode Microcode { get; private set; }
        public InstructionElement Target { get; private set; }
        public InstructionElement Source { get; private set; }
        public InstructionElement Argument1 { get; private set; }
        public InstructionElement Argument2 { get; private set; }
        public WordRegister IndexedRegister { get; private set; }
        public bool TargetsByteRegister { get; private set; }
        public bool TargetsWordRegister { get; private set; }
        public bool TargetsByteInMemory { get; private set; }
        public ByteRegister? CopyResultTo { get; private set; }

        public Instruction(string fullOpcode, string mnemonic, Condition condition, InstructionElement target, InstructionElement source, InstructionElement arg1, InstructionElement arg2, 
            byte sizeInBytes, IEnumerable<MachineCycle> machineCycles, ByteRegister? copyResultTo = ByteRegister.None, IMicrocode microcode = null)
        {
            CopyResultTo = copyResultTo;

            if (int.TryParse(fullOpcode[..^2], NumberStyles.HexNumber, null, out int prefix)) Prefix = prefix;
            LastOpcodeByte = byte.Parse(fullOpcode[^2..], NumberStyles.HexNumber);
            OpcodeString = fullOpcode;
            
            byte[] opcodeBytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(opcodeBytes, int.Parse(fullOpcode, NumberStyles.HexNumber));
            OpcodeBytes = opcodeBytes.Take(fullOpcode.Length / 2).ToArray();
            
            Mnemonic = mnemonic;
            SizeInBytes = sizeInBytes;
            Target = target;
            Source = source;
            Argument1 = arg1;
            Argument2 = arg2;
            Condition = condition;

            TargetsByteRegister = Target >= InstructionElement.A && Target <= InstructionElement.IYl;
            TargetsWordRegister = Target >= InstructionElement.AF && Target <= InstructionElement.SP;
            TargetsByteInMemory = Target >= InstructionElement.AddressFromHL && Target <= InstructionElement.AddressFromIYAndOffset;
            IndexedRegister = WordRegister.None;
            IsIndexed = Source.IsAddressFromIndexAndOffset() || Target.IsAddressFromIndexAndOffset();
            HasIntermediateDisplacementByte = Prefix == 0xDDCB || Prefix == 0xFDCB;
            IsConditional = Condition != Condition.None;
            IsLoopingInstruction = (new[] { "CPDR", "CPIR", "INDR", "INIR", "OTDR", "OTIR", "LDDR", "LDIR" }).Contains(mnemonic);
            
            if (IsIndexed)
            {
                IndexedRegister = Source.IsAddressFromIndexAndOffset() ? Source.AsWordRegister() : Target.AsWordRegister();
            }

            // this is expensive, but only done once at startup; binds the Instruction directly to the method instance implementing it
            if (microcode == null)
            {
                Type microcodeType = Assembly.GetExecutingAssembly().GetTypes().
                    SingleOrDefault(x => x.Name == mnemonic.Split(' ')[0].TrimEnd('0','1','2','3','4','5','6','7')); // cater for duplicate instructions (NEG2 etc)
                Microcode = (IMicrocode)Activator.CreateInstance(microcodeType);
            }
            else
            {
                Microcode = microcode;
            }

            // deal with timing + any exceptions
            MachineCycles = new InstructionMachineCycles(machineCycles);
        }
    }
}

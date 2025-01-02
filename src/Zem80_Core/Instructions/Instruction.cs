﻿using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Zem80.Core.CPU
{
    public class Instruction
    {
        public int Opcode { get; private set; }
        public byte[] OpcodeBytes { get; private set; }
        public int Prefix { get; private set; }
        public byte LastOpcodeByte { get; private set; }
        public string Mnemonic { get; private set; }
        public Condition Condition { get; private set; }
        public byte SizeInBytes { get; private set; }
        public InstructionMachineCycles MachineCycles { get; private set; }
        public bool IsIndexed { get; private set; }
        public bool IsConditional { get; private set; }
        public bool AccessesMemory { get; private set; }
        public bool PerformsIO { get; private set; }
        public bool HasIntermediateDisplacementByte { get; private set; }
        public bool IsLoopingInstruction { get; private set; }
        public byte BitIndex { get; private set; }
        public IMicrocode Microcode { get; private set; }
        public InstructionElement Target { get; private set; }
        public InstructionElement Source { get; private set; }
        public InstructionElement Argument1 { get; private set; }
        public InstructionElement Argument2 { get; private set; }
        public WordRegister IndexedRegister { get; private set; }
        public bool TargetsByteRegister { get; private set; }
        public bool TargetsWordRegister { get; private set; }
        public bool TargetsByteInMemory { get; private set; }
        public bool CopiesResultToRegister { get; private set; }
        public ByteRegister CopyResultTo { get; private set; }

        public Instruction(int opcode, string mnemonic, Condition condition, InstructionElement target, InstructionElement source, InstructionElement arg1, InstructionElement arg2, 
            byte sizeInBytes, IEnumerable<MachineCycle> machineCycles, ByteRegister copyResultTo = ByteRegister.None, IMicrocode microcode = null)
        {
            CopiesResultToRegister = copyResultTo != ByteRegister.None;
            CopyResultTo = copyResultTo;

            Opcode = opcode;
            Prefix = opcode >> 8; // prefix is all but the last byte

            // split opcode into bytes
            int opcodeLength = opcode > 0xFFFF ? 3 : opcode > 0xFF ? 2 : 1;
            OpcodeBytes = new byte[opcodeLength];
            for (int i = 0; i < opcodeLength; i++)
            {
                OpcodeBytes[i] = (byte)(opcode >> (8 * (opcodeLength - i - 1))); // shift right to remove the byte/s we've already processed, then convert to byte to shorten it to 8 bits
            }

            LastOpcodeByte = OpcodeBytes[opcodeLength - 1];
            BitIndex = LastOpcodeByte.GetByteFromBits(3, 3);

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
            HasIntermediateDisplacementByte = Prefix == 0xDDCB || Prefix == 0xFDCB;
            IsConditional = Condition != Condition.None;
            IsLoopingInstruction = (new[] { "CPDR", "CPIR", "INDR", "INIR", "OTDR", "OTIR", "LDDR", "LDIR" }).Contains(mnemonic);

            // deal with timing + any exceptions
            MachineCycles = new InstructionMachineCycles(machineCycles);
            AccessesMemory = MachineCycles.Cycles.Any(x => x.HasMemoryAccess);
            PerformsIO = MachineCycles.Cycles.Any(x => x.HasIO);
            IsIndexed = Source.IsAddressFromIndexAndOffset() || Target.IsAddressFromIndexAndOffset();
            IndexedRegister = IsIndexed ? (Source.IsAddressFromIndexAndOffset() ? Source.AsWordRegister() : Target.AsWordRegister()) : WordRegister.None;

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
        }
    }
}

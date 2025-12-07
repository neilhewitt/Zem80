using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZexNext.Core
{
    [Flags]
    public enum FlagState
    {
        None = 0,
        Carry = 1,
        Subtract = 2,
        ParityOverflow = 4,
        X = 8,
        HalfCarry = 16,
        Y = 32,
        Zero = 64,
        Sign = 128
    }

    public struct TestStateDiff
    {
        public bool Opcode;
        public bool A;
        public bool F;
        public bool BC;
        public bool DE;
        public bool HL;
        public bool IX;
        public bool IY;
        public bool SP;
        public bool PC;
        public bool Data;
    }

    public class TestState
    {
        public byte[] Opcode { get; private set; }
        public string Mnemonic { get; private set; }
        public byte[] Data { get; private set; }
        public ushort DataAddress => 0x103;
        public byte A => (byte)(AF / 256);
        public byte F => (byte)(AF % 256);
        public ushort AF { get; private set; }
        public ushort BC { get; private set; }
        public ushort DE { get; private set; }
        public ushort HL { get; private set; }
        public ushort IX { get; private set; }
        public ushort IY { get; private set; }
        public ushort SP { get; private set; }
        public ushort PC { get; private set; }

        public FlagState Flags => (FlagState)F;

        public void MaskFlags(byte mask)
        {
            byte flags = F;
            flags = (byte)(flags & mask);
            AF = (ushort)((A * 256) + flags);
        }

        public override bool Equals(object obj)
        {
            return obj is TestState state &&
                   Opcode.SequenceEqual(state.Opcode) &&
                   AF == state.AF &&
                   BC == state.BC &&
                   DE == state.DE &&
                   HL == state.HL &&
                   IX == state.IX &&
                   IY == state.IY &&
                   SP == state.SP &&
                   PC == state.PC &&
                   Data.SequenceEqual(state.Data);
        }

        public TestStateDiff Diff(TestState state)
        {
            TestStateDiff diff = new TestStateDiff();
            diff.Opcode = !(Opcode.SequenceEqual(state.Opcode));
            diff.A = state.A != A;
            diff.F = state.F != F;
            diff.BC = state.BC != BC;
            diff.DE = state.DE != DE;
            diff.HL = state.HL != HL;
            diff.IX = state.IX != IX;
            diff.IY = state.IY != IY;
            diff.SP = state.SP != SP;
            diff.PC = state.PC != PC;
            diff.Data = !state.Data.SequenceEqual(Data);
            return diff;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Opcode);
            hash.Add(AF);
            hash.Add(BC);
            hash.Add(DE);
            hash.Add(HL);
            hash.Add(IX);
            hash.Add(IY);
            hash.Add(SP);
            hash.Add(PC);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append(Mnemonic.PadRight(4));
            foreach(byte opcodeByte in Opcode)
            {
                output.Append($" {opcodeByte.ToString("X2")}");
            }
            output.Append(" >> ");
            addValue(output, nameof(AF), AF);
            addValue(output, nameof(BC), BC);
            addValue(output, nameof(DE), DE);
            addValue(output, nameof(HL), HL);
            addValue(output, nameof(IX), IX);
            addValue(output, nameof(IY), IY);
            addValue(output, nameof(SP), SP);
            addValue(output, nameof(PC), PC);

            output.Append("Data:");
            for (int i = 0; i < Data.Length; i++)
            {
                output.Append(Data[i].ToString("x2"));
                output.Append(",");
            }

            return output.ToString().TrimEnd(',', ' ');

            void addValue(StringBuilder builder, string label, ushort value)
            {
                builder.Append(label);
                builder.Append(":");
                builder.Append(value.ToString("x4"));
                builder.Append(", ");
            }
        }

        public TestState(TestState original) : 
            this(original.Opcode, original.Mnemonic, original.Data, original.AF, original.BC, original.DE, original.HL, original.IX, original.IY, original.SP, original.PC)
        {
        }

        public TestState(byte[] opcode, string mnemonic, byte[] data, ushort af, ushort bc, ushort de, ushort hl, ushort ix, ushort iy, ushort sp, ushort pc)
        {
            Opcode = new byte[4];
            Array.Copy(opcode, Opcode, opcode.Length <= 4 ? opcode.Length : 4);
            
            if (opcode.Length == 4)
            {
                // special case - remove displacement byte (3rd byte) from opcode pattern
                byte[] shortenedOpcode = new byte[3];
                shortenedOpcode[0] = opcode[0];
                shortenedOpcode[1] = opcode[1];
                shortenedOpcode[2] = opcode[3];
                opcode = shortenedOpcode;
            }

            Mnemonic = mnemonic;
            Data = data;

            AF = af;
            BC = bc;
            DE = de;
            HL = hl;
            IX = ix;
            IY = iy;
            SP = sp;
            PC = pc;
        }
    }
}

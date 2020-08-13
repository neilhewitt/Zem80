﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZexNext.Core
{
    public class TestState
    {
        public byte[] Opcode { get; private set; }
        public string OpcodeString { get; private set; }
        public string Mnemonic { get; private set; }
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
                   PC == state.PC;
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
            output.Append(Mnemonic);
            output.Append("; ");
            addValue(output, nameof(AF), AF);
            addValue(output, nameof(BC), BC);
            addValue(output, nameof(DE), DE);
            addValue(output, nameof(HL), HL);
            addValue(output, nameof(IX), IX);
            addValue(output, nameof(IY), IY);
            addValue(output, nameof(SP), SP);
            addValue(output, nameof(PC), PC);

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
            this(original.Opcode, original.Mnemonic, original.AF, original.BC, original.DE, original.HL, original.IX, original.IY, original.SP, original.PC)
        {
        }

        public TestState(byte[] opcode, string mnemonic, ushort af, ushort bc, ushort de, ushort hl, ushort ix, ushort iy, ushort sp, ushort pc)
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

            OpcodeString = String.Join(null, opcode.Select(x => x.ToString("X2")));
            Mnemonic = mnemonic;

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

using System;
using System.Collections.Generic;
using System.Numerics;
using Z80.Core;

namespace ZexallCSharp
{
    public class TestVector
    {
        private byte[] _bytes = new byte[20];

        public byte[] Bytes => _bytes;
        public InstructionBytes Instruction
        {
            get { return new InstructionBytes(_bytes[0], _bytes[1], _bytes[2], _bytes[3]); }
            set { _bytes[0] = value.First; _bytes[1] = value.Second; _bytes[2] = value.Third; _bytes[3] = value.Fourth; }
        }
        public ushort MemOp { get { return (ushort)(_bytes[4] + (_bytes[5] * 256)); } set { _bytes[4] = (byte)(value % 256); _bytes[5] = (byte)(value / 256); } }
        public ushort IY { get { return (ushort)(_bytes[6] + (_bytes[7] * 256)); } set { _bytes[6] = (byte)(value % 256); _bytes[7] = (byte)(value / 256); } }
        public ushort IX { get { return (ushort)(_bytes[8] + (_bytes[9] * 256)); } set { _bytes[8] = (byte)(value % 256); _bytes[9] = (byte)(value / 256); } }
        public ushort HL { get { return (ushort)(_bytes[10] + (_bytes[11] * 256)); } set { _bytes[10] = (byte)(value % 256); _bytes[11] = (byte)(value / 256); } }
        public ushort DE { get { return (ushort)(_bytes[12] + (_bytes[13] * 256)); } set { _bytes[12] = (byte)(value % 256); _bytes[13] = (byte)(value / 256); } }
        public ushort BC { get { return (ushort)(_bytes[14] + (_bytes[15] * 256)); } set { _bytes[14] = (byte)(value % 256); _bytes[15] = (byte)(value / 256); } }
        public byte F { get { return _bytes[16]; } set { _bytes[16] = value; } }
        public byte A { get { return _bytes[17]; } set { _bytes[17] = value; } }
        public ushort SP { get { return (ushort)(_bytes[18] + (_bytes[19] * 256)); } set { _bytes[18] = (byte)(value % 256); _bytes[19] = (byte)(value / 256); } }

        public TestState State => new TestState(IY, IX, HL, DE, BC, F, A, SP);

        public int OneBits =>
            Instruction.First.CountBits(true) +
            Instruction.Second.CountBits(true) +
            Instruction.Third.CountBits(true) +
            Instruction.Fourth.CountBits(true) +
            MemOp.LowByte().CountBits(true) +
            MemOp.HighByte().CountBits(true) +
            IY.LowByte().CountBits(true) +
            IY.HighByte().CountBits(true) +
            IX.LowByte().CountBits(true) +
            IX.HighByte().CountBits(true) +
            HL.LowByte().CountBits(true) +
            HL.HighByte().CountBits(true) +
            DE.LowByte().CountBits(true) +
            DE.HighByte().CountBits(true) +
            BC.LowByte().CountBits(true) +
            BC.HighByte().CountBits(true) +
            F.CountBits(true) +
            A.CountBits(true) +
            SP.LowByte().CountBits(true) +
            SP.HighByte().CountBits(true);

        public static TestVector operator &(TestVector left, TestVector right)
        {
            TestVector result = new TestVector();

            InstructionBytes instruction = new InstructionBytes(
                (byte)(left.Instruction.First & right.Instruction.First),
                (byte)(left.Instruction.Second & right.Instruction.Second),
                (byte)(left.Instruction.Third & right.Instruction.Third),
                (byte)(left.Instruction.Fourth & right.Instruction.Fourth));
            result.Instruction = instruction;
            result.MemOp = (ushort)(left.MemOp & right.MemOp);
            result.IY = (ushort)(left.IY & right.IY);
            result.IX = (ushort)(left.IX & right.IX);
            result.HL = (ushort)(left.HL & right.HL);
            result.DE = (ushort)(left.DE & right.DE);
            result.BC = (ushort)(left.BC & right.BC);
            result.F = (byte)(left.F & right.F);
            result.A = (byte)(left.A & right.A);
            result.SP = (ushort)(left.SP & right.SP);
            return result;
        }

        public static TestVector operator ^(TestVector left, TestVector right)
        {
            TestVector result = new TestVector();
            InstructionBytes instruction = new InstructionBytes(
                (byte)(left.Instruction.First ^ right.Instruction.First),
                (byte)(left.Instruction.Second ^ right.Instruction.Second),
                (byte)(left.Instruction.Third ^ right.Instruction.Third),
                (byte)(left.Instruction.Fourth ^ right.Instruction.Fourth));
            result.Instruction = instruction;
            result.MemOp = (ushort)(left.MemOp ^ right.MemOp);
            result.IY = (ushort)(left.IY ^ right.IY);
            result.IX = (ushort)(left.IX ^ right.IX);
            result.HL = (ushort)(left.HL ^ right.HL);
            result.DE = (ushort)(left.DE ^ right.DE);
            result.BC = (ushort)(left.BC ^ right.BC);
            result.F = (byte)(left.F ^ right.F);
            result.A = (byte)(left.A ^ right.A);
            result.SP = (ushort)(left.SP ^ right.SP);
            return result;
        }

        public void SetValue(int valueIndex, dynamic value)
        {
            switch (valueIndex)
            {
                case 0: _bytes[0] = (byte)value; break;
                case 1: _bytes[1] = (byte)value; break;
                case 2: _bytes[2] = (byte)value; break;
                case 3: _bytes[3] = (byte)value; break;
                case 4: MemOp = (ushort)value; break;
                case 5: IY = (ushort)value; break;
                case 6: IX = (ushort)value; break;
                case 7: HL = (ushort)value; break;
                case 8: DE = (ushort)value; break;
                case 9: BC = (ushort)value; break;
                case 10: F = (byte)value; break;
                case 11: A = (byte)value; break;
                case 12: SP = (ushort)value; break;
            }
        }

        public dynamic GetValue(int valueIndex)
        {
            return valueIndex switch
            {
                0 => Instruction.First,
                1 => Instruction.Second,
                2 => Instruction.Third,
                3 => Instruction.Fourth,
                4 => MemOp,
                5 => IY,
                6 => IX,
                7 => HL,
                8 => DE,
                9 => BC,
                10 => F,
                11 => A,
                12 => SP,
                _ => null
            };
        }

        public override string ToString()
        {
            return ToString(false, false);
        }

        public string ToString(bool showDividers, bool showLabels)
        {
            return 
                (showLabels ? "Instruction.First:" : "") + Convert.ToString(Instruction.First, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "Instruction.Second:" : "") + Convert.ToString(Instruction.Second, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "Instruction.Third:" : "") + Convert.ToString(Instruction.Third, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "Instruction.Fourth:" : "") + Convert.ToString(Instruction.Fourth, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "MemOp:" : "") + Convert.ToString(MemOp, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "IY:" : "") + Convert.ToString(IY, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "IX:" : "") + Convert.ToString(IX, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "HL:" : "") + Convert.ToString(HL, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "DE:" : "") + Convert.ToString(DE, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "BC:" : "") + Convert.ToString(BC, 2).PadLeft(16, '0') + (showDividers ? "|" : "") +
                (showLabels ? "F:" : "") + Convert.ToString(F, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "A:" : "") + Convert.ToString(A, 2).PadLeft(8, '0') + (showDividers ? "|" : "") +
                (showLabels ? "SP:" : "") + Convert.ToString(SP, 2).PadLeft(16, '0');
        }

        public TestVector()
        {
        }
        public TestVector(string binary)
        {
            if (binary.Length != 160) throw new ArgumentException();

            Instruction = new InstructionBytes(
                Convert.ToByte(binary[0..8], 2),
                Convert.ToByte(binary[8..16], 2),
                Convert.ToByte(binary[16..24], 2),
                Convert.ToByte(binary[24..32], 2)
                );
            MemOp = Convert.ToUInt16(binary[32..48], 2);
            IY = Convert.ToUInt16(binary[48..64], 2);
            IX = Convert.ToUInt16(binary[64..80], 2);
            HL = Convert.ToUInt16(binary[80..96], 2);
            DE = Convert.ToUInt16(binary[96..112], 2);
            BC = Convert.ToUInt16(binary[112..128], 2);
            F = Convert.ToByte(binary[128..136], 2);
            A = Convert.ToByte(binary[136..144], 2);
            SP = Convert.ToUInt16(binary[144..160], 2);
        }

        public TestVector(TestVector initialVector)
        {
            Instruction = initialVector.Instruction;
            MemOp = initialVector.MemOp;
            IY = initialVector.IY;
            IX = initialVector.IX;
            HL = initialVector.HL;
            DE = initialVector.DE;
            BC = initialVector.BC;
            F = initialVector.F;
            A = initialVector.A;
            SP = initialVector.SP;
        }
    }
}

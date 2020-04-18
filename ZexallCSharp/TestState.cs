using Z80.Core;

namespace ZexallCSharp
{
    public class TestState
    {
        public ushort IY { get; set; }
        public ushort IX { get; set; }
        public ushort HL { get; set; }
        public ushort DE { get; set; }
        public ushort BC { get; set; }
        public byte F { get; set; }
        public byte A { get; set; }
        public ushort SP { get; set; }

        public byte[] Bytes => new byte[] {
            IY.LowByte(), IY.HighByte(),
            IX.LowByte(), IX.HighByte(),
            HL.LowByte(), HL.HighByte(),
            DE.LowByte(), DE.HighByte(),
            BC.LowByte(), BC.HighByte(),
            F, A,
            SP.LowByte(), SP.HighByte()
        };

        public TestState(ushort iy, ushort ix, ushort hl, ushort de, ushort bc, byte f, byte a, ushort sp)
        {
            IY = iy;
            IX = ix;
            HL = hl;
            DE = de;
            BC = bc;
            F = f;
            A = a;
            SP = sp;
        }
    }
}

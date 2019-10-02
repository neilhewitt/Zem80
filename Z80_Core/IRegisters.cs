namespace Z80.Core
{
    public interface IRegisters
    {
        byte A { get; set; }
        ushort AF { get; }
        byte B { get; set; }
        ushort BC { get; set; }
        byte C { get; set; }
        byte D { get; set; }
        ushort DE { get; set; }
        byte E { get; set; }
        byte F { get; }
        byte H { get; set; }
        ushort HL { get; set; }
        byte I { get; set; }
        ushort IX { get; set; }
        ushort IY { get; set; }
        byte L { get; set; }
        ushort PC { get; }
        byte R { get; set; }
        ushort SP { get; set; }

        void ExchangeAF();
        void ExchangeBCDEHL();
        Registers Snapshot();
    }
}
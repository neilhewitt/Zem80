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
        byte F { get; set; }
        byte H { get; set; }
        ushort HL { get; set; }
        byte I { get; set; }
        ushort IX { get; set; }
        ushort IY { get; set; }
        byte IXh { get; set; }
        byte IXl { get; set; }
        byte IYh { get; set; }
        byte IYl { get; set; }
        byte L { get; set; }
        ushort PC { get; set; }
        byte R { get; set; }
        ushort SP { get; set; }

        RegisterFlags Flags { get; }

        byte RegisterByIndex(RegisterIndex register);
        byte RegisterByOpcode(byte opcode);

        void ExchangeAF();
        void ExchangeBCDEHL();
        Registers Snapshot();
        void Clear();
    }
}
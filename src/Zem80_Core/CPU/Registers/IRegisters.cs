namespace Zem80.Core.CPU
{
    public interface IRegisters
    {
        byte this[ByteRegister register] { get; set; }
        ushort this[WordRegister registerPair] { get; set; }

        byte A { get; set; }
        ushort AF { get; set; }
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
        ushort IR { get; }
        ushort IX { get; set; }
        byte IXh { get; set; }
        byte IXl { get; set; }
        ushort IY { get; set; }
        byte IYh { get; set; }
        byte IYl { get; set; }
        byte L { get; set; }
        ushort PC { get; set; }
        byte R { get; set; }
        IShadowRegisters Shadow { get; }
        ushort SP { get; set; }
        ushort WZ { get; set; }

        void Clear();
        void ExchangeAF();
        void ExchangeBCDEHL();
        Registers Snapshot();
    }
}
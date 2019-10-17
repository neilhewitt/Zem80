namespace Z80.Core
{
    public interface IFlags
    {
        bool Carry { get; set; }
        bool Five { get; set; }
        bool HalfCarry { get; set; }
        bool Parity { get; set; }
        bool Sign { get; set; }
        bool Subtract { get; set; }
        bool Three { get; set; }
        bool Zero { get; set; }
    }
}
namespace Zem80.Core.CPU
{
    public interface IShadowRegisters
    {
        ushort AF { get; set; }
        ushort BC { get; set; }
        ushort DE { get; set; }
        ushort HL { get; set; }
    }
}
namespace Zem80.Core
{
    public interface IDebugRegisters
    {
        ushort AF { set; }
        byte F { set; }
        ushort AF_ { get; set; }
        ushort BC_ { get; set; }
        ushort DE_ { get; set; }
        ushort HL_ { get; set; }
    }
}
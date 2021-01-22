namespace Zem80.Core
{
    public interface IDirectRegisters
    {
        byte this[ByteRegister register] { get; set; }
        ushort this[WordRegister register] { get; set; }
    }
}
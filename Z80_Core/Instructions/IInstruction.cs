namespace Z80.Core
{
    public interface IInstruction
    {
        int ClockCycles { get; }
        int MachineCycles { get; }
        string Mnemonic { get; }
        byte Opcode { get; }
        string OpcodeAsHexString { get; }
        byte Prefix { get; }
        int SizeInBytes { get; }
    }
}
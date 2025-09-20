namespace Zem80.Core.CPU
{
    public interface IInstructionDecoder
    {
        InstructionPackage DecodeInstruction(byte[] instructionBytes, ushort address, out bool skipNextByte, out bool opcodeErrorNOP);
    }
}
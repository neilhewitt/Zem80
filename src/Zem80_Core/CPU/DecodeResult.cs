namespace Zem80.Core.CPU
{
    public class DecodeResult
    {
        public InstructionPackage InstructionPackage { get; init; }
        public bool OpcodeError { get; init; }
        public bool SkipNextByte { get; init; }
        public byte InstructionSizeInBytes => InstructionPackage?.Instruction.SizeInBytes ?? 0;

        public DecodeResult(InstructionPackage instructionPackage, bool opcodeError, bool skipNextByte)
        {
            InstructionPackage = instructionPackage;
            OpcodeError = opcodeError;
            SkipNextByte = skipNextByte;
        }
    }
}

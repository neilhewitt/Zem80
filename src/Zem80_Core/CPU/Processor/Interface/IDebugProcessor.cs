namespace Zem80.Core.CPU
{
    public interface IDebugProcessor
    {
        void AddBreakpoint(ushort address);
        void EvaluateAndRunBreakpoint(ushort address, InstructionPackage executingPackage);
        void ExecuteDirect(byte[] opcode);
        void ExecuteDirect(string mnemonic, byte? arg1, byte? arg2);
        void RemoveBreakpoint(ushort address);
    }
}
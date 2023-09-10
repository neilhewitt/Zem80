namespace Zem80.Core.CPU
{
    public interface IProcessorTiming
    {
        void AddWaitCycles(int waitCycles);
        void BeginInterruptRequestAcknowledgeCycle(int tStates);
        void BeginPortReadCycle(byte port, bool addressFromBC);
        void BeginPortWriteCycle(byte data, byte port, bool addressFromBC);
        void BeginStackReadCycle();
        void BeginStackWriteCycle(byte data);
        void EndInterruptRequestAcknowledgeCycle();
        void EndPortReadCycle(byte data);
        void EndPortWriteCycle();
        void EndStackReadCycle(byte data);
        void EndStackWriteCycle();
        void InternalOperationCycle(int tStates);
        void MemoryReadCycle(ushort address, byte data, byte tStates);
        void MemoryWriteCycle(ushort address, byte data, byte tStates);
        void OpcodeFetchCycle(ushort address, byte opcode, byte tStates);
        void OpcodeFetchTiming(Instruction instruction, ushort address);
        void OperandReadTiming(Instruction instruction, ushort address, params byte[] operands);
    }
}
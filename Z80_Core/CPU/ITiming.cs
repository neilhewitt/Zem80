namespace Z80.Core
{
    public interface ITiming
    {
        void OpcodeFetchCycle(ushort address, byte data);
        void MemoryReadCycle(ushort address, byte data);
        void MemoryWriteCycle(ushort address, byte data);
        void BeginStackReadCycle();
        void EndStackReadCycle(bool highByte, byte data);
        void StackWriteCycle(bool highByte, byte data);
        void BeginPortReadCycle();
        void CompletePortReadCycle(byte data);
        void BeginPortWriteCycle(byte data);
        void CompletePortWriteCycle();
        void BeginInterruptRequestAcknowledgeCycle(int tStates);
        void EndInterruptRequestAcknowledgeCycle();
        void InternalOperationCycle(int tStates);
    }
}
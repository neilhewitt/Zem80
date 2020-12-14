namespace Zem80.Core
{
    public interface ICycle
    {
        void OpcodeFetchCycle(ushort address, byte data);
        void MemoryReadCycle(ushort address, byte data);
        void MemoryWriteCycle(ushort address, byte data);
        void BeginStackReadCycle();
        void EndStackReadCycle(bool highByte, byte data);
        void BeginStackWriteCycle(bool highByte, byte data);
        void EndStackWriteCycle();
        void BeginPortReadCycle(byte n, bool bc);
        void CompletePortReadCycle(byte data);
        void BeginPortWriteCycle(byte data, byte n, bool bc);
        void CompletePortWriteCycle();
        void BeginInterruptRequestAcknowledgeCycle(int tStates);
        void EndInterruptRequestAcknowledgeCycle();
        void InternalOperationCycle(int tStates);
    }
}
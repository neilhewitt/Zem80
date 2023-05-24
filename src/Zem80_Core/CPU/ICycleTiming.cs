namespace Zem80.Core.CPU
{
    public interface ICycleTiming
    {
        void OpcodeFetchCycle(ushort address, byte data);
        void MemoryReadCycle(ushort address, byte data);
        void MemoryWriteCycle(ushort address, byte data);
        void BeginStackReadCycle();
        void EndStackReadCycle(bool highByte, byte data);
        void BeginStackWriteCycle(bool highByte, byte data);
        void EndStackWriteCycle();
        void BeginPortReadCycle(byte n, bool bc);
        void EndPortReadCycle(byte data);
        void BeginPortWriteCycle(byte data, byte n, bool bc);
        void EndPortWriteCycle();
        void BeginInterruptRequestAcknowledgeCycle(int tStates);
        void EndInterruptRequestAcknowledgeCycle();
        void InternalOperationCycle(int tStates);
    }
}
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public interface IMachineCycleTiming
    {
        void OpcodeFetchCycle(ushort address, byte opcode, byte extraTStates);
        void MemoryReadCycle(ushort address, byte data, byte extraTStatesd);
        void MemoryWriteCycle(ushort address, byte data, byte extraTStates);
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
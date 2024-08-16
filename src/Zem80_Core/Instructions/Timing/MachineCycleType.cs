namespace Zem80.Core.CPU
{
    public enum MachineCycleType
    {
        OpcodeFetch,
        OperandRead,
        OperandReadHigh,
        OperandReadLow,
        MemoryRead,
        MemoryWrite,
        MemoryReadHigh,
        MemoryReadLow,
        MemoryWriteHigh,
        MemoryWriteLow,
        StackReadHigh,
        StackReadLow,
        StackWriteHigh,
        StackWriteLow,
        InternalOperation,
        PortRead,
        PortWrite
    }
}
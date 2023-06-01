namespace Zem80.Core.CPU
{
    public enum MachineCycleType
    {
        InternalOperation,
        MemoryRead,
        MemoryWrite,
        MemoryReadHigh,
        MemoryReadLow,
        MemoryWriteHigh,
        MemoryWriteLow,
        OpcodeFetch,
        OperandRead,
        OperandReadHigh,
        OperandReadLow,
        PortRead,
        PortWrite,
        StackRead,
        StackReadHigh,
        StackReadLow,
        StackWrite,
        StackWriteHigh,
        StackWriteLow
    }
}
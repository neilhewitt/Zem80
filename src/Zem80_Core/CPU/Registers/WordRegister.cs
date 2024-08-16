namespace Zem80.Core.CPU
{
    public enum WordRegister
    {
        // NOTE: the numeric values correspond to the index of the register value in the underlying storage array
        None = 26,
        AF = 0,
        BC = 2,
        DE = 4,
        HL = 6,
        IX = 16,
        IY = 18,
        SP = 20,
        PC = 24
    }
}

namespace Zem80.Core.CPU
{
    public enum HaltReason
    {
        HaltInstruction,
        HaltCalledDirectly,
        HaltedOnTimingError
    }
}
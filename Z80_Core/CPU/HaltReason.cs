namespace Z80.Core
{
    public enum HaltReason
    {
        HaltInstruction,
        HaltCalledDirectly,
        HaltedOnTimingError
    }
}
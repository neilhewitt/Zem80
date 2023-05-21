namespace Zem80.Core.CPU
{
    public interface IDebugStack
    {
        void PushStackDirect(ushort value);
        ushort PopStackDirect();
        ushort PeekStack();
        ushort PeekStack(int wordsFromTop);
    }
}

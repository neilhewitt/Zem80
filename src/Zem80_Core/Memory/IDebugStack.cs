namespace Zem80.Core.Memory
{
    public interface IDebugStack
    {
        void PushStackDirect(ushort value);
        ushort PopStackDirect();
        ushort PeekStack();
    }
}

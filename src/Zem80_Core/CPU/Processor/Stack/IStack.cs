namespace Zem80.Core.CPU
{
    public interface IStack
    {
        IDebugStack Debug { get; }
        ushort Pointer { get; }
        ushort Top { get; }

        void Pop(WordRegister register);
        void Push(WordRegister register);
    }
}
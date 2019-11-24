namespace Z80.Core
{
    public interface IStack
    {
        ushort StartAddress { get; }

        ushort Pop();
        void Push(ushort value);
        void Reset();
        void Initialise(Processor cpu);
    }
}
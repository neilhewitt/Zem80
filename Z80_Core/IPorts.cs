namespace Z80.Core
{
    public interface IPorts
    {
        IPort this[byte portNumber] { get; }
    }
}
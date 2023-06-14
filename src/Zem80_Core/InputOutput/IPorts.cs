namespace Zem80.Core.InputOutput
{
    public interface IPorts
    {
        IPort this[byte portNumber] { get; }

        void DisconnectAll();
    }
}
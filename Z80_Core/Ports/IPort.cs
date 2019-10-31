using System;

namespace Z80.Core
{
    public interface IPort
    {
        byte Number { get; }

        byte ReadByte();
        void WriteByte(byte output);

        void SignalRead();
        void SignalWrite();
        void Connect(Func<byte> reader, Action<byte> writer, Action<PortSignal> signaller);
    }
}
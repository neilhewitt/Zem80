using System;

namespace Zem80.Core.InputOutput
{
    public interface IPort
    {
        byte Number { get; }

        void Connect(Func<byte> reader, Action<byte> writer, Action signalRead, Action signalWrite);
        void Disconnect();
        byte ReadByte(bool bc);
        void SignalRead();
        void SignalWrite();
        void WriteByte(byte output, bool bc);
    }
}
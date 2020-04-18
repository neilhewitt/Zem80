using System;

namespace ZexallCSharp
{
    public struct TestDescriptor
    {
        public string Name;
        public string Message;
        public byte Mask;
        public int Cycles;
        public TestVector Base;
        public TestVector Increment;
        public TestVector Shift;
        public (byte, byte, byte, byte) CRC;
    }
}

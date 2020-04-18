namespace ZexallCSharp
{
    public struct InstructionBytes
    {
        public byte First;
        public byte Second;
        public byte Third;
        public byte Fourth;

        public byte[] AsByteArray()
        {
            byte[] bytes = new byte[4];
            bytes[0] = First;
            bytes[1] = Second;
            bytes[2] = Third;
            bytes[3] = Fourth;
            return bytes;
        }
        public InstructionBytes(byte first, byte second, byte third, byte fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }
    }
}

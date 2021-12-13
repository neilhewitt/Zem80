namespace Zem80.Core.Instructions
{
    // DO NOT RE-ORDER OR INSERT / REMOVE VALUES - order and ordinal values are relied upon in the code for performance reasons
    public enum InstructionElement
    {
        None,
        ByteValue,
        DisplacementValue,
        WordValue,
        PortNumberFromByteValue,
        PortNumberFromC,
        BitIndex,
        A,
        B,
        C,
        D,
        E,
        F,
        H,
        L,
        I,
        R,
        IXh,
        IXl,
        IYh,
        IYl,
        AF,
        HL,
        BC,
        DE,
        IX,
        IY,
        SP,
        AddressFromHL,
        AddressFromBC,
        AddressFromDE,
        AddressFromIX,
        AddressFromIY,
        AddressFromSP,
        AddressFromWordValue,
        AddressFromIXAndOffset,
        AddressFromIYAndOffset
    }

    public struct InstructionElementNew
    {
        public const int None = 0;
        public const int ByteValue = 1;
        public const int DisplacementValue = 2;
        public const int WordValue = 3;
        public const int PortNumberFromByteValue = 4;
        public const int PortNumberFromC = 5;
        public const int BitIndex = 6;
        public const int A = 7;
        public const int B = 8;
        public const int C = 9;
        public const int D = 10;
        public const int E = 11;
        public const int F = 12;
        public const int H = 13;
        public const int L = 14;
        public const int I = 15;
        public const int R = 16;
        public const int IXh = 17;
        public const int IXl = 18;
        public const int IYh = 19;
        public const int IYl = 20;
        public const int AF = 21;
        public const int HL = 22;
        public const int BC = 23;
        public const int DE = 24;
        public const int IX = 25;
        public const int IY = 26;
        public const int SP = 27;
        public const int AddressFromHL = 28;
        public const int AddressFromBC = 29;
        public const int AddressFromDE = 30;
        public const int AddressFromIX = 31;
        public const int AddressFromIY = 32;
        public const int AddressFromSP = 33;
        public const int AddressFromWordValue = 34;
        public const int AddressFromIXAndOffset = 35;
        public const int AddressFromIYAndOffset = 36;
    }
}
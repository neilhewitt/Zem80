namespace Zem80.Core.CPU
{
    public static class LogicalOperations
    {
        public static (byte Result, Flags Flags) And(this byte first, byte second)
        {
            return LogicalFlags(first, second, LogicalOperation.And);
        }

        public static (byte Result, Flags Flags) Or(this byte first, byte second)
        {
            return LogicalFlags(first, second, LogicalOperation.Or);
        }

        public static (byte Result, Flags Flags) Xor(this byte first, byte second)
        {
            return LogicalFlags(first, second, LogicalOperation.Xor);
        }

        private static (byte Result, Flags Flags) LogicalFlags(byte first, byte second, LogicalOperation operation)
        {
            int result = operation switch
            {
                LogicalOperation.And => first & second,
                LogicalOperation.Or => first | second,
                LogicalOperation.Xor => first ^ second,
                _ => 0x00
            };

            Flags flags = new Flags();
            flags.Zero = ((byte)result == 0x00);
            flags.Carry = false;
            flags.Sign = (((sbyte)result) < 0);
            flags.ParityOverflow = ((byte)result).EvenParity();
            flags.HalfCarry = operation == LogicalOperation.And;
            flags.Subtract = false;
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5

            return ((byte)result, flags);
        }
    }
}

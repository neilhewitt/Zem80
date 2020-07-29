using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Z80.Core
{
    public static class LogicalOperations
    {
        public static (byte Result, Flags Flags) Op(LogicalOperation operation, byte left, byte right)
        {
            return operation switch
            {
                LogicalOperation.And => And(left, right),
                LogicalOperation.Or => Or(left, right),
                LogicalOperation.Xor => Xor(left, right),
                _ => throw new NotImplementedException()
            };
        }

        public static (byte Result, Flags Flags) And(byte left, byte right)
        {
            return ((byte)(left & right), FlagLookup.LogicalFlags(left, right, LogicalOperation.And));
        }

        public static (byte Result, Flags Flags) Or(byte left, byte right)
        {
            return ((byte)(left | right), FlagLookup.LogicalFlags(left, right, LogicalOperation.Or));
        }

        public static (byte Result, Flags Flags) Xor(byte left, byte right)
        {
            return ((byte)(left ^ right), FlagLookup.LogicalFlags(left, right, LogicalOperation.Xor));
        }
    }
}

using System;

namespace Zem80.Core
{
    public class RegisterFlags : Flags
    {
        private Registers _registers;

        protected override byte FlagByte { get { return _registers.F; } set { _registers.F = value; } }

        public RegisterFlags(Registers registers)
        {
            _registers = registers;
        }
    }
}

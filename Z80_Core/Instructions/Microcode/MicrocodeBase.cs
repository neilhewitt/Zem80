using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public abstract class MicrocodeBase
    {
        protected Processor _cpu;

        public MicrocodeBase(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

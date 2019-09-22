using System;

namespace Z80.Core
{
    public class Processor
    {
        public Registers Registers { get; private set; }

        public Processor()
        {
            Registers = new Registers();
        }
    }
}

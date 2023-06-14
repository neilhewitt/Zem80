using System;

namespace Zem80.Core.CPU
{
    public interface IMicrocode2
    {
        public void M1();
        public void M2();
        public void M3();
        public void M4();
        public void M5();
        public void M6();
    }

    public interface IMicrocode
    {
        ExecutionResult Execute(Processor cpu, InstructionPackage package);
    }
}
using System;

namespace Z80.Core
{
    public interface IDebugProcessor : IProcessor
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<InstructionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler OnHalt;

        ExecutionResult ExecuteDirect(Instruction instruction, InstructionData data);
    }
}
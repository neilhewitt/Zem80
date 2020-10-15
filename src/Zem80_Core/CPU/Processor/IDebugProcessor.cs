using System;
using System.Collections.Generic;

namespace Zem80.Core
{
    public interface IDebugProcessor
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<InstructionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler<HaltReason> OnHalt;
        event EventHandler<int> OnBeforeInsertWaitCycles;

        ExecutionResult ExecuteDirect(byte[] opcode);
        ExecutionResult ExecuteDirect(InstructionPackage package);
    }
}
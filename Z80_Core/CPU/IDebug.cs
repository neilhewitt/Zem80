using System;
using System.Collections.Generic;

namespace Z80.Core
{
    public interface IDebug
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<ExecutionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler<HaltReason> OnHalt;
        event EventHandler<int> OnBeforeInsertWaitCycles;

        ExecutionResult Execute(byte[] opcode);
        ExecutionResult Execute(ExecutionPackage package);
    }
}
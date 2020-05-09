using System;
using System.Collections.Generic;

namespace Z80.Core
{
    public interface IDebugMode
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<ExecutionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler<HaltReason> OnHalt;
        ExecutionResult Execute(ExecutionPackage package);
    }
}
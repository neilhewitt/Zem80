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
        event EventHandler<TickEvent> OnBeforeTick;
        event EventHandler<TickEvent> OnAfterTick;

        long TimingErrors { get; }
        IEnumerable<TimingErrorLog> TimingErrorLogs { get; } 

        ExecutionResult Execute(ExecutionPackage package);
    }
}
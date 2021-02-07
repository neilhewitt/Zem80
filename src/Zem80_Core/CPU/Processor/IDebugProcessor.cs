using System;
using System.Collections.Generic;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public interface IDebugProcessor
    {
        event EventHandler<InstructionPackage> OnBreakpoint;

        IEnumerable<ushort> Breakpoints { get; }
        void AddBreakpoint(ushort address);
        void RemoveBreakpoint(ushort address);

        ExecutionResult ExecuteDirect(byte[] opcode);
        ExecutionResult ExecuteDirect(string mnemonic, byte? arg1, byte? arg2);
    }
}
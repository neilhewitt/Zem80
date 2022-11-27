using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public partial class Processor : IDebugProcessor
    {
        long _lastRunTimeInMilliseconds;
        private EventHandler<InstructionPackage> _onBreakpoint;
        private IList<ushort> _breakpoints;

        long IDebugProcessor.LastRunTimeInMilliseconds => _lastRunTimeInMilliseconds;
        IEnumerable<ushort> IDebugProcessor.Breakpoints => _breakpoints;

        event EventHandler<InstructionPackage> IDebugProcessor.OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public IDebugProcessor Debug => this;

    }
}

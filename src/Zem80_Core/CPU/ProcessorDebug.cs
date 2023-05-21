using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public partial class Processor : IDebugProcessor
    {
        private EventHandler<InstructionPackage> _onBreakpoint;
        private IList<ushort> _breakpoints;

        long IDebugProcessor.LastRunTimeInMilliseconds => (_lastEnd - _lastStart).Milliseconds;
        IEnumerable<ushort> IDebugProcessor.Breakpoints => _breakpoints;

        event EventHandler<InstructionPackage> IDebugProcessor.OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public IDebugProcessor Debug => this;

        public void SetDataBusDefaultValue(byte defaultValue)
        {
            Interface.SetDataBusDefault(defaultValue);
        }

        void IDebugProcessor.AddBreakpoint(ushort address)
        {
            if (_breakpoints == null) _breakpoints = new List<ushort>();

            // Note that the breakpoint functionality is *very* simple and not checked
            // so if you add a breakpoint for an address which is not the start
            // of an instruction in the code, it will never be triggered as PC will never get there
            if (!_breakpoints.Contains(address))
            {
                _breakpoints.Add(address);
            }
        }

        void IDebugProcessor.RemoveBreakpoint(ushort address)
        {
            if (_breakpoints != null && _breakpoints.Contains(address))
            {
                _breakpoints.Remove(address);
            }
        }
    }
}

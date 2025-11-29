using System;

namespace Zem80.Core.Debugger
{
    public class DebugMonitor
    {
        private ushort? _breakpointAddress;

        public MonitorState State { get; private set; }
        public DebugEventTypes EventTypes { get; }
        public Func<DebugState, DebugResponse> Handler { get; }

        public bool Breaks(ushort instructionAddress)
        {
            if (_breakpointAddress == null || instructionAddress == _breakpointAddress.Value)
            {
                State = MonitorState.Active;
                return true;
            }

            return false;
        }

        internal DebugResponse Step(ushort instructionAddress, DebugState state)
        {
            if (State == MonitorState.Active)
            {
                DebugResponse response = Handler(state);
                if (response == DebugResponse.Stop) // Stop
                {
                    State = MonitorState.PendingBreakpoint;
                }

                return response;
            }

            return DebugResponse.None;
        }

        public DebugMonitor(ushort breakpointAddress, DebugEventTypes eventTypes, Func<DebugState, DebugResponse> handler)
            : this(eventTypes, handler)
        {
            _breakpointAddress = breakpointAddress;
        }

        public DebugMonitor(DebugEventTypes eventTypes, Func<DebugState, DebugResponse> handler)
        {
            State = MonitorState.PendingBreakpoint;
            EventTypes = eventTypes;
            Handler = handler;
        }
    }
}

using System;

namespace Zem80.Core.Debugger
{
    public enum MonitorState
    {
        Active,
        PendingBreakpoint,
        Stopped
    }

    public class DebugMonitor
    {
        private ushort? _breakpointAddress;

        public MonitorState MonitorState { get; private set; }
        public DebugEventTypes EventTypes { get; }
        public Func<DebugState, DebugResponse> Handler { get; }

        public bool Breaks(ushort instructionAddress)
        {
            if (_breakpointAddress == null || instructionAddress == _breakpointAddress.Value)
            {
                MonitorState = MonitorState.Active;
                return true;
            }

            return false;
        }

        public DebugResponse Step(ushort instructionAddress, DebugState state)
        {
            if (MonitorState == MonitorState.Active)
            {
                DebugResponse response = Handler(state);
                if (response.Type == DebugResponseType.Run)
                {
                    if (response.RunToAddress.HasValue)
                    {
                        _breakpointAddress = response.RunToAddress.Value;
                    }
                    
                    MonitorState = MonitorState.PendingBreakpoint;
                }
                else if (response.Type == DebugResponseType.Stop) // Stop
                {
                    MonitorState = MonitorState.Stopped;
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
            MonitorState = MonitorState.PendingBreakpoint;
            EventTypes = eventTypes;
            Handler = handler;
        }
    }
}

using System;
using System.Diagnostics;

namespace Zem80.Core.CPU
{
    public class RealTimeClock : ClockBase
    {
        private Stopwatch _stopwatch;
        
        private int[] _waitPattern;
        private int _waitCount;
        
        private long _lastElapsedTicks;

        public override void Start()
        {
            base.Start();
            _stopwatch.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _stopwatch.Stop();
        }

        public override void WaitForNextClockTick()
        {
            if (_waitCount == _waitPattern.Length) _waitCount = 0;
            int ticksToWait = _waitPattern[_waitCount++];
            long targetTicks = _lastElapsedTicks + ticksToWait;

            // if Processor was created with the RealTimeClock but has never been started, Stopwatch won't be running
            // if you use DebugProcessor.ExecuteDirect() to run instructions directly then this would hang the clock here
            // so we'll check that and if it's not running we'll just skip the wait
            if (_stopwatch.IsRunning)
            {
                while (_stopwatch.ElapsedTicks < targetTicks) ; // spin until enough Windows ticks have elapsed
                _lastElapsedTicks = _stopwatch.ElapsedTicks;
            }

            base.WaitForNextClockTick();
        }

        protected internal RealTimeClock(float frequencyInMHz, int[] waitPattern)
            : base(frequencyInMHz)
        {
            FrequencyInMHz = frequencyInMHz;
            _waitPattern = waitPattern;
            _stopwatch = new Stopwatch();
        }
    }
}

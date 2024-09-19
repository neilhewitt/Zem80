using System;
using System.Diagnostics;

namespace Zem80.Core.CPU
{
    public class RealTimeClock : DefaultClock
    {
        private Stopwatch _stopwatch;
        
        private int[] _waitPattern;
        private int _waitCount;
        
        private long _lastElapsedTicks;

        public bool IsRealTime => Stopwatch.IsHighResolution;

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

            while (_stopwatch.ElapsedTicks < targetTicks) ; // spin until enough Windows ticks have elapsed
            _lastElapsedTicks = _stopwatch.ElapsedTicks;

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

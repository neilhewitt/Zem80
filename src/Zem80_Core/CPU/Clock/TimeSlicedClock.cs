using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Zem80.Core.CPU
{
    public class TimeSlicedClock : ClockBase, IClock, IDisposable
    {
        private int _ticksPerTimeSlice;
        private int _ticksThisTimeSlice;
        private HighResolutionTimer _timer;
        private int _timeSliceInMilliseconds;
        private bool _stopped;

        public int TimeSliceInMilliseconds => _timeSliceInMilliseconds;

        public override void Start()
        {
            _stopped = false;
            _timer.Start();
            base.Start();
        }

        public override void Stop()
        {
            _stopped = true;
            _timer.Stop();
            base.Stop();
        }

        public override void WaitForNextClockTick()
        {
            _ticksThisTimeSlice++;
            if (_ticksThisTimeSlice > _ticksPerTimeSlice)
            {
                _ticksThisTimeSlice = 0;
                _cpu.Suspend();
            }
            
            base.WaitForNextClockTick();
        }

        public void Dispose()
        {
            Stop();
        }

        protected internal TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice) : base(frequencyInMHz)
        {
            int z80TicksPerSecond = (int)(frequencyInMHz * 1000000);
            float timeSliceInSeconds = (float)timeSlice.Ticks / 10000000f; // timeSlice.TotalSeconds is an integer
            _ticksPerTimeSlice = (int)(z80TicksPerSecond * timeSliceInSeconds);
            _timeSliceInMilliseconds = timeSlice.Milliseconds;

            _timer = new HighResolutionTimer(timeSlice.Milliseconds);
            _timer.Elapsed += (sender, args) => { _cpu.Resume(); };

            OnInitialised += (sender, args) =>
            {
                // pause the timer when debugging starts, and resume it when debugging ends
                // while debugging, instructions will run without any timing constraints
                _cpu.Debug.OnBreakpointReached += (sender, args) => { if (!_stopped) _timer.Stop(); };
                _cpu.Debug.OnDebugSessionEnded += (sender, args) => { if (!_stopped) _timer.Start(); };
            };
        }
    }
}
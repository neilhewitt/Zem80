using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = MultimediaTimer.Timer;

namespace Zem80.Core.CPU
{
    public class TimeSlicedClock : DefaultClock, IClock, IDisposable
    {
        private int _ticksPerTimeSlice;
        private int _ticksThisTimeSlice;
        private Timer _timer;

        public bool IsTimeSliced => true;

        public override void Start()
        {
            _timer.Start();
            base.Start();
        }

        public override void Stop()
        {
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

            _timer = new Timer();
            _timer.Interval = timeSlice;
            _timer.Elapsed += (sender, args) => { _cpu.Resume(); };
        }
    }
}
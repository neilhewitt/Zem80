using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Timer = MultimediaTimer.Timer;

namespace Zem80.Core
{
    public class TimeSlicedClock : ClockBase, IDisposable
    {
        private Timer _timer;
        private int _ticksPerTimeSlice;
        private int _ticksThisTimeSlice;
        private bool _handleResume;

        public override void Start()
        {
            base.Start();
            if (_handleResume) _timer.Start();
        }

        public override void Stop()
        {
            base.Stop();
            if (_handleResume) _timer.Stop();
        }

        public override void WaitForNextClockTick()
        {
            _ticksThisTimeSlice++;
            if (_ticksThisTimeSlice >= _ticksPerTimeSlice)
            {
                _ticksThisTimeSlice = 0;
                _cpu.Suspend();
                Task.Run(SignalTimeSliceEnded).ConfigureAwait(false);
            }
            else
            {
                base.WaitForNextClockTick();
            }
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            _cpu.Resume();
        }

        public void Dispose()
        {
            Stop();
        }

        protected internal TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice, bool handleResume)
            : base(frequencyInMHz)
        {
            int z80TicksPerSecond = (int)(frequencyInMHz * 1000000);
            float timeSliceInSeconds = (float)timeSlice.Ticks / 10000000f;
            _ticksPerTimeSlice = (int)(z80TicksPerSecond * timeSliceInSeconds);

            _timer = new Timer();
            _timer.Interval = timeSlice;
            _timer.Elapsed += TimerElapsed;
            _handleResume = handleResume;
        }
    }
}
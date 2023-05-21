using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Timer = MultimediaTimer.Timer;

namespace Zem80.Core
{
    public class TimeSlicedClock : FastClock, IClock, ITimeSliced, IDisposable
    {
        private Timer _timer;
        private int _ticksPerTimeSlice;
        private int _ticksThisTimeSlice;

        public event EventHandler<long> OnTimeSliceEnded;
        public event EventHandler<long> OnTimeSliceStarted;

        public bool IsTimeSliced => true;

        public override void Start()
        {
            base.Start();
            _timer.Start();
            SignalTimeSliceStarted();
        }

        public override void Stop()
        {
            base.Stop();
            _timer.Stop();
            SignalTimeSliceEnded();
        }

        public override void WaitForNextClockTick()
        {
            _ticksThisTimeSlice++;
            if (_ticksThisTimeSlice >= _ticksPerTimeSlice)
            {
                _ticksThisTimeSlice = 0;
                _cpu.Suspend();
                SignalTimeSliceEnded();
            }
            else
            {
                base.WaitForNextClockTick();
            }
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            _cpu.Resume();
            SignalTimeSliceStarted();
        }

        private void SignalTimeSliceStarted()
        {
            // event is dispatched on a thread so the Z80 suspend doesn't have to wait for you to handle it
            Task.Run(() => OnTimeSliceStarted?.Invoke(this, Ticks)).ConfigureAwait(false);
        }

        private void SignalTimeSliceEnded()
        {
            // event is dispatched on a thread so the Z80 resume doesn't have to wait for you to handle it
            Task.Run(() => OnTimeSliceEnded?.Invoke(this, Ticks)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Stop();
        }

        protected internal TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice)
            : base(frequencyInMHz)
        {
            int z80TicksPerSecond = (int)(frequencyInMHz * 1000000);
            float timeSliceInSeconds = (float)timeSlice.Ticks / 10000000f;
            _ticksPerTimeSlice = (int)(z80TicksPerSecond * timeSliceInSeconds);

            _timer = new Timer();
            _timer.Interval = timeSlice;
            _timer.Elapsed += TimerElapsed;
        }
    }
}
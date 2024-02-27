using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = MultimediaTimer.Timer;

namespace Zem80.Core.CPU
{
    public class TimeSlicedClock : DefaultClock, IClock, IDisposable
    {
        private Timer _timer;
        private int _ticksPerTimeSlice;
        private int _ticksThisTimeSlice;
        private bool _elapsed;

        public bool IsTimeSliced => true;

        public event EventHandler OnBeforeSuspend;
        public event EventHandler OnBeforeResume;

        public override void Start()
        {
            base.Start();
            _timer.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _timer.Stop();
        }

        public override void WaitForNextClockTick()
        {
            _ticksThisTimeSlice++;
            if (_ticksThisTimeSlice > _ticksPerTimeSlice)
            {
                _ticksThisTimeSlice = 0;
                OnBeforeSuspend?.Invoke(this, EventArgs.Empty);
                //_cpu.Suspend();
                
                while (!_elapsed) ; // wait here rather than giving control back
                _elapsed = false;
            }
            
            base.WaitForNextClockTick();
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            OnBeforeResume?.Invoke(this, EventArgs.Empty);
            _elapsed = true;
            //_cpu.Resume();
        }

        public void Dispose()
        {
            Stop();
        }

        protected internal TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice) : base(frequencyInMHz)
        {
            int z80TicksPerSecond = (int)(frequencyInMHz * 1000000);
            float timeSliceInSeconds = (float)timeSlice.Ticks / 10000000f;
            _ticksPerTimeSlice = (int)(z80TicksPerSecond * timeSliceInSeconds);

            _timer = new Timer();
            _timer.Resolution = TimeSpan.FromMilliseconds(1);
            _timer.Interval = timeSlice;
            _timer.Elapsed += TimerElapsed;
        }
    }
}
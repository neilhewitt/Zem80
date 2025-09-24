using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Zem80.Core.CPU
{
    // This code is derived from MultimediaTimer by Mike James and is used under the terms of the MIT License
    // See LICENSE.txt in the folder root for MultimediaTimer license information.

    public sealed class HighResolutionTimer
    {
        private static TimerCapabilities _capabilities; // will indicate min and max supported intervals on this platform

        #region static constructor
        static HighResolutionTimer()
        {
            timeGetDevCaps(ref _capabilities, Marshal.SizeOf<TimerCapabilities>(HighResolutionTimer._capabilities));
        }
        #endregion

        #region p/invoke
        [DllImport("winmm.dll")]
        private static extern int timeGetDevCaps(ref TimerCapabilities caps, int sizeOfTimerCaps);

        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerProc proc, int user, int mode);

        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);
        #endregion

        private delegate void TimerProc(int id, int msg, int user, int param1, int param2);

        private int _id;
        private double _interval;
        private TimerProc _onIntervalElapsed;

        public TimeSpan Interval => TimeSpan.FromMilliseconds(_interval);

        public bool IsRunning { get; private set; }
        public static TimerCapabilities Capabilities => _capabilities;

        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Elapsed;
        public event EventHandler Disposed;

        public void Start()
        {
            if (!IsRunning)
            {
                _id = timeSetEvent((int)Interval.TotalMilliseconds, 1, _onIntervalElapsed, 0, 1);
                if (_id == 0)
                {
                    throw new Exception("Unable to start Timer.");
                }

                IsRunning = true;
                OnStarted(EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                timeKillEvent(_id);
                IsRunning = false;
                OnStopped(EventArgs.Empty);
            }
        }

        private void Initialize()
        {
            IsRunning = false;
            _interval = _capabilities.PeriodMin;
            _onIntervalElapsed = new TimerProc(OnIntervalElapsed);
        }

        private void OnIntervalElapsed(int id, int msg, int user, int param1, int param2)
        {
            OnElapsed(EventArgs.Empty);
        }

        private void OnStarted(EventArgs e)
        {
            Started?.Invoke(this, e);
        }

        private void OnStopped(EventArgs e)
        {
            Stopped?.Invoke(this, e);
        }

        private void OnElapsed(EventArgs e)
        {
            Elapsed?.Invoke(this, e);
        }

        public HighResolutionTimer()
        {
            Initialize();
        }

        public HighResolutionTimer(double interval)
        {
            if (interval < _capabilities.PeriodMin || interval > _capabilities.PeriodMax)
            {
                throw new InvalidEnumArgumentException($"Interval must be between {_capabilities.PeriodMin} and {_capabilities.PeriodMax} milliseconds.");
            }

            Initialize();
            _interval = interval; // override the interval
        }

        ~HighResolutionTimer()
        {
            if (IsRunning)
            {
                timeKillEvent(_id);
            }
        }
    }
}

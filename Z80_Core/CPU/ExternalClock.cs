using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Z80.Core
{
    public class ExternalClock
    {
        private Stopwatch _stopwatch;
        private bool _running;
        private double _windowsTickPerClockTick;

        private Thread _clockThread;
        
        public int FrequencyInMhz { get; private set; }
        public int TicksSinceStart { get; private set; }
        public bool Started => _running;

        public event EventHandler OnTick;

        public void Start()
        {
            _running = true;
            TicksSinceStart = 0;
            _clockThread = new Thread(new ThreadStart(ClockTick));
            _clockThread.Start();
        }

        public void Stop()
        {
            _running = false;
        }

        private void ClockTick()
        {
            while (_running)
            {
                _stopwatch.Restart();
                while (_stopwatch.ElapsedTicks < _windowsTickPerClockTick);
                OnTick?.Invoke(this, null);
                TicksSinceStart++;
            }
        }

        public ExternalClock(int frequencyInMhz)
        {
            _windowsTickPerClockTick = ((double)(10 / frequencyInMhz));
            _stopwatch = new Stopwatch();

            FrequencyInMhz = frequencyInMhz;
            TicksSinceStart = 0;
        }
    }
}

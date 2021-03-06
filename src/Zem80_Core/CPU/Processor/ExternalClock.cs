﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Zem80.Core
{
    public class ExternalClock
    {
        private Stopwatch _stopwatch;
        private bool _running;
        private double _windowsTickPerClockTick;

        private Thread _clockThread;
        
        public double FrequencyInMhz { get; private set; }
        public long TicksSinceStart { get; private set; }
        public bool Started => _running;

        public event EventHandler OnTick;

        public void Start()
        {
            if (!_running)
            {
                _running = true;
                TicksSinceStart = 0;
                _clockThread.Start();
            }
        }

        public void Stop()
        {
            _running = false;
            _clockThread = new Thread(new ThreadStart(ClockTick)); // let the old thread die
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

        public ExternalClock(double frequencyInMhz)
        {
            _windowsTickPerClockTick = ((double)(10 / frequencyInMhz));
            _stopwatch = new Stopwatch();

            FrequencyInMhz = frequencyInMhz;
            _clockThread = new Thread(new ThreadStart(ClockTick));
        }
    }
}

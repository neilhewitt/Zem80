using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zem80.Core
{
    public class ProcessorClock : IDisposable
    {
        private int _windowsTicksPerSecond = 10000000;
        private int _windowsTicksPerZ80Tick;
        private Thread _stopwatchHandler;
        private bool _disposing;
        private bool _stopped;

        public event EventHandler OnProcessorClockTick;
        
        public void Start()
        {
            _stopped = false;
        }

        public void Stop()
        {
            _stopped = true;
        }

        public void Dispose()
        {
            _disposing = true;
        }

        private void HandleStopwatch()
        {
            long elapsedTicks = 0;
            long currentTicks;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!_disposing)
            {
                if (!_stopped)
                {
                    long targetTicks = elapsedTicks + _windowsTicksPerZ80Tick;
                    if ((currentTicks = stopwatch.ElapsedTicks) >= targetTicks)
                    {
                        OnProcessorClockTick?.Invoke(this, EventArgs.Empty);
                        elapsedTicks = currentTicks;
                    }
                }
            }
        }

        public ProcessorClock(Processor cpu)
        {
            int cpuTicksPerSecond = (int)(cpu.FrequencyInMHz * 10000000); // frequency in CPU ticks per second
            _windowsTicksPerZ80Tick = (int)Math.Ceiling((double)(_windowsTicksPerSecond / cpuTicksPerSecond));

            _stopped = true;
            _stopwatchHandler = new Thread(HandleStopwatch);
            _stopwatchHandler.IsBackground = true;
            _stopwatchHandler.Start();
        }
    }
}

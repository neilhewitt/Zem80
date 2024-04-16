using System;
using System.Threading.Tasks;

namespace Zem80.Core.CPU
{
    public class ClockEvent
    {
        private Action _callback;
        private DefaultClock _clock;
        bool _repeats;

        public long TicksToWait { get; init; }
        public long TicksSoFar { get; private set; }

        public void Stop()
        {
            _clock.OnTick -= OnTick;
        }

        private void OnTick(object sender, long ticks)
        {
            TicksSoFar++;
            if (TicksSoFar >= TicksToWait)
            {
                TicksSoFar = 0;
                Task.Run(() => _callback());
                if (!_repeats) Stop();
            }
        }

        public ClockEvent(DefaultClock clock, long ticksToWait, Action callback, bool repeats)
        {
            TicksToWait = ticksToWait;
            _callback = callback;
            _clock = clock;
            _clock.OnTick += OnTick;
            _repeats = repeats;
        }
    }
}
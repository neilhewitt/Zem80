using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{

    public class DefaultClock : IClock
    {
        protected bool _started;
        protected Processor _cpu;
        protected IDictionary<int, ClockEvent> _events;

        public float FrequencyInMHz { get; init; }
        public long Ticks { get; private set; }

        public event EventHandler<long> OnTick;

        void IClock.Initialise(Processor cpu)
        { 
            _cpu = cpu;
        }

        public virtual void Start()
        {
            if (_cpu is null) throw new ClockException("Clock must be initialised by calling Initialise() before it can start.");
            _started = true;
        }

        public virtual void Stop()
        {
            _started = false;
        }

        public virtual int SetEvent(long ticks, Action onElapsed, bool repeats)
        {
            int index = _events.Count;
            _events.Add(index, new ClockEvent(this, ticks, onElapsed, repeats));
            return index;
        }

        public virtual void UnsetEvent(int timerIndex)
        {
            if (_events.TryGetValue(timerIndex, out ClockEvent timer))
            {
                timer.Stop();
                _events.Remove(timerIndex);
            }
        }

        public virtual void WaitForNextClockTick()
        {
            while (_cpu.Suspended) ; // if Suspend() is called in the middle of an instruction cycle, we want to stop immediately

            Ticks++;
            OnTick?.Invoke(this, Ticks);
        }

        public void WaitForClockTicks(int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                WaitForNextClockTick();
            }
        }

        protected internal DefaultClock(float frequencyInMHz)
        {
            FrequencyInMHz = frequencyInMHz;
            _events = new Dictionary<int, ClockEvent>();
        }
    }
}
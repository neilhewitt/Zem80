using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zem80.Core.IO;

namespace Zem80.Core.CPU
{
    public class FastClock : IClock
    {
        protected Processor _cpu;

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
        }

        public virtual void Stop()
        {
        }

        public virtual void WaitForNextClockTick()
        {
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

        protected internal FastClock(float frequencyInMHz)
        {
            FrequencyInMHz = frequencyInMHz;
        }
    }
}
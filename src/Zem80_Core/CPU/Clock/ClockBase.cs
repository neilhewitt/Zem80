using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zem80.Core.IO;

namespace Zem80.Core
{
    public abstract class ClockBase : IClock
    {
        protected Processor _cpu;

        public float FrequencyInMHz { get; init; }
        public long Ticks { get; private set; }

        public event EventHandler<long> OnTick;
        public event EventHandler<long> OnTimeSliceEnded;
        public event EventHandler<long> OnTimeSliceStarted;

        public virtual void Initialise(Processor cpu)
        {
            _cpu = cpu;
        }

        public virtual void Start()
        {
            if (_cpu is null) throw new Z80Exception("Clock must be initialised by calling Initialise() before it can start.");
        }

        public virtual void Stop()
        {
            SignalTimeSliceEnded();
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

        protected internal void SignalTimeSliceEnded()
        {
            Task.Run(() => OnTimeSliceEnded?.Invoke(this, Ticks)).ConfigureAwait(false);
        }

        protected internal void SignalTimeSliceStarted()
        {
            Task.Run(() => OnTimeSliceStarted?.Invoke(this, Ticks)).ConfigureAwait(false);
        }

        protected internal ClockBase(float frequencyInMHz)
        {
            FrequencyInMHz = frequencyInMHz;
        }
    }
}
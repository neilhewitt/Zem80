using System;

namespace Zem80.Core
{
    public interface IClock
    {
        float FrequencyInMHz { get; }
        long Ticks { get; }
        bool IsTimeSliced { get { return this is ITimeSliced; } }
        ITimeSliced TimeSliced => IsTimeSliced ? (ITimeSliced)this : null;

        event EventHandler<long> OnTick;

        void Initialise(Processor cpu);
        void Start();
        void Stop();

        void WaitForNextClockTick();
        void WaitForClockTicks(int ticks);

    }

    public interface ITimeSliced
    {
        event EventHandler<long> OnTimeSliceStarted;
        event EventHandler<long> OnTimeSliceEnded;
    }
}

using System;

namespace Zem80.Core
{
    public interface IClock
    {
        float FrequencyInMHz { get; }
        long Ticks { get; }

        event EventHandler<long> OnTick;
        event EventHandler<long> OnTimeSliceEnded;

        void Initialise(Processor cpu);
        void Start();
        void Stop();

        void WaitForNextClockTick();
        void WaitForClockTicks(int ticks);

    }
}

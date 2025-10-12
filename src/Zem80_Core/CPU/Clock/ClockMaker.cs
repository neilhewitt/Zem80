using System;
using System.Diagnostics;

namespace Zem80.Core.CPU
{
    public static class ClockMaker
    {
        // attempts to run all events at the same time intervals they would run on the actual hardware
        public static IClock RealTimeClock(float frequencyInMHz, int[] cycleWaitPattern = null)
        {
            if (cycleWaitPattern is null)
            {
                // HACK ALERT!!!
                // To simulate real-time operation, we need to work out how many Windows ticks of the Stopwatch class there are
                // per emulated Z80 tick, so we can wait for the right amount of time for each clock cycle... easy, right?
                // Well, no. Because it isn't always a whole number, and we can't wait for a fractional number of Stopwatch ticks.
                // So, we need to create a pattern of waiting for one fewer ticks every x ticks to even out the wait time, otherwise
                // timing-critical stuff in your emulated hardware may not work properly (example: Spectrum beeper sound)
                //
                // Client code can supply a custom WaitPattern at constructor time. Otherwise we have to generate one.

                cycleWaitPattern = new int[1] { 1 }; // default if not on a high-resolution platform (will NOT be real-time)

                // high-res stopwatch frequency will be 10MHz
                if (Stopwatch.IsHighResolution)
                {
                    float windowsFrequency = Stopwatch.Frequency / (frequencyInMHz * 1000000);
                    int windowsTicksPerZ80Tick = (int)Math.Ceiling(windowsFrequency);

                    if (windowsTicksPerZ80Tick % 2 == 0) // even number, so the pattern is regular, for example at 5MHz 10/5 = 2
                    {
                        cycleWaitPattern = new int[] { windowsTicksPerZ80Tick };
                    }
                    else // odd number - we're approximating here, some hardware emulation may require a custom pattern
                    {
                        cycleWaitPattern = new int[windowsTicksPerZ80Tick + 1];
                        for (int i = 0; i < windowsTicksPerZ80Tick; i++)
                        {
                            cycleWaitPattern[i] = windowsTicksPerZ80Tick;
                        }
                        cycleWaitPattern[windowsTicksPerZ80Tick] = windowsTicksPerZ80Tick - 2;
                    }
                }
            }

            return new RealTimeClock(frequencyInMHz, cycleWaitPattern);
        }

        // runs as fast as possible, but only for as many ticks as there would be in the defined slice of time
        // after which the CPU *emulation* suspends until the end of the time slice in real time, then begins the next slice
        // (note: suspend is NOT the same as halt - the CPU loop is paused, whereas during halt the CPU executes NOPs)
        public static IClock TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice)
        {
            TimeSlicedClock clock = new TimeSlicedClock(frequencyInMHz, timeSlice);
            return clock;
        }
    }
}
using System;
using System.Diagnostics;

namespace Zem80.Core
{
    public static class ClockMaker
    {
        public static IClock FastClock(float frequencyInMHz) => new FastClock(frequencyInMHz);
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

                cycleWaitPattern ??= new int[1] { 1 }; // default if not high-resolution platform

                // high-res stopwatch frequency will be 10MHz - if we're on a non-high-res platform then real-time mode is not available anyway
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
                        cycleWaitPattern[windowsTicksPerZ80Tick] = windowsTicksPerZ80Tick - 1;
                    }
                }
            }

            return new RealTimeClock(frequencyInMHz, cycleWaitPattern);
        }

        public static IClock TimeSlicedClock(float frequencyInMHz, TimeSpan timeSlice, bool clockHandlesResume)
        {
            return new TimeSlicedClock(frequencyInMHz, timeSlice, clockHandlesResume);
        }
    }
}
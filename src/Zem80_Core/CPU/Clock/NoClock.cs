namespace Zem80.Core.CPU
{
    public class NoClock: ClockBase, IClock
    {
        public NoClock(int pretendFrequencyInMHz = 4) : base(pretendFrequencyInMHz)
        {
        }
    }
}
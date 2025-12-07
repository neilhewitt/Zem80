namespace Zem80.Core.CPU
{
    public class NotAClock: ClockBase, IClock
    {
        public NotAClock(int pretendFrequencyInMHz = 4) : base(pretendFrequencyInMHz)
        {
        }
    }
}
namespace Zem80.Core.CPU
{
    public class NotAClock: ClockBase, IClock
    {
        public NotAClock(float pretendFrequencyInMHz = 4) : base(pretendFrequencyInMHz)
        {
        }
    }
}
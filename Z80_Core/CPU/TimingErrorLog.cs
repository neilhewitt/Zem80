namespace Z80.Core
{
    public class TimingErrorLog
    {
        public Instruction Instruction { get; private set; }
        public long WindowsTicksTaken { get; private set; }
        public long WindowsTicksExpected { get; private set; }

        public TimingErrorLog(Instruction instruction, long ticksTaken, long ticksExpected)
        {
            Instruction = instruction;
            WindowsTicksTaken = ticksTaken;
            WindowsTicksExpected = ticksExpected;
        }
    }
}
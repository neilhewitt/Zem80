namespace Z80.Core
{
    public class TickEvent
    {
        public Instruction Instruction { get; private set; }
        public MachineCycleType MachineCycleType { get; private set; }
        public int ClockCyclesAdded { get; private set; }

        public TickEvent(Instruction instruction, MachineCycleType machineCycleType, int clockCyclesAdded)
        {
            Instruction = instruction;
            MachineCycleType = machineCycleType;
            ClockCyclesAdded = clockCyclesAdded;
        }
    }
}
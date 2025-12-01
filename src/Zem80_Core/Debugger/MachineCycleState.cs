using Zem80.Core.CPU;

namespace Zem80.Core.Debugger
{
    public class MachineCycleState
    {
        public MachineCycle Cycle { get; }
        public ushort Address { get; }
        public byte Data { get; }

        public MachineCycleState(MachineCycle cycle, ushort address, byte data)
        {
            Cycle = cycle;
            Address = address;
            Data = data;
        }
    }
}

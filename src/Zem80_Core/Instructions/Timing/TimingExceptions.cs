using System.Collections;
using System.Linq;

namespace Zem80.Core.Instructions
{
    public class TimingExceptions
    {
        public bool HasProlongedConditionalOperandDataReadHigh { get; private set; }
        public bool HasProlongedMemoryRead { get; private set; }
        public bool HasProlongedMemoryWrite { get; private set; }
        public int ExtraOpcodeFetchTStates { get; private set; }

        public TimingExceptions(Instruction instruction, InstructionTiming timing)
        {
            bool odh4 = false, mr4 = false, mw5 = false;
            if (instruction.Microcode is CALL)
            {
                // specifically for CALL instructions, the high byte operand read is 4 clock cycles rather than 3 *if* the condition is true (or there is no condition)
                odh4 = true;
            }
            if (timing.MachineCycles.Any(x => x.TStates == 4)) mr4 = true;
            if (timing.MachineCycles.Any(x => x.TStates == 5)) mw5 = true;

            HasProlongedConditionalOperandDataReadHigh = odh4;
            HasProlongedMemoryRead = mr4;
            HasProlongedMemoryWrite = mw5;

            ExtraOpcodeFetchTStates = (timing.MachineCycles.Where(x => x.Type == MachineCycleType.OpcodeFetch).Sum(x => x.TStates)) - InstructionTiming.OPCODE_FETCH_TSTATES;
        }
    }
}

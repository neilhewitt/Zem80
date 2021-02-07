using System.Collections;
using System.Linq;

namespace Zem80.Core.Instructions
{
    public class TimingExceptions
    {
        public bool HasConditionalOperandDataReadHigh4 { get; private set; }
        public bool HasMemoryRead4 { get; private set; }
        public bool HasMemoryWrite5 { get; private set; }

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

            HasConditionalOperandDataReadHigh4 = odh4;
            HasMemoryRead4 = mr4;
            HasMemoryWrite5 = mw5;
        }
    }
}

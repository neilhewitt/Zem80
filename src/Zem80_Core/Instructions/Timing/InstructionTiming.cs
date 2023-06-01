using System;
using System.Collections.Generic;
using System.Linq;

namespace Zem80.Core.CPU
{
    public class InstructionTiming
    {
        public const int OPCODE_FETCH_NORMAL_TSTATES = 4;
        public const int MEMORY_READ_NORMAL_TSTATES = 3;
        public const int NMI_INTERRUPT_ACKNOWLEDGE_TSTATES = 5;
        public const int IM0_INTERRUPT_ACKNOWLEDGE_TSTATES = 6;
        public const int IM1_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;
        public const int IM2_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;

        public IEnumerable<MachineCycle> MachineCycles { get; private set; }
        public MachineCycle OpcodeFetch => MachineCycles.First(); // always first
        public IEnumerable<MachineCycle> OperandReads => MachineCycles.Where(x => x.Type == MachineCycleType.OperandRead || x.Type == MachineCycleType.OperandReadHigh || x.Type == MachineCycleType.OperandReadLow);
        public TimingExceptions Exceptions { get; private set; }
        public int TStates { get; init; }

        public InstructionTiming(Instruction instruction, IEnumerable<MachineCycle> machineCycles)
        {
            MachineCycles = machineCycles;
            Exceptions = new TimingExceptions(instruction, this);
            TStates = MachineCycles.Sum(x => x.TStates);
        }
    }
}

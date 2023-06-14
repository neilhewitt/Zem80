using System;
using System.Collections.Generic;
using System.Linq;

namespace Zem80.Core.CPU
{
    public class InstructionMachineCycles
    {
        private MachineCycle[] _machineCycles = new MachineCycle[6];

        public IEnumerable<MachineCycle> Cycles => _machineCycles.Where(x => x is not null);

        public MachineCycle M1 => _machineCycles[0];
        public MachineCycle M2 => _machineCycles[1];
        public MachineCycle M3 => _machineCycles[2];
        public MachineCycle M4 => _machineCycles[3];
        public MachineCycle M5 => _machineCycles[4];
        public MachineCycle M6 => _machineCycles[5];

        public IEnumerable<MachineCycle> OpcodeFetches => Cycles.Where(x => x.Type == MachineCycleType.OpcodeFetch);
        public IEnumerable<MachineCycle> OperandReads => Cycles.Where(x => x.Type == MachineCycleType.OperandRead || x.Type == MachineCycleType.OperandReadHigh || x.Type == MachineCycleType.OperandReadLow);
        public IEnumerable<MachineCycle> MemoryReads => Cycles.Where(x => x.Type == MachineCycleType.MemoryRead || x.Type == MachineCycleType.MemoryReadHigh || x.Type == MachineCycleType.MemoryReadLow);
        public IEnumerable<MachineCycle> MemoryWrites => Cycles.Where(x => x.Type == MachineCycleType.MemoryWrite || x.Type == MachineCycleType.MemoryWriteHigh || x.Type == MachineCycleType.MemoryWriteLow);
        
        public int TStates { get; init; }

        public InstructionMachineCycles(IEnumerable<MachineCycle> machineCycles)
        {
            machineCycles.ToArray().CopyTo(_machineCycles, 0);
            TStates = Cycles.Where(x => x is not null).Sum(x => x.TStates);
        }
    }
}

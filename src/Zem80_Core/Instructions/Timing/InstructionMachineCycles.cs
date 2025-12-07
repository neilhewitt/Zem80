using System;
using System.Collections.Generic;
using System.Linq;

namespace Zem80.Core.CPU
{
    public class InstructionMachineCycles
    {
        private IList<MachineCycle> _machineCycles = new List<MachineCycle>();

        public IEnumerable<MachineCycle> Cycles => _machineCycles.Where(x => x != null);

        public MachineCycle M1 => _machineCycles[0];
        public MachineCycle M2 => _machineCycles[1];
        public MachineCycle M3 => _machineCycles[2];
        public MachineCycle M4 => _machineCycles[3];
        public MachineCycle M5 => _machineCycles[4];
        public MachineCycle M6 => _machineCycles[5];

        public MachineCycle OpcodeFetch1 { get; private set; }
        public MachineCycle OpcodeFetch2 { get; private set; }
        public MachineCycle OpcodeFetch3 { get; private set; }

        public MachineCycle OperandRead { get; private set; }
        public MachineCycle OperandReadLow { get; private set; }
        public MachineCycle OperandReadHigh { get; private set; }

        public MachineCycle StackReadLow { get; private set; }
        public MachineCycle StackReadHigh { get; private set; }

        public MachineCycle MemoryRead { get; private set; }
        public MachineCycle MemoryReadLow { get; private set; }
        public MachineCycle MemoryReadHigh { get; private set; }
        public MachineCycle MemoryWrite { get; private set; }
        public MachineCycle MemoryWriteLow { get; private set; }
        public MachineCycle MemoryWriteHigh { get; private set; }

        public MachineCycle InternalOperation1 { get; private set; }
        public MachineCycle InternalOperation2 { get; private set; }

        internal IEnumerable<MachineCycle> OpcodeFetches => Cycles.Where(x => x.Type == MachineCycleType.OpcodeFetch);
        internal IEnumerable<MachineCycle> OperandReads => Cycles.Where(x => x.Type == MachineCycleType.OperandRead);

        public int TStates { get; private set; }

        private MachineCycle GetMachineCycle(MachineCycleType type, int index)
        {
            MachineCycle[] cycles = _machineCycles.Where(x => x.Type == type).ToArray();
            if (index > (cycles.Length - 1))
            {
                return null;
            }

            return cycles[index];
        }

        public InstructionMachineCycles(IEnumerable<MachineCycle> machineCycles)
        {
            _machineCycles = machineCycles.ToList();
            TStates = Cycles.Where(x => x != null).Sum(x => x.TStates);

            OpcodeFetch1 = GetMachineCycle(MachineCycleType.OpcodeFetch, 0);
            OpcodeFetch2 = GetMachineCycle(MachineCycleType.OpcodeFetch, 1);
            OpcodeFetch3 = GetMachineCycle(MachineCycleType.OpcodeFetch, 2);
            OperandRead = GetMachineCycle(MachineCycleType.OperandRead, 0);
            OperandReadLow = GetMachineCycle(MachineCycleType.OperandReadLow, 0);
            OperandReadHigh = GetMachineCycle(MachineCycleType.OperandReadHigh, 0);
            StackReadLow = GetMachineCycle(MachineCycleType.StackReadLow, 0);
            StackReadHigh = GetMachineCycle(MachineCycleType.StackReadHigh, 0);
            MemoryRead = GetMachineCycle(MachineCycleType.MemoryRead, 0);
            MemoryReadLow = GetMachineCycle(MachineCycleType.MemoryReadLow, 0);
            MemoryReadHigh = GetMachineCycle(MachineCycleType.MemoryReadHigh, 0);
            MemoryWrite = GetMachineCycle(MachineCycleType.MemoryWrite, 0);
            MemoryWriteLow = GetMachineCycle(MachineCycleType.MemoryWriteLow, 0);
            MemoryWriteHigh = GetMachineCycle(MachineCycleType.MemoryWriteHigh, 0);
            InternalOperation1 = GetMachineCycle(MachineCycleType.InternalOperation, 0);
            InternalOperation2 = GetMachineCycle(MachineCycleType.InternalOperation, 1);
        }
    }
}

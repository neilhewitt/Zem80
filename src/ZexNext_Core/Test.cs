using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ZexNext.Core
{
    public class Test
    {
        private List<TestCycle> _cycles;

        public IReadOnlyList<TestCycle> Cycles => _cycles;
        public string Name { get; private set; }
        public byte Mask { get; private set; }

        public void Add(TestCycle testCycle)
        {
            _cycles.Add(testCycle);
        }
        
        public Test(string name, byte mask)
        {
            Name = name;
            Mask = mask;
            _cycles = new List<TestCycle>();
        }
    }
}

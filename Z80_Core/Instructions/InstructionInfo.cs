using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class InstructionInfo
    {
        public string Name { get; private set; }
        public string Opcode { get; private set; }
        public string Argument1 { get; private set; }
        public string Argument2 { get; private set; }
        public string Pattern { get; private set; }
        public string Size { get; private set; }
        public string Timing { get; private set; }
        
        public InstructionInfo(string name, string opcode, string argument1, string argument2, string pattern, string size, string timing)
        {
            Name = name;
            Opcode = opcode;
            Argument1 = argument1;
            Argument2 = argument2;
            Pattern = pattern;
            Size = size;
            Timing = timing;
        }
    }
}

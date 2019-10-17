using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z80.Core;

namespace Formatter
{
    class Program
    {
        private static string _template = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class {{instruction}} : IInstructionImplementation
    {
        public ExecutionResult Execute(InstructionPackage package)
        {
            return new ExecutionResult(new Flags(), 0);
        }
    }
}
";
        static void Main(string[] args)
        {
            string path = @"C:\projects\z80\Z80_Core\Microcode";

            string[] instructions = File.ReadAllLines("..\\..\\..\\..\\UniqueInstructions.txt");
            foreach(string instruction in instructions)
            {
                string output = _template;
                output = output.Replace("{{instruction}}", instruction);
                File.WriteAllText(Path.Combine(path, instruction + ".cs"), output);
            }
        }
    }
}

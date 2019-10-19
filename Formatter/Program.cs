using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z80.Core;

namespace Formatter
{
    class Program
    {
        private static string _template = @"using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class {{classname}} : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
{{unprefixed}}
                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
{{CB}}
                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
{{ED}}
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
{{DD}}
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
{{FD}}
                    }
                    break;

                case InstructionPrefix.DDCB:
                    switch (instruction.Opcode)
                    {
{{DDCB}}
                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
{{FDCB}}
                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public {{classname}}()
        {
        }
    }
}
";
        private static string _caseTemplate = @"                        case 0x{{opcode}}: // {{mnemonic}}
                            // code
                            break;
";

        static void Main(string[] args)
        {
            string path = @"C:\projects\z80\Z80_Core\Instructions\Microcode";

            string[] instructions = File.ReadAllLines("..\\..\\..\\..\\UniqueInstructions.txt");
            foreach (string instructionName in instructions)
            {
                string code = _template;
                code = code.Replace("{{classname}}", instructionName);
                string unprefixed = "";
                string CB = "";
                string ED = "";
                string DD = "";
                string FD = "";
                string DDCB = "";
                string FDCB = "";
                foreach(Instruction instruction in Instruction._instructionSet.Values.SelectMany(x => x.Values).Where(x => x.Mnemonic.StartsWith(instructionName)))
                {
                    string thisCase = _caseTemplate;
                    thisCase = thisCase.Replace("{{opcode}}", instruction.Opcode.ToString("X2")).Replace("{{mnemonic}}", instruction.Mnemonic);
                    switch (instruction.Prefix)
                    {
                        case InstructionPrefix.Unprefixed:
                            unprefixed += thisCase;
                            break;
                        case InstructionPrefix.CB:
                            CB += thisCase;
                            break;
                        case InstructionPrefix.DD:
                            DD += thisCase;
                            break;
                        case InstructionPrefix.ED:
                            ED += thisCase;
                            break;
                        case InstructionPrefix.FD:
                            FD += thisCase;
                            break;
                        case InstructionPrefix.DDCB:
                            DDCB += thisCase;
                            break;
                        case InstructionPrefix.FDCB:
                            FDCB += thisCase;
                            break;
                    }
                }

                code = code.Replace("{{unprefixed}}", unprefixed);
                code = code.Replace("{{CB}}", CB);
                code = code.Replace("{{ED}}", ED);
                code = code.Replace("{{DD}}", DD);
                code = code.Replace("{{FD}}", FD);
                code = code.Replace("{{DDCB}}", DDCB);
                code = code.Replace("{{FDCB}}", FDCB);

                //File.WriteAllText(Path.Combine(path, instructionName + ".cs"), code);
            }
        }
    }
}

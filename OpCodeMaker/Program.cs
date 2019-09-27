using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpCodeMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            List<string> output = new List<string>();
            string[] input = File.ReadAllLines(Path.Combine(path, "z80_instruction.txt"));
            
            List<InstructionPair> instructions = new List<InstructionPair>();
            foreach(string line in input.Skip(1))
            {
                string[] parts = line.Split('\t');
                instructions.Add(new InstructionPair() { Opcode = parts[1], Instruction = parts[0] });
            }

            IList<InstructionPair> DD = instructions.Where(i => i.Opcode.StartsWith("DD ")).ToList();
            IList<InstructionPair> FD = instructions.Where(i => i.Opcode.StartsWith("FD ")).ToList();
            IList<InstructionPair> CB = instructions.Where(i => i.Opcode.StartsWith("CB ")).ToList();
            IList<InstructionPair> ED = instructions.Where(i => i.Opcode.StartsWith("ED ")).ToList();
            IList<InstructionPair> others = instructions.Where(i => !i.Opcode.StartsWith("DD") 
                && !i.Opcode.StartsWith("FD") && !i.Opcode.StartsWith("CB") && !i.Opcode.StartsWith("ED")).ToList();

            IList<InstructionPair> final = others.OrderBy(i => i.Instruction).Concat(
                DD.OrderBy(i => i.Instruction).Concat(
                    FD.OrderBy(i => i.Instruction).Concat(
                        CB.OrderBy(i => i.Instruction).Concat(
                            ED.OrderBy(i => i.Instruction))))).ToList();

            File.WriteAllLines(
                Path.Combine(path, "..\\..\\..\\opcodes.txt"),
                final.Select(i => "case: \"" + i.Opcode + "\" " + new string(' ', 20 - i.Opcode.Length) + "// " + i.Instruction.Trim('\"'))
                );
        }

        public struct InstructionPair
        {
            public string Opcode;
            public string Instruction;
        }
    }
}

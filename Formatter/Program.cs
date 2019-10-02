using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z80.Core;

namespace Formatter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("..\\..\\..\\instruction_table.txt");

            //IDictionary<string, IDictionary<string, string>> opcodeSets =
            //    new Dictionary<string, IDictionary<string, string>>()
            //    {
            //        { "0x00", new Dictionary<string, string>() },
            //        { "0xCB", new Dictionary<string, string>() },
            //        { "0xDD", new Dictionary<string, string>() },
            //        { "0xED", new Dictionary<string, string>() },
            //        { "0xFD", new Dictionary<string, string>() },
            //        { "0xDD 0xCB", new Dictionary<string, string>() },
            //        { "0xFD 0xCB", new Dictionary<string, string>() },
            //    };

            List<string> codeLines = new List<string>();

            foreach (string line in lines.Skip(1))
            {

                string[] parts = line.TrimStart('/', '\t').Split('\t');
                string instruction = parts[0].TrimEnd(' ');
                string opcode = parts[1].TrimEnd(' ');
                string size = parts[2].TrimEnd(' ');
                string timing = parts[3].TrimEnd(' ');

                string opcodeByte = String.Empty;
                string opcodePrefix = "00";
                string opcodeArgument1 = String.Empty;
                string opcodeArgument2 = String.Empty;
                string opcodePattern = String.Empty;

                string[] opcodeParts = opcode.Split(' ');
                switch (opcodeParts.Length)
                {
                    case 1:
                        if (opcodeParts[0].Contains("+"))
                        {
                            opcodeByte = opcodeParts[0].Substring(0, 2);
                            opcodePattern = opcodeParts[0].Substring(3);
                        }
                        else
                        {
                            opcodeByte = opcodeParts[0];
                        }
                        break;
                    case 2:
                        opcodePrefix = opcodeParts[0];
                        if (opcodeParts[1].Contains("+"))
                        {
                            opcodeByte = opcodeParts[1].Substring(0, 2);
                            opcodePattern = opcodeParts[1].Substring(3);
                        }
                        else if (opcodeParts[1] == "n" || opcodeParts[1] == "nn" || opcodeParts[1] == "o")
                        {
                            opcodeArgument1 = opcodeParts[1];
                            opcodePrefix = "00";
                            opcodeByte = opcodeParts[0];
                        }
                        else
                        {
                            opcodeByte = opcodeParts[1];
                        }
                        break;
                    case 3:
                        if (opcodeParts[1] == "nn" && opcodeParts[2] == "nn")
                        {
                            opcodeByte = opcodeParts[0];
                            opcodeArgument1 = opcodeParts[1];
                            opcodeArgument2 = opcodeParts[2];
                        }
                        else
                        {
                            opcodePrefix = opcodeParts[0];
                            if (opcodeParts[1].Contains("+"))
                            {
                                opcodeByte = opcodeParts[1].Substring(0, 2);
                                opcodePattern = opcodeParts[1].Substring(3);
                            }
                            else
                            {
                                opcodeByte = opcodeParts[1];
                            }
                            opcodeArgument1 = opcodeParts[2];
                        }
                        break;
                    case 4:
                        // special case
                        if ((opcodeParts[0] == "DD" || opcodeParts[0] == "FD") && opcodeParts[1] == "CB")
                        {
                            opcodePrefix = opcodeParts[0] + " 0x" + opcodeParts[1];
                            opcodeByte = opcodeParts[0] + " CB ";
                            opcodeArgument1 = "o"; 

                            if (opcodeParts[3].Contains("+"))
                            {
                                opcodeArgument2 = opcodeParts[3].Substring(0, 2);
                                opcodePattern = opcodeParts[3].Substring(3);
                            }
                            else
                            {
                                opcodeArgument2 = opcodeParts[3];
                            }

                            opcodeByte += opcodeArgument2; // have to make the key unique by adding final byte
                        }
                        else
                        {
                            opcodePrefix = opcodeParts[0];
                            opcodeByte = opcodeParts[1];
                            if (opcodeParts[2] == "nn" && opcodeParts[3] == "nn")
                            {
                                opcodeArgument1 = opcodeParts[2];
                                opcodeArgument2 = opcodeParts[3];
                            }
                            else if (opcodeParts[2] == "o")
                            {
                                opcodeArgument1 = opcodeParts[2];
                                if (opcodeParts[3].Contains("+"))
                                {
                                    opcodeArgument2 = opcodeParts[3].Substring(0, 2);
                                    opcodePattern = opcodeParts[3].Substring(3);
                                }
                                else
                                {
                                    opcodeArgument2 = opcodeParts[3];
                                }
                            }
                        }
                        break;
                }

                if (opcodePattern.Length > 0) opcodePattern = "+" + opcodePattern;

                // { 0x00, new InstructionInfo("ADC A,(HL)", 0x8E, "", "", "", 1, "7") }
                string outputLine = "{ \"" + opcodePrefix + "\", new InstructionInfo(\"" + instruction
                     + "\", \"" + opcodeByte + "\", \"" + opcodeArgument1 + "\", \"" + opcodeArgument2
                     + "\", \"" + opcodePattern + "\", \"" + size + "\", \"" + timing + "\") },";

                //opcodeSets["0x" + opcodePrefix].Add(opcodeByte, outputLine);

                codeLines.Add(outputLine);
                File.WriteAllLines("..\\..\\..\\code.txt", codeLines.OrderBy(x => x));
            }
        }
    }
}

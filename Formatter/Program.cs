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

            //string[] lines = File.ReadAllLines("..\\..\\..\\instruction_table.txt");

            ////IDictionary<string, IDictionary<string, string>> opcodeSets =
            ////    new Dictionary<string, IDictionary<string, string>>()
            ////    {
            ////        { "0x00", new Dictionary<string, string>() },
            ////        { "0xCB", new Dictionary<string, string>() },
            ////        { "0xDD", new Dictionary<string, string>() },
            ////        { "0xED", new Dictionary<string, string>() },
            ////        { "0xFD", new Dictionary<string, string>() },
            ////        { "0xDD 0xCB", new Dictionary<string, string>() },
            ////        { "0xFD 0xCB", new Dictionary<string, string>() },
            ////    };

            ////Dictionary<string, List<string>> codeLines = new Dictionary<string, List<string>>();
            //List<string> output = new List<string>();

            //foreach (string line in lines.Skip(1))
            //{

            //    string[] parts = line.TrimStart('/', '\t').Split('\t');
            //    string instruction = parts[0].TrimEnd(' ');
            //    string opcode = parts[1].TrimEnd(' ');
            //    string size = parts[2].TrimEnd(' ');
            //    string timing = parts[3].TrimEnd(' ');

            //    string opcodeByte = String.Empty;
            //    string opcodePrefix = "00";
            //    string opcodeArgument1 = String.Empty;
            //    string opcodeArgument2 = String.Empty;
            //    string opcodePattern = String.Empty;

            //    string[] opcodeParts = opcode.Split(' ');
            //    switch (opcodeParts.Length)
            //    {
            //        case 1:
            //            if (opcodeParts[0].Contains("+"))
            //            {
            //                opcodeByte = opcodeParts[0].Substring(0, 2);
            //                opcodePattern = opcodeParts[0].Substring(3);
            //            }
            //            else
            //            {
            //                opcodeByte = opcodeParts[0];
            //            }
            //            break;
            //        case 2:
            //            opcodePrefix = opcodeParts[0];
            //            if (opcodeParts[1].Contains("+"))
            //            {
            //                opcodeByte = opcodeParts[1].Substring(0, 2);
            //                opcodePattern = opcodeParts[1].Substring(3);
            //            }
            //            else if (opcodeParts[1] == "n" || opcodeParts[1] == "nn" || opcodeParts[1] == "o")
            //            {
            //                opcodeArgument1 = opcodeParts[1];
            //                opcodePrefix = "00";
            //                opcodeByte = opcodeParts[0];
            //            }
            //            else
            //            {
            //                opcodeByte = opcodeParts[1];
            //            }
            //            break;
            //        case 3:
            //            if (opcodeParts[1] == "nn" && opcodeParts[2] == "nn")
            //            {
            //                opcodeByte = opcodeParts[0];
            //                opcodeArgument1 = opcodeParts[1];
            //                opcodeArgument2 = opcodeParts[2];
            //            }
            //            else
            //            {
            //                opcodePrefix = opcodeParts[0];
            //                if (opcodeParts[1].Contains("+"))
            //                {
            //                    opcodeByte = opcodeParts[1].Substring(0, 2);
            //                    opcodePattern = opcodeParts[1].Substring(3);
            //                }
            //                else
            //                {
            //                    opcodeByte = opcodeParts[1];
            //                }
            //                opcodeArgument1 = opcodeParts[2];
            //            }
            //            break;
            //        case 4:
            //            // special case
            //            if ((opcodeParts[0] == "DD" || opcodeParts[0] == "FD") && opcodeParts[1] == "CB")
            //            {
            //                opcodePrefix = opcodeParts[0] + " " + opcodeParts[1];
            //                opcodeByte = opcodeParts[0] + " CB ";
            //                opcodeArgument1 = "o";

            //                if (opcodeParts[3].Contains("+"))
            //                {
            //                    opcodeArgument2 = opcodeParts[3].Substring(0, 2);
            //                    opcodePattern = opcodeParts[3].Substring(3);
            //                }
            //                else
            //                {
            //                    opcodeArgument2 = opcodeParts[3];
            //                }

            //                opcodeByte += opcodeArgument2; // have to make the key unique by adding final byte
            //            }
            //            else
            //            {
            //                opcodePrefix = opcodeParts[0];
            //                opcodeByte = opcodeParts[1];
            //                if (opcodeParts[2] == "nn" && opcodeParts[3] == "nn")
            //                {
            //                    opcodeArgument1 = opcodeParts[2];
            //                    opcodeArgument2 = opcodeParts[3];
            //                }
            //                else if (opcodeParts[2] == "o")
            //                {
            //                    opcodeArgument1 = opcodeParts[2];
            //                    if (opcodeParts[3].Contains("+"))
            //                    {
            //                        opcodeArgument2 = opcodeParts[3].Substring(0, 2);
            //                        opcodePattern = opcodeParts[3].Substring(3);
            //                    }
            //                    else
            //                    {
            //                        opcodeArgument2 = opcodeParts[3];
            //                    }
            //                }
            //            }
            //            break;
            //    }

            //    if (opcodePattern.Length > 0)
            //    {
            //        int index = opcodeByte.LastIndexOf(' ');
            //        if (index == -1) index = 0;
            //        string opcodeRaw = opcodeByte.Substring(index).Replace(" ", "");

            //        opcodePattern = "+" + opcodePattern;
            //        if (opcodePattern == "+r")
            //        {
            //            byte opcodeBase = byte.Parse(opcodeRaw, System.Globalization.NumberStyles.HexNumber);
            //            string code = "";
            //            for (byte b = 0; b < 8; b++)
            //            {
            //                opcodeBase = opcodeBase.SetBits(0, b.GetBits(0, 3));
            //                if (b != 6)
            //                {
            //                    code = "{ (\"" + opcodePrefix + "\", 0x" + opcodeBase.ToString("X2") + "), 0x" + opcodeRaw + "},";
            //                    output.Add(code);
            //                }
            //            }
            //        }
            //        else if (opcodePattern == "+p" || opcodePattern == "+q")
            //        {
            //            byte opcodeBase = byte.Parse(opcodeRaw, System.Globalization.NumberStyles.HexNumber);
            //            string code = "";
            //            for (byte b = 4; b < 6; b++)
            //            {
            //                opcodeBase = opcodeBase.SetBits(0, b.GetBits(0, 3));
            //                code = "{ (\"" + opcodePrefix + "\", 0x" + opcodeBase.ToString("X2") + "), 0x" + opcodeRaw + "},";
            //                output.Add(code);
            //            }
            //        }
            //        else if (opcodePattern == "+8*b" || opcodePattern == "+8*p" || opcodePattern == "+8*q")
            //        {
            //            byte opcodeBase = byte.Parse(opcodeRaw, System.Globalization.NumberStyles.HexNumber);
            //            string code = "";
            //            for (byte b = 0; b < 8; b++)
            //            {
            //                opcodeBase = opcodeBase.SetBits(3, b.GetBits(0, 3));
            //                code = "{ (\"" + opcodePrefix + "\", 0x" + (opcodeBase).ToString("X2") + "), 0x" + opcodeRaw + "}, ";
            //                output.Add(code);
            //            }
            //        }
            //        else if (opcodePattern == "+8*b+r")
            //        {
            //            byte opcodeBase = byte.Parse(opcodeRaw, System.Globalization.NumberStyles.HexNumber);
            //            string code = "";
            //            for (byte r = 0; r < 8; r++)
            //            {
            //                if (r != 6)
            //                {
            //                    opcodeBase = opcodeBase.SetBits(0, r.GetBits(0, 3));
            //                    for (byte b = 0; b < 8; b++)
            //                    {
            //                        opcodeBase = opcodeBase.SetBits(3, b.GetBits(0, 3));
            //                        code = "{ (\"" + opcodePrefix + "\", 0x" + (opcodeBase).ToString("X2") + "), 0x" + opcodeRaw + "}, ";
            //                        output.Add(code);
            //                    }
            //                }
            //            }
            //        }

            //    }

            //    opcodeByte = opcodeByte.PadLeft(2, '0');

            //    File.WriteAllLines("..\\..\\..\\list.txt", output.Distinct());


            //    // { 0x00, new InstructionInfo("ADC A,(HL)", 0x8E, "", "", "", 1, "7") }
            //    //string outputLine = "{ \"" + opcodeByte + "\", new InstructionInfo(\"" + opcodePrefix + "\",\"" + instruction
            //    //     + "\", \"" + opcodeByte + "\", \"" + opcodeArgument1 + "\", \"" + opcodeArgument2
            //    //     + "\", \"" + opcodePattern + "\", \"" + size + "\", \"" + timing + "\") },";

            //    //opcodeSets["0x" + opcodePrefix].Add(opcodeByte, outputLine);

            //    //if (!codeLines.ContainsKey(opcodePrefix))
            //    //{
            //    //    codeLines.Add(opcodePrefix, new List<string>());
            //    //}

            //    //codeLines[opcodePrefix].Add(outputLine);

            //    //foreach (string key in codeLines.Keys)
            //    //{
            //    //    foreach(string content in codeLines[key])
            //    //    {


            //    //    }
            //    //}

            //}
        }
    }
}

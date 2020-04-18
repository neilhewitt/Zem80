using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestMaker
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] specLines = File.ReadAllLines("testspec.txt");
            Queue<string> lines = new Queue<string>(specLines);
            bool processing = true;

            List<TestBlock> tests = new List<TestBlock>();
            while (processing)
            {
                string line = next();
                TestBlock block;
                if (line.StartsWith(";"))
                {
                    // start of test block
                    block = new TestBlock();
                    int endIndex = line.IndexOf("cycles");
                    if (endIndex > -1)
                    {
                        int startIndex = endIndex;
                        while (line[startIndex--] != '(') ;
                        block.Cycles = int.Parse(line.Substring(startIndex + 2, endIndex - startIndex - 3).Replace(",", ""));
                    }
                    line = next();
                    line = removeTrailingComment(line);
                    var lineBits = line.Split(":");
                    block.Name = lineBits[0];
                    block.Mask = hexify(lineBits[1], "db", true);

                    line = next();
                    line = removeTrailingComment(line);
                    if (line.StartsWith("\ttstr"))
                    {
                        block.Base = hexify(line, "tstr");

                        line = next();
                        line = removeTrailingComment(line);
                        block.Incremement = hexify(line, "tstr");
                        
                        line = next();
                        line = removeTrailingComment(line);
                        block.Shift = hexify(line, "tstr");

                        line = next();
                        line = removeTrailingComment(line);
                        block.CRC = hexify(line, "db", true);


                        line = next();
                        if (line.StartsWith("\t"))
                        {
                            block.Message = line.ToUpper().Replace("\tTMSG\t'", "").Replace("'", "");
                        }

                        if (lines.Count > 0) line = next(); // blank line at end
                    }
                    else
                    {
                        // throw wobbly
                        throw new InvalidDataException("Nope. Bad data.");
                    }
                }
                else
                {
                    // throw wobbly
                    throw new InvalidDataException("Nope. Bad data.");
                }

                tests.Add(block);
                if (lines.Count == 0) processing = false;
            }

            string outputTemplate = File.ReadAllText("template.txt");
            StringBuilder output = new StringBuilder();
            foreach(TestBlock test in tests)
            {
                string testOutput = outputTemplate.Replace("{{test_name}}", test.Name);
                testOutput = testOutput.Replace("{{name}}", test.Name);
                testOutput = testOutput.Replace("{{msg}}", test.Message);
                testOutput = testOutput.Replace("{{cycles}}", test.Cycles.ToString());
                testOutput = testOutput.Replace("{{mask}}", test.Mask);
                testOutput = testOutput.Replace("{{state}}", getVectorCode("Base", getVector(test.Base)));
                testOutput = testOutput.Replace("{{increment}}", getVectorCode("Increment", getVector(test.Incremement)));
                testOutput = testOutput.Replace("{{shift}}", getVectorCode("Shift", getVector(test.Shift)));
                testOutput = testOutput.Replace("{{crc}}", test.CRC);
                output.Append(testOutput + ",\n");
            }

            File.WriteAllText("..\\..\\..\\output.txt", output.ToString().TrimEnd(',', '\n'));

            // local functions

            string next()
            {
                return lines.Dequeue();
            }

            string removeTrailingComment(string input)
            {
                return input.Split(';')[0].TrimEnd('\t');
            }

            string hexify(string input, string prefix, bool addHexPrefix = false)
            {
                ushort msbt = 0x0014;
                byte msbthi = (byte)(msbt / 0x100);
                byte msbtlo = (byte)(msbt & 0xFF);

                input = input.ToUpper().Replace($"{ prefix.ToUpper() }", "").Replace("\t","").Replace("H", "");
                string[] bits = input.Split(",");
                for (int i = 0; i < bits.Length; i++)
                {
                    bits[i] = bits[i].Trim();
                    if (bits[i].StartsWith("0") && bits[i] != "0")
                    {
                        // it's hex
                        bits[i] = bits[i].Substring(1);
                        bits[i] = bits[i].TrimEnd('h');
                        if (addHexPrefix) bits[i] = "0x" + bits[i];
                    }
                    else
                    {
                        if (bits[i] == "-1")
                        {
                            bits[i] = (addHexPrefix? "0x" : "") + "FFFF"; // might be a byte but we'll deal with that later
                        }
                        else
                        {
                            if (bits[i].StartsWith("MSBTHI"))
                            {
                                bits[i] = (addHexPrefix ? "0x" : "") + msbthi.ToString("X2");
                            }
                            else if (bits[i].StartsWith("MSBTLO"))
                            {
                                bits[i] = (addHexPrefix ? "0x" : "") + msbtlo.ToString("X2");
                            }
                            else if (bits[i].StartsWith("MSBT"))
                            {
                                if (bits[i].StartsWith("MSBT-"))
                                {
                                    int minus = int.Parse(bits[i].Split("-")[1], System.Globalization.NumberStyles.HexNumber);
                                    bits[i] = (addHexPrefix ? "0x" : "") + (msbt - minus).ToString("X4");
                                }
                                else if (bits[i].StartsWith("MSBT+"))
                                {
                                    int plus = int.Parse(bits[i].Split("+")[1], System.Globalization.NumberStyles.HexNumber);
                                    bits[i] = (addHexPrefix ? "0x" : "") + (msbt + plus).ToString("X4");
                                }
                                else
                                {
                                    bits[i] = (addHexPrefix ? "0x" : "") + msbt.ToString("X4");
                                }
                            }
                            else
                            {
                                // it's a plain decimal number - convert to hex
                                bits[i] = int.Parse(bits[i]).ToString("X4");
                                if (bits[i].StartsWith("00")) bits[i] = bits[i].Substring(3); // in case it's a byte
                                if (addHexPrefix) bits[i] = "0x" + bits[i];
                            }
                        }
                    }
                }

                input = String.Join(',', bits);
                string output = input.Replace($"\t{ prefix }\t", "", StringComparison.InvariantCultureIgnoreCase);
                return output;
            }

            TestVector getVector(string input)
            {
                TestVector vector = new TestVector();
                string[] bits = input.Replace("0x00", "0").Replace("0x", "").Split(",");
                
                vector.Instruction = (
                    byte.Parse(bits[0].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(bits[1].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(bits[2].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(bits[3].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber)
                    );

                vector.MemOp = ushort.Parse(bits[4], System.Globalization.NumberStyles.HexNumber);
                vector.IY = ushort.Parse(bits[5], System.Globalization.NumberStyles.HexNumber);
                vector.IX = ushort.Parse(bits[6], System.Globalization.NumberStyles.HexNumber);
                vector.HL = ushort.Parse(bits[7], System.Globalization.NumberStyles.HexNumber);
                vector.DE = ushort.Parse(bits[8], System.Globalization.NumberStyles.HexNumber);
                vector.BC = ushort.Parse(bits[9], System.Globalization.NumberStyles.HexNumber);
                vector.F = byte.Parse(bits[10].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber);
                vector.A = byte.Parse(bits[11].Replace("FFFF", "FF"), System.Globalization.NumberStyles.HexNumber);
                vector.SP = ushort.Parse(bits[12], System.Globalization.NumberStyles.HexNumber);

                return vector;
            }

            string getVectorCode(string name, TestVector vector)
            {
                string output = "new TestVector()\n\t\t\t\t\t\t{\n";
                output += "\t\t\t\t\t\tInstruction = new InstructionBytes((byte)0x" + vector.Instruction.Item1.ToString("X2") + ", (byte)0x"
                    + vector.Instruction.Item2.ToString("X2") + ", (byte)0x"
                    + vector.Instruction.Item3.ToString("X2") + ", (byte)0x"
                    + vector.Instruction.Item4.ToString("X2") + "),\n";
                output += "\t\t\t\t\t\tMemOp = 0x" + vector.MemOp.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tIY = 0x" + vector.IY.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tIX = 0x" + vector.IX.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tHL = 0x" + vector.HL.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tDE = 0x" + vector.DE.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tBC = 0x" + vector.BC.ToString("X4") + ",\n";
                output += "\t\t\t\t\t\tF = 0x" + vector.F.ToString("X2") + ",\n";
                output += "\t\t\t\t\t\tA = 0x" + vector.A.ToString("X2") + ",\n";
                output += "\t\t\t\t\t\tSP = 0x" + vector.SP.ToString("X4") + "\n";
                output += "\t\t\t\t\t}";

                return output.Replace("0x","0x");
            }
        }
    }

    public struct TestBlock
    {
        public string Mask;
        public string Name;
        public string Message;
        public int Cycles;
        public string Base;
        public string Incremement;
        public string Shift;
        public string CRC;
    }

    public struct TestVector
    {
        public (byte, byte, byte, byte) Instruction;
        public int MemOp;
        public int IY;
        public int IX;
        public int HL;
        public int DE;
        public int BC;
        public short F;
        public short A;
        public int SP;
    }
}

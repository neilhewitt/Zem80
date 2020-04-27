using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z80.Core;

namespace MachineCycleTableMaker
{
    static class Program
    {
        static void Main(string[] args)
        {
            List<string> mc = new List<string>();
            mc.AddRange(File.ReadAllLines("z80_instructions_mcycles.txt").Skip(1));
            Dictionary<string, string> cycles = new Dictionary<string, string>();
            foreach(string m in mc)
            {
                string[] parts = m.Split('\t');
                string mnemonic = parts[0].Replace("\"", "");
                cycles.Add(mnemonic, String.Join('\t', parts[1..]));
            }

            List<string> ops = new List<string>();
            foreach (Instruction i in InstructionSet.Instructions.Values)
            {
                string prefix = i.Prefix.ToString();
                if (i.Prefix == InstructionPrefix.Unprefixed) prefix = "";

                string build = $@"instructions.Add(new Instruction(""{prefix}{i.Opcode.ToString("X2")}"", ";
                build += $@"""{i.Mnemonic}"", ArgumentType.{i.Argument1.ToString()}, ArgumentType.{i.Argument2.ToString()}, ";
                build += $@"ModifierType.{i.Modifier.ToString()}, {i.SizeInBytes}, new MachineCycle[] {{ ";
                string[] machineCycles = cycles[i.Mnemonic].Split('\t');
                foreach(string machineCycle in machineCycles)
                {
                    if (machineCycle != "")
                    {
                        MachineCycleType cycleType = machineCycle switch
                        {
                            var x when x.StartsWith("OCF") => MachineCycleType.OpcodeFetch,
                            var x when x.StartsWith("IO") => MachineCycleType.InternalOperation,
                            var x when x.StartsWith("MRH") => MachineCycleType.MemoryReadHigh,
                            var x when x.StartsWith("MRL") => MachineCycleType.MemoryReadLow,
                            var x when x.StartsWith("MR") => MachineCycleType.MemoryRead,
                            var x when x.StartsWith("MWH") => MachineCycleType.MemoryWriteHigh,
                            var x when x.StartsWith("MWL") => MachineCycleType.MemoryWriteLow,
                            var x when x.StartsWith("MW") => MachineCycleType.MemoryWrite,
                            var x when x.StartsWith("ODH") => MachineCycleType.OperandReadHigh,
                            var x when x.StartsWith("ODL") => MachineCycleType.OperandReadLow,
                            var x when x.StartsWith("OD") => MachineCycleType.OperandRead,
                            var x when x.StartsWith("PR") => MachineCycleType.PortRead,
                            var x when x.StartsWith("PW") => MachineCycleType.PortWrite,
                            var x when x.StartsWith("SRH") => MachineCycleType.StackReadHigh,
                            var x when x.StartsWith("SRL") => MachineCycleType.StackReadLow,
                            var x when x.StartsWith("SWH") => MachineCycleType.StackWriteHigh,
                            var x when x.StartsWith("SWL") => MachineCycleType.StackWriteLow,
                            _ => MachineCycleType.OpcodeFetch
                        };

                        int numberOfCycles = int.Parse(machineCycle.Substring(machineCycle.IndexOf('(') + 1).TrimEnd(')', '*'));

                        build += $"new MachineCycle(MachineCycleType.{cycleType.ToString()}, {numberOfCycles}, {(machineCycle.EndsWith("*") ? "true" : "false")}) , ";
                    }
                }

                build = build.TrimEnd(',', ' ');
                build += "}));";

                ops.Add(build);
                    
            }

            File.WriteAllLines("output.txt", ops);

            //var machineCycles = new Dictionary<(string, InstructionPrefix, byte), IList<(MachineCycleType, int)>>();
            
            //foreach(Instruction instruction in InstructionSet.Instructions.Values.SelectMany(x => x.Values))
            //{

            //    //if (instruction.SizeInBytes == 1 && instruction.ClockCycles == 4)
            //    //{
            //    //    // simplest case
            //    //    machineCycles.AddCycle(instruction, MachineCycleType.OpcodeFetch, 4);
            //    //}

            //    if (instruction.SizeInBytes == 2)
            //    {
            //        if (instruction.ClockCycles == 5)
            //        {
            //            machineCycles.AddCycle(instruction, MachineCycleType.OpcodeFetch, 5);
            //        }
            //        else if (instruction.Argument1 == ArgumentType.None && instruction.Modifier == ModifierType.None)
            //        {
            //            machineCycles.AddCycle(instruction, MachineCycleType.OpcodeFetch, 4);
            //            machineCycles.AddCycle(instruction, MachineCycleType.OpcodeFetch, instruction.ClockCycles - 4); // usually another 4, sometimes 6
            //        }
            //        else if (instruction.Argument1 == ArgumentType.Immediate)
            //        {
            //            machineCycles.AddCycle(instruction, MachineCycleType.OpcodeFetch, 4);
            //            switch(instruction.ClockCycles)
            //            {
            //                case 7:
            //                    machineCycles.AddCycle(instruction, MachineCycleType.OperandRead, 3);
            //                    break;
            //                case 10:
            //                    machineCycles.AddCycle(instruction, MachineCycleType.OperandRead, 3);
            //                    machineCycles.AddCycle(instruction, MachineCycleType.MemoryWrite, 3);
            //                    break;

            //            }
            //        }
            //    }
            //}
        
        }

        static void AddCycle(this Dictionary<(string, InstructionPrefix, byte), IList<(MachineCycleType, int)>> machineCycles, Instruction instruction, MachineCycleType type, int cycles)
        {
            var key = (instruction.Mnemonic, instruction.Prefix, instruction.Opcode);
            
            if (!machineCycles.ContainsKey(key))
            {
                machineCycles.Add(key, new List<(MachineCycleType, int)>());
            }

            machineCycles[key].Add((type, cycles));
        }
    }
}

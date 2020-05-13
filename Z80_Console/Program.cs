using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Z80.Core;
using Z80.SimpleVM;

namespace Z80_Console
{
    class Program
    {
        private static Processor _cpu;
        private static bool _flowChange;
        private static int _tabs;
        private static bool _stepThrough = false;

        static void Main(string[] args)
        {
            // NOTE TO ALL WHO ENTER HERE
            // This has become a general test program / playground. Yes, I know it's messy. Yes, I'm OK with that. Yes, it will get cleaned up later.

            VirtualMachine vm = new VirtualMachine(speed: 4);
            _cpu = (Processor)vm.CPU;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            //vm.Load(0x05, "cpm_patch.bin"); // patch the CP/M BDOS routines out
            //vm.Load(0x100, "zexdoc.bin");

            byte[] program = new byte[10001];
            for (int i = 0; i < 10000; i++) program[i] = 0x80;
            program[10000] = 0x76;
            vm.Load(0x100, program);
            Console.ForegroundColor = ConsoleColor.White;

            //_cpu.Debug.BeforeExecute += Before_Instruction_Execution;
            //_cpu.Debug.AfterExecute += After_Instruction_Execute;
            //_cpu.Debug.OnStop += Debug_OnStop;
            while (true)
            {
                foreach (var tm in new TimingMode[] { TimingMode.FastAndFurious, TimingMode.PseudoRealTime })
                {
                    vm.Start(address: 0x100, timingMode: tm, endOnHalt: true, synchronous: true);
                    int row = tm == TimingMode.FastAndFurious ? 0 : 6;
                    Console.SetCursorPosition(0, row);
                    TimeSpan elapsed = _cpu.Elapsed;
                    TimeSpan expected = TimeSpan.FromTicks((long)((double)_cpu.EmulatedTStates * (double)((double)10 / (double)_cpu.FrequencyInMHz)));
                    TimeSpan difference = elapsed.Subtract(expected);
                    Console.WriteLine(tm.ToString());
                    Console.WriteLine("Elapsed: " + elapsed + "                         ");
                    Console.WriteLine("Expected: " + expected + "                         ");
                    Console.WriteLine("Difference: " + difference + "                         ");
                    Console.WriteLine("Emulated clock ticks: " + _cpu.EmulatedTStates + "                         ");
                    Console.ForegroundColor = ConsoleColor.White;
                    //Console.WriteLine("\r\nProgram has finished. Press enter to exit.");
                    //Console.ReadLine();
                }
                Thread.Sleep(100);
            }
        }

        private static void After_Instruction_Execute(object sender, ExecutionResult e)
        {
            //string mnemonic = e.Instruction.Mnemonic;
            //if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0xx" + e.Data.ArgumentsAsWord.ToString("X4"));
            //else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0xx" + e.Data.Argument1.ToString("X2"));
            //if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0xx" + e.Data.Argument1.ToString("X2"));
            //Console.WriteLine("Executed: " + mnemonic);
            //Console.WriteLine("Time taken: " + _cpu.LastInstructionTime + " ticks");
            //int cycles = (e.Instruction.ClockCyclesConditional.HasValue ? e.Instruction.ClockCyclesConditional.Value : e.Instruction.ClockCycles);
            //Console.WriteLine("Time expected: " + (cycles * 2.5) + " ticks");
            //Console.WriteLine("Instruction clocks: " + e.Instruction.ClockCycles + (e.Instruction.ClockCyclesConditional.HasValue ? (" | " + e.Instruction.ClockCyclesConditional) : ""));
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("-----------------------------------------------------");
            ////if (/*timeTaken > timeInTheory || */(_cpu.LastInstructionClockCycles != e.Instruction.ClockCycles
            ////    && _cpu.LastInstructionClockCycles != e.Instruction.ClockCyclesConditional)) 
            //Console.ReadLine();
        }

        private static void Before_Instruction_Execution(object sender, ExecutionPackage e)
        {
            //ConsoleColor oldColour = Console.ForegroundColor;
            //Console.ForegroundColor = _flowChange ? ConsoleColor.Red : ConsoleColor.Yellow;
            //string values = regValue(ByteRegister.A) + wregValue(WordRegister.BC) + wregValue(WordRegister.DE) + wregValue(WordRegister.HL) + wregValue(WordRegister.SP) + wregValue(WordRegister.PC);
            //values = values.TrimEnd(' ', '|');
            ////Console.WriteLine(values);

            //string mnemonic = e.Instruction.Mnemonic;
            //if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
            //else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
            //if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
            //Console.WriteLine(new string('\t', _tabs) + e.InstructionAddress.ToString("X4") + ": " + mnemonic);
            //Console.ForegroundColor = oldColour;
            //_flowChange = false;

            ////if (_stepThrough) 
            ////    Console.ReadLine();
            //Thread.Sleep(25);

            //string regValue(ByteRegister r)
            //{
            //    byte value = _cpu.Registers[r];
            //    return r.ToString() + ": 0x" + value.ToString("X2") + " | ";
            //}

            //string wregValue(WordRegister r)
            //{
            //    ushort value = _cpu.Registers[r];
            //    return r.ToString() + ": 0x" + value.ToString("X4") + " | ";
            //}
        }
    }
}

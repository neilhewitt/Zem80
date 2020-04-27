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
        private static bool _flowCange;
        private static int _tabs;

        static void Main(string[] args)
        {
            // NOTE TO ALL WHO ENTER HERE
            // This has become a general test program / playground. Yes, I know it's messy. Yes, I'm OK with that. Yes, it will get cleaned up later.

            VirtualMachine vm = new VirtualMachine();
            _cpu = (Processor)vm.CPU;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            //vm.Load(0x05, "cpm_patch.bin"); // patch the CP/M BDOS routines out
            //vm.Load(0x100, "zexdoc.bin");
            List<byte> program = new List<byte>();
            for (int i = 0; i < 10000; i++) program.AddRange(new byte[] { 0xED, 0x7B, 0x00, 0x50 }); // repeat LD SP,(0x5000) 10,000 times (timing test)
            program.Add(0x76);
            vm.Load(0x0100, program.ToArray());

            Console.ForegroundColor = ConsoleColor.White;

            //_cpu.Debug.BeforeExecute += Before_Instruction_Execution;
            //_cpu.Debug.AfterExecute += After_Instruction_Execute;
            _cpu.RegisterDevice("test", TickHandler);
            while (true)
            {
                DateTime starts = DateTime.Now;
                vm.Start(0x0100, true, true);
                DateTime ends = DateTime.Now;
                TimeSpan elapsed = ends - starts;
                Console.WriteLine("Total elapsed: " + elapsed);
                Console.WriteLine("CPU cycles: " + _cpu.ClockCycles);
                Console.WriteLine("CPU ticks: " + _cpu.Ticks);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\r\nProgram has finished. Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static bool TickHandler(TickEvent tick, Processor cpu)
        {
            //Console.WriteLine("Got tick (" + (tick.Instruction?.Mnemonic ?? "(no instruction yet)") + ", " + tick.MachineCycleType.ToString() + ", " + tick.ClockCyclesAdded + " clock cycles");
            //Console.ReadLine();
            return true;
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
            ConsoleColor oldColour = Console.ForegroundColor;
            Console.ForegroundColor = _flowCange ? ConsoleColor.Red : ConsoleColor.Yellow;
            string mnemonic = e.Instruction.Mnemonic;
            if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0xx" + e.Data.ArgumentsAsWord.ToString("X4"));
            else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0xx" + e.Data.Argument1.ToString("X2"));
            if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0xx" + e.Data.Argument1.ToString("X2"));
            Console.WriteLine(new string('\t', _tabs) + _cpu.Registers.PC.ToString("X4") + ": " + mnemonic);
            Console.ForegroundColor = oldColour;
            _flowCange = false;
        }
    }
}

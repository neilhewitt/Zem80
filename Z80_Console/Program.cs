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
        private static IDebugProcessor _cpu;
        private static bool _flowCange;
        private static int _tabs;

        static void Main(string[] args)
        {
            VirtualMachine vm = new VirtualMachine();
            _cpu = (IDebugProcessor)vm.CPU;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            vm.Load(0x05, "cpm_patch.bin"); // patc te CP/M BDOS routines out
            vm.Load(0x100, "zexdoc.bin");

            Console.ForegroundColor = ConsoleColor.Green;

            //((IDebugProcessor)_cpu).BeforeExecute += Before_Instruction_Execution;
            //((IDebugProcessor)_cpu).AfterExecute += After_Instruction_Execute;
            vm.Start(0x0100, true);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\r\nProgram as finised. Press enter to exit.");
            Console.ReadLine();
        }

        private static void After_Instruction_Execute(object sender, ExecutionResult e)
        {
            if (e.InstructionSetsProgramCounter)
            {
                _flowCange = true;
                int previousTabs = _tabs;
                if (e.Instruction.Mnemonic.StartsWith("CALL")) _tabs++;
                if (e.Instruction.Mnemonic.StartsWith("RET")) _tabs--;
                if (_tabs < 0) _tabs = 0;
                if (_tabs == 0 && previousTabs == 1)
                {
                    //Console.ReadLine();
                }
                if (_cpu.Registers.PC == 0)
                {
                    Console.ReadLine();
                }
            }
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

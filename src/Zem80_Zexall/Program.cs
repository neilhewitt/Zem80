using System;
using System.Collections.Generic;
using System.Threading;
using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.SimpleVM;

namespace Zem80_Zexall
{
    class Program
    {
        private static List<ExecutionResult> _results;

        static void Main(string[] args)
        {
            _results = new List<ExecutionResult>();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Zem80 Zexall Instruction Set Test Runner\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;

            VirtualMachine vm = new VirtualMachine(enforceTiming: false, clock: ClockMaker.NoClock());
            vm.Load(0x0005, "..\\..\\..\\zexall\\cpm_patch.bin");
            vm.Load(0x0100, "..\\..\\..\\zexall\\zexdoc.bin");
            vm.Start(address: 0x100, synchronous: true, debugOutput: false);//, callbackAfterInstructionExecute: Callback);//, outputLogPath: "zexall.log");

            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nProgram finished. Press any key to close.");
            Console.ReadKey();
        }

        private static void Callback(ExecutionResult result)
        {
            _results.Add(result);
            if (_results.Count > 10)
            {
                _results.RemoveAt(0);
            }

            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = ConsoleColor.White;
            foreach (ExecutionResult thisResult in _results)
            {
                Console.WriteLine(thisResult.Instruction.Mnemonic.PadRight(20) + " at " + thisResult.InstructionAddress.ToString("X4"));
            }
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}

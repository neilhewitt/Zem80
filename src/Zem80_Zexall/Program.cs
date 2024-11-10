using System;
using System.Threading;
using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.SimpleVM;

namespace Zem80_Zexall
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("Zem80 Zexall Instruction Set Test Runner\n");
            //Console.ForegroundColor = ConsoleColor.Green;

            //VirtualMachine vm = new VirtualMachine(enforceTiming: false);
            //vm.Load(0x0005, "..\\..\\..\\zexall\\cpm_patch.bin");
            //vm.Load(0x0100, "..\\..\\..\\zexall\\zexall.bin");
            //vm.Start(address: 0x100, synchronous: true, debugOutput: false, callbackAfterInstructionExecute: Callback);//, outputLogPath: "zexall.log");

            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("\nProgram finished. Press any key to close.");
            Console.ReadKey();
        }

        private static void Callback(ExecutionResult result)
        {
            Console.ReadKey(false);
        }
    }
}

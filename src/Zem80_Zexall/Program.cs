using System;
using Zem80.SimpleVM;

namespace Zem80_Zexall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Zem80 Zexall Instruction Set Test Runner\n");
            Console.ForegroundColor = ConsoleColor.Green;

            VirtualMachine vm = new VirtualMachine(10);
            vm.Load(0x0005, "..\\..\\..\\zexall\\cpm_patch.bin");
            vm.Load(0x0100, "..\\..\\..\\zexall\\zexall.bin");
            vm.Start(address: 0x100, synchronous: true, outputLogPath: "zexall.log");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nProgram finished. Press any key to close.");
            Console.ReadKey();
        }
    }
}

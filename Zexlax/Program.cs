using System;
using System.IO;
using Z80.Core;
using Z80.SimpleVM;

namespace Zexlax
{
    class Program
    {
        static void Main(string[] args)
        {
            VirtualMachine vm = new VirtualMachine(speed: 8);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            vm.Load(0x05, "cpm_patch.bin"); // patch the CP/M BDOS routines out
            vm.Load(0x100, "zexall.bin");

            File.Delete("output.txt");
            vm.Start(address: 0x0100, timingMode: TimingMode.FastAndFurious, endOnHalt: true, synchronous: true, 
                debugOutput: false, outputLogPath: "output.txt");

            Console.WriteLine("Program ended. Press ENTER to quit.");
            Console.ReadLine();
        }
    }
}

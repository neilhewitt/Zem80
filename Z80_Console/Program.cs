using System;
using System.Threading;
using Z80.Core;

namespace Z80_Console
{
    class Program
    {
        private static double _lowestRatio = 1000;
        private static double _highestRatio = 0;

        static void Main(string[] args)
        {
            Processor cpu = Bootstrapper.BuildProcessor();
            uint address = 0;

            for (int i = 0; i < 5000; i++)
            {
                cpu.Memory.WriteBytesAt(address, 0x01, 0x03, 0x14, 0x8E, 0xDD, 0x8E, 0xFF, 0xDD, 0x8C, 0xDD, 0xDD, 0xDD, 0xCB, 0xDD, 0x46);
                //cpu.Memory.WriteBytesAt(address, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD, 0xDD);
                address += 12;
            }

            int cycles = 0;
            while (true)
            {
                cpu.Start();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Cycles: " + cycles++);
            }
        }        
    }
}

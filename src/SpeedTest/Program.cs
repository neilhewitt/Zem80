using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.Core.CPU;

//test how near the actual execution speed of the emulated CPU is to the theoretical speed

Console.WriteLine("Zem80 Speed Test\n");

int[] cpuWaitPattern = new int[] {
#if RELEASE
                3, 3, 3, 3, 3, 2
#else
                2, 2, 2, 2, 1, 1
#endif
            };

Processor cpu = new Processor(clock: ClockMaker.RealTimeClock(4, cpuWaitPattern));
Instruction lde = InstructionSet.Instructions[0x1E];
int ticks = ((lde.MachineCycles.TStates * 10000) + 4); // +4 is for the final HALT instruction

byte[] program = new byte[20001]; // 20000 x LD E,A
for (int i = 0; i < 20000; i++)
{
    program[i] = lde.LastOpcodeByte;
}
program[20000] = 0x76; // HALT

cpu.Memory.Untimed.WriteBytesAt(0, program);

bool quit = false;
while (!quit)
{
    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine("\nPress Q to stop, any key to start next run.");
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
        {
            quit = true;
            break;
        }

        Console.WriteLine("Run #" + i);
        long ticksIn = cpu.Clock.Ticks;
        cpu.Start(endOnHalt: true);
        cpu.RunUntilStopped();
        long ticksOut = cpu.Clock.Ticks;
        Console.WriteLine($"Was {ticksOut - ticksIn} ticks, should be { ticks} ticks.");
        // should be 40ms
        Console.WriteLine($"Elapsed was {cpu.LastRunTimeInMilliseconds}ms, should be 20ms");
    }
}

Console.WriteLine("\nFinished. Press any key to end.");
Console.ReadKey();


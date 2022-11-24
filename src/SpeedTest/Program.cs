using Zem80.Core;
using Zem80.Core.Instructions;

//test how near the actual execution speed of the emulated CPU is to the theoretical speed

Console.WriteLine("Zem80 Speed Test\n");

int[] cpuWaitPattern = new int[] {
#if RELEASE
            3, 3, 3, 2, 3, 3, 3, 2, 3, 3, 3, 2, 3, 3, 3, 3
#else
            3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 2, 2, 3, 3, 3, 2
#endif
            };

Processor cpu = new Processor(frequencyInMHz: 3.5f);//, waitPattern: cpuWaitPattern);
Instruction lde = InstructionSet.Instructions[0x1E];
int ticks = ((lde.Timing.TStates * 10000) + 4); // +4 is for the final HALT instruction

byte[] program = new byte[20001]; // 20000 x LD E,A
for (int i = 0; i < 20000; i++)
{
    program[i] = lde.Opcode;
}
program[20000] = 0x76; // HALT

cpu.Memory.Untimed.WriteBytesAt(0, program);

bool quit = false;
while (!quit)
{
    for (int i = 1; i <= 10; i++)
    {
        cpu.Initialise(endOnHalt: true, timingMode: TimingMode.PseudoRealTime);
        Console.WriteLine("\nPress Q to stop, any key to start next run.");
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
        {
            quit = true;
            break;
        }

        Console.WriteLine("Run #" + i);
        long tStatesIn = cpu.EmulatedTStates;
        cpu.Start();
        cpu.RunUntilStopped();
        long tStatesOut = cpu.EmulatedTStates;
        Console.WriteLine($"Was {tStatesOut - tStatesIn} ticks, should be { ticks} ticks.");
        // should be 40ms
        Console.WriteLine($"Elapsed was {cpu.Debug.LastRunTimeInMilliseconds}ms, should be 20ms");
    }
}

Console.WriteLine("\nFinished. Press any key to end.");
Console.ReadKey();


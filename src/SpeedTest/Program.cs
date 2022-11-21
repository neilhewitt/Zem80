using Zem80.Core;
using Zem80.Core.Instructions;

//test how near the actual execution speed of the emulated CPU is to the theoretical speed

Console.WriteLine("Zem80 Speed Test\n");

Processor cpu = new Processor(frequencyInMHz: 3.5f);
Instruction lde = InstructionSet.Instructions[0x1E];
int ticks = ((lde.Timing.TStates * 10000) + 4); // +4 is for the final HALT instruction

byte[] program = new byte[40001]; // 20000 x LD E,A
for (int i = 0; i < 40000; i++)
{
    program[i] = lde.Opcode;
}
program[40000] = 0x76; // HALT

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
        Console.WriteLine($"Elapsed was {cpu.LastRunTimeInMilliseconds}ms, should be 40ms");
    }
}

Console.WriteLine("\nFinished. Press any key to end.");
Console.ReadKey();


using Zem80.Core;
using Zem80.Core.Instructions;

Console.WriteLine("Zem80 Speed Test\n");

Processor cpu = new Processor(frequencyInMHz: 3.5f);
Instruction lde = InstructionSet.Instructions[0x1E];
int ticks = ((lde.Timing.TStates * 10000) + 2); // +2 is for the final HALT instruction

byte[] program = new byte[20001];
for (int i = 0; i < 20000; i++)
{
    program[i] = 0x1E;
}
program[20000] = 0x76; // JP back to 0000

cpu.Memory.Untimed.WriteBytesAt(0, program);

while (true)
{
    for (int i = 1; i <= 10; i++)
    {
        cpu.Suspend();
        cpu.Start(endOnHalt: true, timingMode: TimingMode.PseudoRealTime); // CPU doesn't run yet, as suspended - gives time to set up etc.
        Console.WriteLine("\nPress any key to start next run.");
        Console.ReadKey();
        Console.WriteLine("Run #" + i);
        long tStatesIn = cpu.EmulatedTStates;
        cpu.Resume();
        cpu.RunUntilStopped();
        long tStatesOut = cpu.EmulatedTStates;
        Console.WriteLine($"Was { tStatesOut - tStatesIn } ticks.");
        Console.WriteLine("Should be: " + ticks + " ticks.");
    }
}

Console.WriteLine("\nFinished. Press any key to end.");
Console.ReadKey();


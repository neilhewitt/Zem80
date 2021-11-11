using Zem80.Core;
using Zem80.Core.Instructions;

Console.WriteLine("Zem80 Speed Test\n");

Processor cpu = new Processor(frequencyInMHz: 3.5f);
Instruction lde = InstructionSet.Instructions[0x1E];
int ticks = (lde.Timing.TStates * 10000) + 1; // +1 is for the final HALT instruction

byte[] program = new byte[10001];
for (int i = 0; i < 10000; i++)
{
    program[i] = 0x1E;
}
program[10000] = 0xC3; // JP back to 0000

cpu.Memory.Untimed.WriteBytesAt(0, program);

cpu.Suspend();
cpu.Start(endOnHalt: true, timingMode: TimingMode.FastAndFurious); // CPU doesn't run yet, as suspended - gives time to set up etc.
while (true)
{
    for (int i = 1; i <= 10; i++)
    {
        //Console.WriteLine("\nPress any key to start next run.");
        //Console.ReadKey();
        Console.WriteLine("Run #" + i);
        DateTime now = DateTime.Now;
        Console.WriteLine("Time in: " + now.ToString());
        cpu.Resume();
        cpu.RunUntilStopped();
        //DateTime then = DateTime.Now;
        //Console.WriteLine("Time out: " + then.ToString());

        //TimeSpan runTime = then - now;
        //Console.WriteLine("Total time elapsed: " + runTime.Ticks + " ticks.");
        //Console.WriteLine("Should be: " + ticks + " ticks.");
    }
}

Console.WriteLine("\nFinished. Press any key to end.");
Console.ReadKey();


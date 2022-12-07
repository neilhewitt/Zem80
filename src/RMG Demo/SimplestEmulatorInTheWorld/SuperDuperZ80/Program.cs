namespace SuperDuperZ80
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating Z80...");
            
            Z80 z80 = new Z80();
            z80.OnAfterInstruction += Z80_OnAfterInstruction;

            // load the program
            z80.Poke(0, 0x3C); // INC A
            z80.Poke(1, 0xC3); // JP 0x0000

            Console.WriteLine($"Ready: \t A: {z80.A} \t PC: {z80.PC}");
            Console.ReadKey(false);

            z80.Run();
        }

        private static void Z80_OnAfterInstruction(object sender, string instruction)
        {
            Z80 z80 = (Z80)sender;
            Console.WriteLine($"Instruction: {instruction} \t A: {z80.A} \t PC: {z80.PC}");
            Console.ReadKey(false);
        }
    }
}
using System;
using System.Threading;
using Z80.Core;

namespace Z80_Console
{
    class Program
    {
        private static InstructionPackage _package;
        private static Processor _cpu;
        private static bool _first = true;

        static void Main(string[] args)
        {
            Processor cpu = Bootstrapper.BuildDefault();
            _cpu = cpu;
            ushort address = 0;

            cpu.Memory.WriteBytesAt(address, 0xDD, 0x21, 0x69, 0x00, 0xDD, 0x7E, 0x00, 0xE6, 0xFF, 0x20, 0x07, 0xDD, 0x21, 0x69, 0x00, 0xDD, 0x7E, 0x00, 0x16, 0x00, 0x5F, 0xCB, 0x13, 0xCB, 0x12, 0xCB, 
                0x13, 0xCB, 0x12, 0xCB, 0x13, 0xCB, 0x12, 0x2A, 0x36, 0x5C, 0x19, 0x11, 0x7F, 0x00, 0x01, 0x08, 0x00, 0xED, 0xB0, 0x0E, 0x08, 0xC5, 0x11, 0x7F, 0x00, 0x21, 0xFF, 0x50, 0x0E, 0x08, 0xEB, 0xCB, 
                0x26, 0xCB, 0x17, 0xEB, 0xCB, 0x2F, 0xCB, 0x16, 0xCB, 0x17, 0x06, 0x1F, 0x2B, 0xCB, 0x2F, 0xCB, 0x16, 0xCB, 0x17, 0x10, 0xF7, 0x13, 0xD5, 0x11, 0x1F, 0x01, 0x19, 0xD1, 0x0D, 0x20, 0xDF, 0x21, 
                0x00, 0x06, 0x2B, 0x7C, 0xB5, 0x20, 0xFB, 0xC1, 0x0D, 0x20, 0xCA, 0xDD, 0x23, 0x18, 0x9B, 0x20, 0x20, 0x20, 0x20, 0x20, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x20, 
                0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);

            cpu.BeforeExecute += OnBeforeExecute;
            cpu.AfterExecute += OnAfterExecute;

            cpu.Start();
        }

        private static void DisplayState(Processor cpu, InstructionPackage package)
        {
            Console.SetCursorPosition(0, 0);
            IFlags flags = cpu.Registers.Flags;
            string output = $"A: { Value(cpu.Registers.A, 10) } F: { Value(cpu.Registers.F, 10) } AF: { Value(cpu.Registers.AF, 10) }\r\n";
            output += $"B: { Value(cpu.Registers.B, 10) } C: { Value(cpu.Registers.C, 10) } BC: { Value(cpu.Registers.BC, 10) }\r\n";
            output += $"D: { Value(cpu.Registers.D, 10) } E: { Value(cpu.Registers.E, 10) } DE: { Value(cpu.Registers.DE, 10) }\r\n";
            output += $"H: { Value(cpu.Registers.H, 10) } L: { Value(cpu.Registers.L, 10) } HL: { Value(cpu.Registers.HL, 10) }\r\n";
            output += "\r\n";
            output += $"IXh: { Value(cpu.Registers.IXh, 10) } IXl: { Value(cpu.Registers.IXl, 10) } IX: { Value(cpu.Registers.IX, 10) }\r\n";
            output += $"IYh: { Value(cpu.Registers.IYh, 10) } IYl: { Value(cpu.Registers.IYl, 10) } IY: { Value(cpu.Registers.IY, 10) }\r\n";
            output += "\r\n";
            output += $"SP: { Value(cpu.Registers.SP, 10) }\r\n";
            output += $"PC: { Value(cpu.Registers.PC, 10) }\r\n";
            output += "\r\n";
            output += $"I: { Value(cpu.Registers.I, 10) } R: { Value(cpu.Registers.R, 10) }\r\n";
            output += "\r\n";
            output += "S|Z|5|H|3|P|N|C\r\n";
            output += Flag(flags.Sign) + Flag(flags.Zero) + Flag(flags.Five) + Flag(flags.HalfCarry) + Flag(flags.Three) + Flag(flags.ParityOverflow) + Flag(flags.Subtract) + Flag(flags.Carry);
            output += "\r\n\r\n\r\n";

            if (package != null)
            {
                output += package.Instruction.Mnemonic;
                if (package.Instruction.Argument1 != ArgumentType.None) output += " (Arg1 = 0x" + package.Data.Argument1.ToHexString() + ")";
                if (package.Instruction.Argument2 != ArgumentType.None) output += " (Arg2 = 0x" + package.Data.Argument2.ToHexString() + ")";
                if (package.Instruction.Argument1 != ArgumentType.None && package.Instruction.Argument2 != ArgumentType.None) output += " (ArgWord = 0x" + package.Data.ArgumentsAsWord.ToString("X4") + ")";
                Console.WriteLine(output);
            }
        }

        private static string Flag(bool flag)
        {
            return flag ? "1 " : "0 ";
        }

        private static string Value(byte input, int columnSize)
        {
            return "0x" + input.ToString("X2").PadRight(columnSize, ' ');
        }

        private static string Value(ushort input, int columnSize)
        {
            return "0x" + input.ToString("X4").PadRight(columnSize, ' ');
        }

        private static void OnAfterExecute(object sender, ExecutionResult e)
        {
            DisplayState(_cpu, _package);
            Thread.Sleep(500);
            //Console.WriteLine("\r\n\r\nPress any key to move to next instruction.");
            //Console.ReadKey();
            Console.Clear();
        }

        private static void OnBeforeExecute(object sender, InstructionPackage e)
        {
            if (_first)
            {
                DisplayState(_cpu, null);
                _first = false;
            }
            _package = e;
        }
    }
}

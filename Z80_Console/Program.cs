using System;
using System.Threading;
using Z80.Core;
using Z80.SimpleVM;

namespace Z80_Console
{
    class Program
    {
        private static InstructionPackage _package;
        private static IDebugProcessor _cpu;
        private static bool _first = true;

        static void Main(string[] args)
        {
            VirtualMachine vm = new VirtualMachine();
            _cpu = (IDebugProcessor)vm.CPU;

            //_cpu.BeforeExecute += OnBeforeExecute;
            //_cpu.AfterExecute += OnAfterExecute;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            while (true)
            {
                vm.Load(0, 0xDD, 0x21, 0x14, 0x00, 0xDD, 0x7E, 0x00, 0x0E, 0x00, 0xFE, 0x00, 0x28, 0x06, 0xD3, 0x00, 0xDD, 0x23, 0x18, 0xF1, 0x76, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x00);

                /* TEST PROGRAM
                
                    LD	    IX, Message
                    
                    GetChar:
                    LD	    A, (IX+0)		
                    CP      0
                    JR      Z, TheEnd
                    LD      C, 0
                    OUT     (0), A
                    INC     IX
                    JR      GetChar

                    TheEnd:
                    HALT    		

                    Message:
                    DB	"Hello World", 0
                    
                 */

                Console.ForegroundColor = ConsoleColor.Green;

                vm.Start();

                while (_cpu.State == ProcessorState.Running)
                {
                    //DisplayState(_cpu, null);
                    //var key = Console.ReadKey(true);
                    //if (key.KeyChar == 'h') { _cpu.Halt(); DisplayState(_cpu, null); }
                    //if (key.KeyChar == 'r') _cpu.Resume();
                    //if (key.KeyChar == 's') _cpu.Stop();
                }

                Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("\r\n\r\nResetting VM...\r\n");

                vm.Reset(true);
            }
        }

        private static void DisplayState(IDebugProcessor cpu, InstructionPackage package)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            IFlags flags = cpu.Registers.Flags;
            Console.WriteLine("State: " + cpu.State.ToString().ToUpper() + ", Ticks: " + cpu.InstructionTicks.ToString() + "                                           \r\n\r\n");
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
                if (package.Instruction.Argument1 == ArgumentType.None) output += "".PadRight(60, ' ');
            }

            Console.WriteLine(output);
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
            //DisplayState(_cpu, _package);
            //Thread.Sleep(50);
            //Console.WriteLine("\r\n\r\nPress any key to move to next instruction.");
            //Console.ReadKey();
        }

        private static void OnBeforeExecute(object sender, InstructionPackage e)
        {
            //DisplayState(_cpu, e);
            _package = e;
            //Console.WriteLine("\r\n\r\nPress any key to execute instruction.");
            //Console.ReadKey();
        }
    }
}

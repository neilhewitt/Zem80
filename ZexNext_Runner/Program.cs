using System;
using ZexNext.Core;
using Z80.Core;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Threading;

namespace ZexNext_Runner
{
    class Program
    {
        private static Processor _cpu;
        private static int _y;
        private static int BUFFER_LINES = 20;

        static void Main(string[] args)
        {
            bool showCycles = false;

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(200, 40);
            
            _cpu = Bootstrapper.BuildCPU(enableFlagPrecalculation: false);
            Console.WriteLine("ZexNext Console Test Runner (C)2020 Neil Hewitt\n\nCompiling tests...\n");
            TestRunner runner = new TestRunner("zexdoc.zxl");
            foreach (Test test in runner.TestSet.Tests)
            {
                if (showCycles)
                {
                    Console.SetCursorPosition(0, 2);
                    Console.WriteLine("Beginning test: " + test.Name + " (" + test.Cycles.Count() + " cycles)");
                    Console.CursorVisible = false;
                    string blank = new string(' ', 200);
                    for (int i = 0; i < 30; i++) Console.WriteLine(blank);
                    Thread.Sleep(3000);

                    _y = 0;
                    IEnumerable<TestResult> results = runner.Run(runner.TestSet.Tests[0], ExecuteTestCycle, AcceptResultForCycle);
                    Console.SetCursorPosition(0, 26);
                    Console.Write("TEST");
                    if (results.Any(x => x.Passed == false))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" FAILED ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("(" + results.Where(x => x.Passed == false).Count().ToString() + " cycles failed.)");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" PASSED");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine("\nStarting next test in 5 seconds...");
                    Thread.Sleep(10000);

                    Console.CursorVisible = true;
                }
                else
                {
                    Console.Write("Test: " + test.Name);
                    IEnumerable<TestResult> results = runner.Run(runner.TestSet.Tests[0], ExecuteTestCycle);
                    Console.CursorLeft = 42;
                    if (results.Where(x => x.Passed == false).Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("PASSED ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("FAILED ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            Console.WriteLine("Test execution finished. Press any key to exit.");
            Console.Read();
        }

        public static bool AcceptResultForCycle(TestResult result)
        {
            Console.SetCursorPosition(0, 4 + _y++);
            if (_y > BUFFER_LINES) _y = 0;

            if (result.Passed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("PASSED ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Expected: " + result.ExpectedState.ToString() + " | Actual: " + result.ActualState.ToString());
            if (!result.Passed) Console.ReadLine();

            return true;
        }

        public static TestState ExecuteTestCycle(TestState input)
        {
            _cpu.Registers.AF = input.AF;
            _cpu.Registers.BC = input.BC;
            _cpu.Registers.DE = input.DE;
            _cpu.Registers.HL = input.HL;
            _cpu.Registers.IX = input.IX;
            _cpu.Registers.IY = input.IY;
            _cpu.Registers.SP = input.SP;
            _cpu.Registers.PC = input.PC;

            ExecutionResult result = _cpu.Debug.Execute(input.Opcode);
            

            TestState afterExecution = new TestState(
                result.Instruction.FullOpcodeBytes,
                result.Instruction.Mnemonic,
                _cpu.Registers.AF,
                _cpu.Registers.BC,
                _cpu.Registers.DE,
                _cpu.Registers.HL,
                _cpu.Registers.IX,
                _cpu.Registers.IY,
                _cpu.Registers.SP,
                result.InstructionAddress // needs to be reset
                );

            return afterExecution;
        }
    }
}

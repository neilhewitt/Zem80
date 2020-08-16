using System;
using ZexNext.Core;
using Z80.Core;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Reflection.PortableExecutable;

namespace ZexNext_Runner
{
    class Program
    {
        private static Processor _cpu;
        private static int _y;
        private static int BUFFER_LINES = 20;
        private static bool _pauseOnFailure = false;
        private static int _cycles;

        static void Main(string[] args)
        {
            Console.WriteLine("ZexNext Console Test Runner (C)2020 Neil Hewitt (All Rights Reserved)\n");

            _cpu = Bootstrapper.BuildCPU(enableFlagPrecalculation: false);

            string path = args.FirstOrDefault();
            if (path == null) path = "zexall.zxl";
            string[] testFilePaths = GetTestFiles(path);

            Console.Write("Show full test cycle results (y/n)? ");
            var input = Console.ReadKey(false);
            bool showCycles = input.KeyChar == 'y' || input.KeyChar == 'Y';

            if (showCycles)
            {
                Console.Write("\nPause on failed cycle (y/n)? ");
                input = Console.ReadKey(false);
                _pauseOnFailure = input.KeyChar == 'y' || input.KeyChar == 'Y';
            }

            Console.Write("\nUse flag masks (tests only documented flags and instructions) (y/n)? ");
            input = Console.ReadKey(false);
            bool useFlagMasks = input.KeyChar == 'y' || input.KeyChar == 'Y';

            Console.WriteLine("\n\nCompiling tests...\n");
            TestRunner runner = new TestRunner(
                (address, data) => _cpu.Memory.WriteBytesAt(address, data, true), 
                testFilePaths);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine("ZexNext Console Test Runner (C)2020 Neil Hewitt (All Rights Reserved)\n");

            foreach (TestSet testSet in runner.TestSets)
            {
                Console.SetCursorPosition(0, 2);
                Console.WriteLine("Beginning test set: " + testSet.Name + "                                            \n");
                Thread.Sleep(3000);

                _cycles = 0;
                foreach (Test test in testSet.Tests)
                {
                    if (showCycles)
                    {
                        Console.SetWindowSize(200, 40);

                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine("Beginning test: " + test.Name + " (" + test.Cycles.Count() + " cycles)                                ");
                        Console.CursorVisible = false;
                        Thread.Sleep(2000);
                        string blank = new string(' ', 200);
                        for (int i = 0; i < 30; i++) Console.WriteLine(blank);

                        _y = 0;
                        IEnumerable<TestResult> results = runner.Run(test, ExecuteTestCycle, useFlagMasks, AcceptResultForCycle);
                        _cycles = 0;
                        Console.SetCursorPosition(0, 28);
                        Console.Write("TEST");
                        if (results.Any(x => x.Passed == false))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" FAILED          ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("(" + results.Where(x => x.Passed == false).Count().ToString() + " cycles failed.)");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" PASSED          ");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        for (int i = 3; i > 0; i--)
                        {
                            Console.SetCursorPosition(0, 30);
                            Console.WriteLine($"Starting next test in {i.ToString()} seconds...");
                            Thread.Sleep(1000);
                        }

                        Console.CursorVisible = true;
                    }
                    else
                    {
                        Console.Write("Test: " + test.Name);
                        IEnumerable<TestResult> results = runner.Run(test, ExecuteTestCycle, useFlagMasks);
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

                Console.WriteLine("\nTest execution finished. Press any key to exit.");
                Console.Read();
            }
        }

        public static bool AcceptResultForCycle(TestResult result)
        {
            _cycles++;
            Console.SetCursorPosition(0, 28);
            Console.WriteLine("Cycles: " + _cycles.ToString());

            Console.SetCursorPosition(0, 6 + _y++);
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

            if (!result.Passed && _pauseOnFailure)
            {
                Flags e = new Flags(result.ExpectedState.F);
                Flags a = new Flags(result.ActualState.F);
                Console.ReadKey();
            }

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
            _cpu.Registers.PC = 0x1D42;
            _cpu.Memory.WriteBytesAt(input.DataAddress, input.Data, true);

            ExecutionResult result = _cpu.Debug.Execute(input.Opcode);

            TestState afterExecution = new TestState(
            input.Opcode,
            input.Mnemonic,
            _cpu.Memory.ReadBytesAt(input.DataAddress, 16, true),
            _cpu.Registers.AF,
            _cpu.Registers.BC,
            _cpu.Registers.DE,
            _cpu.Registers.HL,
            _cpu.Registers.IX,
            _cpu.Registers.IY,
            _cpu.Registers.SP,
            _cpu.Registers.PC
            );

            return afterExecution;
        }

        private static string[] GetTestFiles(string path)
        {
            List<string> paths = new List<string>();
            if (path == null)
            {
                path = ".";
            }

            path = Path.GetFullPath(path); // expand from relative path if needed

            if (path.EndsWith(".zip") && File.Exists(path))
            {
                paths = UnzipArchive(path);
            }
            else if (Directory.Exists(path))
            {
                foreach (string filePath in Directory.GetFiles(path).Where(x => x.EndsWith(".zxl")))
                {
                    paths.Add(filePath);
                }
            }
            else if (File.Exists(path))
            {
                paths.Add(path);
            }

            return paths.ToArray();
        }

        private static List<string> UnzipArchive(string path)
        {
            List<string> filePaths = new List<string>();
            // unzip into the same folder and look for .zxl files           
            ZipArchive zip = ZipFile.OpenRead(path);
            string extractPath = Path.GetDirectoryName(path);
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name.EndsWith(".zxl"))
                {
                    string filePath = Path.Combine(extractPath, entry.Name);
                    entry.ExtractToFile(filePath, true);
                    filePaths.Add(filePath);
                }
            }
            return filePaths;
        }
    }
}

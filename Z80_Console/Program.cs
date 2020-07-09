using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using Z80.Core;
using Z80.SimpleVM;
using Z80.ZXSpectrumVM;

namespace Z80_Console
{
    class Program
    {
        private static Processor _cpu;
        private static bool _flowChange;
        private static int _tabs;
        private static bool _stepThrough = true;
        private static Registers _lastRegisters;
        private static ushort _targetPC = 0;

        static void Main(string[] args)
        {
            // NOTE TO ALL WHO ENTER HERE
            // This has become a general test program / playground. Yes, I know it's messy. Yes, I'm OK with that. Yes, it will get cleaned up later.

            //VirtualMachine vm = new VirtualMachine(speed: 3.5);
            Spectrum48K vm = new Spectrum48K();
            _cpu = (Processor)vm.CPU;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Starting VM...\r\n");

            //vm.Load(0x05, "cpm_patch.bin"); // patch the CP/M BDOS routines out
            //vm.Load(0x100, "zexdoc.bin");
            //vm.Load(0x00, "zx_spectrum_48k_ROM.bin");

            //byte[] program = new byte[10001];
            //for (int i = 0; i < 10000; i++) program[i] = 0x80;
            //program[10000] = 0x76;
            //vm.Load(0x100, program);
            //Console.ForegroundColor = ConsoleColor.White;

            //_cpu.Debug.BeforeExecute += Before_Instruction_Execution;
            _cpu.Debug.AfterExecute += After_Instruction_Execute;
            //_cpu.Debug.OnStop += Debug_OnStop;
            //while (true)
            //{
            //    foreach (var tm in new TimingMode[] { TimingMode.FastAndFurious, TimingMode.PseudoRealTime })
            //    {
            //vm.Start(address: 0x0000, timingMode: TimingMode.FastAndFurious, endOnHalt: true, synchronous: true);
            vm.Start();
            vm.CPU.RunUntilStopped();

            //int row = tm == TimingMode.FastAndFurious ? 0 : 6;
            Console.SetCursorPosition(0, 0);// row);
            TimeSpan elapsed = _cpu.Elapsed;
            TimeSpan expected = TimeSpan.FromTicks((long)((double)_cpu.EmulatedTStates * (double)((double)10 / (double)_cpu.FrequencyInMHz)));
            TimeSpan difference = elapsed.Subtract(expected);
            //Console.WriteLine(tm.ToString());
            Console.WriteLine("Elapsed: " + elapsed + "                         ");
            Console.WriteLine("Expected: " + expected + "                         ");
            Console.WriteLine("Difference: " + difference + "                         ");
            Console.WriteLine("Emulated clock ticks: " + _cpu.EmulatedTStates + "                         ");
            Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("\r\nProgram has finished. Press enter to exit.");
            //Console.ReadLine();
                //}
                //Thread.Sleep(100);
            //}
        }

        private static void After_Instruction_Execute(object sender, ExecutionResult e)
        {
            if (_targetPC == 0 || _targetPC == e.InstructionAddress)
            {
                string mnemonic = e.Instruction.Mnemonic;
                if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
                else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
                if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
                Console.Write(e.InstructionAddress.ToString("X4") + ": " + mnemonic.PadRight(20));
                regValue(ByteRegister.A); wregValue(WordRegister.BC); wregValue(WordRegister.DE); wregValue(WordRegister.HL); wregValue(WordRegister.SP); wregValue(WordRegister.PC);
                Console.Write(_cpu.Registers.Flags.State);
            }

            _lastRegisters = _cpu.Registers.Snapshot();

            if (!_stepThrough && _targetPC == 0)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == 's')
                    {
                        _stepThrough = !_stepThrough;
                    }
                    else if (key.KeyChar == 'w')
                    {
                        Console.Write("\nEnter address to run to (Hex): ");
                        string pc = Console.ReadLine();
                        if (ushort.TryParse(pc, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort address))
                        {
                            _targetPC = address;
                        }
                    }
                }
            }
            else if (_targetPC == e.InstructionAddress)
            {
                _stepThrough = true;
                _targetPC = 0;
            }

            if (_stepThrough)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == 's') _stepThrough = false;
                else if (key.KeyChar == 'w')
                {
                    Console.Write("\nEnter address to run to (Hex): ");
                    string pc = Console.ReadLine();
                    if (ushort.TryParse(pc, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort address))
                    {
                        _targetPC = address;
                        _stepThrough = false;
                    }
                }
                Console.WriteLine();
            }
            else if (_targetPC == 0)
            {
                Console.WriteLine();
            }    

            void regValue(ByteRegister r)
            {
                byte value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(r.ToString() + ": 0x" + value.ToString("X2"));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }

            void wregValue(WordRegister r)
            {
                ushort value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(r.ToString() + ": 0x" + value.ToString("X4"));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }
        }

        private static void Before_Instruction_Execution(object sender, ExecutionPackage e)
        {
            string mnemonic = e.Instruction.Mnemonic;
            if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
            else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
            if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
            Console.Write(e.InstructionAddress.ToString("X4") + ": " + mnemonic.PadRight(20));
            regValue(ByteRegister.A); wregValue(WordRegister.BC); wregValue(WordRegister.DE); wregValue(WordRegister.HL); wregValue(WordRegister.SP); wregValue(WordRegister.PC);
            if (e.Instruction.Condition != Condition.None)
            {
                Console.Write(e.Instruction.Condition.ToString() + ": " + (_cpu.Registers.Flags.SatisfyCondition(e.Instruction.Condition) ? "true" : "false"));
            }

            _lastRegisters = _cpu.Registers.Snapshot();

            Console.WriteLine();

            void regValue(ByteRegister r)
            {
                byte value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(r.ToString() + ": 0x" + value.ToString("X2"));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }

            void wregValue(WordRegister r)
            {
                ushort value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(r.ToString() + ": 0x" + value.ToString("X4"));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }
        }
    }
}

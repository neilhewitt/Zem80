using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Zem80.Core;
using Zem80.ZXSpectrumVM;

namespace Z80_Console
{
    class Program
    {
        private static Processor _cpu;
        private static bool _stepThrough = true;
        private static Registers _lastRegisters;
        private static ushort? _targetPC = null;
        private static Spectrum48K _vm;
        private static char? _lastKey;

        static void Main(string[] args)
        {
            _vm = new Spectrum48K();
            _cpu = _vm.CPU;
            _cpu.Debug.AfterExecute += After_Instruction_Execute;

            char quitKey;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Starting VM...\r\n");

                _vm.Start();
                while (_cpu.State != ProcessorState.Stopped)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        _lastKey = key.KeyChar;
                    }
                }
                _vm.Stop();

                Console.WriteLine("\nSTOPPED. R to restart, any other key to quit.");
                quitKey = Console.ReadKey(true).KeyChar;
            }
            while (quitKey == 'r');
        }

        private static char? CheckKey()
        {
            if (_lastKey.HasValue)
            {
                char key = _lastKey.Value;
                _lastKey = null;
                return key;
            }

            return null;
        }

        private static void RunTo()
        {
            Console.Write("\nEnter address to run to (Hex): ");
            string pc = Console.ReadLine();
            if (ushort.TryParse(pc, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort address))
            {
                _targetPC = address;
                _stepThrough = false;
            }
        }

        private static void ToggleStepThrough()
        {
            _stepThrough = !_stepThrough;
        }

        private static void Quit()
        {
            _vm.Stop();
        }

        private static void After_Instruction_Execute(object sender, ExecutionResult e)
        {
            if (_targetPC == null || _targetPC == e.InstructionAddress)
            {
                string mnemonic = e.Instruction.Mnemonic;
                if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
                else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
                if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
                Console.Write(e.InstructionAddress.ToString("X4") + ": " + mnemonic.PadRight(20));
                regValue(ByteRegister.A); wregValue(WordRegister.BC); wregValue(WordRegister.DE); wregValue(WordRegister.HL); wregValue(WordRegister.SP); wregValue(WordRegister.PC);
                Console.WriteLine(_cpu.Registers.Flags.State);
            }

            _lastRegisters = _cpu.Registers.Snapshot();

            if (_targetPC == e.InstructionAddress)
            {
                _stepThrough = true;
                _targetPC = null;
            }

            char? key;
            do
            {
                key = CheckKey();
                if (key.HasValue)
                {
                    switch (key)
                    {
                        case 's': ToggleStepThrough(); break;
                        case 'w': RunTo(); break;
                        case 'q': Quit(); break;
                        default:
                            _vm.KeyDown(key.ToString());
                            _vm.KeyUp(key.ToString());
                            break;
                    }
                }
            }
            while (key == null && _stepThrough);

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

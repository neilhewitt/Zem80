using System;
using System.Globalization;
using Z80.Core;
using Z80.ZXSpectrumVM;

namespace ZXSpectrumConsole
{
    class Program
    {
        private static Processor _cpu;
        private static ushort _address;
        private static bool _endOnHalt;
        private static TimingMode _timingMode;
        private static bool _synchronous;
        private static bool _flowChange;
        private static int _tabs;
        private static bool _stepThrough = true;
        private static Registers _lastRegisters;
        private static ushort _targetPC = 0;
        private static string _outputLogPath;

        static void Main(string[] args)
        {
            Spectrum48K spectrum = new Spectrum48K();
            spectrum.CPU.Debug.AfterExecute += Debug_AfterExecute;
            _cpu = spectrum.CPU;
            spectrum.Start();
        }

        private static void Debug_AfterExecute(object sender, ExecutionResult e)
        {
            if (_targetPC == 0 || _targetPC == e.InstructionAddress)
            {
                string mnemonic = e.Instruction.Mnemonic;
                if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
                else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
                if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
                Write(e.InstructionAddress.ToString("X4") + ": " + mnemonic.PadRight(20));
                regValue(ByteRegister.A); wregValue(WordRegister.BC); wregValue(WordRegister.DE); wregValue(WordRegister.HL); wregValue(WordRegister.SP); wregValue(WordRegister.PC);
                Write(_cpu.Registers.Flags.State.ToString());
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
                        Write("\nEnter address to run to (Hex): ");
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
                    Write("\nEnter address to run to (Hex): ");
                    string pc = Console.ReadLine();
                    if (ushort.TryParse(pc, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort address))
                    {
                        _targetPC = address;
                        _stepThrough = false;
                    }
                }
                WriteLine();
            }
            else if (_targetPC == 0)
            {
                WriteLine();
            }

            void regValue(ByteRegister r)
            {
                byte value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Write(r.ToString() + ": 0x" + value.ToString("X2"));
                Console.ForegroundColor = ConsoleColor.White;
                Write(" | ");
            }

            void wregValue(WordRegister r)
            {
                ushort value = _cpu.Registers[r];
                if (_lastRegisters != null && value != _lastRegisters[r]) Console.ForegroundColor = ConsoleColor.Green;
                Write(r.ToString() + ": 0x" + value.ToString("X4"));
                Console.ForegroundColor = ConsoleColor.White;
                Write(" | ");
            }
        }

        private static void Write(string output)
        {
            Console.Write(output);
        }

        private static void WriteLine(string output = null)
        {
            if (output == null) Console.WriteLine();
            else Console.WriteLine(output);
        }
    }
}

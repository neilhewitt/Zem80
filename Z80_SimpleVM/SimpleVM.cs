using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Z80.Core;

namespace Z80.SimpleVM
{
    public class VirtualMachine
    {
        private Processor _cpu;
        private ushort _address;
        private bool _endOnHalt;
        private TimingMode _timingMode;
        private bool _synchronous;
        private static bool _flowChange;
        private static int _tabs;
        private static bool _stepThrough = true;
        private static Registers _lastRegisters;
        private static ushort _targetPC = 0;
        private static string _outputLogPath;

        public Processor CPU => _cpu;

        public void Start(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious, bool synchronous = false, bool debugOutput = false, string outputLogPath = null)
        {
            _address = address;
            _endOnHalt = endOnHalt;
            _timingMode = timingMode;
            _synchronous = synchronous;
            _outputLogPath = outputLogPath;

            _cpu.Start(address, endOnHalt, timingMode);
            if (debugOutput)
            {
                _cpu.Debug.AfterExecute += Debug_AfterExecute;
            }

            if (synchronous) _cpu.RunUntilStopped();
        }

        private void Debug_AfterExecute(object sender, ExecutionResult e)
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

        private void Write(string output)
        {
            Console.Write(output);
            if (_outputLogPath != null)
            {
                File.AppendAllText(_outputLogPath, output);
            }
        }

        private void WriteLine(string output = null)
        {
            if (output == null) Console.WriteLine();
            else Console.WriteLine(output);
            if (_outputLogPath != null)
            {
                File.AppendAllText(_outputLogPath, output + "\n");
            }
        }

        public void Stop()
        {
            _cpu.Stop();
        }

        public void Reset()
        {
            _cpu.Stop();
            _cpu.ResetAndClearMemory();
            _cpu.Start(_address, _endOnHalt, _timingMode);
            if (_synchronous) _cpu.RunUntilStopped();
        }

        public void Load(ushort address, string path)
        {
            _cpu.Memory.WriteBytesAt(address, File.ReadAllBytes(path), true);
        }

        public void Load(ushort address, params byte[] code)
        {
            _cpu.Memory.WriteBytesAt(address, code, true);
        }

        public void Poke(ushort address, byte value)
        {
            _cpu.Memory.WriteByteAt(address, value, true);
        }

        public byte Peek(ushort address)
        {
            return _cpu.Memory.ReadByteAt(address, true);
        }

        private byte ReadChar()
        {
            return 0;
        }

        private void WriteChar(byte input)
        {
            char c = Convert.ToChar(input);
            Console.Write(c);
            if (_outputLogPath != null)
            {
                File.AppendAllText(_outputLogPath, c.ToString());
            }
        }

        private byte ReadByte()
        {
            return 0;
        }

        private void WriteByte(byte input)
        {
            string s = input.ToString("X2");
            Console.Write(s);
            if (_outputLogPath != null)
            {
                File.AppendAllText(_outputLogPath, s);
            }
        }

        private void SignalWrite()
        {
        }

        private void SignalRead()
        {
        }

        public VirtualMachine(double speed = 4)
        {
            _cpu = Bootstrapper.BuildCPU(speedInMHz: speed, enableFlagPrecalculation: false);
            _cpu.Ports[0].Connect(ReadChar, WriteChar, SignalRead, SignalWrite);
            _cpu.Ports[1].Connect(ReadByte, WriteByte, SignalRead, SignalWrite);
        }
    }
}

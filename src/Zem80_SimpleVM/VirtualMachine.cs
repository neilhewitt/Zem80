using System;
using System.Globalization;
using System.IO;
using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.Core.CPU;

namespace Zem80.SimpleVM
{
    public class VirtualMachine
    {
        private Processor _cpu;
        private ushort _address;
        private bool _endOnHalt;
        private bool _synchronous;
        private static string _outputLogPath;
        private static Registers _lastRegisters;
        private Action<ExecutionResult> _callback;

        public Processor CPU => _cpu;

        public void Start(ushort address = 0x0000, bool endOnHalt = false, bool synchronous = false, bool debugOutput = false, string outputLogPath = null, Action<ExecutionResult> callbackAfterInstructionExecute = null)
        {
            _address = address;
            _endOnHalt = endOnHalt;
            _synchronous = synchronous;
            _outputLogPath = outputLogPath;
            _callback = callbackAfterInstructionExecute;

            if (debugOutput)
            {
                _cpu.AfterExecuteInstruction += DebugOutput_AfterExecute;
            }

            _cpu.Start(address, endOnHalt);
            if (synchronous) _cpu.RunUntilStopped();
        }

        public void Resume()
        {
            _cpu.Resume();
        }

        public void Stop()
        {
            _cpu.Stop();
        }

        public void Reset()
        {
            _cpu.Stop();
            _cpu.ResetAndClearMemory();
            _cpu.Start(_address, _endOnHalt);
            if (_synchronous) _cpu.RunUntilStopped();
        }

        public void Load(ushort address, string path)
        {
            _cpu.Memory.Untimed.WriteBytesAt(address, File.ReadAllBytes(path));
        }

        public void Load(ushort address, params byte[] code)
        {
            _cpu.Memory.Untimed.WriteBytesAt(address, code);
        }

        public void Poke(ushort address, byte value)
        {
            _cpu.Memory.Untimed.WriteByteAt(address, value);
        }

        public byte Peek(ushort address)
        {
            return _cpu.Memory.Untimed.ReadByteAt(address);
        }

        private void DebugOutput_AfterExecute(object sender, ExecutionResult e)
        {
            string mnemonic = e.Instruction.Mnemonic;
            if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
            else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
            if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
            Write(e.InstructionAddress.ToString("X4") + ": " + mnemonic.PadRight(20));
            regValue(ByteRegister.A); wregValue(WordRegister.BC); wregValue(WordRegister.DE); wregValue(WordRegister.HL); wregValue(WordRegister.SP); wregValue(WordRegister.PC);
            Write(_cpu.Flags.State.ToString());
            Write("\n");

            _lastRegisters = _cpu.Registers.Snapshot();

            _callback?.Invoke(e);

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


        private byte ReadChar()
        {
            return (byte)(Console.KeyAvailable ? Console.ReadKey(true).KeyChar : 0);
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

        public VirtualMachine(bool enforceTiming = true, IClock clock = null)
        {
            clock ??= enforceTiming ? ClockMaker.RealTimeClock(4) : null;
            _cpu = new Processor(clock: clock);
            _cpu.Ports[0].Connect(ReadChar, WriteChar, SignalRead, SignalWrite);
            _cpu.Ports[1].Connect(ReadByte, WriteByte, SignalRead, SignalWrite);
        }
    }
}

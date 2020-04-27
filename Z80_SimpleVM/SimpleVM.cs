using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Z80.Core;

namespace Z80.SimpleVM
{
    public class VirtualMachine
    {
        private Action<char> _output;
        private Processor _cpu;

        public Processor CPU => _cpu;

        public void Start(ushort address = 0x0000, bool runSyncronous = false, bool endOnHalt = false, Action<char> outputChar = null)
        {
            if (outputChar != null) _output = outputChar;
            
            _cpu.Start(runSyncronous, address, endOnHalt);
        }

        public void Stop()
        {
            _cpu.Stop();
        }

        public void Reset()
        {
            _cpu.Stop();
            _cpu.ResetAndClearMemory();
            _cpu.Start();
        }

        public void Load(ushort address, string path)
        {
            _cpu.Memory.WriteBytesAt(address, File.ReadAllBytes(path));
        }

        public void Load(ushort address, params byte[] code)
        {
            _cpu.Memory.WriteBytesAt(address, code, true);
        }

        private byte ReadChar()
        {
            return 0;
        }

        private void WriteChar(byte input)
        {
            char c = Convert.ToChar(input);
            if (_output != null) _output(Convert.ToChar(c));
            else Console.Write(c);
        }

        private byte ReadByte()
        {
            return 0;
        }

        private void WriteByte(byte input)
        {
            string s = input.ToString("X2");
            if (_output != null) { _output(s[0]); _output(s[1]); }
            else Console.Write(s);
        }

        private void SignalWrite()
        {
        }

        private void SignalRead()
        {
        }

        public VirtualMachine()
        {
            _cpu = Bootstrapper.BuildCPU();
            _cpu.Ports[0].Connect(ReadChar, WriteChar, SignalRead, SignalWrite);
            _cpu.Ports[1].Connect(ReadByte, WriteByte, SignalRead, SignalWrite);
        }
    }
}

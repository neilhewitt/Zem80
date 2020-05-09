using System;
using System.Collections.Generic;
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

        public Processor CPU => _cpu;

        public void Start(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious, bool synchronous = false)
        {
            _address = address;
            _endOnHalt = endOnHalt;
            _timingMode = timingMode;
            _synchronous = synchronous;

            _cpu.Start(address, endOnHalt, timingMode);
            if (synchronous) _cpu.WaitUntilStopped();
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
            if (_synchronous) _cpu.WaitUntilStopped();
        }

        public void Load(ushort address, string path)
        {
            _cpu.Memory.WriteBytesAt(address, File.ReadAllBytes(path), true);
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
            Console.Write(c);
        }

        private byte ReadByte()
        {
            return 0;
        }

        private void WriteByte(byte input)
        {
            string s = input.ToString("X2");
            Console.Write(s);
        }

        private void SignalWrite()
        {
        }

        private void SignalRead()
        {
        }

        public VirtualMachine(int speed = 4)
        {
            _cpu = Bootstrapper.BuildCPU(speedInMHz: speed);
            _cpu.Ports[0].Connect(ReadChar, WriteChar, SignalRead, SignalWrite);
            _cpu.Ports[1].Connect(ReadByte, WriteByte, SignalRead, SignalWrite);
        }
    }
}

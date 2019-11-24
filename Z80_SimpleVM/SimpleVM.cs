using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.SimpleVM
{
    public class VirtualMachine
    {
        private Processor _cpu;

        public Processor CPU => _cpu;

        public void Start()
        {
            _cpu.Start();
        }

        public void Reset(bool stopAfterReset)
        {
            _cpu.Reset(stopAfterReset);
        }

        public void Load(ushort address, params byte[] code)
        {
            _cpu.Memory.WriteBytesAt(address, code);
        }

        private byte ReadByte()
        {
            return 0;
        }

        private void WriteByte(byte input)
        {
            Console.Write(Convert.ToChar(input));
        }

        private void Signal(PortSignal signal)
        {
        }

        public VirtualMachine()
        {
            _cpu = Bootstrapper.BuildDefaultCPU();
            _cpu.Ports[0].Connect(ReadByte, WriteByte, Signal);
        }
    }
}

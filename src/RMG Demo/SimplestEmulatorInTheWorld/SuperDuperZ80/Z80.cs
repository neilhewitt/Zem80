using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperDuperZ80
{
    public class Z80
    {
        private byte[] _memory = new byte[65536];
        
        public byte A { get; set; }
        public ushort PC { get; set; }

        public event EventHandler<string> OnAfterInstruction;

        public void Poke(ushort address, byte value) => _memory[address] = value;

        public void Run()
        {
            PC = 0;

            while (PC < _memory.Length)
            {
                byte opcode = _memory[PC++];
                string instruction = "";

                switch (opcode)
                {
                    case 0x00: // NOP
                        instruction = "NOP";
                        break;

                    case 0x3C: // INC A
                        instruction = "INC A";
                        A = (byte)(A + 1);
                        break;

                    case 0xC3: // JP nn
                        ushort address = (ushort)((_memory[PC++] * 256) + _memory[PC]);
                        instruction = $"JP {address.ToString("X6")}";
                        PC = address;
                        break;
                }

                OnAfterInstruction?.Invoke(this, instruction);
            }
        }

        public Z80()
        {
        }
    }
}

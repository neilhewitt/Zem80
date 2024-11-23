using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;
using Zem80.Core.CPU;

namespace ZXSpectrum.VM
{
    public class Loader
    {
        private Processor _cpu;
        private ULA _ula;

        public void LoadSnapshot(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Snapshot file {path} does not exist.");
            }

            try
            {
                switch (Path.GetExtension(path).ToUpper())
                {
                    case ".SNA":
                        LoadSNA(path);
                        break;

                    case ".Z80":
                        LoadZ80(path);
                        break;

                    default:
                        throw new Exception("Unsupported snapshot format. Only SNA and Z80 formats are supported.");
                }
            }
            catch (Exception ex) // could be lots of things
            {
                throw new Exception($"Error loading snapshot: {ex.Message}", ex);
            }
        }

        private void LoadSNA(string path)
        {
            byte[] snapshot = File.ReadAllBytes(path);
            IRegisters r = _cpu.Registers;

            r.I = snapshot[0];

            r.ExchangeBCDEHL();
            r.HL = getWord(1);
            r.DE = getWord(3);
            r.BC = getWord(5);
            r.AF = getWord(7);

            r.ExchangeBCDEHL();
            r.HL = getWord(9);
            r.DE = getWord(11);
            r.BC = getWord(13);
            r.IY = getWord(15);
            r.IX = getWord(17);

            _cpu.Interrupts.Disable();
            if (snapshot[19].GetBit(2))
            {
                _cpu.Interrupts.Enable();
            }

            r.R = snapshot[20];
            r[WordRegister.AF] = getWord(21);
            r.SP = getWord(23);

            _cpu.Interrupts.SetMode((InterruptMode)snapshot[25]);
            _ula.SetBorderColour(snapshot[26]);

            _cpu.Memory.WriteBytesAt(16384, snapshot[27..]);
            ushort pc = _cpu.Stack.Debug.PopStackDirect(); // pops the top value from the stack and moves the stack pointer, but doesn't run any internal cycles
            _cpu.Registers.PC = pc;

            ushort getWord(int index)
            {
                return (snapshot[index], snapshot[index + 1]).ToWord();
            }
        }

        private void LoadZ80(string path)
        {
            // BUG: sometimes overfills memory - look at decompression routine

            byte[] snapshot = File.ReadAllBytes(path);
            IRegisters r = _cpu.Registers;

            r.AF = getWord(0);
            r.BC = getWord(2);
            r.HL = getWord(4);
            r.PC = getWord(6);
            r.SP = getWord(8);
            r.I = snapshot[10];
            r.R = snapshot[11];

            byte twelve = snapshot[12];
            if (twelve == 255) twelve = 1;
            byte borderColour = twelve.GetByteFromBits(1, 3);
            _ula.SetBorderColour(borderColour);

            bool compressed = twelve.GetBit(5);

            r.DE = getWord(13);

            r.ExchangeBCDEHL();
            r.ExchangeAF();
            r.BC = getWord(15);
            r.DE = getWord(17);
            r.HL = getWord(19);
            r.AF = getWord(21);
            r.ExchangeAF();
            r.ExchangeBCDEHL();

            r.IY = getWord(23);
            r.IX = getWord(25);

            _cpu.Interrupts.Disable();
            if (snapshot[27] == 1)
            {
                _cpu.Interrupts.Enable();
            }

            InterruptMode interruptMode = (InterruptMode)snapshot[29].GetByteFromBits(0, 2);
            if (interruptMode == InterruptMode.IM0) interruptMode = InterruptMode.IM1; // IM0 not used on Spectrum
            _cpu.Interrupts.SetMode(interruptMode);

            byte[] memoryImage;
            if (compressed)
            {
                List<byte> expanded = new List<byte>();
                for (int i = 30; i < snapshot.Length - 4; i++)
                {
                    if (snapshot[i] == 0xED && snapshot[i + 1] == 0xED)
                    {
                        byte repeats = snapshot[i + 2];
                        byte value = snapshot[i + 3];

                        byte[] sequence = (byte[])Array.CreateInstance(typeof(byte), repeats);
                        if (value > 0)
                        {
                            for (int j = 0; j < sequence.Length; j++)
                            {
                                sequence[j] = value;
                            }
                        }

                        expanded.AddRange(sequence);
                        i += 3; // jump ahead
                    }
                    else
                    {
                        expanded.Add(snapshot[i]);
                    }
                }

                memoryImage = expanded.ToArray();
            }
            else
            {
                memoryImage = snapshot[30..^4];
            }

            _cpu.Memory.WriteBytesAt(16384, memoryImage);

            ushort getWord(int index)
            {
                return (snapshot[index], snapshot[index + 1]).ToWord();
            }
        }


        public Loader(Processor cpu, ULA ula)
        {
            _cpu = cpu;
            _ula = ula;
        }
    }
}

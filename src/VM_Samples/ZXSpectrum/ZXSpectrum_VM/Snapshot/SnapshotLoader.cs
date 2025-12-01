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
    public class SnapshotLoader
    {
        private Processor _cpu;
        private ULA _ula;

        public void Load(string snapshotPath)
        {
            if (!File.Exists(snapshotPath))
            {
                throw new FileNotFoundException($"Snapshot file {snapshotPath} does not exist.");
            }

            try
            {
                switch (Path.GetExtension(snapshotPath).ToUpper())
                {
                    case ".SNA":
                        LoadSNA(snapshotPath);
                        break;

                    case ".Z80":
                        LoadZ80(snapshotPath);
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

            // shadow registers first
            r.ExchangeBCDEHL();
            r.HL = GetWord(snapshot, 1);
            r.DE = GetWord(snapshot, 3);
            r.BC = GetWord(snapshot, 5);
            r.AF = GetWord(snapshot, 7);

            // main registers
            r.ExchangeBCDEHL();
            r.HL = GetWord(snapshot, 9);
            r.DE = GetWord(snapshot, 11);
            r.BC = GetWord(snapshot, 13);
            r.IY = GetWord(snapshot, 15);
            r.IX = GetWord(snapshot, 17);

            _cpu.Interrupts.Disable();
            if (snapshot[19].GetBit(2))
            {
                _cpu.Interrupts.Enable();
            }

            r.R = snapshot[20];
            r[WordRegister.AF] = GetWord(snapshot, 21);
            r.SP = GetWord(snapshot, 23);

            _cpu.Interrupts.SetMode((InterruptMode)snapshot[25]);
            _ula.SetBorderColour(snapshot[26]);

            _cpu.Memory.WriteBytesAt(16384, snapshot[27..]);
            ushort pc = _cpu.Stack.Debug.PopStackDirect(); // pops the top value from the stack and moves the stack pointer, but doesn't run any internal cycles
            _cpu.Registers.PC = pc;

            //ushort GetWord(snapshot, int index)
            //{
            //    return (snapshot[index], snapshot[index + 1]).ToWord();
            //}
        }

        private void LoadZ80(string path)
        {
            // BUG: sometimes overfills memory - look at decompression routine

            byte[] snapshot = File.ReadAllBytes(path);
            IRegisters r = _cpu.Registers;

            // set main registers (AF, BC, HL, PC, SP, I, R, DE)
            r.AF = GetWord(snapshot, 0);
            r.BC = GetWord(snapshot, 2);
            r.HL = GetWord(snapshot, 4);
            r.PC = GetWord(snapshot, 6);
            r.SP = GetWord(snapshot, 8);
            r.I = snapshot[10];
            r.R = snapshot[11];
            r.DE = GetWord(snapshot, 13);

            // set shadow registers (AF', BC', DE', HL')
            r.ExchangeBCDEHL();
            r.ExchangeAF();
            r.BC = GetWord(snapshot, 15);
            r.DE = GetWord(snapshot, 17);
            r.HL = GetWord(snapshot, 19);
            r.AF = GetWord(snapshot, 21);
            r.ExchangeAF();
            r.ExchangeBCDEHL();

            // index registers (IX, IY)
            r.IY = GetWord(snapshot, 23);
            r.IX = GetWord(snapshot, 25);

            byte statusByte = snapshot[12];
            if (statusByte == 255) statusByte = 1;

            _cpu.Interrupts.Disable();
            if (snapshot[27] == 1)
            {
                _cpu.Interrupts.Enable();
            }

            InterruptMode interruptMode = (InterruptMode)snapshot[29].GetByteFromBits(0, 2);
            if (interruptMode == InterruptMode.IM0) interruptMode = InterruptMode.IM1; // IM0 not used on Spectrum
            _cpu.Interrupts.SetMode(interruptMode);

            // set border colour before we start the machine
            byte borderColour = statusByte.GetByteFromBits(1, 3);
            _ula.SetBorderColour(borderColour);

            byte[] memoryImage;
            if (statusByte.GetBit(5)) // compressed data
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
        }

        private ushort GetWord(byte[] snapshot, int index)
        {
            return (snapshot[index], snapshot[index + 1]).ToWord();
        }

        public SnapshotLoader(Processor cpu, ULA ula)
        {
            _cpu = cpu;
            _ula = ula;
        }
    }
}

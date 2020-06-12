using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Z80.Core;

namespace Z80.ZXSpectrumVM
{
    public class Spectrum48K
    {
        private Processor _cpu;
        private int _ticksSinceLastDisplayUpdate;
        private int _displayUpdatesSinceLastFlash;
        private ScreenMap _screen;

        private IDictionary<int, int> _displayWaits = new Dictionary<int, int>();

        public event EventHandler<byte[]> OnUpdateDisplay;
        public event EventHandler<(string, string)> OnAfterExecuteInstruction;

        public Processor CPU => _cpu;

        public void Start()
        {
            _cpu.Start(timingMode: TimingMode.FastAndFurious);
        }

        private void _cpu_OnClockTick(object sender, ExecutionPackage e)
        {
            if (_displayWaits.ContainsKey(_ticksSinceLastDisplayUpdate))
            {
                _cpu.AddWaitCycles(_displayWaits[_ticksSinceLastDisplayUpdate]);
            }

            _ticksSinceLastDisplayUpdate++;
            
            if (_ticksSinceLastDisplayUpdate > 69887)
            {
                UpdateDisplay();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void UpdateDisplay()
        {
            byte[] pixelBuffer = _cpu.Memory.ReadBytesAt(0x4000, 6144, true);
            byte[] attributeBuffer = _cpu.Memory.ReadBytesAt(0x5800, 768, true);

            int columnCounter = 0, rowCounter = 0;
            foreach (byte column in pixelBuffer)
            {
                for (int i = 0; i < 8; i++)
                {
                    _screen.PixelMap[rowCounter, columnCounter] = column.GetBit(i);
                }

                columnCounter++;
                if (columnCounter == 32)
                {
                    columnCounter = 0;
                    rowCounter++;
                }
            }

            columnCounter = 0;
            rowCounter = 0;
            foreach (byte attribute in attributeBuffer)
            {
                // 32 x 24
                _screen.AttributeMap[rowCounter, columnCounter] = new DisplayAttribute()
                {
                    Ink = DisplayColour.FromThreeBit(attribute.GetByteFromBits(0, 3)),
                    Paper = DisplayColour.FromThreeBit(attribute.GetByteFromBits(3, 3)),
                    Bright = attribute.GetBit(6),
                    Flash = attribute.GetBit(7)
                };

                columnCounter++;
                if (columnCounter == 32)
                {
                    columnCounter = 0;
                    rowCounter++;
                }
            }

            OnUpdateDisplay?.Invoke(this, _screen.AsRGBA(_displayUpdatesSinceLastFlash++ >= 50));
            if (_displayUpdatesSinceLastFlash > 50) _displayUpdatesSinceLastFlash = 0;
        }

        private void _cpu_AfterInstruction(object sender, ExecutionResult e)
        {
            string mnemonic = e.Instruction.Mnemonic;
            if (mnemonic.Contains("nn")) mnemonic = mnemonic.Replace("nn", "0x" + e.Data.ArgumentsAsWord.ToString("X4"));
            else if (mnemonic.Contains("n")) mnemonic = mnemonic.Replace("n", "0x" + e.Data.Argument1.ToString("X2"));
            if (mnemonic.Contains("o")) mnemonic = mnemonic.Replace("o", "0x" + e.Data.Argument1.ToString("X2"));
            string output = e.InstructionAddress.ToString("X4") + ": " + mnemonic;

            string values = regValue(ByteRegister.A) + wregValue(WordRegister.BC) + wregValue(WordRegister.DE) + wregValue(WordRegister.HL) + wregValue(WordRegister.SP) + wregValue(WordRegister.PC);
            values = values.TrimEnd(' ', '\n');

            OnAfterExecuteInstruction?.Invoke(this, (output, values));

            string regValue(ByteRegister r)
            {
                byte value = _cpu.Registers[r];
                return r.ToString() + ": 0x" + value.ToString("X2") + "\n";
            }

            string wregValue(WordRegister r)
            {
                ushort value = _cpu.Registers[r];
                return r.ToString() + ": 0x" + value.ToString("X4") + "\n";
            }
        }

        private byte ReadULA()
        {
            Console.WriteLine("\n\nRead from ULA\n\n");
            return 0;
        }

        private void WriteULA(byte output)
        {
            ColourValue newBorder = DisplayColour.FromThreeBit(output.GetByteFromBits(0, 3));
            _screen.SetBorderColour(newBorder);

            Console.WriteLine("\n\nWrite " + output.ToString("X2") + " to ULA");
            Console.WriteLine("Border: " + newBorder.Name + "\n\n");
        }

        private void SignalULARead()
        {

        }

        private void SignalULAWrite()
        {

        }

        private void FillDisplayWaits()
        {
            int wait = 6;
            for (int i = 14335; i < 14341; i++) _displayWaits.Add(i, wait--);
            wait = 6;
            for (int i = 14343; i < 14349; i++) _displayWaits.Add(i, wait--);
        }

        public Spectrum48K()
        {
            _screen = new ScreenMap(192, 256, 8, 8);
            FillDisplayWaits();

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(0, File.ReadAllBytes("rom\\48.bin")));
            map.Map(new MemorySegment(16384, 49152));

            _cpu = Bootstrapper.BuildCPU(map: map, speedInMHz: 3.5);

            // connect all even-numbered ports to the ULA handlers
            for (byte i = 0; i < 255; i++)
            {
                if (i % 2 == 0) _cpu.Ports[i].Connect(ReadULA, WriteULA, SignalULARead, SignalULAWrite);
            }

            _cpu.OnClockTick += _cpu_OnClockTick;
            _cpu.Debug.AfterExecute += _cpu_AfterInstruction;
            _ticksSinceLastDisplayUpdate = 69888; // trigger initial display buffer fill
        }
    }
}

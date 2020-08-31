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
        public const int TICKS_BETWEEN_FRAMES = 69887; // PAL
        public const int FLASH_FRAME_RATE = 16; // PAL

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
            // handle memory contention
            if (_displayWaits.ContainsKey(_ticksSinceLastDisplayUpdate))
            {
                _cpu.AddWaitCycles(_displayWaits[_ticksSinceLastDisplayUpdate]);
            }

            _ticksSinceLastDisplayUpdate++;
            
            if (_ticksSinceLastDisplayUpdate > TICKS_BETWEEN_FRAMES)
            {
                UpdateDisplay();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void UpdateDisplay()
        {
            // this does an 'instant' frame update every TICKS_BETWEEN_FRAMES t-states
            // TODO: move to a bit-by-bit refresh to emulate how TV scan-lines work, as this
            // will be necessary for certain games; but this will work for now!

            // the ULA accesses the memory directly, not via the CPU, so these reads are done
            // without any CPU timing
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

            OnUpdateDisplay?.Invoke(this, _screen.AsRGBA(_displayUpdatesSinceLastFlash++ >= FLASH_FRAME_RATE));
            if (_displayUpdatesSinceLastFlash > FLASH_FRAME_RATE) _displayUpdatesSinceLastFlash = 0;
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

        private void _cpu_OnBeforeInsertWaitCycles(object sender, int e)
        {
        }

        private byte ReadPort()
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            return 0xFF;
        }

        private void WritePort(byte output)
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            if (portAddress % 2 == 0)
            {
                // ULA will respond
                if ((portAddress & 0xFFFE) == 0) // PORT OxFE
                {
                    // TODO: handle MIC, EAR and speaker activation (bit 3 == MIC, bit 4 == EAR / speaker)

                    ColourValue newBorder = DisplayColour.FromThreeBit(output);
                    _screen.SetBorderColour(newBorder);
                }
            }
        }

        private void SignalPortRead()
        {
        }

        private void SignalPortWrite()
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

            // The Spectrum doesn't handle ports using actual port numbers, instead all port reads / writes go to all ports and 
            // devices signal or respond based on a bit-field signature across the 16-bit port address held on the address bus at read/write time.
            // Connect all ports to the handlers, which will then work out which device is being addressed.
            for (byte i = 0; i < 255; i++)
            {
                _cpu.Ports[i].Connect(ReadPort, WritePort, SignalPortRead, SignalPortWrite);
            }

            _cpu.OnClockTick += _cpu_OnClockTick;
            _cpu.Debug.OnBeforeInsertWaitCycles += _cpu_OnBeforeInsertWaitCycles;
            _cpu.Debug.AfterExecute += _cpu_AfterInstruction;
            _ticksSinceLastDisplayUpdate = TICKS_BETWEEN_FRAMES; // trigger initial display buffer fill
        }
    }
}

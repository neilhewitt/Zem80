using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
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
        private IDictionary<int, ushort> _screenLineAddresses;
        private IDictionary<int, int> _displayWaits = new Dictionary<int, int>();

        public event EventHandler<byte[]> OnUpdateDisplay;

        public Processor CPU => _cpu;

        public void Start()
        {
            _cpu.Start(timingMode: TimingMode.FastAndFurious);
        }

        public void Stop()
        {
            _cpu.Stop();
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
                MaskableInterrupt();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void MaskableInterrupt()
        {
            _cpu.RaiseInterrupt();
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

            // 256 * 192
            for (byte y = 0; y < 192; y++)
            {
                ushort address = _screenLineAddresses[y]; // base address for this screen line
                for (byte x = 0; x < 32; x++)
                {
                    byte pixels = pixelBuffer[address + x];
                    _screen.SetPixels(y, x * 8, pixels);
                }
            }

            int columnCounter = 0;
            int rowCounter = 0;
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

        private byte ReadPort()
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            return 0xFF; // no keyboard input yet!
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
            _screen = new ScreenMap(192, 256, 32, 8, 8);
            FillDisplayWaits();

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(0, File.ReadAllBytes("rom\\48k.rom")));
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
            _ticksSinceLastDisplayUpdate = TICKS_BETWEEN_FRAMES; // trigger initial display buffer fill

            // screen pixel layout is not linear in memory - it's done in thirds
            // this pre-calculates the address index of each screen line
            _screenLineAddresses = new Dictionary<int, ushort>();
            for (byte y = 0; y < 192; y++)
            {
                ushort address = 0x0000;
                address = address.SetBit(8, y.GetBit(0));
                address = address.SetBit(9, y.GetBit(1));
                address = address.SetBit(10, y.GetBit(2));
                address = address.SetBit(5, y.GetBit(3));
                address = address.SetBit(6, y.GetBit(4));
                address = address.SetBit(7, y.GetBit(5));
                address = address.SetBit(11, y.GetBit(6));
                address = address.SetBit(12, y.GetBit(7));
                _screenLineAddresses.Add(y, address);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Zem80.Core;
using Zem80.Core.Memory;

namespace Zem80.ZXSpectrumVM
{
    public class Spectrum48K
    {
        public const int TICKS_BETWEEN_FRAMES = 69887; // PAL
        public const int FLASH_FRAME_RATE = 16; // PAL

        private IDictionary<int, string> _keyMatrix = new Dictionary<int, string>()
        {
            { 3, "12345" },
            { 2, "QWERT" },
            { 1, "ASDFG" },
            { 0, "^ZXCV" },
            { 4, "09876" },
            { 5, "POIUY" },
            { 6, "#LKJH" },
            { 7, " *MNB" }
        };

        private Processor _cpu;
        private int _ticksSinceLastDisplayUpdate;
        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;
        private ScreenMap _screen;
        private IDictionary<int, int> _displayWaits = new Dictionary<int, int>();

        private IList<char> _keysDown = new List<char>();

        public event EventHandler<byte[]> OnUpdateDisplay;

        public Processor CPU => _cpu;

        public void Start()
        {
            _cpu.Start(timingMode: TimingMode.PseudoRealTime);
        }

        public void Stop()
        {
            _cpu.Stop();
        }

        public bool KeyDown(string keyName)
        {
            char key = KeyCharFromKeyString(keyName);

            if (!_keysDown.Contains(key))
            {
                _keysDown.Add(key);
                return true;
            }

            return false;
        }

        public bool KeyUp(string keyName)
        {
            char key = KeyCharFromKeyString(keyName);

            if (_keysDown.Contains(key))
            {
                _keysDown.Remove(key);
                return true;
            }

            return false;
        }

        private void _cpu_OnClockTick(object sender, ExecutionPackage e)
        {
            // handle memory contention (where the ULA is reading the video memory and blocks the CPU from running)

            if (_displayWaits.ContainsKey(_ticksSinceLastDisplayUpdate))
            {
                _cpu.AddWaitCycles(_displayWaits[_ticksSinceLastDisplayUpdate]);
            }

            _ticksSinceLastDisplayUpdate++;
            
            if (_ticksSinceLastDisplayUpdate > TICKS_BETWEEN_FRAMES)
            {
                UpdateDisplay();
                RaiseInterruptForSystemRoutines();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void RaiseInterruptForSystemRoutines()
        {
            // before each display draw the Spectrum raises an interrupt on the CPU which forces
            // a call to 0x0038 where the ROM routines begin to service keyboard input, play sound etc
            
            _cpu.RaiseInterrupt();
        }

        private void UpdateDisplay()
        {
            // this does a screen update after every TICKS_BETWEEN_FRAMES t-states

            // the ULA accesses the memory directly, not via the CPU, so these reads are done
            // without any CPU timing
            byte[] pixelBuffer = _cpu.Memory.ReadBytesAt(0x4000, 6144, true);
            byte[] attributeBuffer = _cpu.Memory.ReadBytesAt(0x5800, 768, true);
            _screen.Fill(pixelBuffer, attributeBuffer);

            if (_displayUpdatesSinceLastFlash++ >= FLASH_FRAME_RATE)
            {
                _flashOn = !_flashOn;
            }

            byte[] screenBitmap = _screen.AsRGBA(_flashOn);
            OnUpdateDisplay?.Invoke(this, screenBitmap);

            if (_displayUpdatesSinceLastFlash > FLASH_FRAME_RATE) _displayUpdatesSinceLastFlash = 0;
        }

        private byte ReadPort()
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            byte result = 0xFF;

            if (portAddress.LowByte() == 0xFE)
            {
                byte keyResult = ReadKeyboard(portAddress.HighByte());
                result = keyResult;
            }

            return result; // no keyboard input yet!
        }

        private void WritePort(byte output)
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            
            if (portAddress % 2 == 0)
            {
                // ULA will respond to all even port numbers
                if (portAddress.LowByte() == 0xFE) // PORT OxFE
                {
                    // TODO: handle MIC, EAR and speaker activation (bit 3 == MIC, bit 4 == EAR / speaker)

                    if (portAddress >> 8 < 8)
                    {
                        ColourValue newBorder = DisplayColour.FromThreeBit(output);
                        if (newBorder != null)
                        {
                            _screen.SetBorderColour(newBorder);
                        }
                    }
                    else
                    {
                        if (_cpu.IO.D4)
                        {
                            // beeper time!
                            bool beep = true;
                        }
                    }
                }
            }
        }

        private byte ReadKeyboard(byte rowSelector)
        {
            byte keysPressed = 0xFF;
            keysPressed = keysPressed.SetBit(6, false);

            if (_keysDown.Count() > 0)
            {
                List<string> selectedRows = new List<string>();
                for (int i = 0; i < 8; i++)
                {
                    if (rowSelector.GetBit(i) == false)
                    {
                        selectedRows.Add(_keyMatrix[i]);
                    }
                }

                foreach (string selectedRow in selectedRows)
                {
                    for (int i = 0; i < selectedRow.Length; i++)
                    {
                        char spectrumKey = selectedRow[i];

                        foreach (char key in _keysDown)
                        {
                            if (spectrumKey == key)
                            {
                                keysPressed = keysPressed.SetBit(i, false);
                            }
                        }
                    }
                }
            }

            return keysPressed;
        }

        private void SignalPortRead()
        {
        }

        private void SignalPortWrite()
        {
        }

        private char KeyCharFromKeyString(string keyName)
        {
            char key = keyName.ToUpper() switch
            {
                "LEFTALT" => '^',
                "RIGHTALT" => '#',
                "LEFTSHIFT" => '*',
                "RIGHTSHIFT" => '*',
                _ => keyName[0]
            };

            return char.ToUpper(key);
        }

        private void GenerateDisplayWaits()
        {
            int wait = 6;
            for (int i = 14335; i < 14341; i++) _displayWaits.Add(i, wait--);
            wait = 6;
            for (int i = 14343; i < 14349; i++) _displayWaits.Add(i, wait--);
        }

        public Spectrum48K()
        {
            _screen = new ScreenMap(192, 256, 32, 8, 8);
            GenerateDisplayWaits();

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(0, File.ReadAllBytes("rom\\48k.rom")));
            map.Map(new MemorySegment(16384, 49152));

            _cpu = new Processor(map: map, frequencyInMHz: 3.5);

            // The Spectrum doesn't handle ports using the actual port numbers, instead all port reads / writes go to all ports and 
            // devices signal or respond based on a bit-field signature across the 16-bit port address held on the address bus at read/write time.
            // Connect all ports to the handlers, which will then work out which device is being addressed.
            for (byte i = 0; i < 255; i++)
            {
                _cpu.Ports[i].Connect(ReadPort, WritePort, SignalPortRead, SignalPortWrite);
            }

            _cpu.OnClockTick += _cpu_OnClockTick;
            _ticksSinceLastDisplayUpdate = TICKS_BETWEEN_FRAMES; // trigger initial display buffer fill
        }
    }
}

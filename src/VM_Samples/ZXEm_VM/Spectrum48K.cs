﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Zem80.Core;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;

namespace ZXEm.VM
{
    public class Spectrum48K
    {
        public const int TICKS_BETWEEN_FRAMES = 69887; // PAL only
        public const int FLASH_FRAME_RATE = 16; // PAL only

        private Processor _cpu;
        private int _ticksSinceLastDisplayUpdate;
        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;
        private ScreenMap _screen;
        private IDictionary<int, int> _displayWaits = new Dictionary<int, int>();

        public event EventHandler<byte[]> OnUpdateDisplay;

        public Processor CPU => _cpu;

        public void Start()
        {
            _cpu.Debug.OnBreakpoint += Debug_OnBreakpoint;
            _cpu.Start(timingMode: TimingMode.PseudoRealTime);
        }

        private void Debug_OnBreakpoint(object sender, InstructionPackage e)
        {
        }

        public void Stop()
        {
            _cpu.Stop();
        }

        public void LoadAndRun(string path, ushort address)
        {           
            _cpu.Memory.WriteBytesAt(address, File.ReadAllBytes(path), true);
            _cpu.Stop();
            _cpu.Start(address, timingMode: TimingMode.FastAndFurious);
        }

        private void _cpu_OnClockTick(object sender, InstructionPackage e)
        {
            // handle memory contention (where the ULA is reading the video memory and blocks the CPU from running)
            // we don't emulate the ULA directly and no actual memory reads are occurring here, but that's fine (see UpdateDisplay())

            if (_displayWaits.ContainsKey(_ticksSinceLastDisplayUpdate))
            {
                _cpu.AddWaitCycles(_displayWaits[_ticksSinceLastDisplayUpdate]);
            }

            _ticksSinceLastDisplayUpdate++;
            
            if (_ticksSinceLastDisplayUpdate > TICKS_BETWEEN_FRAMES)
            {
                // we've reached the vertical blanking interval, so raise an interrupt for the keyboard processing etc.
                // and then update the display
                UpdateDisplay();
                _cpu.RaiseInterrupt();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void UpdateDisplay()
        {
            // this does a screen update after every TICKS_BETWEEN_FRAMES t-states

            // we're faking the screen update process here:
            // up to this point, we've been adding wait cycles to the processor
            // to simulate the contention of video memory (the ULA would be reading
            // the video RAM at this point, but in our emulation nothing happpens);
            // then, when we have counted enough ticks since the last screen update 
            // to be at the vertical blanking interval (that's crazy CRT talk, but Google it 
            // if you want to understand how TVs used to work!), we 'instantaneously' 
            // update the display (and since this happens during the OnClockTick event, 
            // the processor is effectively in suspended animation while this occurs, 
            // which of course skews the clock thread, however the clock will simply 
            // pick up at the next available tick and from the emulated Z80's perspective, 
            // no time has elapsed at all - this should be fine for most programs)

            byte[] pixelBuffer = _cpu.Memory.ReadBytesAt(0x4000, 6144, true);
            byte[] attributeBuffer = _cpu.Memory.ReadBytesAt(0x5800, 768, true);
            _screen.Fill(pixelBuffer, attributeBuffer);

            // every FLASH_FRAME_RATE frames, we invert any attribute block that has FLASH set
            if (_displayUpdatesSinceLastFlash++ >= FLASH_FRAME_RATE)
            {
                _flashOn = !_flashOn;
            }

            byte[] screenBitmap = _screen.ToRGBA(_flashOn);
            OnUpdateDisplay?.Invoke(this, screenBitmap);

            if (_displayUpdatesSinceLastFlash > FLASH_FRAME_RATE) _displayUpdatesSinceLastFlash = 0;
        }

        private byte ReadPort()
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            byte result = 0xFF;

            if (portAddress.LowByte() == 0xFE)
            {
                result = ReadKeyboard(portAddress.HighByte(), result);
            }

            return result;
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
                    // bits 0,1,2 encode the border colour
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

        private byte ReadKeyboard(byte rowSelector, byte value)
        {
            IEnumerable<SpectrumKey> keysPressed = SpectrumKeyboard.KeyPressMap.Where(x => x.Value == true).Select(x => x.Key);

            lock (keysPressed)
            {
                if (keysPressed.Count() > 0)
                {
                    foreach (SpectrumKey key in keysPressed)
                    {
                        var mapping = SpectrumKeyboard.GetKeyboardMappingFor(key);
                        if (mapping.RowPort == rowSelector)
                        {
                            value = value.SetBit(mapping.BitIndex, false);
                        }
                    }
                }
            }

            return value;
        }

        private void SignalPortRead()
        {
        }

        private void SignalPortWrite()
        {
        }

        private void GenerateDisplayWaits()
        {
            // Since two devices cannot read RAM at the same time, when the ULA would be reading the screen RAM directly
            // during the display update, the CPU needs to be turned off so no memory reads can occur. This is called 'memory contention'.
            // To do this, the ULA holds the CPU's WAIT pin low to cause the CPU to suspend for a certain number of cycles while it reads
            // the screen RAM. This function generates the correct number of wait cycles at the right points in the screen update cycle, which 
            // will be used when we update the display.
            int wait = 6;
            for (int i = 14335; i < 14341; i++) _displayWaits.Add(i, wait--);
            wait = 6;
            for (int i = 14343; i < 14349; i++) _displayWaits.Add(i, wait--);
        }

        public Spectrum48K(string romPath)
        {
            _screen = new ScreenMap(192, 256, 32, 8, 8);
            GenerateDisplayWaits();

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(0, File.ReadAllBytes(romPath)));
            map.Map(new MemorySegment(16384, 49152));

            _cpu = new Processor(map: map, frequencyInMHz: 3.5);

            // The Spectrum doesn't handle ports using the actual port numbers, instead all port reads / writes go to all ports and 
            // devices signal or respond based on a bit-field signature across the 16-bit port address held on the address bus at read/write time.
            // We'll connect all ports to the same handlers, which will then work out which device is being addressed and function accordingly.
            for (byte i = 0; i < 255; i++)
            {
                _cpu.Ports[i].Connect(ReadPort, WritePort, SignalPortRead, SignalPortWrite);
            }

            _cpu.OnClockTick += _cpu_OnClockTick;
            _ticksSinceLastDisplayUpdate = TICKS_BETWEEN_FRAMES; // trigger initial display buffer fill
        }
    }
}
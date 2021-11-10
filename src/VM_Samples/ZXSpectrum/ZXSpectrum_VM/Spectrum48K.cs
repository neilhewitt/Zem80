using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zem80.Core;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;
using ZXSpectrum.VM.Sound;

namespace ZXSpectrum.VM
{
    public class Spectrum48K
    {
        public const int TICKS_BETWEEN_FRAMES = 69887; // PAL only
        public const int FLASH_FRAME_RATE = 16; // PAL only

        private Processor _cpu;
        private Beeper _beeper;
        private int _ticksSinceLastDisplayUpdate;
        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;
        private ScreenMap _screen;
        private IDictionary<int, int> _displayWaits = new Dictionary<int, int>();
        private Thread _displayUpdateThread;
        private bool _killDisplayUpdateThread;
        private bool _refreshDisplay;

        public event EventHandler<byte[]> OnUpdateDisplay;

        public Processor CPU => _cpu;

        public void Start()
        {
            if (_cpu.State == ProcessorState.Running)
            {
                throw new Exception("VM is already started!");
            }

            _cpu.Start(timingMode: TimingMode.PseudoRealTime);
        }

        public void StartWithSnapshot(string path)
        {
            if (_cpu.State == ProcessorState.Running)
            {
                throw new Exception("VM is already started!");
            }

            _cpu.Suspend();
            _cpu.Start(timingMode: TimingMode.PseudoRealTime); // will initialise the CPU but not start processing, as it's suspended
            LoadSnapshot(path);
            _cpu.Resume();
        }

        public void Stop()
        {
            _killDisplayUpdateThread = true;
            _cpu.Stop();
            _beeper.Dispose();
        }

        private void LoadSnapshot(string path)
        {
            switch (Path.GetExtension(path).ToUpper())
            {
                case ".SNA":
                    LoadSNA(path);
                    break;

                case ".Z80":
                    LoadZ80(path);
                    break;

                case ".TAP":
                    LoadTAP(path);
                    break;
            }
        }

        private void LoadSNA(string path)
        {
            byte[] snapshot = File.ReadAllBytes(path);
            Registers r = _cpu.Registers;

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

            if (snapshot[19].GetBit(2))
            {
                _cpu.EnableInterrupts();
            }

            r.R = snapshot[20];
            r[WordRegister.AF] = getWord(21);
            r.SP = getWord(23);

            _cpu.SetInterruptMode((InterruptMode)snapshot[25]);
            _screen.SetBorderColour(DisplayColour.FromThreeBit(snapshot[26]));

            _cpu.Memory.Untimed.WriteBytesAt(16384, snapshot[27..]);
            _cpu.Pop(WordRegister.PC);
            _cpu.RestoreInterruptsFromNMI();

            ushort getWord(int index)
            {
                return (snapshot[index], snapshot[index + 1]).ToWord();
            }
        }

        private void LoadZ80(string path)
        {
            byte[] snapshot = File.ReadAllBytes(path);
            Registers r = _cpu.Registers;

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
            _screen.SetBorderColour(DisplayColour.FromThreeBit(borderColour));

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
            if (snapshot[27] == 1)
            {
                _cpu.EnableInterrupts();
            }

            _cpu.SetInterruptMode((InterruptMode)snapshot[29].GetByteFromBits(0, 2));

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

            _cpu.Memory.Untimed.WriteBytesAt(16384, memoryImage);
            _cpu.RestoreInterruptsFromNMI();

            ushort getWord(int index)
            {
                return (snapshot[index], snapshot[index + 1]).ToWord();
            }
        }

        private void LoadTAP(string path)
        {
            byte[] tapData = TAPLoader.LoadToBytes(path);
            _cpu.Memory.Untimed.WriteBytesAt(0x4000, tapData);
        }


        private void CPU_OnClockTick(object sender, InstructionPackage e)
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
                _refreshDisplay = true;
                _cpu.RaiseInterrupt();
                _ticksSinceLastDisplayUpdate = 0;
            }
        }

        private void UpdateDisplay()
        {
            while (!_killDisplayUpdateThread)
            {
                if (_refreshDisplay)
                {
                    _refreshDisplay = false;
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

                    byte[] pixelBuffer = _cpu.Memory.Untimed.ReadBytesAt(0x4000, 6144);
                    byte[] attributeBuffer = _cpu.Memory.Untimed.ReadBytesAt(0x5800, 768);
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
            }
        }

        private byte ReadPort()
        {
            ushort portAddress = _cpu.IO.ADDRESS_BUS;
            byte result = 0xFF;

            if (portAddress.LowByte() == 0xFE)
            {
                result = SpectrumKeyboard.GetBitValuesFor(portAddress.HighByte(), result);
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
                    _beeper.SetBeeperState(_cpu.IO.D4);

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
                }
            }
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

        public Spectrum48K()
        {
            string romPath = "rom\\48k.rom";

            _screen = new ScreenMap();
            GenerateDisplayWaits();

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(File.ReadAllBytes(romPath)), 0);
            map.Map(new MemorySegment(49152), 16384);

            _cpu = new Processor(map: map, frequencyInMHz: 3.5f);
            _beeper = new Beeper(_cpu);

            // The Spectrum doesn't handle ports using the actual port numbers, instead all port reads / writes go to all ports and 
            // devices signal or respond based on a bit-field signature across the 16-bit port address held on the address bus at read/write time.
            // We'll connect all ports to the same handlers, which will then work out which device is being addressed and function accordingly.
            for (byte i = 0; i < 255; i++)
            {
                _cpu.Ports[i].Connect(ReadPort, WritePort, SignalPortRead, SignalPortWrite);
            }

            _cpu.OnClockTick += CPU_OnClockTick;
            _ticksSinceLastDisplayUpdate = TICKS_BETWEEN_FRAMES; // trigger initial display buffer fill

            _displayUpdateThread = new Thread(UpdateDisplay);
            _displayUpdateThread.Start();
        }
    }
}

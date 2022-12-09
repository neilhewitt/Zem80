using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zem80.Core;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;
using ZXSpectrum.VM.Sound;
using MultimediaTimer;
using Timer = MultimediaTimer.Timer;

namespace ZXSpectrum.VM
{
    public class Spectrum48K
    {
        public const int FLASH_FRAME_RATE = 16; // PAL only

        private Processor _cpu;
        private Beeper _beeper;
        private Timer _timer;
        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;
        private ScreenMap _screen;

        public event EventHandler<byte[]> OnUpdateDisplay;

        public Processor CPU => _cpu;

        public void Start()
        {
            StartInternal();
        }

        public void StartWithSnapshot(string path)
        {
            StartInternal(path);
        }

        public void Stop()
        {
            _timer.Stop();
            _beeper.Dispose();
            _cpu.Stop();
        }

        private void LoadSnapshot(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Snapshot file {path} does not exist.");
            }
            
            switch (Path.GetExtension(path).ToUpper())
            {
                case ".SNA":
                    LoadSNA(path);
                    break;

                case ".Z80":
                    LoadZ80(path);
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
            ushort pc = _cpu.Stack.Debug.PopStackDirect(); // pops the top value from the stack and moves the stack pointer, but doesn't run any internal cycles
            _cpu.Registers.PC = pc;
            _cpu.RestoreInterruptsAfterNMI();

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

            InterruptMode interruptMode = (InterruptMode)snapshot[29].GetByteFromBits(0, 2);
            if (interruptMode == InterruptMode.IM0) interruptMode = InterruptMode.IM1; // IM0 not used on Spectrum
            _cpu.SetInterruptMode(interruptMode);

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
            _cpu.RestoreInterruptsAfterNMI();

            ushort getWord(int index)
            {
                return (snapshot[index], snapshot[index + 1]).ToWord();
            }
        }

        private void StartInternal(string snapshotPath = null)
        {
            // We're moving the CPU timing into this class by setting the CPU to run SliceTimed and
            // specifying a time slice in ticks (at 3.5MHz, 70000 = 20ms). The CPU will pause after executing
            // those 70000 ticks, which in non-real-time will happen in <1ms, and then begin thread.sleeping;
            // then when the next timer tick occurs after the *real* 20ms, the CPU is resumed and quickly
            // does the interrupt, sound update, screen update etc.

            // We do lose some real-time aspects such as synchronised calls to OnClockTick etc, but for the Spectrum
            // emulation we don't need these. It also makes the keyboard *slightly* less responsive but for human
            // beings this is unlikely to be a problem.

            // Kudos due to SoftSpectrum48 which uses this technique to get real-time Spectrum performance without
            // having to spin the PC CPU all the time. This reduces our CPU use considerably
            // (but not as much as theirs does... not sure why).

            _cpu.Initialise(timingMode: TimingMode.TimeSliced, ticksPerTimeSlice: 70000);
            _cpu.OnTimeSliceEnded += OnTimeSliceEnded;
            
            _cpu.Debug.SetDataBusDefaultValue(0xFF); // Spectrum has pull-up resistors on data bus lines, so will always read 0xFF if not otherwise set by the ULA

            // load a snapshot if we have one
            if (snapshotPath != null) LoadSnapshot(snapshotPath);
            
            // this timer will control the display updates and audio sync
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Elapsed += UpdateDisplay;

            _cpu.Start();
            _timer.Start();
            _beeper.Start();
        }

        private void OnTimeSliceEnded(object sender, long e)
        {
            _cpu.Suspend();
        }

        private void UpdateDisplay(object sender, EventArgs e)
        {
            _cpu.Resume();
            _cpu.RaiseInterrupt();

            // we're faking the screen update process here - in reality there are lots
            // of timing issues around 'contended' memory access by the ULA, and tstate counting etc
            // but for our purposes here we don't need any of that - remember, this is just a demo VM!

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
                    _screen.SetBorderColour(output);
                }

                _beeper.Update(output);
            }
        }

        private void SignalPortRead()
        {
        }

        private void SignalPortWrite()
        {
        }

        public Spectrum48K()
        {
            string romPath = "rom\\48k.rom";

            _screen = new ScreenMap();

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
        }
    }
}

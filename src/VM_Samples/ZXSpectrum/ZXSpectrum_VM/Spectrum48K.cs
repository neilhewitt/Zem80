using System;
using System.Collections.Generic;
using System.IO;
using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.Core.Memory;

namespace ZXSpectrum.VM
{
    public class Spectrum48K
    {
        private Processor _cpu;
        private ULA _ula;
        private SnapshotLoader _loader;

        public Processor CPU => _cpu;
        
        public void Start(string snapshotPath = null)
        {
            _cpu.IO.SetDataBusDefaultValue(0xFF); // Spectrum has pull-up resistors on data bus lines, so will always read 0xFF, not 0x00, if not otherwise set by the ULA
            _cpu.AfterInitialise += (sender, e) =>
            {
                if (snapshotPath != null) _loader.Load(snapshotPath); // snapshot loading must happen after CPU is initialised, but before it starts
            };

            _cpu.Start();
            _ula.Start();
        }

        public void Stop()
        {
            _ula.Stop();
            _cpu.Stop();
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
                // ULA will respond to all even port numbers - this is
                // a supreme Sinclair hack, but it works well

                if (portAddress.LowByte() == 0xFE)
                {
                    _ula.SetBorderColour(output);
                }

                _ula.SetBeeper(output);
            }
        }

        public void Mute()
        {
            _ula.MuteBeeper();
        }

        public void Unmute()
        {
            _ula.UnmuteBeeper();
        }

        public Spectrum48K(Action<byte[]> OnUpdateDisplay)
        {
            string romPath = "rom\\48k.rom";

            // set up the memory map - 16K ROM + 48K RAM = 64K

            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(File.ReadAllBytes(romPath)), 0);
            map.Map(new MemorySegment(49152), 16384);

            // We're setting the CPU to run TimeSliced. The CPU will run as fast as possible (ie not real time),
            // then pause after executing a fixed number of ticks (20ms worth in real-time at the CPU frequency),
            // which in non-real-time will happen in a much shorter time (generally <1ms), and begin sleeping;
            // then after 20ms actually elapses, the CPU is resumed and we do the interrupt, sound update, screen update etc.

            // We do lose some real-time aspects such as synchronised calls to OnClockTick etc, but for the Spectrum
            // emulation we don't need these. It also makes the keyboard *slightly* less responsive since the key must
            // be held down until the next time slice runs and samples the keyboard, but for human
            // beings this is unlikely to be a problem since pressing a key for <20ms is quite hard to do.

            // Kudos due to SoftSpectrum48 which uses this technique to get real-time Spectrum performance without
            // having to spin a CPU core all the time. This reduces our CPU use considerably.

            _cpu = new Processor(
                map: map,
                clock: ClockMaker.TimeSlicedClock(
                    3f, // Real Spectrum runs at 3.5MHz but contended memory etc slows it down, so we'll run at 3MHz which seems near enough right
                    TimeSpan.FromMilliseconds(20)
                    )
                );

            _ula = new ULA(_cpu); // models the ULA chip which controls the screen and sound
            _ula.OnUpdateDisplay += (sender, rgba) => OnUpdateDisplay(rgba);

            _loader = new SnapshotLoader(_cpu, _ula); // handles loading of snapshots

            // The Spectrum doesn't handle ports using the actual port numbers, instead all port reads / writes go to all ports and 
            // devices signal or respond based on a bit-field signature across the 16-bit port address held on the address bus at read/write time.
            // We'll connect all ports to the same handlers, which will then work out which device is being addressed and function accordingly.
            for (byte i = 0; i < 255; i++)
            {
                _cpu.Ports[i].Connect(ReadPort, WritePort, null, null);
            }
        }
    }
}

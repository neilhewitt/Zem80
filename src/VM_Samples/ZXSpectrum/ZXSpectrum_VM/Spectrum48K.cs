﻿using System;
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
            _ula.Stop();
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

        private void StartInternal(string snapshotPath = null)
        {
            // We're setting the CPU to run TimeSliced. The CPU will run as fast as possible (ie not real time),
            // then pause after executing 70000 ticks (20ms worth in real-time), which in non-real-time will happen
            // in a much shorter time (generally <1ms), and begin sleeping;
            // then after the *real* 20ms, the CPU is resumed and we do the interrupt, sound update, screen update etc.

            // We do lose some real-time aspects such as synchronised calls to OnClockTick etc, but for the Spectrum
            // emulation we don't need these. It also makes the keyboard *slightly* less responsive but for human
            // beings this is unlikely to be a problem.

            // Kudos due to SoftSpectrum48 which uses this technique to get real-time Spectrum performance without
            // having to spin the PC CPU all the time. This reduces our CPU use considerably.

            _cpu.IO.SetDataBusDefaultValue(0xFF); // Spectrum has pull-up resistors on data bus lines, so will always read 0xFF, not 0x00, if not otherwise set by the ULA
            _cpu.AfterInitialise += (sender, e) =>
            {
                if (snapshotPath != null) LoadSnapshot(snapshotPath); // snapshot loading must happen after CPU is initialised, but before it starts
            };

            _cpu.Start();
            _ula.Start();
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

        private void SignalPortRead()
        {
        }

        private void SignalPortWrite()
        {
        }

        public Spectrum48K(EventHandler<byte[]> OnUpdateDisplay)
        {
            string romPath = "rom\\48k.rom";

            // set up the memory map - 16K ROM + 48K RAM = 64K
            IMemoryMap map = new MemoryMap(65536, false);
            map.Map(new ReadOnlyMemorySegment(File.ReadAllBytes(romPath)), 0);
            map.Map(new MemorySegment(49152), 16384);

            _cpu = new Processor(
                map: map,
                clock: ClockMaker.TimeSlicedClock(
                    3.5f, // 3.5MHz 
                    TimeSpan.FromMilliseconds(20)
                    )
                );

            _ula = new ULA(_cpu); // models the ULA chip which controls the screen and sound
            _ula.OnUpdateDisplay += OnUpdateDisplay;

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

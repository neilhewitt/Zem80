using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zem80.Core;
using Zem80.Core.CPU;
using ZXSpectrum.VM.Sound;
using Timer = MultimediaTimer.Timer;

namespace ZXSpectrum.VM
{
    public class ULA
    {
        public const int FLASH_FRAME_RATE = 16; // PAL only
        public const int TICKS_PER_FRAME = 69888;
        public const int CONTENTION_START = 14335;
        public const int CONTENTION_END = 14463;

        private Processor _cpu;
        private ScreenMap _screen;
        private Beeper _beeper;

        private Timer _screenRefreshTimer;
        private Dictionary<int, int> _displayWaits;
        private int _ticksThisFrame;
        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;

        public event EventHandler<byte[]> OnUpdateDisplay;

        public void Start()
        {
            //_screenRefreshTimer.Start();
            _beeper.Start();
        }

        public void Stop()
        {
            //_screenRefreshTimer?.Stop();
            _beeper.Dispose();
        }

        public void SetBorderColour(byte colour)
        {
            _screen.SetBorderColour(colour);
        }

        public void SetBeeper(byte output)
        {
            _beeper.Update(output);
        }

        public void UpdateDisplay()
        {
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

            // this gives us 50 screen updates per second, faking a PAL TV display; however, since the screen painting
            // is not at all synchronised with Windows screen refresh, we will see tearing; the only way to fix this
            // would be to enable some form of vsync with Windows, probably via DirectX, which is very much
            // out of scope for this sample.

            _cpu.Interrupts.RaiseMaskable();
        }
        private void Clock_OnTick(object sender, long e)
        {
            _ticksThisFrame++;
        }

        private void OnInterrupt(object sender, long e)
        {
            _ticksThisFrame = 0;
        }

        private void BeforeExecuteInstruction(object sender, InstructionPackage e)
        {
            Instruction instruction = e.Instruction;
            int ticksToAdd = 0;

            // handle contended memory access
            // and contended I/O

            // contended memory access - if the running instruction has memory read / write cycles, we need to pause the CPU for x cycles
            // where x depends on the number of ticks since the last interrupt
            int ticksSoFar = _ticksThisFrame;
            if (instruction.AccessesMemory || instruction.PerformsIO)
            {
                foreach (MachineCycle machineCycle in instruction.MachineCycles.Cycles)
                {
                    int waits = 0;
                    _displayWaits.TryGetValue(ticksSoFar, out waits);
                    if (waits > 0)
                    {
                        if (machineCycle.HasMemoryAccess)
                        {
                            addStandardWaits(machineCycle, waits);
                        }
                        else if (machineCycle.HasIO)
                        {
                            byte port = _cpu.Registers.C;
                            if (instruction.Mnemonic == "IN A,(n)" || instruction.Mnemonic == "OUT (n),A") port = e.Data.Argument1;

                            bool contended = !port.GetBit(0); // if bit 0 of the port # is reset, the ULA will pause the CPU
                            bool falselyContended = false;
                            if (port > 0x40 && port < 0x7F)
                            {
                                falselyContended = true;
                            }

                            if (contended && !falselyContended)
                            {
                                addStandardWaits(machineCycle, waits);
                            }
                            else if (contended && falselyContended)
                            {
                                addCustomWaits(waits, 1);
                                addCustomWaits(waits, 3);
                            }
                            else if (!contended && falselyContended)
                            {
                                addCustomWaits(waits, 1);
                                addCustomWaits(waits, 1);
                                addCustomWaits(waits, 1);
                                addCustomWaits(waits, 1);
                            }
                        }
                    }

                    void addStandardWaits(MachineCycle machineCycle, int waits)
                    {
                        ticksToAdd += waits;
                        ticksSoFar += machineCycle.TStates + waits;
                    }

                    void addCustomWaits(int waits, int cycleStates)
                    {
                        ticksToAdd += waits;
                        ticksSoFar += cycleStates + waits;
                    }
                }
            }

            if (ticksToAdd > 0)
            {
                _cpu.Clock.WaitForClockTicks(ticksToAdd);
            }
        }

        private void GenerateDisplayWaits()
        {
            _displayWaits = new Dictionary<int, int>();

            // Since two devices cannot read RAM at the same time, when the ULA would be reading the screen RAM directly
            // during the display update, the CPU needs to be turned off so no memory reads can occur. This is called 'memory contention'.
            // To do this, the ULA actually blocks the clock connection to the Z80 to cause it to suspend for a certain number of cycles while it reads
            // the screen RAM. This function generates the correct number of wait cycles at the right points in the screen update cycle, which 
            // will be used when we update the display.

            MapWaits(14335, 14463);
            MapWaits(14559, 16095);

            void MapWaits(int start, int end)
            {
                for (int i = start; i <= end; i += 8)
                {
                    int tstate = i;
                    for (int j = 6; j >= 0; j--)
                    {
                        _displayWaits[tstate++] = j;
                    }
                    _displayWaits[tstate] = 0;
                }
            }
        }

        public ULA(Processor cpu)
        {
            _cpu = cpu;
            cpu.Clock.OnTick += Clock_OnTick;
            //cpu.BeforeExecuteInstruction += BeforeExecuteInstruction;
            cpu.Interrupts.OnMaskableInterrupt += OnInterrupt;
            GenerateDisplayWaits();

            _screen = new ScreenMap();
            //_screenRefreshTimer = new Timer();
            //_screenRefreshTimer.Interval = TimeSpan.FromMilliseconds(20);
            //_screenRefreshTimer.Elapsed += UpdateDisplay;

            _beeper = new Beeper(cpu);
        }
    }
}

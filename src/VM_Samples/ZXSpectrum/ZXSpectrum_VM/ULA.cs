using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zem80.Core;
using Zem80.Core.CPU;
using ZXSpectrum.VM.Sound;

namespace ZXSpectrum.VM
{
    public class ULA
    {
        public const int FLASH_FRAME_RATE = 16; // PAL only
        public const int TICKS_PER_FRAME = 70000;

        private Processor _cpu;
        private ScreenMap _screen;
        private Beeper _beeper;

        private int _displayUpdatesSinceLastFlash;
        private bool _flashOn;

        public event EventHandler<byte[]> OnUpdateDisplay;

        public void Start()
        {
            _beeper.Start();
        }

        public void Stop()
        {
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

        public ULA(Processor cpu)
        {
            _cpu = cpu;
            _cpu.Clock.SetEvent(TICKS_PER_FRAME, () => UpdateDisplay(), true);

            _screen = new ScreenMap();
            _beeper = new Beeper(cpu);
        }
    }
}

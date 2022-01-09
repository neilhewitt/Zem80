using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZXSpectrum.VM.Sound
{
    public class Speaker : ISampleProvider, IDisposable
    {
        private const int BUFFER_SIZE = 480000;
        private const int TICKS_PER_FRAME = 208;
        private const int TICKS_PER_SAMPLE = 3;

        private Stopwatch _stopwatch;
        private bool _killSampleThread;
        private int _ticksPerFrame = 208;
        private int _ticksThisFrame;
        private int _onCountThisFrame;
        private SampleBuffer _buffer;
        
        public bool On { get; set; }

        public WaveFormat WaveFormat { get; init; } = WaveFormat.CreateIeeeFloatWaveFormat(48000, 1);

        public int Read(float[] buffer, int offset, int count)
        {
            return _buffer.Read(buffer, offset, count);
        }

        public void Dispose()
        {
            _killSampleThread = true;
        }

        private void SampleThread()
        {
            _buffer = new SampleBuffer(BUFFER_SIZE);
            _stopwatch.Start();

            // every 3 windows ticks, sample the speaker state
            // every 208 windows ticks, calculate a sample value and add to the buffer

            long lastStateTicks = 0;
            long lastBufferTicks = 0;

            while (!_killSampleThread)
            {
                long ticksToState = lastStateTicks + TICKS_PER_SAMPLE;
                long ticksToBuffer = lastBufferTicks + TICKS_PER_FRAME;
                long elapsed = _stopwatch.ElapsedTicks;

                if (elapsed >= ticksToState)
                {
                    if (_ticksThisFrame++ < _ticksPerFrame)
                    {
                        if (On) _onCountThisFrame++;
                    }

                    lastStateTicks = _stopwatch.ElapsedTicks;
                }

                if (elapsed >= ticksToBuffer)
                {
                    float fraction = ((float)_onCountThisFrame / (float)_ticksPerFrame);
                    ushort sample = (ushort)(fraction * 65535);
                    _buffer.Add(sample);
                    _onCountThisFrame = 0;
                    _ticksThisFrame = 0;

                    lastStateTicks = _stopwatch.ElapsedTicks;
                }
            }
        }

        public Speaker()
        {
            _stopwatch = new Stopwatch();
            Thread sampleThread = new Thread(SampleThread);
            sampleThread.Start();
        }
    }
}

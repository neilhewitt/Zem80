using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Zem80.Core;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Linq;
using System.Threading.Tasks;

namespace ZXSpectrum.VM.Sound
{
    public class Beeper : IDisposable
    {
        private Processor _cpu;
        private Thread _addSamplesThread;
        private bool _killThread;
        private int _clockTicksPerFrame = 70;
        private int _bufferEmptyThreshold = 4;

        private int _ticksThisFrame;
        private int _frameTStatesOn;
        private byte[] _inBuffer;
        private byte[] _outBuffer;
        private int _bufferIndex;

        private bool _beeperOn;
        private bool _addSamples;

        public Beeper(Processor cpu)
        {
            _cpu = cpu;

            _inBuffer = new byte[_bufferEmptyThreshold];

            _cpu.OnClockTick += CPU_OnClockTick;
            _addSamplesThread = new Thread(AddSamples);
            _addSamplesThread.IsBackground = true;
            _addSamplesThread.Start();
        }

        public void SetBeeperState(bool on)
        {
            _beeperOn = on;
        }

        public void Dispose()
        {
            _killThread = true;
        }

        private void AddSamples()
        {
            WaveOutEvent player = new WaveOutEvent();
            WaveFormat format = new WaveFormat(48000, 16, 1);
            
            BufferedWaveProvider provider = new BufferedWaveProvider(format);
            provider.BufferDuration = TimeSpan.FromSeconds(30);
            provider.DiscardOnBufferOverflow = true;
            
            player.Init(provider);
            player.Volume = 0.33f;
            player.Play();

            while (!_killThread)
            {
                if (_addSamples)
                {
                    _addSamples = false;
                    provider.AddSamples(_outBuffer, 0, _bufferEmptyThreshold);
                }
            }

            player.Dispose();
        }

        private void CPU_OnClockTick(object sender, Zem80.Core.Instructions.InstructionPackage e)
        {
            if (_ticksThisFrame <= _clockTicksPerFrame)
            {
                if (_beeperOn) _frameTStatesOn++;
                _ticksThisFrame++;
            }
            else if (_bufferIndex < _bufferEmptyThreshold)
            {
                float fraction = ((float)_frameTStatesOn / (float)_clockTicksPerFrame);
                ushort sample = (ushort)(fraction * 65535);
                _inBuffer[_bufferIndex++] = (byte)(sample / 256);
                _inBuffer[_bufferIndex++] = (byte)(sample % 256);
                _frameTStatesOn = 0;
                _ticksThisFrame = 0;
            }
            else
            {
                _outBuffer = (byte[])_inBuffer.Clone();
                _addSamples = true;
                _bufferIndex = 0;
                _frameTStatesOn = 0;
                _ticksThisFrame = 0;
            }
        }
    }
}

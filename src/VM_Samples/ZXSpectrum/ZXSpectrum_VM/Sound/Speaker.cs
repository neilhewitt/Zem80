using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;
using Zem80.Core.Instructions;

namespace ZXSpectrum.VM.Sound
{
    public class Speaker : IDisposable
    {
        private const int BUFFER_SIZE = 48;
        private const int TICKS_PER_FRAME = 73;

        private byte[] _playBuffer;
        private byte[] _dataBuffer = new byte[BUFFER_SIZE];
        private int _bufferIndex;
        private int _ticksThisFrame;
        private int _ticksOn;

        private IWavePlayer _player;
        private MixingSampleProvider _mixer;
        private BufferedWaveProvider _provider;

        public bool On { get; set; }

        public void Dispose()
        {
            _player.Dispose();
        }

        private void Tick(object sender, InstructionPackage package)
        {
            if (_ticksThisFrame++ <= TICKS_PER_FRAME)
            {
                if (On) _ticksOn++;
            }
            else if (_bufferIndex < BUFFER_SIZE)
            {
                float fraction = ((float)_ticksOn / (float)TICKS_PER_FRAME);
                ushort sample = (ushort)(fraction * 65535);
                _dataBuffer[_bufferIndex++] = (byte)(sample / 256);
                _dataBuffer[_bufferIndex++] = (byte)(sample % 256);
                _ticksOn = 0;
                _ticksThisFrame = 0;
            }
            else
            {
                _playBuffer = _dataBuffer;
                _dataBuffer = new byte[BUFFER_SIZE];
                _provider.AddSamples(_playBuffer, 0, BUFFER_SIZE);
                _bufferIndex = 0;
                _ticksOn = 0;
                _ticksThisFrame = 0;
            }
        }

        public Speaker(Processor cpu)
        {
            _player = new WaveOutEvent();

            WaveFormat format = WaveFormat.CreateIeeeFloatWaveFormat(48000, 1);

            _mixer = new MixingSampleProvider(format);
            _mixer.ReadFully = true;

            _provider = new BufferedWaveProvider(format);
            _provider.ReadFully = true;
            _provider.BufferDuration = TimeSpan.FromSeconds(30);
            _provider.DiscardOnBufferOverflow = true;

            _mixer.AddMixerInput(new Pcm16BitToSampleProvider(_provider));
            _player.Init(_mixer);
            _player.Volume = 0.33f;
            _player.Play();

            cpu.OnClockTick += Tick;
        }
    }
}

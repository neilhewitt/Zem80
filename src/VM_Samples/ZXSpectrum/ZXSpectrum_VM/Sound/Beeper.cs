using System;
using Zem80.Core;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ZXSpectrum.VM.Sound
{
    public class Beeper : IDisposable
    {
        private Processor _cpu;
        private int _ticksPerSample = 73;
        private int _bytesPerFrame = 960;
        private int _ticksThisSample;
        private int _ticksOn;
        private int _bytesSoFar;
        private byte[] _buffer;

        private bool _beeperOn;
        private bool _started;
        private WaveOutEvent _player;
        private BufferedWaveProvider _provider;
        private MixingSampleProvider _mixer;

        public Beeper(Processor cpu)
        {
            _cpu = cpu;
            _buffer = new byte[_bytesPerFrame];

            _provider = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 1));
            _provider.BufferDuration = TimeSpan.FromMilliseconds(1000);
            _provider.ReadFully = true;

            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 1));
            _mixer.ReadFully = true;
            _mixer.AddMixerInput(new Pcm16BitToSampleProvider(_provider));

            _player = new WaveOutEvent();
            _player.Init(_mixer);
            _player.Volume = 0.33f;
        }

        public void Start()
        {
            if (!_started)
            {
                _provider.AddSamples(new byte[_bytesPerFrame], 0, _bytesPerFrame);
                _player.Play();
                _started = true;
            }
        }

        public void Dispose()
        {
            _player.Dispose();
        }

        public void SetBeeperState(bool on)
        {
            _beeperOn = on;
        }

        public void Tick()
        {
            if (!_started) Start();

            if (_ticksThisSample <= _ticksPerSample)
            {
                if (_beeperOn) _ticksOn++;
                _ticksThisSample++;
            }
            else
            {
                float fraction = ((float)_ticksOn / (float)_ticksPerSample);
                ushort sample = (ushort)(fraction * 65535);
                int index = _bytesSoFar;
                _buffer[index] = (byte)(sample / 256);
                _buffer[index + 1] = (byte)(sample % 256);
                _ticksOn = 0;
                _ticksThisSample = 0;
                _bytesSoFar+=2;
                
                if (_bytesSoFar >= _bytesPerFrame)
                {
                    _provider.AddSamples(_buffer, 0, _buffer.Length);
                    _bytesSoFar = 0;
                }
            }
        }
    }
}

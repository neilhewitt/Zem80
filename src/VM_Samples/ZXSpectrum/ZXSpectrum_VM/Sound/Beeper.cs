using System;
using Zem80.Core;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Zem80.Core.Instructions;

namespace ZXSpectrum.VM.Sound
{
    public class Beeper : IDisposable
    {
        private Processor _cpu;

        private WaveOutEvent _player;
        private Speaker _speaker;

        public Beeper(Processor cpu)
        {
            _cpu = cpu;
            
            _speaker = new Speaker();

            _player = new WaveOutEvent();
            _player.DesiredLatency = 100;
            _player.Init(_speaker);
            _player.Volume = 0.33f;

            _player.Play();
        }

        public void Dispose()
        {
            _speaker.Dispose();
            _player.Dispose();
        }

        public void SetBeeperState(bool on)
        {
            _speaker.On = on;
        }
    }
}

﻿using System;
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
    public class Beeper
    {
        private Processor _cpu;
        private int _clockTicksPerFrame = 73;
        private int _bufferSamples = 882;

        private int _ticksThisFrame;
        private int _frameTStatesOn;
        private byte[] _buffer;
        private int _bufferIndex;

        private bool _beeperOn;

        private IWavePlayer _player;
        private MixingSampleProvider _mixer;
        private BufferedWaveProvider _provider;

        public Beeper(Processor cpu)
        {
            _cpu = cpu;
            _cpu.OnClockTick += CPU_OnClockTick;

            _buffer = new byte[_bufferSamples];

            _player = new WaveOutEvent();
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            _mixer.ReadFully = true;
            _provider = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            _provider.BufferDuration = TimeSpan.FromSeconds(5);
            _provider.DiscardOnBufferOverflow = true;
            _mixer.AddMixerInput(new Pcm8BitToSampleProvider(_provider));
            _player.Init(_mixer);
            _player.Volume = 1f;
            _player.Play();
        }

        public void Beep(bool on)
        {
            _beeperOn = on;
        }

        private void CPU_OnClockTick(object sender, Zem80.Core.Instructions.InstructionPackage e)
        {
            if (_ticksThisFrame < _clockTicksPerFrame - 1)
            {
                if (_beeperOn) _frameTStatesOn++;
                _ticksThisFrame++;
            }
            else if (_bufferIndex < _bufferSamples)
            {
                if (_beeperOn) _frameTStatesOn++;
                float fraction = ((float)_frameTStatesOn / (float)_clockTicksPerFrame);
                _buffer[_bufferIndex] = (byte)(fraction * 255);
                _bufferIndex++;
                _frameTStatesOn = 0;
                _ticksThisFrame = 0;
            }
            else
            {
                _provider.AddSamples(_buffer, 0, _buffer.Length);
                _bufferIndex = 0;
                _frameTStatesOn = 0;
                _ticksThisFrame = 0;
            }
        }
    }
}

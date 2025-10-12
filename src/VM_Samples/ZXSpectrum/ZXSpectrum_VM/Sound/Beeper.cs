using NAudio.Wave;
using System;
using Zem80.Core;
using Zem80.Core.CPU;

namespace ZXSpectrum.VM.Sound
{
    /*
     * This class is based on a design from SoftSpectrum48 by Magnus Crook. Since the source code for SoftSpectrum 48
     * is freely distributed by Magnus on his Web site, but no license file or information is included or provided
     * anywhere that I could find, I have written this code entirely from scratch but have based it on the design of the 
     * relevant class in SoftSpectrum 48. 
     * 
     * This implementation is not by any means perfect. We still get audio glitching. For a demo VM, I'm not that bothered.
     * If you want to fix it... feel free to open a PR :-)
     */

    public class Beeper : IDisposable
    {
        private Processor _cpu;

        // NOTE - varies by Spectrum model / region - TODO make configurable
        private int _ticksPerSample;
        private int _ticksPerFrame;
        private int _sampleFactor = 2;
        private int _bufferSize = 120000; // big enough but not big enough to cause audio latency

        private byte[][] _sampleData; // divided into three sets: empty, low frequency, high frequency

        private bool _lastBit3 = true;
        private bool _lastBit4 = true;
        private byte _lastBorderColour;
        private long _lastTStates;
        
        private int _currentFrequencyRange;

        private IWavePlayer _player;
        private BufferedWaveProvider _provider;

        public void Start()
        {
            _player.Play();
        }

        public void Dispose()
        {
            _player.Stop();
            _player.Dispose();
        }

        public void Update(byte outValue)
        {
            int frequencyRange = 0;
            byte borderColour = outValue.GetByteFromBits(0, 3);

            bool changedState = false;
            bool bit3 = ((outValue >> 3) & 1) != 0;
            bool bit4 = ((outValue >> 4) & 1) != 0;

            if (bit3 != _lastBit3)
            {
                frequencyRange = 1;
                changedState = true;
                _lastBit3 = bit3;
            }

            if (bit4 != _lastBit4)
            {
                frequencyRange = 2;
                changedState = true;
                _lastBit4 = bit4;
            }

            if (borderColour != _lastBorderColour && (outValue & 8) == 8)
            {
                frequencyRange = 1;
                changedState = true;
                _lastBorderColour = borderColour;
            }

            if (changedState)
            {
                long currentTStates = _cpu.Clock.Ticks;
                long ticks = currentTStates - _lastTStates;

                long samplesRequired = (ticks / _ticksPerSample) / _sampleFactor;
                if (samplesRequired <= _bufferSize && samplesRequired > 0)
                {
                    _currentFrequencyRange = _currentFrequencyRange == 0 ? frequencyRange : 0;
                    _provider.AddSamples(_sampleData[_currentFrequencyRange], 0, (int)samplesRequired);
                }
                else
                {
                    _currentFrequencyRange = 0;
                }

                _lastTStates = currentTStates;
            }
        }

        private void SetupSamples()
        {
            _sampleData = new byte[3][];

            for (int i = 0; i < 3; i++)
            {
                _sampleData[i] = new byte[_bufferSize];
                for (int j = 0; j < _bufferSize; j++)
                {
                    // 0 = no sound, 10 = low frequency sound, 20 = high frequency sound
                    // basically, oscillating between low/high at different rates creates the tones
                    _sampleData[i][j] = i switch { 1 => 10, 2 => 20, _ => 0 };
                }
            }
        }

        public Beeper(Processor cpu, int displayFramesPerSecond)
        {
            try
            {
                _cpu = cpu;

                // Spectrum normally clocks at 3.5MHz, and would require 8 ticks per audio sample
                // But in order to allow the Spectrum to run at a different clock speed, we need
                // work out the divisor for the audio frequency based on the actual clock speed of 
                // the emulated CPU.
                long frequencyDivisor = 3500000 / 8; 

                // we need the actual frequency of the emulated CPU in Hz
                long frequency = (long)(_cpu.Clock.FrequencyInMHz * 1000000);

                // now work out the number of ticks per frame and per sample
                _ticksPerFrame = (int)(frequency / displayFramesPerSecond);
                _ticksPerSample = (int)(frequency / frequencyDivisor);

                // generate sample data for each frequency range
                SetupSamples();

                // 1000 / displayFramesPerSecond is the required latency in milliseconds (ie 1000ms / 50fps = 20ms) and 
                // we need to use the smallest latency that we can
                _player = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 1000 / displayFramesPerSecond); // WasapiOut can do lower latency than WaveOut

                // finally, work out the sample rate for the WAV data
                int sampleRate = ((_ticksPerFrame * displayFramesPerSecond) / (_ticksPerSample * _sampleFactor));
                WaveFormat format = new WaveFormat(sampleRate, 8, 1);

                // the BufferedWaveProvider can run for as long as we need it, adding to the buffer as we go, and
                // circularly overwriting the buffer as we run out of space
                _provider = new BufferedWaveProvider(format);
                _provider.BufferLength = _bufferSize;
                _provider.DiscardOnBufferOverflow = true;

                _player.Init(_provider);
                _player.Volume = 1f;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

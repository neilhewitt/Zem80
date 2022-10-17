using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core;
using Zem80.Core.Instructions;

namespace ZXSpectrum.VM.Sound
{
    /*
     * This class is based on a design from SoftSpectrum48 by Magnus Crook. Since the source code for SoftSpectrum 48
     * is freely distributed by Magnus on his Web site, but no license file or information is included or provided
     * anywhere that I could find, I have written this code entirely from scratch but have based it on the design of the 
     * relevant class in SoftSpectrum 48. 
     * 
     * Sadly, despite my efforts, the audio is still noisy (but no longer choppy - using WasapiOut fixes that!)
     */
    
    public class Beeper : IDisposable
    {
        private Processor _cpu;

        // NOTE - varies by Spectrum model / region - TODO make configurable
        private const int TICKS_PER_SAMPLE = 8;
        private const int TICKS_PER_FRAME = 69888; 
        private const int FRAMES_PER_SECOND = 50;
        private const int SAMPLE_SIZE = 120000;

        private byte[][] _sampleData; // divided into three sets: empty, low frequency, high frequency

        private bool _lastBit3 = true;
        private bool _lastBit4 = true;
        private byte _lastBorderColour;
        private long _lastTStates;
        
        private int _currentFrequencyRange;

        private IWavePlayer _player;
        private BufferedWaveProvider _provider;

        public void Dispose()
        {
            _player.Dispose();
        }

        public void Update(byte outValue, byte borderColour)
        {
            int frequencyRange = 0;

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
                long currentTStates = _cpu.EmulatedTStates;
                long ticks = currentTStates - _lastTStates;

                long samplesRequired =  ticks / TICKS_PER_SAMPLE;
                if (samplesRequired <= SAMPLE_SIZE && samplesRequired > 0)
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
                _sampleData[i] = new byte[SAMPLE_SIZE];
                for (int j = 0; j < SAMPLE_SIZE; j++)
                {
                    // 0 = no sound, 10 = low frequency sound, 20 = high frequency sound
                    // basically, oscillating between low/high at different rates creates the tones
                    _sampleData[0][j] = i switch { 1 => 10, 2 => 20, _ => 0};
                }
            }
        }

        public Beeper(Processor cpu)
        {
            _cpu = cpu;

            SetupSamples();

            _player = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, true, 40); // can do lower latency than WaveOut
            
            int sampleRate = ((TICKS_PER_FRAME * FRAMES_PER_SECOND) / TICKS_PER_SAMPLE);
            WaveFormat format = new WaveFormat(sampleRate, 8, 1);

            _provider = new BufferedWaveProvider(format);
            _provider.DiscardOnBufferOverflow = true;

            _player.Init(_provider);
            _player.Volume = 0.33f;
            _player.Play();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZXSpectrum.VM.Sound
{
    public class SampleBuffer
    {
        private int _bufferLength;
        private float[] _samples;
        private int _lastRead;
        private int _lastWrite;

        public int LengthInSamples { get; init; }
        public int LengthInBytes => LengthInSamples * 2;

        public void Add(ushort sample)
        {
            if (_lastWrite >= _bufferLength)
            {
                _lastWrite = 0;
            }

            _samples[_lastWrite++] = sample;
        }

        public int Read(float[] destination, int offset, int sampleCount)
        {
            int startLeft = _lastRead;
            int endLeft = _lastWrite;
            int endRight = 0;

            int read = 0;
            
            if (endLeft < startLeft)
            {
                endLeft = _samples.Length;
                endRight = _lastWrite;
            }

            if (endLeft > startLeft + sampleCount)
            {
                endLeft = startLeft + sampleCount;
                endRight = 0;
            }

            for (int i = startLeft; i < endLeft; i++)
            {
                destination[offset++] = _samples[i];
                read++;
            }

            _lastRead = endLeft;

            if (endRight > (sampleCount - read))
            {
                endRight = (sampleCount - read);
            }

            if (endRight > 0)
            {
                for (int i = 0; i < endRight; i++)
                {
                    destination[offset++] = _samples[i];
                    read++;
                }

                _lastRead = endRight;
            }

            return read;
        }

        public SampleBuffer(int bufferLength)
        {
            _bufferLength = bufferLength;
            _samples = new float[bufferLength];
            LengthInSamples = bufferLength;
        }
    }
}

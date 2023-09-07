using Spect.Net.SpectrumEmu;
using Spect.Net.SpectrumEmu.Cpu;
using Spect.Net.SpectrumEmu.Devices.Memory;
using Spect.Net.SpectrumEmu.Devices.Ports;

namespace SpectNetRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Z80Cpu cpu = new Z80Cpu(new Spectrum48MemoryDevice(), new Spectrum48PortDevice(), true);
            

        }
    }
}
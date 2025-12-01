namespace Zem80.Core.CPU
{
    public class PortReadWriteCycleInfo : ReadWriteCycleInfo
    {
        public byte Port { get; set; }
        public bool AddressFromBC { get; set; }
    }
}

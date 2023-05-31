using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.InputOutput
{
    public class Ports
    {
        private IDictionary<byte, Port> _ports;

        public Port this[byte portNumber]
        {
            get
            {
                return _ports[portNumber];
            }
        }

        public void DisconnectAll()
        {
            foreach(Port port in _ports.Values)
            {
                port.Disconnect();
            }
        }

        public Ports(IMachineCycleTiming timing)
        {
            _ports = new Dictionary<byte, Port>();
            for (int i = 0; i <= 255; i++)
            {
                Port port = new Port((byte)i, timing);
                _ports.Add((byte)i, port);
            }
        }
    }
}

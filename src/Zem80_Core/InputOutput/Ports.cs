using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.InputOutput
{
    public class Ports : IPorts
    {
        private IDictionary<byte, IPort> _ports;

        public IPort this[byte portNumber]
        {
            get
            {
                return _ports[portNumber];
            }
        }

        public void DisconnectAll()
        {
            foreach (IPort port in _ports.Values)
            {
                port.Disconnect();
            }
        }

        public Ports(IProcessorTiming timing)
        {
            _ports = new Dictionary<byte, IPort>();
            for (int i = 0; i <= 255; i++)
            {
                IPort port = new Port((byte)i, timing);
                _ports.Add((byte)i, port);
            }
        }
    }
}

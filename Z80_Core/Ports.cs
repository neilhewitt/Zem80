using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
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

        public Ports()
        {
            for (byte i = 0; i <= 255; i++)
            {
                Port port = new Port(i);
                _ports.Add(i, port);
            }
        }

        public Ports(IPort[] ports)
        {
            foreach(IPort port in ports)
            {
                _ports.Add(port.Number, port);
            }
        }
    }
}

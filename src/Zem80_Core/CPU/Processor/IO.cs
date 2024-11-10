namespace Zem80.Core.CPU
{
    public class IO : IIO
    {
        /*
            This class represents the IO pin state of the Z80. The pin values are read-only, but there are public methods to set various states
            as required by the processor. You cannot set or change the state of the IO pins directly. Please see the Processor class for details of how to 
            integrate emulated hardware devices via interrupts etc.

            --------------------------------------------
            
            A0..A15 (out): this is the 16 bit address-bus, used for addressing up to 64 KBytes of memory or as port number for communicating with other chips and hardware devices
            D0..D7 (in/out): the 8-bit data bus, the address bus says where to read or write something, and the data bus says what
            MREQ (out): the memory request pin is active when the CPU wants to perform a memory access
            IORQ (out): likewise the I/O request pin is active when the CPU wants to perform an I/O device access (via the special IN/OUT instructions of the Z80)
            RD (out): the read pin is used together with the MREQ or IORQ to identify a memory-read or IO-device-input operation
            WR (out): and this is for the opposite direction, a memory-write or IO-device-output
            M1 (out): machine cycle one, this pin is active during an opcode fetch machine cycle and can be used to differentiate an opcode fetch from a normal memory read operation
            WAIT (in): this pin is set to active by the system to inject a wait state into the CPU, the CPU will only check this pin during a read or write operation
            INT (in): this pin is set to active by the system to initiate an interrupt request cycle
            NMI (in): this pin is set to active by the system to initiate a non-maskable interrupt request cycle
            RESET (in): this pin is set to active by the system to perform a system reset 
        */

        private Processor _cpu;
        private byte _defaultDataBusValue = 0;

        public ushort ADDRESS_BUS { get; private set; }
        public bool A0 { get { return (ADDRESS_BUS & 0x01) != 0; } }
        public bool A1 { get { return (ADDRESS_BUS & 0x02) != 0; } }
        public bool A2 { get { return (ADDRESS_BUS & 0x04) != 0; } }
        public bool A3 { get { return (ADDRESS_BUS & 0x08) != 0; } }
        public bool A4 { get { return (ADDRESS_BUS & 0x10) != 0; } }
        public bool A5 { get { return (ADDRESS_BUS & 0x20) != 0; } }
        public bool A6 { get { return (ADDRESS_BUS & 0x40) != 0; } }
        public bool A7 { get { return (ADDRESS_BUS & 0x80) != 0; } }
        public bool A8 { get { return (ADDRESS_BUS & 0x100) != 0; } }
        public bool A9 { get { return (ADDRESS_BUS & 0x200) != 0; } }
        public bool A10 { get { return (ADDRESS_BUS & 0x400) != 0; } }
        public bool A11 { get { return (ADDRESS_BUS & 0x800) != 0; } }
        public bool A12 { get { return (ADDRESS_BUS & 0x1000) != 0; } }
        public bool A13 { get { return (ADDRESS_BUS & 0x2000) != 0; } }
        public bool A14 { get { return (ADDRESS_BUS & 0x4000) != 0; } }
        public bool A15 { get { return (ADDRESS_BUS & 0x8000) != 0; } }

        public byte DATA_BUS { get; private set; }
        public bool D0 { get { return (DATA_BUS & 0x01) != 0; } }
        public bool D1 { get { return (DATA_BUS & 0x02) != 0; } }
        public bool D2 { get { return (DATA_BUS & 0x04) != 0; } }
        public bool D3 { get { return (DATA_BUS & 0x08) != 0    ; } }
        public bool D4 { get { return (DATA_BUS & 0x10) != 0; } }
        public bool D5 { get { return (DATA_BUS & 0x20) != 0; } }
        public bool D6 { get { return (DATA_BUS & 0x40) != 0; } }
        public bool D7 { get { return (DATA_BUS & 0x80) != 0; } }

        public bool MREQ { get; private set; }
        public bool IORQ { get; private set; }
        public bool RD { get; private set; }
        public bool WR { get; private set; }
        public bool M1 { get; private set; }
        public bool WAIT { get; private set; }
        public bool INT { get; private set; }
        public bool NMI { get; private set; }
        public bool RESET { get; private set; }

        public void Clear()
        {
            ADDRESS_BUS = 0;
            DATA_BUS = _defaultDataBusValue;
            MREQ = false;
            IORQ = false;
            RD = false;
            WR = false;
            M1 = false;
            WAIT = false;
            INT = false;
            NMI = false;
            RESET = false;
        }

        public void SetOpcodeFetchState(ushort address)
        {
            M1 = true;
            ADDRESS_BUS = address;
            MREQ = true;
            RD = true;
        }

        public void AddOpcodeFetchData(byte data)
        {
            DATA_BUS = data;
        }

        public void EndOpcodeFetchState()
        {
            MREQ = false;
            RD = false;
            M1 = false;
            DATA_BUS = _defaultDataBusValue;
        }

        public void SetMemoryReadState(ushort address)
        {
            MREQ = true;
            RD = true;
            ADDRESS_BUS = address;
        }

        public void AddMemoryData(byte data)
        {
            DATA_BUS = data;
        }

        public void EndMemoryReadState()
        {
            DATA_BUS = _defaultDataBusValue;
            MREQ = false;
            RD = false;
        }

        public void SetMemoryWriteState(ushort address, byte data)
        {
            MREQ = true;
            WR = true;
            ADDRESS_BUS = address;
            DATA_BUS = data;
        }

        public void EndMemoryWriteState()
        {
            DATA_BUS = _defaultDataBusValue;
            MREQ = false;
            WR = false;
        }

        public void SetPortReadState(ushort portAddress)
        {
            IORQ = true;
            RD = true;
            ADDRESS_BUS = portAddress;
        }

        public void AddPortReadData(byte data)
        {
            DATA_BUS = data;
        }

        public void EndPortReadState()
        {
            DATA_BUS = _defaultDataBusValue;
            IORQ = false;
            RD = false;
        }

        public void SetPortWriteState(ushort portAddress, byte data)
        {
            IORQ = true;
            WR = true;
            ADDRESS_BUS = portAddress;
            DATA_BUS = data;
        }

        public void EndPortWriteState()
        {
            DATA_BUS = _defaultDataBusValue;
            IORQ = false;
            WR = false;
        }

        public void SetInterruptState()
        {
            INT = true;
            M1 = true;
            IORQ = true;
        }

        public void EndInterruptState()
        {
            INT = false;
            M1 = false;
            IORQ = false;
        }

        public void SetNMIState()
        {
            NMI = true;
            M1 = true;
            IORQ = true;
        }

        public void EndNMIState()
        {
            NMI = false;
            M1 = false;
            IORQ = false;
        }

        public void SetWaitState()
        {
            WAIT = true;
        }

        public void EndWaitState()
        {
            WAIT = false;
        }

        public void SetResetState()
        {
            RESET = true;
        }

        public void SetAddressBusValue(ushort value)
        {
            ADDRESS_BUS = value;
        }

        public void SetDataBusValue(byte value)
        {
            DATA_BUS = value;
        }

        public void ResetDataBusValue()
        {
            DATA_BUS = _defaultDataBusValue;
        }

        public void SetDataBusDefaultValue(byte defaultValue)
        {
            _defaultDataBusValue = defaultValue;
        }

        public IO(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

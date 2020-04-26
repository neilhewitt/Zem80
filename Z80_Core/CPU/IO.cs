namespace Z80.Core
{
    public class IO
    {
        /*  
         *  A0..A15 (out): this is the 16 bit address-bus, used for addressing 64 KBytes of memory or as port number for communicating with other chips and hardware devices
            D0..D7 (in/out): the 8-bit data bus, the address bus says ‘where’ to read or write something, and the data bus says ‘what’
            MREQ (out): the ‘memory request’ pin is active when the CPU wants to perform a memory access
            IORQ (out): likewise the ‘I/O request’ pin is active when the CPU wants to perform an I/O device access (via the special IN/OUT instructions of the Z80)
            RD (out): the ‘read’ pin is used together with the MREQ or IORQ to identify a memory-read or IO-device-input operation
            WR (out): …and this is for the opposite direction, a memory-write or IO-device-output
            M1 (out): ‘machine cycle one’, this pin is active during an opcode fetch machine cycle and can be used to differentiate an opcode fetch from a normal memory read operation
            WAIT (in): this pin is set to active by the ‘system’ to inject a wait state into the CPU, the CPU will only check this pin during a read or write operation
            INT (in): this pin is set to active by the ‘system’ to initiate an interrupt- request cycle
            RESET (in): this pin is set to active by the ‘system’ to perform a system reset 
            */

        private Processor _cpu;

        public AddressBus ADDRESS_BUS { get; } = new AddressBus();
        public DataBus DATA_BUS { get; } = new DataBus();

        public ReadOnlyPin MREQ { get; } = new ReadOnlyPin();
        public ReadOnlyPin IORQ { get; } = new ReadOnlyPin();
        public ReadOnlyPin RD { get; } = new ReadOnlyPin();
        public ReadOnlyPin WR { get; } = new ReadOnlyPin();
        public ReadOnlyPin M1 { get; } = new ReadOnlyPin();
        public ReadWritePin WAIT { get; } = new ReadWritePin();
        public ReadWritePin INT { get; } = new ReadWritePin();
        public ReadWritePin NMI { get; } = new ReadWritePin();
        public ReadWritePin RESET { get; } = new ReadWritePin();

        public IO(Processor cpu)
        {
            _cpu = cpu;
        }
        
        public class ReadOnlyPin
        {
            public bool Value { get; internal set; }
        }

        public class ReadWritePin
        {
            public bool Value { get; set; }
        }
        
        public class DataBus
        {
            public byte Value { get; set; }
            public bool D0 { get { return Value.GetBit(0); } set { Value.SetBit(0, value); } }
            public bool D1 { get { return Value.GetBit(1); } set { Value.SetBit(1, value); } }
            public bool D2 { get { return Value.GetBit(2); } set { Value.SetBit(2, value); } }
            public bool D3 { get { return Value.GetBit(3); } set { Value.SetBit(3, value); } }
            public bool D4 { get { return Value.GetBit(4); } set { Value.SetBit(4, value); } }
            public bool D5 { get { return Value.GetBit(5); } set { Value.SetBit(5, value); } }
            public bool D6 { get { return Value.GetBit(6); } set { Value.SetBit(6, value); } }
            public bool D7 { get { return Value.GetBit(7); } set { Value.SetBit(7, value); } }
        }

        public class AddressBus
        {
            public ushort Value { get; internal set; }
            public bool A0 { get { return Value.GetBit(0); } }
            public bool A1 { get { return Value.GetBit(1); } }
            public bool A2 { get { return Value.GetBit(2); } }
            public bool A3 { get { return Value.GetBit(3); } }
            public bool A4 { get { return Value.GetBit(4); } }
            public bool A5 { get { return Value.GetBit(5); } }
            public bool A6 { get { return Value.GetBit(6); } }
            public bool A7 { get { return Value.GetBit(7); } }
            public bool A8 { get { return Value.GetBit(8); } }
            public bool A9 { get { return Value.GetBit(9); } }
            public bool A10 { get { return Value.GetBit(10); } }
            public bool A11 { get { return Value.GetBit(11); } }
            public bool A12 { get { return Value.GetBit(12); } }
            public bool A13 { get { return Value.GetBit(13); } }
            public bool A14 { get { return Value.GetBit(14); } }
            public bool A15 { get { return Value.GetBit(15); } }
        }
    }
}

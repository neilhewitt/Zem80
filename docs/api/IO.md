# I/O System

The Zem80_Core I/O system provides accurate simulation of the Z80's I/O pins and port operations, enabling emulation of hardware devices and external interfaces. The system includes pin-level simulation, port management, and hardware device connection capabilities.

## Core Interfaces and Classes

### IIO Interface

Provides access to the Z80's I/O pin states for hardware interfacing and debugging.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IIO
```

#### Address Bus Properties
```csharp
ushort ADDRESS_BUS { get; }  // Complete 16-bit address bus value
bool A0 { get; }   // Address line 0 (LSB)
bool A1 { get; }   // Address line 1
bool A2 { get; }   // Address line 2
bool A3 { get; }   // Address line 3
bool A4 { get; }   // Address line 4
bool A5 { get; }   // Address line 5
bool A6 { get; }   // Address line 6
bool A7 { get; }   // Address line 7
bool A8 { get; }   // Address line 8
bool A9 { get; }   // Address line 9
bool A10 { get; }  // Address line 10
bool A11 { get; }  // Address line 11
bool A12 { get; }  // Address line 12
bool A13 { get; }  // Address line 13
bool A14 { get; }  // Address line 14
bool A15 { get; }  // Address line 15 (MSB)
```

The 16-bit address bus is used for:
- Memory addressing (0x0000-0xFFFF)
- Port addressing (typically only A0-A7 used, A8-A15 may contain other data)
- Interrupt vector addressing

#### Data Bus Properties
```csharp
byte DATA_BUS { get; }  // Complete 8-bit data bus value
bool D0 { get; }  // Data line 0 (LSB)
bool D1 { get; }  // Data line 1
bool D2 { get; }  // Data line 2
bool D3 { get; }  // Data line 3
bool D4 { get; }  // Data line 4
bool D5 { get; }  // Data line 5
bool D6 { get; }  // Data line 6
bool D7 { get; }  // Data line 7 (MSB)
```

The 8-bit data bus carries:
- Data to/from memory
- Data to/from I/O ports
- Instruction opcodes during fetch
- Interrupt vectors

#### Control Signal Properties
```csharp
bool MREQ { get; }   // Memory Request - active during memory operations
bool IORQ { get; }   // I/O Request - active during port operations and interrupts
bool RD { get; }     // Read - active during read operations
bool WR { get; }     // Write - active during write operations
bool M1 { get; }     // Machine cycle 1 - active during instruction/interrupt fetch
bool INT { get; }    // Interrupt - maskable interrupt request
bool NMI { get; }    // Non-Maskable Interrupt - priority interrupt
bool RESET { get; }  // Reset - processor reset state
bool WAIT { get; }   // Wait - external wait request
```

#### Pin State Management Methods

##### Clear
```csharp
void Clear()
```
Resets all I/O pins to their default (inactive) states.

**Example:**
```csharp
processor.IO.Clear(); // All pins set to inactive state
```

##### Bus Control Methods

###### SetAddressBusValue
```csharp
void SetAddressBusValue(ushort value)
```
Sets the 16-bit address bus value.

###### SetDataBusValue / ResetDataBusValue
```csharp
void SetDataBusValue(byte value)
void ResetDataBusValue()
void SetDataBusDefaultValue(byte defaultValue)
```
Controls data bus value and default state.

##### Memory Operation State Methods

###### SetMemoryReadState
```csharp
void SetMemoryReadState(ushort address)
```
Activates memory read state (MREQ=1, RD=1, address on bus).

###### AddMemoryData
```csharp
void AddMemoryData(byte data)
```
Places data on the data bus during memory read.

###### EndMemoryReadState
```csharp
void EndMemoryReadState()
```
Deactivates memory read state and resets data bus.

###### SetMemoryWriteState
```csharp
void SetMemoryWriteState(ushort address, byte data)
```
Activates memory write state (MREQ=1, WR=1, address and data on buses).

###### EndMemoryWriteState
```csharp
void EndMemoryWriteState()
```
Deactivates memory write state.

##### I/O Port Operation State Methods

###### SetPortReadState
```csharp
void SetPortReadState(ushort portAddress)
```
Activates port read state (IORQ=1, RD=1, port address on bus).

###### AddPortReadData
```csharp
void AddPortReadData(byte data)
```
Places data on the data bus during port read.

###### EndPortReadState
```csharp
void EndPortReadState()
```
Deactivates port read state.

###### SetPortWriteState
```csharp
void SetPortWriteState(ushort portAddress, byte data)
```
Activates port write state (IORQ=1, WR=1, address and data on buses).

###### EndPortWriteState
```csharp
void EndPortWriteState()
```
Deactivates port write state.

##### Instruction and Interrupt State Methods

###### SetOpcodeFetchState
```csharp
void SetOpcodeFetchState(ushort address)
```
Activates instruction fetch state (M1=1, MREQ=1, RD=1).

###### AddOpcodeFetchData
```csharp
void AddOpcodeFetchData(byte data)
```
Places instruction opcode on data bus.

###### EndOpcodeFetchState
```csharp
void EndOpcodeFetchState()
```
Deactivates instruction fetch state.

###### SetInterruptState / EndInterruptState
```csharp
void SetInterruptState()
void EndInterruptState()
```
Controls maskable interrupt acknowledge state.

###### SetNMIState / EndNMIState
```csharp
void SetNMIState()
void EndNMIState()
```
Controls non-maskable interrupt state.

##### Wait and Reset State Methods

###### SetWaitState / EndWaitState
```csharp
void SetWaitState()
void EndWaitState()
```
Controls wait state for external timing control.

###### SetResetState
```csharp
void SetResetState()
```
Activates reset state for processor initialization.

### IPorts Interface

Manages the collection of I/O ports (0-255).

#### Namespace
`Zem80.Core.InputOutput`

#### Declaration
```csharp
public interface IPorts
```

#### Properties

##### Indexer
```csharp
IPort this[byte portNumber] { get; }
```
Provides access to a specific port by number.

**Example:**
```csharp
IPort keyboardPort = processor.Ports[0xFE]; // ZX Spectrum keyboard port
```

#### Methods

##### DisconnectAll
```csharp
void DisconnectAll()
```
Disconnects all hardware devices from all ports.

**Example:**
```csharp
processor.Ports.DisconnectAll(); // Remove all connected devices
```

### IPort Interface

Represents a single I/O port for hardware device connection.

#### Namespace
`Zem80.Core.InputOutput`

#### Declaration
```csharp
public interface IPort
```

#### Properties

##### Number
```csharp
byte Number { get; }
```
The port number (0-255).

#### Methods

##### Connect
```csharp
void Connect(Func<byte> reader, Action<byte> writer, Action signalRead, Action signalWrite)
```
Connects hardware device functions to the port.

**Parameters:**
- `reader`: Function called when CPU reads from port (returns byte value)
- `writer`: Action called when CPU writes to port (receives byte value)
- `signalRead`: Action called before read operation (for device preparation)
- `signalWrite`: Action called before write operation (for device preparation)

**Example:**
```csharp
// Connect a simple LED display to port 0x01
processor.Ports[0x01].Connect(
    reader: () => ledStatus,              // Reading returns LED status
    writer: (value) => SetLEDs(value),    // Writing controls LED pattern
    signalRead: () => Console.WriteLine("Reading LEDs"),
    signalWrite: () => Console.WriteLine("Writing LEDs")
);
```

##### Disconnect
```csharp
void Disconnect()
```
Removes all hardware device connections from this port.

##### ReadByte
```csharp
byte ReadByte(bool bc)
```
Reads a byte from the port (internal use by processor).

**Parameters:**
- `bc`: True if using BC register for port addressing, false for direct addressing

##### WriteByte
```csharp
void WriteByte(byte output, bool bc)
```
Writes a byte to the port (internal use by processor).

**Parameters:**
- `output`: Byte value to write
- `bc`: True if using BC register for port addressing

##### SignalRead / SignalWrite
```csharp
void SignalRead()
void SignalWrite()
```
Signals the connected device that a read/write operation is about to occur.

## Concrete Implementations

### IO Class

Standard implementation of the IIO interface.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public class IO : IIO
```

#### Constructor
```csharp
public IO(Processor cpu)
```
Creates an I/O interface connected to the specified processor.

### Ports Class

Standard port collection implementation.

#### Namespace
`Zem80.Core.InputOutput`

#### Declaration
```csharp
public class Ports : IPorts
```

#### Constructor
```csharp
public Ports(IProcessorTiming timing)
```
Creates a collection of 256 ports (0-255) with timing support.

### Port Class

Standard port implementation.

#### Namespace
`Zem80.Core.InputOutput`

#### Declaration
```csharp
public class Port : IPort
```

#### Constructor
```csharp
public Port(byte number, IProcessorTiming timing)
```
Creates a port with the specified number and timing support.

## Usage Examples

### Basic Port I/O
```csharp
var processor = new Processor();

// Connect a simple output device to port 0x10
processor.Ports[0x10].Connect(
    reader: null, // No read function needed
    writer: (value) => Console.WriteLine($"Output device received: 0x{value:X2}"),
    signalRead: null,
    signalWrite: () => Console.WriteLine("Device ready for output")
);

// Load program that outputs to port
byte[] program = {
    0x3E, 0x42,       // LD A, 0x42
    0xD3, 0x10,       // OUT (0x10), A
    0x76              // HALT
};

processor.Memory.WriteBytesAt(0x0000, program);
processor.Start(endOnHalt: true);
processor.RunUntilStopped();

// Output: "Device ready for output"
// Output: "Output device received: 0x42"
```

### Keyboard Input Simulation
```csharp
var processor = new Processor();
var keyboardState = new byte[8]; // 8 rows of keyboard matrix

// Connect keyboard port (ZX Spectrum style)
processor.Ports[0xFE].Connect(
    reader: () =>
    {
        // Read keyboard matrix based on address lines A8-A15
        ushort addressBus = processor.IO.ADDRESS_BUS;
        byte keyRow = addressBus.HighByte();
        
        byte result = 0xFF; // All keys released by default
        for (int row = 0; row < 8; row++)
        {
            if (!keyRow.GetBit(row)) // Row is selected (active low)
            {
                result &= keyboardState[row]; // AND with key state
            }
        }
        return result;
    },
    writer: null, // Keyboard is read-only
    signalRead: () => UpdateKeyboard(), // Update key state before read
    signalWrite: null
);

// Simulate pressing a key
keyboardState[0] = 0xFE; // Key in row 0, bit 0 pressed (active low)

// Program to read keyboard
byte[] program = {
    0x01, 0xFE, 0xFE, // LD BC, 0xFEFE (select row 0, port 0xFE)
    0xED, 0x78,       // IN A, (C)
    0x76              // HALT
};

processor.Memory.WriteBytesAt(0x0000, program);
processor.Start(endOnHalt: true);
processor.RunUntilStopped();

Console.WriteLine($"Key state read: 0x{processor.Registers.A:X2}"); // Should be 0xFE
```

### Hardware Device Emulation
```csharp
// Emulate a simple UART device
public class UARTDevice
{
    private Queue<byte> _receiveBuffer = new Queue<byte>();
    private Queue<byte> _transmitBuffer = new Queue<byte>();
    private byte _status = 0x00;
    
    public void Connect(IPorts ports)
    {
        // Data port
        ports[0x80].Connect(
            reader: () => _receiveBuffer.Count > 0 ? _receiveBuffer.Dequeue() : (byte)0x00,
            writer: (value) => _transmitBuffer.Enqueue(value),
            signalRead: null,
            signalWrite: null
        );
        
        // Status port
        ports[0x81].Connect(
            reader: () =>
            {
                byte status = 0x00;
                if (_receiveBuffer.Count > 0) status |= 0x01; // Receive ready
                if (_transmitBuffer.Count < 16) status |= 0x02; // Transmit ready
                return status;
            },
            writer: null, // Status is read-only
            signalRead: null,
            signalWrite: null
        );
    }
    
    public void SendByte(byte data) => _receiveBuffer.Enqueue(data);
    public byte[] GetTransmittedData() => _transmitBuffer.ToArray();
}

// Use the UART
var processor = new Processor();
var uart = new UARTDevice();
uart.Connect(processor.Ports);

// Send data to processor
uart.SendByte(0x48); // 'H'
uart.SendByte(0x65); // 'e'

// Program to read UART data
byte[] program = {
    0xDB, 0x81,       // IN A, (0x81) - check status
    0xE6, 0x01,       // AND 0x01 - test receive ready
    0x28, 0xFA,       // JR Z, -6 - wait for data
    0xDB, 0x80,       // IN A, (0x80) - read data
    0x76              // HALT
};

processor.Memory.WriteBytesAt(0x0000, program);
processor.Start(endOnHalt: true);
processor.RunUntilStopped();

Console.WriteLine($"Received: {(char)processor.Registers.A}"); // Should be 'H'
```

### I/O Pin Monitoring
```csharp
var processor = new Processor();

// Monitor I/O pin states during execution
processor.BeforeExecuteInstruction += (sender, package) =>
{
    var io = processor.IO;
    if (io.IORQ) // I/O operation
    {
        string operation = io.RD ? "READ" : io.WR ? "WRITE" : "UNKNOWN";
        Console.WriteLine($"I/O {operation} - Port: 0x{io.ADDRESS_BUS.LowByte():X2}, Data: 0x{io.DATA_BUS:X2}");
    }
    
    if (io.MREQ) // Memory operation  
    {
        string operation = io.RD ? "READ" : io.WR ? "WRITE" : "UNKNOWN";
        Console.WriteLine($"Memory {operation} - Address: 0x{io.ADDRESS_BUS:X4}, Data: 0x{io.DATA_BUS:X2}");
    }
    
    if (io.M1) // Instruction fetch
    {
        Console.WriteLine($"Instruction fetch - Address: 0x{io.ADDRESS_BUS:X4}, Opcode: 0x{io.DATA_BUS:X2}");
    }
};

// Load and run a simple program
byte[] program = {
    0x3E, 0x55,       // LD A, 0x55
    0xD3, 0x01,       // OUT (0x01), A
    0xDB, 0x02,       // IN A, (0x02)
    0x76              // HALT
};

processor.Memory.WriteBytesAt(0x0000, program);
processor.Start(endOnHalt: true);
processor.RunUntilStopped();
```

### Memory-Mapped I/O
```csharp
// Create custom memory segment that acts as I/O
public class MemoryMappedIOSegment : IMemorySegment
{
    private readonly IPorts _ports;
    private readonly byte _basePort;
    
    public MemoryMappedIOSegment(IPorts ports, byte basePort)
    {
        _ports = ports;
        _basePort = basePort;
    }
    
    public byte ReadByteAt(ushort offset)
    {
        // Memory read becomes port read
        byte portNum = (byte)(_basePort + (offset & 0xFF));
        return _ports[portNum].ReadByte(false);
    }
    
    public void WriteByteAt(ushort offset, byte value)
    {
        // Memory write becomes port write
        byte portNum = (byte)(_basePort + (offset & 0xFF));
        _ports[portNum].WriteByte(value, false);
    }
    
    // Implement other IMemorySegment members...
    public ushort StartAddress { get; private set; }
    public uint SizeInBytes => 256;
    public bool ReadOnly => false;
    public void MapAt(ushort address) => StartAddress = address;
    public void Clear() { }
}

// Use memory-mapped I/O
var processor = new Processor();
var mmio = new MemoryMappedIOSegment(processor.Ports, 0x80);

// Map I/O into memory space
var memoryMap = new MemoryMap(65536);
memoryMap.Map(mmio, 0xC000); // I/O appears at 0xC000-0xC0FF

// Connect device to port
processor.Ports[0x80].Connect(
    reader: () => 0x42,
    writer: (value) => Console.WriteLine($"MMIO write: 0x{value:X2}"),
    signalRead: null,
    signalWrite: null
);

// Access I/O through memory operations
byte value = processor.Memory.ReadByteAt(0xC000);  // Reads from port 0x80
processor.Memory.WriteByteAt(0xC000, 0x33);       // Writes to port 0x80
```

### Interrupt-Driven I/O
```csharp
var processor = new Processor();

// Set up interrupt-generating I/O device
bool dataReady = false;
byte incomingData = 0x00;

processor.Ports[0x20].Connect(
    reader: () => incomingData,
    writer: null,
    signalRead: () => dataReady = false, // Clear interrupt on read
    signalWrite: null
);

// Interrupt handler at 0x0038 (IM1 mode)
byte[] interruptHandler = {
    0xF5,             // PUSH AF
    0xDB, 0x20,       // IN A, (0x20)
    0x32, 0x00, 0x90, // LD (0x9000), A  - store received data
    0xF1,             // POP AF
    0xFB,             // EI
    0xED, 0x4D        // RETI
};
processor.Memory.WriteBytesAt(0x0038, interruptHandler);

// Main program
byte[] program = {
    0xFB,             // EI - enable interrupts
    0x76              // HALT - wait for interrupt
};
processor.Memory.WriteBytesAt(0x0000, program);

// Start processor with interrupts
processor.Start(0x0000, false, InterruptMode.IM1);
processor.Interrupts.Enable();

// Simulate data arrival and interrupt
Task.Run(async () =>
{
    await Task.Delay(100);
    incomingData = 0x48; // 'H'
    dataReady = true;
    processor.Interrupts.TriggerMaskableInterrupt();
});

processor.RunUntilStopped();

// Check if interrupt was handled
byte storedData = processor.Memory.ReadByteAt(0x9000);
Console.WriteLine($"Interrupt handled, data stored: {(char)storedData}");
```

## Z80 I/O Addressing Modes

The Z80 supports two I/O addressing modes:

### Direct Port Addressing
Used by IN A,(n) and OUT (n),A instructions.
- Port address in instruction (8-bit)
- Full 16-bit address bus contains: A15-A8=A register, A7-A0=port address

### Indirect Port Addressing  
Used by IN r,(C) and OUT (C),r instructions.
- Port address in C register (8-bit)
- Full 16-bit address bus contains: A15-A8=B register, A7-A0=C register

**Example:**
```csharp
// Monitor addressing mode differences
processor.BeforeExecuteInstruction += (sender, package) =>
{
    if (package.Instruction.Mnemonic.Contains("OUT") && processor.IO.IORQ)
    {
        ushort fullAddress = processor.IO.ADDRESS_BUS;
        byte portLow = fullAddress.LowByte();
        byte portHigh = fullAddress.HighByte();
        
        if (package.Instruction.Mnemonic.Contains("(C)"))
        {
            Console.WriteLine($"Indirect: Port={portLow:X2}, B={portHigh:X2}");
        }
        else
        {
            Console.WriteLine($"Direct: Port={portLow:X2}, A={portHigh:X2}");
        }
    }
};
```

## Performance Considerations

- **Pin state access**: Reading pin states is optimized for debugging/monitoring
- **Port connections**: Use lightweight delegates for device connections
- **Timing simulation**: I/O timing adds overhead; disable if not needed for accuracy
- **Device complexity**: Keep connected device logic simple for better performance

## Thread Safety

The I/O system shares the same thread safety characteristics as the processor:

- **Safe operations**: Reading pin states for monitoring
- **Unsafe operations**: Modifying port connections while processor is running
- **Device callbacks**: Must be thread-safe if accessed from multiple threads

## See Also

- [Processor](./Processor.md) - Processor class that manages the I/O system
- [Memory](./Memory.md) - Memory system for memory-mapped I/O scenarios
- [Instructions](./Instructions.md) - I/O instruction implementations
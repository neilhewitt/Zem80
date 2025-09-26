# Memory System

The Zem80_Core memory system provides flexible memory management for Z80 emulation with support for different memory types (RAM, ROM), memory banking, and custom memory implementations. The system is designed to be both accurate to Z80 behavior and extensible for advanced use cases.

## Core Interfaces and Classes

### IMemoryBank Interface

The main interface for accessing the Z80's memory space.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public interface IMemoryBank
```

#### Properties

##### SizeInBytes
```csharp
uint SizeInBytes { get; }
```
Total size of the memory bank in bytes (typically 65536 for full Z80 addressing).

#### Methods

##### Clear
```csharp
void Clear()
```
Clears all writable memory segments to zero. Read-only segments are not affected.

**Example:**
```csharp
memory.Clear(); // All RAM cleared, ROM unchanged
```

##### Initialise
```csharp
void Initialise(Processor cpu, IMemoryMap map)
```
Initializes the memory bank with a processor reference and memory map configuration.

**Parameters:**
- `cpu`: The processor that will use this memory bank
- `map`: Memory map defining the memory layout

##### Single Byte Operations

###### ReadByteAt
```csharp
byte ReadByteAt(ushort address, byte? tStates = null)
```
Reads a single byte from memory at the specified address.

**Parameters:**
- `address`: Memory address to read from (0x0000-0xFFFF)
- `tStates`: Optional timing information for cycle-accurate emulation

**Returns:** Byte value at the address (0x00 if address is unmapped)

**Example:**
```csharp
byte value = memory.ReadByteAt(0x1000);
// Read with timing
byte timedValue = memory.ReadByteAt(0x2000, 3); // 3 T-states
```

###### WriteByteAt
```csharp
void WriteByteAt(ushort address, byte value, byte? tStates = null)
```
Writes a single byte to memory at the specified address.

**Parameters:**
- `address`: Memory address to write to
- `value`: Byte value to write
- `tStates`: Optional timing information

**Note:** Writes to read-only memory are silently ignored.

**Example:**
```csharp
memory.WriteByteAt(0x8000, 0x42);
// Write with timing
memory.WriteByteAt(0x8001, 0x43, 3); // 3 T-states
```

##### Multi-Byte Operations

###### ReadBytesAt
```csharp
byte[] ReadBytesAt(ushort address, ushort numberOfBytes, byte? tStatesPerByte = null)
```
Reads multiple consecutive bytes from memory.

**Parameters:**
- `address`: Starting address
- `numberOfBytes`: Number of bytes to read
- `tStatesPerByte`: Optional timing per byte

**Returns:** Array of bytes read from memory

**Example:**
```csharp
// Read 16 bytes starting at 0x1000
byte[] data = memory.ReadBytesAt(0x1000, 16);

// Read with timing (3 T-states per byte)
byte[] timedData = memory.ReadBytesAt(0x2000, 8, 3);
```

###### WriteBytesAt
```csharp
void WriteBytesAt(ushort address, byte[] bytes, byte? tStatesPerByte = null)
```
Writes multiple consecutive bytes to memory.

**Parameters:**
- `address`: Starting address
- `bytes`: Array of bytes to write
- `tStatesPerByte`: Optional timing per byte

**Example:**
```csharp
byte[] program = { 0x3E, 0x42, 0x76 }; // LD A,42h; HALT
memory.WriteBytesAt(0x0000, program);

// Write with timing
memory.WriteBytesAt(0x1000, data, 3); // 3 T-states per byte
```

##### Word Operations

###### ReadWordAt
```csharp
ushort ReadWordAt(ushort address, byte? tStatesPerByte = null)
```
Reads a 16-bit word from memory using little-endian byte order.

**Parameters:**
- `address`: Address of the low byte
- `tStatesPerByte`: Optional timing per byte

**Returns:** 16-bit word (low byte at address, high byte at address+1)

**Example:**
```csharp
ushort word = memory.ReadWordAt(0x1000);
// If memory[0x1000] = 0x34 and memory[0x1001] = 0x12, returns 0x1234
```

###### WriteWordAt
```csharp
void WriteWordAt(ushort address, ushort value, byte? tStatesPerByte = null)
```
Writes a 16-bit word to memory using little-endian byte order.

**Parameters:**
- `address`: Address to write the low byte
- `value`: 16-bit value to write
- `tStatesPerByte`: Optional timing per byte

**Example:**
```csharp
memory.WriteWordAt(0x1000, 0x1234);
// Writes 0x34 to 0x1000 and 0x12 to 0x1001
```

### IMemoryMap Interface

Defines memory layout and segment mapping.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public interface IMemoryMap
```

#### Properties

##### SizeInBytes
```csharp
uint SizeInBytes { get; }
```
Total size of the addressable memory space.

#### Methods

##### Map
```csharp
void Map(IMemorySegment memory, ushort startAddress, bool overwriteMappedPages = false)
```
Maps a memory segment into the address space at the specified location.

**Parameters:**
- `memory`: Memory segment to map
- `startAddress`: Starting address for the segment (must be page-aligned)
- `overwriteMappedPages`: Whether to overwrite existing mappings

**Example:**
```csharp
var rom = new ReadOnlyMemorySegment(8192, romData); // 8KB ROM
var ram = new MemorySegment(32768); // 32KB RAM

map.Map(rom, 0x0000); // ROM at 0x0000-0x1FFF
map.Map(ram, 0x8000); // RAM at 0x8000-0xFFFF
```

##### ClearAllWritableMemory
```csharp
void ClearAllWritableMemory()
```
Clears all writable (non-ROM) memory segments to zero.

##### SegmentFor
```csharp
IMemorySegment SegmentFor(ushort address)
```
Returns the memory segment that contains the specified address.

**Parameters:**
- `address`: Address to look up

**Returns:** Memory segment containing the address, or null if unmapped

### IMemorySegment Interface

Represents a contiguous block of memory.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public interface IMemorySegment
```

#### Properties

##### StartAddress
```csharp
ushort StartAddress { get; }
```
Starting address where this segment is mapped.

##### SizeInBytes
```csharp
uint SizeInBytes { get; }
```
Size of the memory segment in bytes.

##### ReadOnly
```csharp
bool ReadOnly { get; }
```
Whether this segment is read-only (ROM) or writable (RAM).

#### Methods

##### ReadByteAt
```csharp
byte ReadByteAt(ushort offset)
```
Reads a byte at the specified offset within the segment.

**Parameters:**
- `offset`: Offset from the start of the segment

##### WriteByteAt
```csharp
void WriteByteAt(ushort offset, byte value)
```
Writes a byte at the specified offset within the segment.

**Parameters:**
- `offset`: Offset from the start of the segment
- `value`: Value to write

##### MapAt
```csharp
void MapAt(ushort address)
```
Sets the starting address for this segment.

##### Clear
```csharp
void Clear()
```
Clears the segment to all zeros (if writable).

## Concrete Implementations

### MemoryBank Class

Standard implementation of IMemoryBank.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public class MemoryBank : IMemoryBank
```

#### Constructor
```csharp
public MemoryBank()
```
Creates a new memory bank. Must be initialized before use.

### MemoryMap Class

Standard memory mapping implementation with page-based allocation.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public class MemoryMap : IMemoryMap
```

#### Constructor
```csharp
public MemoryMap(uint sizeInBytes, bool fillWithRAM = false)
```

**Parameters:**
- `sizeInBytes`: Total memory space size (typically 65536)
- `fillWithRAM`: If true, fills entire space with RAM segment

**Example:**
```csharp
// Create empty 64KB memory map
var map = new MemoryMap(65536);

// Create 64KB memory map filled with RAM
var ramMap = new MemoryMap(65536, true);
```

#### Memory Paging

The MemoryMap uses 1024-byte pages for efficient segment management:

- Page size: 1024 bytes
- Addresses must be page-aligned (multiple of 1024)
- Segments are mapped across one or more pages
- Unmapped pages return 0x00 on read, ignore writes

### MemorySegment Class

Standard RAM implementation.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public class MemorySegment : IMemorySegment
```

#### Constructors

##### Basic Constructor
```csharp
public MemorySegment(uint sizeInBytes)
```
Creates a new RAM segment with the specified size, initialized to zeros.

**Parameters:**
- `sizeInBytes`: Size of the segment (max 65536)

##### Pre-loaded Constructor
```csharp
protected MemorySegment(uint sizeInBytes, byte[] contents)
```
Creates a segment with pre-loaded content.

**Parameters:**
- `sizeInBytes`: Size of the segment
- `contents`: Initial content to copy into the segment

**Example:**
```csharp
// 32KB RAM segment
var ram = new MemorySegment(32768);

// Pre-loaded with data
byte[] bootCode = LoadFromFile("boot.bin");
var bootRom = new MemorySegment((uint)bootCode.Length, bootCode);
```

### ReadOnlyMemorySegment Class

ROM implementation that prevents writes.

#### Namespace
`Zem80.Core.Memory`

#### Declaration
```csharp
public class ReadOnlyMemorySegment : MemorySegment
```

#### Properties
```csharp
public override bool ReadOnly => true;
```

#### Constructor
```csharp
public ReadOnlyMemorySegment(uint sizeInBytes, byte[] contents)
```
Creates a read-only memory segment with the specified content.

**Parameters:**
- `sizeInBytes`: Size of the ROM segment
- `contents`: ROM content to load

**Example:**
```csharp
// Load ROM from file
byte[] romData = File.ReadAllBytes("system.rom");
var rom = new ReadOnlyMemorySegment((uint)romData.Length, romData);
```

## Usage Examples

### Basic Memory Setup
```csharp
// Create memory system
var memory = new MemoryBank();
var memoryMap = new MemoryMap(65536);

// Create and map segments
var rom = new ReadOnlyMemorySegment(8192, romData); // 8KB ROM
var ram = new MemorySegment(32768); // 32KB RAM

memoryMap.Map(rom, 0x0000); // ROM: 0x0000-0x1FFF
memoryMap.Map(ram, 0x8000); // RAM: 0x8000-0xFFFF

// Initialize processor with memory
var processor = new Processor(memory, memoryMap);
```

### Loading Programs
```csharp
// Load ROM image
byte[] biosRom = File.ReadAllBytes("bios.rom");
var rom = new ReadOnlyMemorySegment(16384, biosRom); // 16KB BIOS ROM

// Create RAM for programs
var ram = new MemorySegment(49152); // 48KB RAM

// Set up memory map (typical ZX Spectrum-like layout)
var map = new MemoryMap(65536);
map.Map(rom, 0x0000); // ROM: 0x0000-0x3FFF
map.Map(ram, 0x4000); // RAM: 0x4000-0xFFFF

var processor = new Processor(new MemoryBank(), map);

// Load a program into RAM
byte[] program = {
    0x21, 0x00, 0x80, // LD HL, 0x8000
    0x36, 0x42,       // LD (HL), 0x42
    0x76              // HALT
};
processor.Memory.WriteBytesAt(0x4000, program);
```

### Memory Inspection and Debugging
```csharp
var processor = new Processor();

// Load test program
byte[] testProgram = { 0x3E, 0xFF, 0x47, 0x76 }; // LD A,0xFF; LD B,A; HALT
processor.Memory.WriteBytesAt(0x0000, testProgram);

// Inspect memory before execution
Console.WriteLine("Program bytes:");
for (int i = 0; i < testProgram.Length; i++)
{
    byte value = processor.Memory.ReadByteAt((ushort)i);
    Console.WriteLine($"0x{i:X4}: 0x{value:X2}");
}

// Execute program
processor.Start(endOnHalt: true);
processor.RunUntilStopped();

// Check results
Console.WriteLine($"A = 0x{processor.Registers.A:X2}");
Console.WriteLine($"B = 0x{processor.Registers.B:X2}");
```

### Custom Memory Implementation
```csharp
// Custom banked memory for advanced systems
public class BankedMemory : IMemoryBank
{
    private MemoryBank[] _banks = new MemoryBank[4];
    private int _currentBank = 0;
    
    public void SwitchBank(int bankNumber)
    {
        if (bankNumber >= 0 && bankNumber < _banks.Length)
            _currentBank = bankNumber;
    }
    
    public byte ReadByteAt(ushort address, byte? tStates = null)
    {
        return _banks[_currentBank].ReadByteAt(address, tStates);
    }
    
    // ... implement other IMemoryBank methods
}

// Use custom memory
var bankedMemory = new BankedMemory();
var processor = new Processor(bankedMemory);
```

### Memory-Mapped I/O
```csharp
// Custom memory segment that handles I/O operations
public class MemoryMappedIO : IMemorySegment
{
    private readonly IPort _ioPort;
    
    public MemoryMappedIO(IPort ioPort)
    {
        _ioPort = ioPort;
    }
    
    public byte ReadByteAt(ushort offset)
    {
        // Reading from this memory segment actually reads from an I/O port
        return _ioPort.Read();
    }
    
    public void WriteByteAt(ushort offset, byte value)
    {
        // Writing to this memory segment actually writes to an I/O port
        _ioPort.Write(value);
    }
    
    // ... implement other IMemorySegment methods
}

// Map I/O device into memory space
var ioDevice = new Port();
var mmio = new MemoryMappedIO(ioDevice);
memoryMap.Map(mmio, 0xC000); // I/O device appears at 0xC000-0xC3FF
```

### Performance-Optimized Memory Access
```csharp
// Bulk memory operations for better performance
var processor = new Processor();

// Load large program efficiently
byte[] largeProgram = File.ReadAllBytes("large_program.bin");
processor.Memory.WriteBytesAt(0x1000, largeProgram); // Single operation

// Efficient memory copying
byte[] sourceData = processor.Memory.ReadBytesAt(0x2000, 1024);
processor.Memory.WriteBytesAt(0x3000, sourceData);

// Word-based operations for 16-bit data
ushort[] addresses = { 0x1000, 0x1234, 0x5678 };
foreach (ushort addr in addresses)
{
    processor.Memory.WriteWordAt(0x8000, addr); // Store addresses as little-endian words
}
```

## Memory Layout Examples

### ZX Spectrum-style Memory Map
```csharp
var map = new MemoryMap(65536);

// ROM (16KB)
var rom = new ReadOnlyMemorySegment(16384, spectrumRom);
map.Map(rom, 0x0000); // 0x0000-0x3FFF

// Screen RAM (6.75KB)
var screenRam = new MemorySegment(6912);
map.Map(screenRam, 0x4000); // 0x4000-0x5AFF

// Available RAM (41KB+)
var ram = new MemorySegment(41984);
map.Map(ram, 0x5B00); // 0x5B00-0xFFFF
```

### CP/M-style Memory Map
```csharp
var map = new MemoryMap(65536);

// System area (256 bytes)
var systemRam = new MemorySegment(256);
map.Map(systemRam, 0x0000); // 0x0000-0x00FF

// User program area (65KB-)
var userRam = new MemorySegment(65280);
map.Map(userRam, 0x0100); // 0x0100-0xFFFF
```

## Performance Considerations

- **Page-based mapping**: 1KB pages provide good balance between granularity and efficiency
- **Direct array access**: Memory segments use byte arrays for optimal access speed
- **Bulk operations**: ReadBytesAt/WriteBytesAt are more efficient for large transfers
- **Memory pooling**: Consider reusing MemorySegment objects for dynamic scenarios
- **Timing overhead**: T-state timing adds overhead; use only when cycle accuracy is required

## Thread Safety

The memory system is **not** inherently thread-safe. When the processor is running:

- **Safe operations**: Reading memory for inspection/debugging
- **Unsafe operations**: Writing to memory (except through processor events)
- **Segment operations**: Mapping/unmapping segments while processor is running

For thread-safe inspection, suspend the processor before accessing memory directly.

## Exception Handling

The memory system throws specific exceptions for error conditions:

- **MemoryNotInitialisedException**: Memory bank used before initialization
- **MemoryMapException**: Invalid memory mapping operation
- **MemorySegmentException**: Invalid segment size or configuration
- **MemoryNotWritableException**: Attempt to write to read-only memory
- **MemoryNotPresentException**: Access to unmapped memory address

## See Also

- [Processor](./Processor.md) - Processor class that uses the memory system
- [IO](./IO.md) - I/O system for hardware interfacing
- [Exceptions](./Exceptions.md) - Memory-related exception types
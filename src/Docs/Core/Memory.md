# Memory System - Memory Banks and Mapping

[? Back to Documentation Home](../README.md) | [? Core Components](../README.md#core-components)

The Zem80 Core memory system provides flexible memory management with support for different memory types, protection levels, and custom memory implementations. The system is designed to accurately emulate Z80 memory access patterns while providing modern flexibility for various emulation scenarios.

## Overview

The memory system consists of:
- **Memory Banks** - Main memory storage and access interface
- **Memory Maps** - Define memory layout and segment properties
- **Memory Segments** - Individual memory regions with specific characteristics
- **Memory Protection** - Read-only and read-write controls
- **Timing Integration** - Accurate memory access timing simulation

## Core Interfaces

### IMemoryBank Interface

The primary interface for memory operations:

```csharp
public interface IMemoryBank
{
    uint SizeInBytes { get; }

    // Single byte operations
    byte ReadByteAt(ushort address, byte? tStates = null);
    void WriteByteAt(ushort address, byte value, byte? tStates = null);

    // 16-bit word operations (little-endian)
    ushort ReadWordAt(ushort address, byte? tStatesPerByte = null);
    void WriteWordAt(ushort address, ushort value, byte? tStatesPerByte = null);

    // Bulk operations
    byte[] ReadBytesAt(ushort address, ushort numberOfBytes, byte? tStatesPerByte = null);
    void WriteBytesAt(ushort address, byte[] bytes, byte? tStatesPerByte = null);

    // Memory management
    void Clear();
    void Initialise(Processor cpu, IMemoryMap map);
}
```

### IMemoryMap Interface

Defines memory layout and segment management:

```csharp
public interface IMemoryMap
{
    uint SizeInBytes { get; }
    IMemorySegment SegmentFor(ushort address);
    void ClearAllWritableMemory();
}
```

### IMemorySegment Interface

Represents individual memory regions:

```csharp
public interface IMemorySegment
{
    ushort StartAddress { get; }
    uint SizeInBytes { get; }
    bool ReadOnly { get; }
    
    byte ReadByteAt(ushort offset);
    void WriteByteAt(ushort offset, byte value);
}
```

## Memory Bank Implementation

### Basic Usage

```csharp
// Create processor with default 64KB memory
var processor = new Processor();
var memory = processor.Memory;

// Basic read/write operations
memory.WriteByteAt(0x8000, 0x42);
byte value = memory.ReadByteAt(0x8000);

// 16-bit operations (little-endian)
memory.WriteWordAt(0x8000, 0x1234);
ushort word = memory.ReadWordAt(0x8000);
// Memory will contain: [0x8000] = 0x34, [0x8001] = 0x12

// Bulk operations
byte[] data = { 0x01, 0x02, 0x03, 0x04 };
memory.WriteBytesAt(0x9000, data);
byte[] readData = memory.ReadBytesAt(0x9000, 4);
```

### Memory with Timing

```csharp
// Memory operations with T-state timing
byte value = memory.ReadByteAt(0x8000, tStates: 3);
memory.WriteByteAt(0x8000, 0x42, tStates: 3);

// Bulk operations with timing
byte[] data = memory.ReadBytesAt(0x8000, 256, tStatesPerByte: 3);
memory.WriteBytesAt(0x8000, data, tStatesPerByte: 3);
```

## Memory Mapping

### Default Memory Map

```csharp
// Default configuration: 64KB of read-write memory
var memoryMap = new MemoryMap(65536, allowWrite: true);
var memoryBank = new MemoryBank();
memoryBank.Initialise(processor, memoryMap);
```

### Custom Memory Layout

```csharp
// Create custom memory map
var memoryMap = new MemoryMap(65536, false); // No default segment

// Add ROM region (read-only)
var romSegment = new ReadOnlyMemorySegment(0x0000, romData);
memoryMap.AddSegment(romSegment);

// Add RAM region (read-write)  
var ramSegment = new MemorySegment(0x8000, 32768, readOnly: false);
memoryMap.AddSegment(ramSegment);

// Initialize memory bank
var memoryBank = new MemoryBank();
memoryBank.Initialise(processor, memoryMap);
```

## Memory Segments

### Standard Memory Segment

Provides read-write memory storage:

```csharp
public class MemorySegment : IMemorySegment
{
    public ushort StartAddress { get; }
    public uint SizeInBytes { get; }
    public bool ReadOnly { get; }

    public MemorySegment(ushort startAddress, uint sizeInBytes, bool readOnly = false)
    {
        StartAddress = startAddress;
        SizeInBytes = sizeInBytes;
        ReadOnly = readOnly;
    }
}
```

### Read-Only Memory Segment

For ROM, firmware, or protected memory regions:

```csharp
public class ReadOnlyMemorySegment : IMemorySegment
{
    public bool ReadOnly => true;

    public ReadOnlyMemorySegment(ushort startAddress, byte[] data)
    {
        StartAddress = startAddress;
        Data = data;
        SizeInBytes = (uint)data.Length;
    }
}
```

### Usage Examples

```csharp
// Create ROM segment with BIOS data
byte[] biosData = LoadBiosData();
var biosRom = new ReadOnlyMemorySegment(0x0000, biosData);

// Create RAM segment
var systemRam = new MemorySegment(0x4000, 32768, readOnly: false);

// Create video RAM (could be custom implementation)
var videoRam = new VideoMemorySegment(0xC000, 16384);
```

## Memory Protection and Error Handling

### Memory Exceptions

The memory system defines several exception types:

```csharp
// Base exception for memory operations
public class MemoryException : Z80Exception

// Specific memory exceptions
public class MemoryNotPresentException : MemoryException
public class MemoryNotWritableException : MemoryException
public class MemoryNotInitialisedException : MemoryException
public class MemorySegmentException : MemoryException
```

### Handling Memory Errors

```csharp
try
{
    // Attempt to write to ROM region
    memory.WriteByteAt(0x0000, 0x42);
}
catch (MemoryNotWritableException ex)
{
    Console.WriteLine($"Cannot write to ROM at address {ex.Address:X4}");
}

try
{
    // Read from unmapped memory
    byte value = memory.ReadByteAt(0xFFFF);
}
catch (MemoryNotPresentException ex)
{
    Console.WriteLine($"No memory present at address {ex.Address:X4}");
    // Returns 0x00 by default
}
```

## Advanced Memory Scenarios

### Bank Switching Implementation

```csharp
public class BankSwitchedMemory : IMemoryBank
{
    private IMemoryBank[] banks = new IMemoryBank[4];
    private int currentBank = 0;

    public byte ReadByteAt(ushort address, byte? tStates = null)
    {
        if (address >= 0xC000) // Bank-switched region
        {
            return banks[currentBank].ReadByteAt((ushort)(address - 0xC000), tStates);
        }
        return baseMemory.ReadByteAt(address, tStates);
    }

    public void SwitchBank(int bankNumber)
    {
        if (bankNumber >= 0 && bankNumber < banks.Length)
        {
            currentBank = bankNumber;
        }
    }
}
```

### Memory-Mapped I/O

```csharp
public class MemoryMappedIO : IMemorySegment
{
    public bool ReadOnly => false;

    public byte ReadByteAt(ushort offset)
    {
        // Read from I/O device based on offset
        return ioDevice.ReadRegister(offset);
    }

    public void WriteByteAt(ushort offset, byte value)
    {
        // Write to I/O device register
        ioDevice.WriteRegister(offset, value);
    }
}
```

### Paged Memory System

```csharp
public class PagedMemoryMap : IMemoryMap
{
    private IMemorySegment[,] pages; // [page][bank]
    private int[] currentBanks;

    public IMemorySegment SegmentFor(ushort address)
    {
        int page = address / pageSize;
        int bank = currentBanks[page];
        return pages[page, bank];
    }

    public void SwitchPageBank(int page, int bank)
    {
        currentBanks[page] = bank;
    }
}
```

## Memory Initialization Patterns

### Loading ROM Data

```csharp
// Load ROM from file
byte[] romData = File.ReadAllBytes("system.rom");
var romSegment = new ReadOnlyMemorySegment(0x0000, romData);

// Load ROM from embedded resource
var assembly = Assembly.GetExecutingAssembly();
using var stream = assembly.GetManifestResourceStream("MyEmulator.bios.rom");
byte[] biosData = new byte[stream.Length];
stream.Read(biosData, 0, (int)stream.Length);
var biosRom = new ReadOnlyMemorySegment(0x0000, biosData);
```

### Setting Up Memory Layout

```csharp
public void SetupZX80MemoryLayout(Processor processor)
{
    var memoryMap = new MemoryMap(65536, false);

    // ROM: 0x0000-0x0FFF (4KB)
    var rom = new ReadOnlyMemorySegment(0x0000, zx80RomData);
    memoryMap.AddSegment(rom);

    // RAM: 0x4000-0x43FF (1KB)
    var ram = new MemorySegment(0x4000, 1024, false);
    memoryMap.AddSegment(ram);

    // Video RAM: 0x4400-0x47FF (1KB)
    var videoRam = new VideoMemorySegment(0x4400, 1024);
    memoryMap.AddSegment(videoRam);

    processor.Memory.Initialise(processor, memoryMap);
}
```

## Performance Considerations

### Memory Access Optimization

```csharp
// Efficient bulk operations
byte[] program = LoadProgram();
memory.WriteBytesAt(0x8000, program); // Single operation

// vs. inefficient loop
for (int i = 0; i < program.Length; i++)
{
    memory.WriteByteAt((ushort)(0x8000 + i), program[i]); // Multiple operations
}
```

### Memory Segment Caching

```csharp
public class CachedMemoryBank : IMemoryBank
{
    private IMemorySegment lastSegment;
    private ushort lastAddress;

    public byte ReadByteAt(ushort address, byte? tStates = null)
    {
        // Cache segment lookup for sequential access
        if (lastSegment == null || !IsInSegment(address, lastSegment))
        {
            lastSegment = memoryMap.SegmentFor(address);
        }
        
        return lastSegment.ReadByteAt(CalculateOffset(address, lastSegment));
    }
}
```

## Debugging Memory Operations

### Memory Access Tracing

```csharp
public class TracingMemoryBank : IMemoryBank
{
    private readonly IMemoryBank baseMemory;
    private readonly List<MemoryAccess> accessLog = new();

    public byte ReadByteAt(ushort address, byte? tStates = null)
    {
        byte value = baseMemory.ReadByteAt(address, tStates);
        accessLog.Add(new MemoryAccess
        {
            Type = AccessType.Read,
            Address = address,
            Value = value,
            Timestamp = DateTime.Now
        });
        return value;
    }

    public void WriteByteAt(ushort address, byte value, byte? tStates = null)
    {
        baseMemory.WriteByteAt(address, value, tStates);
        accessLog.Add(new MemoryAccess
        {
            Type = AccessType.Write,
            Address = address,
            Value = value,
            Timestamp = DateTime.Now
        });
    }
}
```

## Example: Complete Memory Configuration

```csharp
using Zem80.Core.CPU;
using Zem80.Core.Memory;

public class GameConsoleEmulator
{
    public void SetupMemory(Processor processor)
    {
        var memoryMap = new MemoryMap(65536, false);

        // System ROM (32KB)
        byte[] systemRom = LoadSystemRom();
        var romSegment = new ReadOnlyMemorySegment(0x0000, systemRom);
        memoryMap.AddSegment(romSegment);

        // Work RAM (8KB)
        var workRam = new MemorySegment(0x8000, 8192, false);
        memoryMap.AddSegment(workRam);

        // Video RAM (16KB)
        var videoRam = new MemorySegment(0xA000, 16384, false);
        memoryMap.AddSegment(videoRam);

        // I/O Region (256 bytes, memory-mapped)
        var ioRegion = new MemoryMappedIO(0xFF00, hardwareController);
        memoryMap.AddSegment(ioRegion);

        // Initialize memory bank
        processor.Memory.Initialise(processor, memoryMap);

        // Load game cartridge
        LoadCartridge(processor, gameRomData);
    }

    private void LoadCartridge(Processor processor, byte[] cartridgeData)
    {
        // Cartridges load into specific memory region
        processor.Memory.WriteBytesAt(0x8000, cartridgeData);
    }
}
```

## Thread Safety

The memory system is **not inherently thread-safe**. Consider these patterns:

```csharp
// Thread-safe memory wrapper
public class ThreadSafeMemoryBank : IMemoryBank
{
    private readonly IMemoryBank baseMemory;
    private readonly object lockObject = new object();

    public byte ReadByteAt(ushort address, byte? tStates = null)
    {
        lock (lockObject)
        {
            return baseMemory.ReadByteAt(address, tStates);
        }
    }

    public void WriteByteAt(ushort address, byte value, byte? tStates = null)
    {
        lock (lockObject)
        {
            baseMemory.WriteByteAt(address, value, tStates);
        }
    }
}
```

## Related Topics

- **[Processor](Processor.md)** - Main processor implementation
- **[Timing & Clocks](Timing.md)** - Memory access timing
- **[Exception Handling](../Advanced/Exceptions.md)** - Memory error handling
- **[Debug Interface](../Advanced/Debug.md)** - Memory inspection and debugging

---

*[? Back to Documentation Home](../README.md)*
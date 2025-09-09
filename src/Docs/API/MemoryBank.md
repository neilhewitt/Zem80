# MemoryBank Class

[? Back to API Reference](README.md)

## Overview

The `MemoryBank` class implements the Z80's main memory model. It provides read/write access to memory, supports memory segmentation, and integrates with the CPU for timing and initialization.

## Public API

### Properties
- `SizeInBytes` (`uint`): Total size of the memory bank.

### Methods
- `Clear()`: Clears all writable memory.
- `Initialise(Processor cpu, IMemoryMap map)`: Initializes the memory bank with a CPU and memory map.
- `ReadByteAt(address, tStates)`: Reads a byte from memory at the specified address.
- `WriteByteAt(address, value, tStates)`: Writes a byte to memory at the specified address.
- `ReadWordAt(address, tStatesPerByte)`: Reads a 16-bit word from memory.
- `WriteWordAt(address, value, tStatesPerByte)`: Writes a 16-bit word to memory.
- `ReadBytesAt(address, numberOfBytes, tStatesPerByte)`: Reads a sequence of bytes from memory.
- `WriteBytesAt(address, bytes, tStatesPerByte)`: Writes a sequence of bytes to memory.

## Usage Example

```csharp
var mem = new MemoryBank();
mem.Initialise(cpu, new MemoryMap(65536, true));
mem.WriteByteAt(0x1000, 0x42, 3);
byte value = mem.ReadByteAt(0x1000, 3);
```

## Internal Details
- Uses `IMemoryMap` and `IMemorySegment` for flexible memory segmentation.
- Integrates with CPU timing for accurate emulation.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

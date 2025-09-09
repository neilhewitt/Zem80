# Memory Model

[? Back to Architecture & Design](README.md)

## Segmented Memory
- **MemoryBank**: Main memory abstraction, supports up to 64KB
- **MemoryMap**: Maps address ranges to memory segments (RAM, ROM, etc.)
- **MemorySegment**: Represents a contiguous block of memory
- **ReadOnlyMemorySegment**: For ROM or protected memory

## Features
- Supports custom memory layouts (paging, banking)
- Accurate timing for memory access
- Memory can be cleared or initialized at runtime

## Example: Custom Memory Map
```csharp
var map = new MemoryMap(65536, true);
var mem = new MemoryBank();
mem.Initialise(cpu, map);
```

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

# Memory & IO

[? Back to Domain Concepts](README.md)

## Z80 Memory Model
- 16-bit address bus: 64KB addressable memory
- Memory is byte-addressable
- Supports RAM and ROM segments

## IO Model
- Port-mapped IO: 8-bit port addresses (256 ports)
- IN/OUT instructions for device communication

## Representation in Zem80
- `MemoryBank` and `MemoryMap` classes model memory, segmentation, and access
- `IO` and `Ports` classes model IO pins and port-mapped devices

## Example: Reading/Writing Memory and IO
```csharp
cpu.Memory.WriteByteAt(0x4000, 0x99, 3);
byte value = cpu.Memory.ReadByteAt(0x4000, 3);
cpu.IO.SetPortWriteState(0xFE, 0x55);
```

[? Back to Domain Concepts](README.md)
[? Back to Main Index](../README.md)

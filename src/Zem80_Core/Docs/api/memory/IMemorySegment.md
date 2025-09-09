# IMemorySegment Interface

[? Back to Memory API](README.md)

Defines the contract for a segment of memory.

## Key Properties
- `ushort StartAddress`: Start address of the segment.
- `ushort EndAddress`: End address of the segment.
- `bool IsReadOnly`: Indicates if the segment is read-only.

## Key Methods
- `byte Read(ushort address)`: Reads a byte from the segment.
- `void Write(ushort address, byte value)`: Writes a byte to the segment.
- `void Clear()`: Clears the segment.

---

[? Back to Memory API](README.md) | [API Index](../README.md)

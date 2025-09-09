# IMemoryBank Interface

[? Back to Memory API](README.md)

Defines the contract for memory bank implementations.

## Key Methods
- `void Initialise(Processor cpu, IMemoryMap map)`: Initializes the memory bank.
- `byte ReadByteAt(ushort address)`: Reads a byte from memory.
- `void WriteByteAt(ushort address, byte value)`: Writes a byte to memory.
- `byte[] ReadBytesAt(ushort address, int count)`: Reads a sequence of bytes.
- `void WriteBytesAt(ushort address, byte[] data)`: Writes a sequence of bytes.
- `void Clear()`: Clears memory.

---

[? Back to Memory API](README.md) | [API Index](../README.md)

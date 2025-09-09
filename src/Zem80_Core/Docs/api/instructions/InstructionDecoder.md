# InstructionDecoder Class

[? Back to Instructions API](README.md)

Decodes bytes from memory into `InstructionPackage` objects for execution.

## Key Methods
- `InstructionPackage DecodeInstruction(byte[] instructionBytes, ushort address, out bool skipNextByte, out bool opcodeErrorNOP)`: Decodes instruction bytes at a given address.

## Constructor
- `InstructionDecoder(Processor cpu)`: Binds the decoder to a processor instance.

---

[? Back to Instructions API](README.md) | [API Index](../README.md)

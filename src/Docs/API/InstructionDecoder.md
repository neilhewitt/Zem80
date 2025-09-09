# InstructionDecoder Class

[? Back to API Reference](README.md)

## Overview

The `InstructionDecoder` class decodes raw opcode bytes into `InstructionPackage` objects, handling Z80 instruction prefixes, operand extraction, and error handling.

## Public API

### Methods
- `DecodeInstruction(byte[] instructionBytes, ushort address, out bool skipNextByte, out bool opcodeErrorNOP)`: Decodes a 4-byte instruction sequence at a given address, returning an `InstructionPackage` and flags for special cases.

### Constructor
- `InstructionDecoder(Processor cpu)`: Binds the decoder to a CPU instance.

## Usage Example

```csharp
var decoder = new InstructionDecoder(cpu);
var package = decoder.DecodeInstruction(bytes, 0x100, out var skip, out var error);
```

## Internal Details
- Handles all Z80 instruction prefix rules and error cases (e.g., invalid opcodes).
- Used by the main processor loop and debug processor.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

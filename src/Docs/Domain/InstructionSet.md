# Instruction Set

[? Back to Domain Concepts](README.md)

## Z80 Instruction Set
The Z80 supports over 150 documented instructions, with many more undocumented variants. Instructions cover:
- Data transfer (LD, PUSH, POP)
- Arithmetic (ADD, SUB, INC, DEC)
- Logic (AND, OR, XOR, CP)
- Bit manipulation (BIT, SET, RES, RL, RR, etc.)
- Control flow (JP, JR, CALL, RET, RST)
- IO (IN, OUT)
- Special (DI, EI, HALT, NOP)

## Instruction Format
- 1 to 4 bytes per instruction
- May include prefixes (CB, ED, DD, FD)
- May use immediate, register, or memory operands

## Representation in Zem80
- `Instruction` class models each instruction
- `InstructionSet` builds and indexes all instructions
- `InstructionDecoder` decodes raw bytes to instructions

## Example: Decoding an Instruction
```csharp
var bytes = new byte[] { 0x3E, 0x42, 0x00, 0x00 }; // LD A,0x42
var package = cpu.InstructionDecoder.DecodeInstruction(bytes, 0x0000, out var skip, out var error);
```

[? Back to Domain Concepts](README.md)
[? Back to Main Index](../README.md)

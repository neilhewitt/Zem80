# Registers & Flags

[? Back to Domain Concepts](README.md)

## Z80 Registers
The Z80 CPU has a set of general-purpose and special-purpose registers:
- **A**: Accumulator (main arithmetic register)
- **F**: Flags (status register)
- **B, C, D, E, H, L**: General-purpose 8-bit registers
- **AF, BC, DE, HL**: Register pairs (16-bit)
- **IX, IY**: Index registers for advanced addressing
- **SP**: Stack Pointer
- **PC**: Program Counter
- **I**: Interrupt vector
- **R**: Memory refresh
- **Shadow registers**: Alternate set for fast context switching

## Flags
The F register contains status flags:
- **Sign (S)**: Set if result is negative
- **Zero (Z)**: Set if result is zero
- **Half Carry (H)**: Set on half-byte carry/borrow
- **Parity/Overflow (P/V)**: Set on even parity or overflow
- **Subtract (N)**: Set if last operation was subtraction
- **Carry (C)**: Set if result overflowed/underflowed

## Representation in Zem80
- The `Registers` class models all registers and pairs.
- The `Flags` class models the F register and exposes individual flags.

## Example: Accessing Registers
```csharp
var regs = cpu.Registers;
regs.A = 0x10;
regs.HL = 0x1234;
if (cpu.Flags.Zero) { /* ... */ }
```

[? Back to Domain Concepts](README.md)
[? Back to Main Index](../README.md)

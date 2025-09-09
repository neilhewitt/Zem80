# Registers & IRegisters

[? Back to CPU API](README.md)

## Registers Class

- **Namespace**: `Zem80.Core.CPU`
- Models the Z80's register set, including general, special, and shadow registers.

### Key Properties
- `A, B, C, D, E, H, L` (byte): General purpose registers
- `F` (byte): Flags register
- `PC` (ushort): Program Counter
- `SP` (ushort): Stack Pointer
- `IX, IY` (ushort): Index registers
- `IR` (ushort): Interrupt register
- `WZ` (ushort): Internal register (MEMPTR)
- `Shadow` (IShadowRegisters): Alternate register set

### Key Methods
- `void ExchangeAF()`: Exchanges AF with shadow AF'
- `Registers Snapshot()`: Returns a copy of the register state
- `void Clear()`: Resets all registers

## IRegisters Interface

- Contract for register access and manipulation.

---

[? Back to CPU API](README.md) | [API Index](../README.md)

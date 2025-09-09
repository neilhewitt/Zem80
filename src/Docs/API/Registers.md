# Registers Class

[? Back to API Reference](README.md)

## Overview

The `Registers` class models all Z80 CPU registers, including general-purpose, special, and shadow registers. It provides indexed and named access to 8-bit and 16-bit registers, as well as methods for register exchange and snapshotting.

## Public API

### Properties
- 8-bit: `A`, `F`, `B`, `C`, `D`, `E`, `H`, `L`
- 16-bit: `AF`, `BC`, `DE`, `HL`, `IX`, `IY`, `SP`, `PC`, `IR`
- Shadow registers: `Shadow.AF`, `Shadow.BC`, `Shadow.DE`, `Shadow.HL`
- Special: `I`, `R`, `WZ` (internal)
- Indexers: `this[ByteRegister]`, `this[WordRegister]`

### Methods
- `ExchangeAF()`: Swap AF with AF'.
- `ExchangeBCDEHL()`: Swap BC/DE/HL with their shadows.
- `Snapshot()`: Returns a copy of the current register state.
- `Clear()`: Resets all registers.

## Usage Example

```csharp
var regs = new Registers();
regs.A = 0x42;
regs.HL = 0x1234;
regs.ExchangeAF();
```

## Internal Details
- Registers are stored in a 26-byte array for fast access.
- Shadow registers are accessed via the `IShadowRegisters` interface.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

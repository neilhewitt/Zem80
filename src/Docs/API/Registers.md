# Registers API

[? Back to API Reference](README.md)

The `IRegisters` interface and `Registers` class provide access to the Z80's register set, including 8-bit and 16-bit registers, shadow registers, and special registers.

## Overview
The `Registers` class models all Z80 registers, including shadow and special registers. It supports direct access, swapping, and cloning.

## Properties
- 8-bit: `A`, `F`, `B`, `C`, `D`, `E`, `H`, `L`, `I`, `R`, `IXh`, `IXl`, `IYh`, `IYl`
- 16-bit: `AF`, `BC`, `DE`, `HL`, `IX`, `IY`, `SP`, `PC`, `IR`, `WZ`
- Shadow: `Shadow` property exposes AF', BC', DE', HL'

## Methods
- `Clear()`: Reset all registers.
- `ExchangeAF()`: Swap AF with AF'.
- `ExchangeBCDEHL()`: Swap BC/DE/HL with their shadows.
- `Snapshot()`: Clone register state.

## Example: Swapping Registers
````csharp
processor.Registers.ExchangeAF();
````

---

[? Back to API Reference](README.md)

# Flags Class

[? Back to API Reference](README.md)

## Overview

The `Flags` class models the Z80's flags register, providing access to individual flag bits and methods for condition evaluation and cloning.

## Public API

### Properties
- `Sign`, `Zero`, `Y`, `HalfCarry`, `X`, `ParityOverflow`, `Subtract`, `Carry`: Individual flag bits.
- `Value` (`byte`): The full flags byte.
- `ReadOnly` (`bool`): If true, the flags are immutable.
- `State` (`FlagState`): Enum representation of the flags.

### Methods
- `Reset()`: Clears all flags.
- `SatisfyCondition(condition)`: Checks if a condition is satisfied by the current flags.
- `Equals(obj)`, `GetHashCode()`: Equality overrides.
- `Clone()`: Returns a copy of the flags.

### Constructors
- `Flags()`: Default constructor.
- `Flags(FlagState flags, bool readOnly = false)`: From enum.
- `Flags(byte flags, bool readOnly = false)`: From byte.

## Usage Example

```csharp
var flags = new Flags();
flags.Carry = true;
if (flags.SatisfyCondition(Condition.C)) { /* ... */ }
```

## Internal Details
- Used by the processor, instructions, and condition evaluation.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

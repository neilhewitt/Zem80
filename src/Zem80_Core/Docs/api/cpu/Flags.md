# Flags & IReadOnlyFlags

[? Back to CPU API](README.md)

## Flags Class

- **Namespace**: `Zem80.Core.CPU`
- Represents the Z80 flags register (F), including all status flags.
- Provides methods for flag manipulation and condition evaluation.

### Key Properties
- `Value` (byte): The raw flags value.
- `ReadOnly` (bool): Indicates if the flags are read-only.
- `State` (FlagState): The current state of all flags.

### Key Methods
- `void Reset()`: Resets all flags.
- `bool SatisfyCondition(Condition condition)`: Checks if a condition is satisfied by the current flags.
- `Flags Clone()`: Returns a copy of the flags.

## IReadOnlyFlags Interface

- Provides a read-only view of the flags register.

---

[? Back to CPU API](README.md) | [API Index](../README.md)

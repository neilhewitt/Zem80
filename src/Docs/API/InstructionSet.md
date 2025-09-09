# InstructionSet Class

[? Back to API Reference](README.md)

## Overview

The `InstructionSet` class manages the complete set of Z80 instructions, including lookup by opcode and mnemonic. It builds the instruction table at startup and provides access to all supported instructions.

## Public API

### Properties
- `Instructions` (`IDictionary<int, Instruction>`): Lookup by opcode.
- `InstructionsByMnemonic` (`IDictionary<string, Instruction>`): Lookup by mnemonic.
- `NOP` (`Instruction`): The NOP instruction instance.

### Methods
- `Build()`: Builds the instruction set and populates lookup tables.

## Usage Example

```csharp
InstructionSet.Build();
var ldInstr = InstructionSet.InstructionsByMnemonic["LD"];
```

## Internal Details
- Handles undocumented instruction overloads.
- Used by the instruction decoder and execution engine.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

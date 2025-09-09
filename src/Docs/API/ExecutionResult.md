# ExecutionResult Class

[? Back to API Reference](README.md)

## Overview

The `ExecutionResult` class encapsulates the result of executing a single instruction, including the instruction, its data, resulting flags, and the address at which it was executed.

## Public API

### Properties
- `Instruction` (`Instruction`): The executed instruction.
- `Data` (`InstructionData`): The data/operands used.
- `Flags` (`Flags`): The resulting flags after execution.
- `InstructionAddress` (`ushort`): The address where the instruction was executed.

### Constructor
- `ExecutionResult(InstructionPackage package, Flags flags)`: Creates a result from a package and flags.

## Usage Example

```csharp
var result = new ExecutionResult(package, flags);
```

## Internal Details
- Used by the processor and debugging APIs to report instruction outcomes.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

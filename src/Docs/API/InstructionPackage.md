# InstructionPackage Class

[? Back to API Reference](README.md)

## Overview

The `InstructionPackage` class bundles an `Instruction`, its operand data, and the address at which it is to be executed. It is the main unit passed between decoding and execution.

## Public API

### Properties
- `Instruction` (`Instruction`): The instruction to execute.
- `Data` (`InstructionData`): The operands/arguments for the instruction.
- `InstructionAddress` (`ushort`): The address of the instruction in memory.

### Constructor
- `InstructionPackage(Instruction instruction, InstructionData data, ushort instructionAddress)`: Creates a new package.

## Usage Example

```csharp
var package = new InstructionPackage(instr, data, 0x2000);
```

## Internal Details
- Used throughout the processor, decoder, and debug APIs.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

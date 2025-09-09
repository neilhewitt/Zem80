# Instruction Class

[? Back to API Reference](README.md)

## Overview

The `Instruction` class represents a single Z80 instruction, including its opcode, mnemonic, operands, timing, and associated microcode for execution.

## Public API

### Properties
- `Prefix`, `LastOpcodeByte`, `OpcodeString`, `OpcodeBytes`: Opcode details.
- `Mnemonic`: Instruction mnemonic (e.g., "LD", "ADD").
- `Condition`: Conditional execution (if any).
- `SizeInBytes`: Instruction length.
- `MachineCycles`: Timing information.
- `IsIndexed`, `IsConditional`, `AccessesMemory`, `PerformsIO`, `IsLoopingInstruction`: Instruction characteristics.
- `BitIndex`: Bit index for bitwise instructions.
- `Microcode`: The microcode implementation for this instruction.
- `Target`, `Source`, `Argument1`, `Argument2`: Operand details.
- `IndexedRegister`, `TargetsByteRegister`, `TargetsWordRegister`, `TargetsByteInMemory`, `CopiesResultToRegister`, `CopyResultTo`: Operand/targeting details.

### Constructor
- `Instruction(...)`: Constructs an instruction with all metadata and microcode.

## Usage Example

```csharp
var instr = new Instruction("7E", "LD", Condition.None, InstructionElement.A, InstructionElement.HL, InstructionElement.None, InstructionElement.None, 1, cycles);
```

## Internal Details
- Binds to a microcode implementation for execution.
- Used by the instruction decoder and execution engine.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

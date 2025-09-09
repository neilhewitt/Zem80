# DebugProcessor Class

[? Back to API Reference](README.md)

## Overview

The `DebugProcessor` class provides debugging support for the CPU, including breakpoints and direct instruction execution for testing and diagnostics.

## Public API

### Methods
- `AddBreakpoint(address)`: Adds a breakpoint at the specified address.
- `RemoveBreakpoint(address)`: Removes a breakpoint.
- `EvaluateAndRunBreakpoint(address, executingPackage)`: Checks for and triggers breakpoints.
- `ExecuteDirect(opcode)`: Executes an instruction directly from opcode bytes.
- `ExecuteDirect(mnemonic, arg1, arg2)`: Executes an instruction by mnemonic and arguments.

### Constructor
- `DebugProcessor(Processor cpu, Action<InstructionPackage> executeInstruction)`: Binds to a CPU and execution delegate.

## Usage Example

```csharp
debug.AddBreakpoint(0x1234);
debug.ExecuteDirect("LD", 0x42, null);
```

## Internal Details
- Used by the processor for debugging and test scenarios.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

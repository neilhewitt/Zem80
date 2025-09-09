# ProcessorTiming Class

[? Back to API Reference](README.md)

## Overview

The `ProcessorTiming` class implements the Z80's timing model, simulating t-states for opcode fetch, memory access, IO, and interrupts. It ensures accurate emulation of instruction timing and wait states.

## Public API

### Methods
- `AddWaitCycles(waitCycles)`: Adds wait cycles to be inserted.
- `OpcodeFetchTiming(instruction, address)`: Simulates timing for opcode fetch.
- `OpcodeFetchCycle(address, opcode, tStates)`: Simulates a single opcode fetch cycle.
- `OperandReadTiming(instruction, address, operands)`: Simulates timing for operand reads.
- `MemoryReadCycle(address, data, tStates)`: Simulates a memory read.
- `MemoryWriteCycle(address, data, tStates)`: Simulates a memory write.
- `BeginStackReadCycle()`, `EndStackReadCycle(data)`: Simulates stack read.
- `BeginStackWriteCycle(data)`, `EndStackWriteCycle()`: Simulates stack write.
- `BeginPortReadCycle(port, addressFromBC)`, `EndPortReadCycle(data)`: Simulates port read.
- `BeginPortWriteCycle(data, port, addressFromBC)`, `EndPortWriteCycle()`: Simulates port write.
- `BeginInterruptRequestAcknowledgeCycle(tStates)`, `EndInterruptRequestAcknowledgeCycle()`: Simulates interrupt acknowledge.
- `InternalOperationCycle(tStates)`, `InternalOperationCycles(params tStates)`: Simulates internal CPU cycles.

### Constructor
- `ProcessorTiming(Processor cpu)`: Binds timing to a CPU instance.

## Usage Example

```csharp
timing.OpcodeFetchTiming(instr, 0x1000);
timing.MemoryReadCycle(0x2000, 0x42, 3);
```

## Internal Details
- Used by the processor and microcode to ensure cycle-accurate emulation.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

# Processor Class

[? Back to API Reference](README.md)

## Overview

The `Processor` class is the core of the Zem80 emulator. It models the Z80 CPU, managing registers, memory, IO, timing, interrupts, and instruction execution. It provides the main emulation loop and exposes events for instruction execution and state changes.

## Public API

### Properties
- `Registers` (`IRegisters`): Access to all CPU registers.
- `Stack` (`IStack`): The stack abstraction for push/pop operations.
- `Ports` (`IPorts`): IO port interface.
- `IO` (`IIO`): Models the Z80's IO pin state.
- `Interrupts` (`IInterrupts`): Handles maskable and non-maskable interrupts.
- `Timing` (`IProcessorTiming`): Controls instruction and memory timing.
- `Debug` (`IDebugProcessor`): Debugging and breakpoint support.
- `Memory` (`IMemoryBank`): Main memory interface.
- `Clock` (`IClock`): Controls CPU clocking.
- `Flags` (`IReadOnlyFlags`): Read-only view of the current flags register.
- `Running`, `Halted`, `Suspended`: CPU state flags.
- `LastStarted`, `LastStopped`, `LastRunTime`: Execution timing info.

### Events
- `BeforeExecuteInstruction`, `AfterExecuteInstruction`: Raised before/after each instruction.
- `OnStop`, `OnSuspend`, `OnResume`, `OnHalt`: State change events.

### Methods
- `Start(address, endOnHalt, interruptMode)`: Starts execution at a given address.
- `Stop()`: Stops execution.
- `Suspend()`, `Resume()`: Pauses/resumes execution.
- `RunUntilStopped()`: Blocks until stopped.
- `ResetAndClearMemory(restart, startAddress, interruptMode)`: Resets CPU and memory.
- `Halt(reason)`: Halts execution.
- `Dispose()`: Cleans up resources.

## Usage Example

```csharp
var cpu = new Processor();
cpu.Start(0x0000);
// ... load program into memory ...
cpu.RunUntilStopped();
```

## Internal Details
- The main execution loop fetches, decodes, and executes instructions, updating registers and memory.
- Handles Z80-specific behaviors like memory refresh, HALT/NOP, and interrupt cycles.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

# Processor Class

[? Back to CPU API](README.md)

The `Processor` class is the core of the Zem80 CPU emulation. It manages the execution of instructions, coordinates subsystems (memory, I/O, timing, debugging), and provides the main API for controlling the emulated CPU.

## Summary

- **Namespace**: `Zem80.Core.CPU`
- **Implements**: `IDisposable`

## Key Responsibilities

- Emulate the Z80 instruction cycle
- Manage registers, memory, stack, I/O, and interrupts
- Provide hooks for debugging and timing
- Support for halting, suspending, resuming, and resetting

## Constructor

```
Processor(
    IMemoryBank memory = null,
    IMemoryMap map = null,
    IStack stack = null,
    IClock clock = null,
    IRegisters registers = null,
    IPorts ports = null,
    IProcessorTiming cycleTiming = null,
    IIO io = null,
    IInterrupts interrupts = null,
    IDebugProcessor debug = null,
    ushort topOfStackAddress = 0xFFFF
)
```
- All parameters are optional; defaults are provided for each subsystem.

## Properties

- `Registers` (`IRegisters`): CPU register set
- `Stack` (`IStack`): Stack abstraction
- `Ports` (`IPorts`): I/O ports
- `IO` (`IIO`): I/O state
- `Interrupts` (`IInterrupts`): Interrupt controller
- `Timing` (`IProcessorTiming`): Timing and cycle management
- `Debug` (`IDebugProcessor`): Debugging interface
- `Memory` (`IMemoryBank`): Main memory
- `Clock` (`IClock`): Clock/timing source
- `Flags` (`IReadOnlyFlags`): Read-only view of the flags register
- `Running`, `Halted`, `Suspended` (bool): CPU state
- `LastStarted`, `LastStopped`, `LastRunTime`: Execution timing
- `EndOnHalt` (bool): Whether to stop on HALT instruction

## Methods

- `void Start(ushort address = 0x0000, bool endOnHalt = false, InterruptMode interruptMode = InterruptMode.IM0)`
  - Starts execution at the specified address.
- `void Stop()`
  - Stops execution and the clock.
- `void Suspend()` / `void Resume()`
  - Pauses/resumes execution.
- `void RunUntilStopped()`
  - Blocks until the CPU is stopped.
- `void ResetAndClearMemory(bool restartAfterReset = true, ushort startAddress = 0, InterruptMode interruptMode = InterruptMode.IM0)`
  - Resets CPU and clears memory.
- `void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)`
  - Halts execution for the given reason.
- `void Dispose()`
  - Cleans up resources.

## Events

- `BeforeExecuteInstruction`, `AfterExecuteInstruction`: Instruction execution hooks
- `OnStop`, `OnSuspend`, `OnResume`, `OnHalt`: State change events

## Example Usage

```csharp
var cpu = new Processor();
cpu.Start(0x0000);
// ...
cpu.Stop();
```

---

[? Back to CPU API](README.md) | [API Index](../README.md)

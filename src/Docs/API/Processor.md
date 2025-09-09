# Processor Class

[? Back to API Reference](README.md)

The `Processor` class is the core of the Zem80 Z80 CPU emulation. It coordinates memory, IO, instruction execution, timing, and interrupts.

## Overview
The `Processor` class emulates the Z80 CPU, managing the fetch-decode-execute cycle, memory, IO, timing, and interrupts. It is highly extensible and can be customized with different memory, IO, and timing implementations.

## Constructor
````csharp
public Processor(
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
    ushort topOfStackAddress = 0xFFFF)
````
- **Inputs:**
    - All parameters are optional and allow you to inject custom implementations for memory, IO, timing, etc.
- **Behavior:**
    - If not provided, default implementations are used.
    - Initializes all subsystems and prepares the CPU for execution.

## Properties
- `Registers` (`IRegisters`): Access CPU registers.
- `Stack` (`IStack`): Stack operations.
- `Ports` (`IPorts`): IO port access.
- `IO` (`IIO`): Bus and signal lines.
- `Interrupts` (`IInterrupts`): Interrupt controller.
- `Timing` (`IProcessorTiming`): Timing and wait states.
- `Debug` (`IDebugProcessor`): Debugging and breakpoints.
- `Memory` (`IMemoryBank`): Main memory.
- `Clock` (`IClock`): CPU clock.
- `Flags` (`IReadOnlyFlags`): Current flags register.
- `Running`, `Halted`, `Suspended`: State flags.
- `LastStarted`, `LastStopped`, `LastRunTime`: Execution timing info.

## Methods
- `Start(ushort address = 0x0000, bool endOnHalt = false, InterruptMode interruptMode = InterruptMode.IM0)`
    - Starts execution at the given address. Optionally stops on HALT or sets interrupt mode.
- `Stop()`
    - Stops execution and clock.
- `Suspend()`, `Resume()`
    - Pauses/resumes execution. `Resume` advances PC if resuming from HALT.
- `RunUntilStopped()`
    - Blocks the calling thread until the CPU is stopped.
- `ResetAndClearMemory(bool restartAfterReset = true, ushort startAddress = 0, InterruptMode interruptMode = InterruptMode.IM0)`
    - Resets CPU, clears memory, and optionally restarts execution.
- `Halt(HaltReason reason = HaltReason.HaltCalledDirectly)`
    - Halts execution for the given reason.
- `Dispose()`
    - Cleans up resources and stops execution.

## Events
- `BeforeExecuteInstruction`, `AfterExecuteInstruction`: Instruction execution hooks.
- `OnStop`, `OnSuspend`, `OnResume`, `OnHalt`: State change events.

## Example: Running a Program
````csharp
var processor = new Processor();
// Load program into memory...
processor.Start(0x0000);
processor.RunUntilStopped();
````

---

[? Back to API Reference](README.md)

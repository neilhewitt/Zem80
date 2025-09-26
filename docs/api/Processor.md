# Processor Class

The `Processor` class is the main orchestrator of the Z80 CPU emulation. It manages the instruction fetch-decode-execute cycle, coordinates all subsystems, and provides the primary interface for controlling the emulated processor.

## Namespace
`Zem80.Core.CPU`

## Declaration
```csharp
public class Processor : IDisposable
```

## Constants

### MAX_MEMORY_SIZE_IN_BYTES
```csharp
public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
```
Maximum memory size supported by the Z80 (64KB).

### DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ
```csharp
public const float DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ = 4;
```
Default processor frequency when no clock is specified (4MHz).

## Properties

### Core Subsystems

#### Registers
```csharp
public IRegisters Registers { get; init; }
```
Provides access to all Z80 registers including main registers (A, B, C, D, E, H, L), shadow registers, index registers (IX, IY), and special registers (PC, SP, I, R).

#### Stack
```csharp
public IStack Stack { get; init; }
```
Stack management interface for push/pop operations and stack pointer control.

#### Memory
```csharp
public IMemoryBank Memory { get; init; }
```
Memory subsystem providing read/write access to the 64KB address space.

#### IO
```csharp
public IIO IO { get; init; }
```
I/O pin state management for hardware interfacing and port operations.

#### Ports
```csharp
public IPorts Ports { get; init; }
```
Port management system for connecting emulated hardware devices.

#### Interrupts
```csharp
public IInterrupts Interrupts { get; init; }
```
Interrupt handling system supporting all Z80 interrupt modes.

#### Timing
```csharp
public IProcessorTiming Timing { get; init; }
```
Timing control for cycle-accurate emulation or performance optimization.

#### Debug
```csharp
public IDebugProcessor Debug { get; init; }
```
Debugging interface with breakpoints, tracing, and inspection capabilities.

#### Clock
```csharp
public IClock Clock { get; init; }
```
Clock management for controlling processor frequency and timing.

### State Properties

#### Flags
```csharp
public IReadOnlyFlags Flags => new Flags(Registers.F, true);
```
Read-only access to Z80 flags (Sign, Zero, HalfCarry, ParityOverflow, Subtract, Carry).

#### Running
```csharp
public bool Running => _running;
```
Indicates whether the processor is currently executing instructions.

#### Halted
```csharp
public bool Halted => _halted;
```
Indicates whether the processor has encountered a HALT instruction.

#### Suspended
```csharp
public bool Suspended => _suspended;
```
Indicates whether execution is temporarily suspended.

#### EndOnHalt
```csharp
public bool EndOnHalt { get; private set; }
```
When true, processor stops execution on HALT instead of waiting for interrupts.

### Timing Properties

#### LastStarted
```csharp
public DateTime LastStarted { get; private set; }
```
Timestamp when the processor was last started.

#### LastStopped
```csharp
public DateTime LastStopped { get; private set; }
```
Timestamp when the processor was last stopped.

#### LastRunTime
```csharp
public TimeSpan LastRunTime { get; private set; }
```
Duration of the last execution session.

## Events

### BeforeExecuteInstruction
```csharp
public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
```
Fired before each instruction is executed. Provides access to the decoded instruction package.

**Example:**
```csharp
processor.BeforeExecuteInstruction += (sender, package) =>
{
    Console.WriteLine($"Executing: {package.Instruction.Mnemonic} at {package.InstructionAddress:X4}");
};
```

### AfterExecuteInstruction
```csharp
public event EventHandler<ExecutionResult> AfterExecuteInstruction;
```
Fired after each instruction execution completes.

**Example:**
```csharp
processor.AfterExecuteInstruction += (sender, result) =>
{
    Console.WriteLine($"Instruction took {result.TStatesConsumed} T-states");
};
```

### BeforeInsertWaitCycles
```csharp
public event EventHandler<int> BeforeInsertWaitCycles;
```
Fired before inserting wait cycles for timing accuracy.

### OnStop
```csharp
public event EventHandler OnStop;
```
Fired when processor execution stops.

### OnSuspend
```csharp
public event EventHandler OnSuspend;
```
Fired when processor execution is suspended.

### OnResume
```csharp
public event EventHandler OnResume;
```
Fired when processor execution resumes from suspension.

### OnHalt
```csharp
public event EventHandler<HaltReason> OnHalt;
```
Fired when processor encounters a halt condition.

## Methods

### Start
```csharp
public void Start(ushort address = 0x0000, bool endOnHalt = false, InterruptMode interruptMode = InterruptMode.IM0)
```
Starts processor execution from the specified address.

**Parameters:**
- `address`: Starting program counter value (default: 0x0000)
- `endOnHalt`: If true, stops execution on HALT instruction (default: false)
- `interruptMode`: Initial interrupt mode (default: IM0)

**Example:**
```csharp
// Start from address 0x8000 with IM1 interrupts
processor.Start(0x8000, endOnHalt: false, InterruptMode.IM1);
```

### Stop
```csharp
public void Stop()
```
Stops processor execution and updates timing statistics.

**Example:**
```csharp
processor.Stop();
Console.WriteLine($"Ran for {processor.LastRunTime.TotalMilliseconds}ms");
```

### Suspend
```csharp
public void Suspend()
```
Temporarily suspends execution without stopping the processor thread.

**Example:**
```csharp
processor.Suspend();
// Processor state can be inspected while suspended
byte accumulator = processor.Registers.A;
processor.Resume();
```

### Resume
```csharp
public void Resume()
```
Resumes execution from a suspended state. If resuming from HALT, advances PC past the HALT instruction.

### RunUntilStopped
```csharp
public void RunUntilStopped()
```
Blocks the calling thread until processor execution stops. Useful for synchronous execution patterns.

**Example:**
```csharp
processor.Start(0x0000, endOnHalt: true);
processor.RunUntilStopped(); // Blocks until HALT instruction
Console.WriteLine($"Final accumulator value: {processor.Registers.A:X2}");
```

### ResetAndClearMemory
```csharp
public void ResetAndClearMemory(bool restartAfterReset = true, ushort startAddress = 0, InterruptMode interruptMode = InterruptMode.IM0)
```
Performs a complete system reset, clearing all registers and memory.

**Parameters:**
- `restartAfterReset`: Whether to restart execution after reset (default: true)
- `startAddress`: Address to restart from (default: 0x0000)
- `interruptMode`: Interrupt mode after reset (default: IM0)

**Example:**
```csharp
// Reset and restart from ROM entry point
processor.ResetAndClearMemory(true, 0x0000, InterruptMode.IM1);
```

### Halt
```csharp
public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
```
Forces the processor into a halted state.

**Parameters:**
- `reason`: Reason for the halt (default: HaltCalledDirectly)

## Constructors

### Processor
```csharp
public Processor(IMemoryBank memory = null, IMemoryMap map = null, IStack stack = null, 
    IClock clock = null, IRegisters registers = null, IPorts ports = null,
    IProcessorTiming cycleTiming = null, IIO io = null, IInterrupts interrupts = null, 
    IDebugProcessor debug = null, ushort topOfStackAddress = 0xFFFF)
```

Creates a new processor instance with optional custom subsystem implementations.

**Parameters:**
- `memory`: Custom memory implementation (default: MemoryBank)
- `map`: Memory mapping configuration (default: full 64KB map)
- `stack`: Custom stack implementation (default: Stack)
- `clock`: Custom clock implementation (default: DefaultClock at 4MHz)
- `registers`: Custom register implementation (default: Registers)
- `ports`: Custom port management (default: Ports)
- `cycleTiming`: Custom timing implementation (default: ProcessorTiming)
- `io`: Custom I/O implementation (default: IO)
- `interrupts`: Custom interrupt handling (default: Interrupts)
- `debug`: Custom debugging implementation (default: DebugProcessor)
- `topOfStackAddress`: Initial stack pointer value (default: 0xFFFF)

**Example:**
```csharp
// Create processor with custom memory size and clock
var customMap = new MemoryMap(32768); // 32KB instead of 64KB
var fastClock = ClockMaker.FastClock(8.0f); // 8MHz fast clock

var processor = new Processor(
    map: customMap,
    clock: fastClock,
    topOfStackAddress: 0x7FFF
);
```

## Usage Examples

### Basic Program Execution
```csharp
// Create processor with default configuration
var processor = new Processor();

// Load a simple program: LD A, 42h; HALT
byte[] program = { 0x3E, 0x42, 0x76 };
processor.Memory.WriteBytesAt(0x0000, program);

// Execute until HALT
processor.Start(0x0000, endOnHalt: true);
processor.RunUntilStopped();

Console.WriteLine($"A register = {processor.Registers.A:X2}"); // Output: A register = 42
```

### Interrupt-Driven Execution
```csharp
var processor = new Processor();

// Set up interrupt handler
processor.Memory.WriteBytesAt(0x0038, new byte[] { 
    0x3C, // INC A
    0xFB, // EI
    0xED, 0x4D // RETI
});

// Start with interrupts enabled
processor.Start(0x0000, endOnHalt: false, InterruptMode.IM1);
processor.Interrupts.Enable();

// Trigger an interrupt after some time
Task.Delay(100).ContinueWith(_ => processor.Interrupts.TriggerMaskableInterrupt());
```

### Debugging and Inspection
```csharp
var processor = new Processor();

// Set up debugging
processor.BeforeExecuteInstruction += (sender, package) =>
{
    Console.WriteLine($"PC:{package.InstructionAddress:X4} {package.Instruction.Mnemonic}");
};

processor.Debug.SetBreakpoint(0x1000);

// Load and run program
processor.Memory.WriteBytesAt(0x0000, myProgram);
processor.Start();

// Breakpoint will trigger, allowing inspection
```

### Performance Monitoring
```csharp
var processor = new Processor();
long totalCycles = 0;

processor.AfterExecuteInstruction += (sender, result) =>
{
    totalCycles += result.TStatesConsumed;
};

processor.Start(0x0000, endOnHalt: true);
processor.RunUntilStopped();

Console.WriteLine($"Total cycles executed: {totalCycles}");
Console.WriteLine($"Effective frequency: {totalCycles / processor.LastRunTime.TotalSeconds / 1_000_000:F2} MHz");
```

## Thread Safety

The Processor class is designed to run the instruction execution loop on a background thread while providing thread-safe access to registers and state for debugging and inspection. The following operations are safe from other threads:

- Reading register values
- Reading processor state (Running, Halted, Suspended)
- Calling Suspend(), Resume(), Stop()
- Setting breakpoints
- Memory read operations (for inspection)

Memory write operations and direct register modifications should generally be avoided while the processor is running unless you understand the timing implications.

## Performance Considerations

- The processor uses pre-built instruction lookup tables for fast decode
- Timing can be switched between cycle-accurate and performance modes
- Event handlers should be lightweight to avoid impacting execution speed
- Memory access is optimized but can be customized for specific use cases

## See Also

- [IRegisters](./Registers.md#iregisters) - Register access interface
- [IMemoryBank](./Memory.md#imemorybank) - Memory management interface  
- [IIO](./IO.md#iio) - I/O pin management interface
- [IClock](./Timing.md#iclock) - Clock management interface
# Processor - Core CPU Emulation Engine

[? Back to Documentation Home](../README.md)

The `Processor` class is the heart of the Zem80 Core emulator, providing complete Z80 CPU emulation with cycle-accurate timing and full instruction set support.

## Overview

The Processor class orchestrates all aspects of Z80 emulation:
- **Instruction fetch, decode, and execution**
- **Register and flag management**
- **Memory and I/O operations**
- **Interrupt handling**
- **Timing and clock management**
- **Debug interface integration**

## Class Structure

```csharp
public class Processor : IDisposable
{
    // Core components
    public IRegisters Registers { get; init; }
    public IMemoryBank Memory { get; init; }
    public IPorts Ports { get; init; }
    public IInterrupts Interrupts { get; init; }
    public IProcessorTiming Timing { get; init; }
    public IDebugProcessor Debug { get; init; }
    public IStack Stack { get; init; }
    public IClock Clock { get; init; }
    
    // State properties
    public bool Running { get; }
    public bool Halted { get; }
    public bool Suspended { get; }
    public IReadOnlyFlags Flags { get; }
    
    // Execution control
    public void Start(ushort address = 0x0000, bool endOnHalt = false, InterruptMode interruptMode = InterruptMode.IM0);
    public void Stop();
    public void Suspend();
    public void Resume();
    public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly);
}
```

## Constructor and Configuration

### Default Constructor
```csharp
var processor = new Processor();
```

Creates a processor with default implementations for all components:
- **64KB memory bank** with full read/write access
- **Default clock** running at 4MHz simulation speed
- **Standard register set** with shadow registers
- **I/O ports** with configurable timing
- **Interrupt controller** supporting all Z80 modes

### Custom Component Constructor
```csharp
var processor = new Processor(
    memory: customMemoryBank,
    clock: realTimeClock,
    registers: customRegisters,
    // ... other custom components
);
```

## Execution Control

### Starting Execution

```csharp
// Start execution from address 0x0000
processor.Start();

// Start from specific address
processor.Start(address: 0x8000);

// End automatically on HALT instruction
processor.Start(endOnHalt: true);

// Set interrupt mode
processor.Start(interruptMode: InterruptMode.IM2);
```

**Parameters:**
- `address` (ushort): Starting address for program counter (default: 0x0000)
- `endOnHalt` (bool): Whether to stop execution on HALT instruction (default: false)
- `interruptMode` (InterruptMode): Initial interrupt mode (default: IM0)

### Execution States

#### Running State
```csharp
if (processor.Running)
{
    Console.WriteLine("Processor is executing instructions");
}
```

#### Halted State
```csharp
if (processor.Halted)
{
    Console.WriteLine("Processor is halted (HALT instruction executed)");
    // Resume execution
    processor.Resume();
}
```

#### Suspended State
```csharp
// Temporarily pause execution
processor.Suspend();

// Check state
if (processor.Suspended)
{
    Console.WriteLine("Processor is suspended");
}

// Resume execution
processor.Resume();
```

### Stopping Execution

```csharp
// Stop the processor
processor.Stop();

// Check execution time
TimeSpan executionTime = processor.LastRunTime;
Console.WriteLine($"Executed for: {executionTime.TotalMilliseconds}ms");
```

## Component Access

### Register Operations
```csharp
// Access 8-bit registers
byte accumulator = processor.Registers.A;
processor.Registers.B = 0x42;

// Access 16-bit register pairs
ushort stackPointer = processor.Registers.SP;
processor.Registers.HL = 0x8000;

// Access using enumerations
processor.Registers[ByteRegister.A] = 0xFF;
ushort bc = processor.Registers[WordRegister.BC];
```

### Memory Operations
```csharp
// Read/write single bytes
byte value = processor.Memory.ReadByteAt(0x8000);
processor.Memory.WriteByteAt(0x8000, 0x42);

// Read/write 16-bit words (little-endian)
ushort word = processor.Memory.ReadWordAt(0x8000);
processor.Memory.WriteWordAt(0x8000, 0x1234);

// Bulk operations
byte[] data = processor.Memory.ReadBytesAt(0x8000, 256);
processor.Memory.WriteBytesAt(0x8000, data);
```

### Flag Access
```csharp
// Read-only access to flags
bool carrySet = processor.Flags.Carry;
bool zeroSet = processor.Flags.Zero;
bool signSet = processor.Flags.Sign;

// Flags are updated automatically during instruction execution
```

## Events and Monitoring

### Execution Events
```csharp
// Before instruction execution
processor.BeforeExecuteInstruction += (sender, package) =>
{
    Console.WriteLine($"Executing: {package.Instruction.Mnemonic} at {package.InstructionAddress:X4}");
};

// After instruction execution
processor.AfterExecuteInstruction += (sender, result) =>
{
    Console.WriteLine($"Completed: {result.Package.Instruction.Mnemonic}");
};

// Processor state changes
processor.OnHalt += (sender, reason) =>
{
    Console.WriteLine($"Processor halted: {reason}");
};

processor.OnStop += (sender, args) =>
{
    Console.WriteLine("Processor stopped");
};
```

### Timing Events
```csharp
processor.BeforeInsertWaitCycles += (sender, cycles) =>
{
    Console.WriteLine($"Inserting {cycles} wait cycles");
};
```

## Instruction Execution Cycle

The processor follows the standard Z80 instruction execution cycle:

1. **Fetch**: Read instruction bytes from memory at PC
2. **Decode**: Parse instruction and create execution package
3. **Execute**: Run instruction microcode
4. **Update**: Update registers, flags, and timing
5. **Interrupt**: Check and handle pending interrupts
6. **Refresh**: Update memory refresh register

```csharp
// The cycle runs continuously in a background thread when started
while (Running && !Suspended)
{
    // Fetch next instruction
    var instructionBytes = Memory.ReadBytesAt(Registers.PC, 4);
    
    // Decode instruction
    var package = instructionDecoder.DecodeInstruction(instructionBytes, Registers.PC);
    
    // Execute instruction microcode
    var result = package.Instruction.Microcode.Execute(this, package);
    
    // Update processor state
    if (result.Flags != null) 
        Registers.F = result.Flags.Value;
    
    // Handle interrupts
    Interrupts.HandleAll();
    
    // Memory refresh
    RefreshMemory();
}
```

## Reset and Initialization

### Soft Reset
```csharp
// Reset without clearing memory
processor.ResetAndClearMemory(restartAfterReset: false);
```

### Full Reset
```csharp
// Reset and clear all memory
processor.ResetAndClearMemory(restartAfterReset: true, startAddress: 0x0000);
```

Reset operations:
- Clear all registers to default values
- Reset stack pointer to top of stack
- Clear I/O port states
- Reset interrupt state
- Optionally clear memory contents
- Optionally restart execution

## Constants and Defaults

```csharp
public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;        // 64KB maximum
public const float DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ = 4; // 4MHz simulation
```

## Thread Safety

The Processor class is **not thread-safe**. External synchronization is required when:
- Accessing processor state from multiple threads
- Modifying memory while processor is running
- Changing I/O port configurations during execution

## Performance Considerations

- The processor runs in a background thread when started
- Instruction execution is optimized for performance
- Memory access patterns affect overall speed
- Clock implementation impacts timing accuracy vs. performance

## Example: Complete Execution Session

```csharp
using Zem80.Core.CPU;

// Create processor
using var processor = new Processor();

// Load a simple program
var program = new byte[]
{
    0x21, 0x00, 0x80,  // LD HL, 8000h
    0x36, 0x42,        // LD (HL), 42h
    0x2A, 0x00, 0x80,  // LD HL, (8000h)
    0x76               // HALT
};

processor.Memory.WriteBytesAt(0x0000, program);

// Set up event monitoring
processor.AfterExecuteInstruction += (s, result) =>
{
    Console.WriteLine($"PC: {processor.Registers.PC:X4}, " +
                     $"HL: {processor.Registers.HL:X4}, " +
                     $"Instruction: {result.Package.Instruction.Mnemonic}");
};

// Execute program
processor.Start(endOnHalt: true);
processor.RunUntilStopped();

// Check results
byte memoryValue = processor.Memory.ReadByteAt(0x8000);
Console.WriteLine($"Memory at 8000h: {memoryValue:X2}"); // Should be 42h
```

## Related Topics

- **[Registers](Registers.md)** - Z80 register set details
- **[Memory System](Memory.md)** - Memory management
- **[Timing & Clocks](Timing.md)** - Clock and timing systems
- **[Debug Interface](../Advanced/Debug.md)** - Debugging capabilities
- **[Instruction Set](../Instructions/InstructionSet.md)** - Supported instructions

---

*[? Back to Documentation Home](../README.md)*
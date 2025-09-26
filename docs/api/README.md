# Zem80_Core API Reference

This documentation covers all public and internal APIs for the Zem80_Core Z80 processor emulation library.

## Overview

Zem80_Core is a comprehensive Z80 CPU emulator that provides cycle-accurate emulation of the Z80 processor, including full instruction set support, interrupt handling, memory management, and I/O operations.

## Core Components

### [Processor](./Processor.md)
The main CPU emulator class that orchestrates all Z80 operations.

### [ArithmeticExtensions](./ArithmeticExtensions.md)
Static extension methods for bit manipulation and arithmetic operations.

### [Registers](./Registers.md)
Z80 register system including main registers, shadow registers, and index registers.

### [Memory System](./Memory.md)
Memory management with banks, segments, and memory mapping capabilities.
- [IMemoryBank](./Memory.md#imemorybank)
- [IMemoryMap](./Memory.md#imemorymap)
- [MemorySegment](./Memory.md#memorysegment)

### [I/O System](./IO.md)
Input/Output pin simulation and port management.
- [IIO](./IO.md#iio)
- [IPorts](./IO.md#iports)
- [Port](./IO.md#port)

### [Instruction System](./Instructions.md)
Complete Z80 instruction set implementation with microcode.
- [InstructionSet](./Instructions.md#instructionset)
- [Instruction](./Instructions.md#instruction)
- [Microcode](./Instructions.md#microcode)

### [Exceptions](./Exceptions.md)
Custom exception hierarchy for Z80 emulation errors.

### [Timing and Clock](./Timing.md)
Clock management and timing control for cycle-accurate emulation.

## Quick Start Example

```csharp
using Zem80.Core;
using Zem80.Core.CPU;
using Zem80.Core.Memory;

// Create memory bank
var memory = new MemoryBank();
var memoryMap = new MemoryMap();
memoryMap.Add(new MemorySegment(0x0000, 0xFFFF, memory));

// Create processor with default configuration
var processor = ProcessorBuilder.Create()
    .WithMemory(memoryMap)
    .WithDefaultConfiguration()
    .Build();

// Load program into memory
byte[] program = { 0x3E, 0x42 }; // LD A, 42h
memory.WriteBytesAt(0x0000, program);

// Execute program
processor.Start(0x0000, endOnHalt: true);

// Check result
byte result = processor.Registers.A; // Should be 0x42
```

## Architecture

The Zem80_Core library is designed with modularity and extensibility in mind:

- **Processor**: Central orchestrator managing all subsystems
- **Memory**: Flexible memory banking and mapping system
- **I/O**: Hardware pin simulation for interfacing with emulated devices
- **Instructions**: Complete Z80 instruction set with cycle-accurate timing
- **Debugging**: Built-in debugging and tracing capabilities

## Thread Safety

The processor is designed to run on a background thread and provides thread-safe access to registers and memory for debugging and inspection purposes.

## Performance

Zem80_Core is optimized for performance while maintaining accuracy:
- Pre-built instruction lookup tables
- Optimized bit manipulation operations
- Minimal memory allocations during execution
- Support for different timing modes (cycle-accurate vs. fast)

## Compatibility

The emulator supports all documented and undocumented Z80 instructions, including:
- All standard Z80 instruction set
- Extended instructions (ED prefix)
- Indexed instructions (DD/FD prefixes)  
- Bit manipulation instructions (CB prefix)
- Undocumented flags and instructions
- All three interrupt modes (IM0, IM1, IM2)

## Contributing

When extending the API, please ensure:
- All public methods are documented with XML comments
- Usage examples are provided for complex operations
- Thread safety considerations are documented
- Performance implications are noted
# Zem80 Core - Z80 Processor Emulator Library

Welcome to the comprehensive documentation for **Zem80 Core**, a high-fidelity Z80 processor emulator implemented in C# for .NET 8. This library provides accurate emulation of the Zilog Z80 microprocessor, including all documented and undocumented instructions, precise timing simulation, and complete processor state management.

## Overview

Zem80 Core is designed to emulate the Z80 processor with cycle-accurate timing and complete instruction set compatibility. The library supports:

- **Complete Z80 instruction set** including undocumented instructions
- **Accurate timing simulation** with machine cycle precision  
- **Memory management** with flexible memory mapping
- **I/O port handling** for peripheral device simulation
- **Interrupt processing** with multiple interrupt modes
- **Debug capabilities** with breakpoints and step execution
- **Extensible architecture** for custom memory and I/O implementations

## Quick Start

```csharp
using Zem80.Core.CPU;

// Create a new processor instance
var processor = new Processor();

// Load some Z80 assembly code into memory
processor.Memory.WriteBytesAt(0x0000, new byte[] { 
    0x3E, 0x42,    // LD A, 42h
    0x06, 0x10,    // LD B, 10h
    0x80,          // ADD A, B
    0x76           // HALT
});

// Start execution from address 0x0000
processor.Start(0x0000, endOnHalt: true);

// Wait for completion
processor.RunUntilStopped();

// Check the result
byte result = processor.Registers.A; // Should be 0x52 (0x42 + 0x10)
```

## Documentation Structure

### Core Components
- **[Processor](Core/Processor.md)** - Main CPU emulation engine
- **[Memory System](Core/Memory.md)** - Memory banks and mapping
- **[Registers](Core/Registers.md)** - Z80 register set implementation
- **[Flags](Core/Flags.md)** - Processor flags and status
- **[Clock & Timing](Core/Timing.md)** - Timing and clock management

### Instruction System
- **[Instruction Set](Instructions/InstructionSet.md)** - Complete Z80 instruction set
- **[Microcode](Instructions/Microcode.md)** - Instruction execution engine
- **[Timing & Machine Cycles](Instructions/Timing.md)** - Instruction timing details

### I/O and Peripherals
- **[I/O Ports](IO/Ports.md)** - Port-based I/O handling
- **[Interrupts](IO/Interrupts.md)** - Interrupt processing system

### Advanced Features
- **[Debug Interface](Advanced/Debug.md)** - Debugging and inspection tools
- **[Exception Handling](Advanced/Exceptions.md)** - Error handling framework
- **[Extensions](Advanced/Extensions.md)** - Extending the emulator

### Architecture & Design
- **[Design Principles](Architecture/Design.md)** - Core design philosophy
- **[Emulation Accuracy](Architecture/Accuracy.md)** - Fidelity and compatibility
- **[Performance](Architecture/Performance.md)** - Optimization strategies

### Examples & Tutorials
- **[Basic Usage](Examples/BasicUsage.md)** - Getting started examples
- **[Advanced Scenarios](Examples/Advanced.md)** - Complex emulation scenarios
- **[Integration](Examples/Integration.md)** - Integrating with applications

## Key Features

### Accurate Z80 Emulation
- All 695 documented instructions implemented
- 20+ undocumented instructions and behaviors
- Cycle-accurate timing for all instructions
- Proper implementation of undocumented flag behaviors

### Flexible Memory System
- Memory banking and mapping support
- Read-only and read-write memory segments
- Memory protection and access control
- Easy integration with custom memory implementations

### Comprehensive I/O Support
- Port-based I/O with configurable timing
- Interrupt handling (IM0, IM1, IM2)
- Maskable and non-maskable interrupts
- Custom I/O device integration

### Development & Debugging
- Single-step execution
- Breakpoint support
- Register and memory inspection
- Execution tracing and profiling

## Target Audience

This library is designed for:
- **Retro computing enthusiasts** building Z80-based system emulators
- **Education** - learning about microprocessor architecture
- **Game preservation** - emulating classic arcade and home computer systems
- **Embedded simulation** - testing Z80-based control systems
- **Research** - studying historical computing architectures

## Compatibility

- **.NET 8.0** target framework
- **Cross-platform** compatibility (Windows, Linux, macOS)
- **NuGet package** distribution ready
- **High performance** with minimal memory footprint

## License & Contributions

Please refer to the project repository for licensing information and contribution guidelines.

---

*This documentation covers Zem80 Core version 1.0 and later. For version-specific information, please check the release notes.*
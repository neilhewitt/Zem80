# Z80 CPU Overview

[? Back to Domain Concepts](README.md)

## What is the Z80 CPU?
The Zilog Z80 is an 8-bit microprocessor introduced in 1976, widely used in home computers, embedded systems, and arcade machines. It features a rich instruction set, multiple registers, and support for interrupts and IO operations.

## Key Features
- 8-bit data bus, 16-bit address bus (64KB addressable memory)
- 6 general-purpose 8-bit registers (B, C, D, E, H, L)
- 2 special-purpose 8-bit registers (A: accumulator, F: flags)
- 2 index registers (IX, IY)
- Stack Pointer (SP), Program Counter (PC)
- Interrupt support (maskable and non-maskable)
- Memory-mapped and port-mapped IO

## Z80 in Zem80
Zem80 models the Z80 CPU as the `Processor` class, with supporting classes for registers, memory, IO, and timing. The emulator aims for accuracy and idiomatic C# design.

## Example: Loading and Running a Program
```csharp
var cpu = new Processor();
cpu.Memory.WriteByteAt(0x0000, 0x3E); // LD A,0x42
cpu.Memory.WriteByteAt(0x0001, 0x42);
cpu.Start(0x0000);
cpu.RunUntilStopped();
```

[? Back to Domain Concepts](README.md)
[? Back to Main Index](../README.md)

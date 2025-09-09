# Architecture Overview

[? Back to Architecture & Design](README.md)

## Modular Design
Zem80 is organized into modular components, each responsible for a core aspect of Z80 emulation:
- **Processor**: Main CPU logic and execution loop
- **Registers**: All CPU registers and shadow registers
- **Memory**: Segmented memory model
- **IO**: Pin and port-mapped IO
- **Timing**: Cycle-accurate timing and wait states
- **Instruction Set**: Decoding and execution of all Z80 instructions
- **Interrupts**: Maskable and non-maskable interrupt handling
- **Debugging**: Breakpoints and direct execution for testing

## Key Principles
- **Separation of Concerns**: Each subsystem is encapsulated in its own class or set of classes
- **Extensibility**: Interfaces and composition allow for custom memory, IO, and timing implementations
- **Accuracy**: Emulates Z80 behavior as closely as possible, including timing and undocumented instructions
- **Idiomatic C#**: Designed for clarity and maintainability

## High-Level Flow
1. **Initialization**: CPU, memory, IO, and instruction set are initialized
2. **Execution Loop**: Processor fetches, decodes, and executes instructions
3. **Timing**: Each operation is timed to match Z80 t-states
4. **Interrupts**: Handled according to Z80 rules
5. **Debugging**: Optional breakpoints and direct execution

## Example: Customizing Memory
```csharp
var customMemory = new MyCustomMemoryBank();
var cpu = new Processor(memory: customMemory);
```

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

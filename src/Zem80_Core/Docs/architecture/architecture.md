# Architecture & Design

[? Back to Main Index](../README.md)

This document provides an overview of the architecture and design of the Zem80 Core library.

## Overview

Zem80 Core is a modular, extensible .NET library for emulating the Zilog Z80 CPU. It is designed to be:

- **Modular**: Components such as memory, I/O, timing, and debugging are abstracted via interfaces.
- **Testable**: The design supports unit testing and custom implementations for memory, I/O, and timing.
- **Extensible**: You can provide your own implementations for most subsystems.

## Main Components

- **Processor**: The central class that emulates the Z80 CPU, manages execution, and coordinates subsystems.
- **Instruction Set & Decoder**: Decodes and executes Z80 instructions using microcode classes.
- **Registers & Flags**: Models the Z80's register set and flag logic.
- **Memory & I/O**: Abstracted via interfaces for flexible emulation of different hardware.
- **Timing & Clocks**: Supports real-time and simulated timing.
- **Debugging**: Breakpoints and direct instruction execution for testing and debugging.

## Execution Flow

1. **Initialization**: The `Processor` is constructed with (optionally) custom subsystems.
2. **Start**: The processor starts execution at a given address, running in a background thread.
3. **Instruction Cycle**: Fetch, decode, and execute instructions, handling timing and interrupts.
4. **Events**: Hooks for before/after instruction execution, halts, suspends, and resumes.

## Extending the Emulator

- Implement custom memory or I/O by providing your own `IMemoryBank`, `IMemoryMap`, or `IPorts`.
- Add new debugging features by extending `IDebugProcessor`.
- Swap out the clock or timing for different emulation speeds.

## File Structure

- [`api/`](../api/README.md): API reference for all classes, interfaces, and enums.
- [`domain_concepts.md`](domain_concepts.md): Z80 and emulation domain concepts.
- [`usage/`](../usage/examples.md): Usage examples and code snippets.

---

[? Back to Main Index](../README.md)

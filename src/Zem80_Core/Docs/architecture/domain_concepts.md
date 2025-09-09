# Domain Concepts

[? Back to Architecture](architecture.md)

This document explains the key domain concepts relevant to Z80 CPU emulation and how they are represented in Zem80 Core.

## Z80 CPU Overview

The Zilog Z80 is an 8-bit microprocessor with a 16-bit address bus, supporting 64KB of addressable memory. It features a rich instruction set, multiple registers, and support for interrupts and I/O operations.

### Registers
- **General Purpose**: A, B, C, D, E, H, L
- **Special Purpose**: F (Flags), PC (Program Counter), SP (Stack Pointer), IX, IY (Index Registers), IR (Interrupt Register)
- **Shadow Registers**: Alternate set for fast context switching

### Flags
- **S** (Sign), **Z** (Zero), **H** (Half Carry), **P/V** (Parity/Overflow), **N** (Add/Subtract), **C** (Carry)

### Memory & I/O
- **Memory**: 64KB addressable, can be RAM or ROM
- **I/O Ports**: 256 ports for device communication

### Interrupts
- **Modes**: IM0, IM1, IM2
- **Maskable/Non-maskable**: Supports both

## Emulation Concepts

- **Instruction Decoding**: Translating bytes from memory into executable instructions
- **Microcode**: Each instruction is implemented as a microcode class for modularity
- **Timing**: Emulates CPU clock cycles and instruction timing
- **Debugging**: Breakpoints, direct execution, and state inspection

## Zem80 Core Approach

- **Abstraction**: Interfaces for memory, I/O, timing, and debugging
- **Extensibility**: Plug in custom components for different hardware or testing
- **Threading**: CPU runs in a background thread for real-time emulation

---

[? Back to Architecture](architecture.md) | [Main Index](../README.md)

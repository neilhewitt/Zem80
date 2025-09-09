# Microcode & IMicrocode

[? Back to Instructions API](README.md)

## IMicrocode Interface

- Contract for microcode implementations of Z80 instructions.
- Method: `ExecutionResult Execute(Processor cpu, InstructionPackage package)`

## Implementations

- Each instruction (e.g., `ADD`, `SBC`, `LD`, etc.) has a corresponding microcode class implementing `IMicrocode`.

---

[? Back to Instructions API](README.md) | [API Index](../README.md)

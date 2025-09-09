# Instruction Decoding & Execution

[? Back to Architecture & Design](README.md)

## Decoding
- **InstructionDecoder**: Decodes up to 4 bytes from memory into an `InstructionPackage`
- Handles Z80 prefixes (CB, ED, DD, FD) and operand extraction
- Invalid opcodes are handled as NOPs per Z80 rules

## Execution
- **Processor**: Main loop fetches, decodes, and executes instructions
- **Instruction**: Contains metadata and a reference to its microcode implementation
- **Microcode**: Each instruction's logic is implemented in a dedicated class
- **Timing**: Each step is timed for cycle accuracy

## Example: Decoding and Executing
```csharp
var package = cpu.InstructionDecoder.DecodeInstruction(bytes, address, out var skip, out var error);
cpu.ExecuteInstruction(package);
```

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

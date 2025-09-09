# Instruction Class

[? Back to Instructions API](README.md)

Represents a single Z80 instruction, including its opcode, mnemonic, operands, and microcode implementation.

## Key Properties
- `Prefix` (int): Instruction prefix
- `LastOpcodeByte` (byte): Last byte of the opcode
- `OpcodeString` (string): String representation of the opcode
- `OpcodeBytes` (byte[]): Opcode bytes
- `Mnemonic` (string): Instruction mnemonic
- `Condition` (Condition): Condition for conditional instructions
- `SizeInBytes` (byte): Instruction size
- `MachineCycles` (InstructionMachineCycles): Timing info
- `IsIndexed`, `IsConditional`, `AccessesMemory`, `PerformsIO`, `HasIntermediateDisplacementByte`, `IsLoopingInstruction` (bool): Instruction characteristics
- `BitIndex` (byte): Bit index for bitwise instructions
- `Microcode` (IMicrocode): Microcode implementation
- `Target`, `Source`, `Argument1`, `Argument2` (InstructionElement): Operands
- `IndexedRegister` (WordRegister): Index register used
- `TargetsByteRegister`, `TargetsWordRegister`, `TargetsByteInMemory`, `CopiesResultToRegister` (bool): Operand targeting

## Constructor
```
Instruction(string fullOpcode, string mnemonic, Condition condition, InstructionElement target, InstructionElement source, InstructionElement arg1, InstructionElement arg2, byte sizeInBytes, IEnumerable<MachineCycle> machineCycles, ByteRegister copyResultTo = ByteRegister.None, IMicrocode microcode = null)
```

---

[? Back to Instructions API](README.md) | [API Index](../README.md)

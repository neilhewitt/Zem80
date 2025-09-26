# Instruction System

The Zem80_Core instruction system provides complete Z80 instruction set implementation with cycle-accurate execution, including all documented and undocumented instructions, extended instruction sets, and microcode-level implementation.

## Core Components

### InstructionSet Class

Static class that manages the complete Z80 instruction set.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public static class InstructionSet
```

#### Properties

##### Instructions
```csharp
public static IDictionary<int, Instruction> Instructions { get; private set; }
```
Dictionary mapping instruction opcodes to Instruction objects. Key is the complete opcode as an integer (including prefixes).

##### InstructionsByMnemonic
```csharp
public static IDictionary<string, Instruction> InstructionsByMnemonic { get; private set; }
```
Dictionary mapping instruction mnemonics to Instruction objects for lookup by name.

##### NOP
```csharp
public static Instruction NOP => Instructions[0]
```
Quick access to the NOP instruction (opcode 0x00).

#### Methods

##### Build
```csharp
public static void Build()
```
Builds the complete Z80 instruction set. Called automatically when processor is created. This method:

- Creates all 900+ instruction variants (including undocumented instructions)
- Binds each instruction to its microcode implementation
- Builds lookup tables for fast instruction decode
- Sets up timing and metadata for each instruction

**Example:**
```csharp
// Typically called automatically, but can be called manually
InstructionSet.Build();
Console.WriteLine($"Built {InstructionSet.Instructions.Count} instructions");
```

### Instruction Class

Represents a single Z80 instruction with complete metadata and execution capability.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public class Instruction
```

#### Opcode Properties

##### OpcodeString
```csharp
public string OpcodeString { get; private set; }
```
Complete opcode as hex string (e.g., "DD46" for LD B,(IX+d)).

##### OpcodeBytes
```csharp
public byte[] OpcodeBytes { get; private set; }
```
Opcode bytes as array (prefix bytes + main opcode).

##### Prefix
```csharp
public int Prefix { get; private set; }
```
Instruction prefix (0 for none, 0xCB, 0xDD, 0xED, 0xFD, 0xDDCB, 0xFDCB).

##### LastOpcodeByte
```csharp
public byte LastOpcodeByte { get; private set; }
```
The final opcode byte (used for bit instruction decoding).

#### Instruction Properties

##### Mnemonic
```csharp
public string Mnemonic { get; private set; }
```
Human-readable instruction mnemonic (e.g., "LD A,(HL)", "JP NZ,nn").

##### SizeInBytes
```csharp
public byte SizeInBytes { get; private set; }
```
Total instruction size including opcode bytes and operands (1-4 bytes).

##### Condition
```csharp
public Condition Condition { get; private set; }
```
Conditional execution condition (None for unconditional instructions).

#### Operand Properties

##### Target
```csharp
public InstructionElement Target { get; private set; }
```
Instruction target (destination) operand.

##### Source
```csharp
public InstructionElement Source { get; private set; }
```
Instruction source operand.

##### Argument1
```csharp
public InstructionElement Argument1 { get; private set; }
```
First instruction argument (immediate values, addresses).

##### Argument2
```csharp
public InstructionElement Argument2 { get; private set; }
```
Second instruction argument (for 16-bit immediate values).

#### Classification Properties

##### IsConditional
```csharp
public bool IsConditional { get; private set; }
```
True if instruction execution depends on flag conditions.

##### IsIndexed
```csharp
public bool IsIndexed { get; private set; }
```
True if instruction uses IX or IY index registers.

##### IndexedRegister
```csharp
public WordRegister IndexedRegister { get; private set; }
```
Which index register is used (IX or IY, or None).

##### AccessesMemory
```csharp
public bool AccessesMemory { get; private set; }
```
True if instruction reads from or writes to memory.

##### PerformsIO
```csharp
public bool PerformsIO { get; private set; }
```
True if instruction performs I/O port operations.

##### IsLoopingInstruction
```csharp
public bool IsLoopingInstruction { get; private set; }
```
True for block instructions (LDIR, CPIR, etc.) that repeat until BC=0.

#### Target Classification Properties

##### TargetsByteRegister
```csharp
public bool TargetsByteRegister { get; private set; }
```
True if target is an 8-bit register.

##### TargetsWordRegister
```csharp
public bool TargetsWordRegister { get; private set; }
```
True if target is a 16-bit register pair.

##### TargetsByteInMemory
```csharp
public bool TargetsByteInMemory { get; private set; }
```
True if target is a memory location.

#### Execution Properties

##### Microcode
```csharp
public IMicrocode Microcode { get; private set; }
```
Microcode implementation that executes this instruction.

##### MachineCycles
```csharp
public InstructionMachineCycles MachineCycles { get; private set; }
```
Detailed timing information for cycle-accurate execution.

#### Special Properties

##### BitIndex
```csharp
public byte BitIndex { get; private set; }
```
Bit number for bit manipulation instructions (0-7).

##### HasIntermediateDisplacementByte
```csharp
public bool HasIntermediateDisplacementByte { get; private set; }
```
True for DDCB/FDCB prefix instructions with displacement before final opcode.

##### CopiesResultToRegister
```csharp
public bool CopiesResultToRegister { get; private set; }
```
True if instruction result is copied to a specific register.

##### CopyResultTo
```csharp
public ByteRegister CopyResultTo { get; private set; }
```
Target register for result copying.

### IMicrocode Interface

Interface for instruction execution implementations.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IMicrocode
```

#### Methods

##### Execute
```csharp
ExecutionResult Execute(Processor cpu, InstructionPackage package)
```
Executes the instruction and returns execution result.

**Parameters:**
- `cpu`: Processor instance
- `package`: Complete instruction package with operands

**Returns:** ExecutionResult with flags and timing information

## Enumerations

### Condition Enumeration

Represents Z80 conditional execution conditions.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public enum Condition
```

#### Values
```csharp
Z,      // Zero flag set
NZ,     // Zero flag clear  
C,      // Carry flag set
NC,     // Carry flag clear
PO,     // Parity/Overflow flag clear (parity odd)
PE,     // Parity/Overflow flag set (parity even)
M,      // Sign flag set (minus/negative)
P,      // Sign flag clear (plus/positive)
None    // Unconditional
```

### InstructionElement Enumeration

Represents instruction operands and addressing modes.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public enum InstructionElement
```

#### Values

##### Basic Elements
```csharp
None,                    // No operand
ByteValue,              // 8-bit immediate value
WordValue,              // 16-bit immediate value  
DisplacementValue,      // Signed 8-bit displacement
BitIndex,              // Bit number (0-7)
```

##### Port Addressing
```csharp
PortNumberFromByteValue, // Port number from immediate byte
PortNumberFromC,        // Port number from C register
```

##### 8-bit Registers
```csharp
A, B, C, D, E, F, H, L, // Main registers
I, R,                   // Special registers
IXh, IXl, IYh, IYl     // Index register halves
```

##### 16-bit Register Pairs
```csharp
AF, BC, DE, HL,        // Main register pairs
IX, IY, SP             // Index and stack pointer
```

##### Memory Addressing
```csharp
AddressFromHL,         // (HL)
AddressFromBC,         // (BC)
AddressFromDE,         // (DE)
AddressFromIX,         // (IX)
AddressFromIY,         // (IY)
AddressFromSP,         // (SP)
AddressFromWordValue,  // (nn)
AddressFromIXAndOffset, // (IX+d)
AddressFromIYAndOffset  // (IY+d)
```

## Usage Examples

### Instruction Set Inspection
```csharp
// Build instruction set and inspect
InstructionSet.Build();

Console.WriteLine($"Total instructions: {InstructionSet.Instructions.Count}");

// Find all conditional jump instructions
var conditionalJumps = InstructionSet.InstructionsByMnemonic
    .Where(kvp => kvp.Key.StartsWith("JP ") && kvp.Value.IsConditional)
    .Select(kvp => kvp.Value);

foreach (var jump in conditionalJumps)
{
    Console.WriteLine($"{jump.Mnemonic} - Condition: {jump.Condition}, Size: {jump.SizeInBytes}");
}

// Output:
// JP Z,nn - Condition: Z, Size: 3
// JP NZ,nn - Condition: NZ, Size: 3
// JP C,nn - Condition: C, Size: 3
// etc.
```

### Instruction Analysis
```csharp
// Analyze instruction characteristics
var instruction = InstructionSet.InstructionsByMnemonic["LD A,(IX+5)"];

Console.WriteLine($"Mnemonic: {instruction.Mnemonic}");
Console.WriteLine($"Opcode: {instruction.OpcodeString}");
Console.WriteLine($"Size: {instruction.SizeInBytes} bytes");
Console.WriteLine($"Is Indexed: {instruction.IsIndexed}");
Console.WriteLine($"Index Register: {instruction.IndexedRegister}");
Console.WriteLine($"Accesses Memory: {instruction.AccessesMemory}");
Console.WriteLine($"Target: {instruction.Target}");
Console.WriteLine($"Source: {instruction.Source}");

// Output:
// Mnemonic: LD A,(IX+5)
// Opcode: DD7E05
// Size: 3 bytes
// Is Indexed: True
// Index Register: IX
// Accesses Memory: True
// Target: A
// Source: AddressFromIXAndOffset
```

### Instruction Timing Analysis
```csharp
// Analyze instruction timing
var instruction = InstructionSet.InstructionsByMnemonic["LDIR"];

Console.WriteLine($"Instruction: {instruction.Mnemonic}");
Console.WriteLine($"Is Looping: {instruction.IsLoopingInstruction}");
Console.WriteLine($"Machine Cycles: {instruction.MachineCycles.Cycles.Count()}");

foreach (var cycle in instruction.MachineCycles.Cycles)
{
    Console.WriteLine($"  {cycle.Type}: {cycle.TStates} T-states");
}

// Analyze conditional timing
var conditionalCall = InstructionSet.InstructionsByMnemonic["CALL Z,nn"];
Console.WriteLine($"\nConditional: {conditionalCall.Mnemonic}");
Console.WriteLine($"Condition: {conditionalCall.Condition}");
// Timing varies based on whether condition is met
```

### Custom Instruction Execution Monitoring
```csharp
var processor = new Processor();

// Monitor different instruction types
processor.BeforeExecuteInstruction += (sender, package) =>
{
    var inst = package.Instruction;
    
    if (inst.IsConditional)
    {
        bool willExecute = processor.Flags.SatisfyCondition(inst.Condition);
        Console.WriteLine($"Conditional {inst.Mnemonic}: {(willExecute ? "TAKEN" : "NOT TAKEN")}");
    }
    
    if (inst.IsIndexed)
    {
        ushort indexValue = processor.Registers[inst.IndexedRegister];
        sbyte displacement = (sbyte)package.Data.Argument1;
        ushort effectiveAddress = (ushort)(indexValue + displacement);
        Console.WriteLine($"Indexed {inst.Mnemonic}: {inst.IndexedRegister}=0x{indexValue:X4}, effective=0x{effectiveAddress:X4}");
    }
    
    if (inst.IsLoopingInstruction)
    {
        ushort counter = processor.Registers.BC;
        Console.WriteLine($"Block {inst.Mnemonic}: BC=0x{counter:X4} iterations remaining");
    }
};
```

### Instruction Set Statistics
```csharp
// Generate instruction set statistics
InstructionSet.Build();

var stats = new
{
    Total = InstructionSet.Instructions.Count,
    Conditional = InstructionSet.Instructions.Values.Count(i => i.IsConditional),
    Indexed = InstructionSet.Instructions.Values.Count(i => i.IsIndexed),
    MemoryAccess = InstructionSet.Instructions.Values.Count(i => i.AccessesMemory),
    IOAccess = InstructionSet.Instructions.Values.Count(i => i.PerformsIO),
    Looping = InstructionSet.Instructions.Values.Count(i => i.IsLoopingInstruction)
};

Console.WriteLine($"Z80 Instruction Set Statistics:");
Console.WriteLine($"  Total Instructions: {stats.Total}");
Console.WriteLine($"  Conditional: {stats.Conditional}");
Console.WriteLine($"  Indexed: {stats.Indexed}");
Console.WriteLine($"  Memory Access: {stats.MemoryAccess}");
Console.WriteLine($"  I/O Access: {stats.IOAccess}");
Console.WriteLine($"  Looping: {stats.Looping}");

// Breakdown by prefix
var prefixGroups = InstructionSet.Instructions.Values
    .GroupBy(i => i.Prefix)
    .OrderBy(g => g.Key);

Console.WriteLine("\nBy Prefix:");
foreach (var group in prefixGroups)
{
    string prefixName = group.Key switch
    {
        0 => "Main",
        0xCB => "CB (Bit)",
        0xDD => "DD (IX)",
        0xED => "ED (Extended)",
        0xFD => "FD (IY)",
        0xDDCB => "DDCB (IX+Bit)",
        0xFDCB => "FDCB (IY+Bit)",
        _ => $"0x{group.Key:X}"
    };
    Console.WriteLine($"  {prefixName}: {group.Count()} instructions");
}
```

### Dynamic Instruction Discovery
```csharp
// Find instructions by characteristics
public static class InstructionQuery
{
    public static IEnumerable<Instruction> FindByTarget(InstructionElement target)
    {
        return InstructionSet.Instructions.Values
            .Where(i => i.Target == target);
    }
    
    public static IEnumerable<Instruction> FindByMnemonic(string pattern)
    {
        return InstructionSet.InstructionsByMnemonic
            .Where(kvp => kvp.Key.Contains(pattern))
            .Select(kvp => kvp.Value);
    }
    
    public static IEnumerable<Instruction> FindMemoryInstructions()
    {
        return InstructionSet.Instructions.Values
            .Where(i => i.AccessesMemory);
    }
}

// Usage examples
var loadAInstructions = InstructionQuery.FindByTarget(InstructionElement.A)
    .Where(i => i.Mnemonic.StartsWith("LD A,"));

var rotateInstructions = InstructionQuery.FindByMnemonic("RL");

var memoryInstructions = InstructionQuery.FindMemoryInstructions()
    .Take(10);

foreach (var inst in loadAInstructions)
{
    Console.WriteLine($"{inst.Mnemonic} - {inst.OpcodeString}");
}
```

### Microcode Implementation Example
```csharp
// Example of how microcode is implemented (simplified)
public class LD : IMicrocode
{
    public ExecutionResult Execute(Processor cpu, InstructionPackage package)
    {
        var instruction = package.Instruction;
        var data = package.Data;
        
        // Get source value
        byte sourceValue = GetOperandValue(cpu, instruction.Source, data);
        
        // Set target
        SetOperandValue(cpu, instruction.Target, sourceValue, data);
        
        // LD instructions don't affect flags (except LD A,I and LD A,R)
        byte? newFlags = null;
        if (instruction.Source == InstructionElement.I || instruction.Source == InstructionElement.R)
        {
            var flags = new Flags(cpu.Registers.F);
            flags.Sign = sourceValue.GetBit(7);
            flags.Zero = sourceValue == 0;
            flags.HalfCarry = false;
            flags.ParityOverflow = cpu.Interrupts.Enabled; // IFF2 state
            flags.Subtract = false;
            // Carry unchanged
            newFlags = flags.Value;
        }
        
        return new ExecutionResult(instruction.MachineCycles.TotalTStates, newFlags);
    }
    
    private byte GetOperandValue(Processor cpu, InstructionElement element, InstructionData data)
    {
        return element switch
        {
            InstructionElement.A => cpu.Registers.A,
            InstructionElement.B => cpu.Registers.B,
            InstructionElement.ByteValue => data.Argument1,
            InstructionElement.AddressFromHL => cpu.Memory.ReadByteAt(cpu.Registers.HL),
            // ... handle all operand types
            _ => 0
        };
    }
    
    private void SetOperandValue(Processor cpu, InstructionElement element, byte value, InstructionData data)
    {
        switch (element)
        {
            case InstructionElement.A:
                cpu.Registers.A = value;
                break;
            case InstructionElement.B:
                cpu.Registers.B = value;
                break;
            case InstructionElement.AddressFromHL:
                cpu.Memory.WriteByteAt(cpu.Registers.HL, value);
                break;
            // ... handle all operand types
        }
    }
}
```

## Z80 Instruction Set Coverage

The Zem80_Core instruction system includes:

### Standard Instructions
- **Load Instructions**: LD, LDD, LDI, LDIR, LDDR
- **Arithmetic**: ADD, ADC, SUB, SBC, INC, DEC, NEG
- **Logical**: AND, OR, XOR, CP
- **Rotate/Shift**: RLCA, RLA, RRCA, RRA, RLC, RL, RRC, RR, SLA, SRA, SRL
- **Bit Operations**: BIT, SET, RES
- **Jump/Call**: JP, JR, CALL, RET, RST
- **Stack**: PUSH, POP
- **I/O**: IN, OUT
- **Block**: LDIR, LDDR, CPIR, CPDR, INIR, INDR, OTIR, OTDR
- **Control**: NOP, HALT, DI, EI, IM
- **Exchange**: EX, EXX

### Extended Instructions (ED prefix)
- **Extended I/O**: IN r,(C), OUT (C),r
- **16-bit Arithmetic**: ADC HL,ss, SBC HL,ss
- **Block Instructions**: LDI, LDD, CPI, CPD, INI, IND, OUTI, OUTD and repeat versions
- **Load**: LD (nn),dd, LD dd,(nn)
- **Misc**: NEG, RETI, RETN, RRD, RLD

### Index Instructions (DD/FD prefix)
- All standard instructions adapted for IX/IY registers
- Indexed addressing: (IX+d), (IY+d)
- Index register arithmetic and operations

### Bit Instructions (CB prefix)
- Single-bit operations: BIT, SET, RES
- All rotate/shift operations for all registers and memory locations

### Undocumented Instructions
- Partial index register operations (IXh, IXl, IYh, IYl)
- Undocumented flag behaviors
- Duplicate instruction opcodes
- SLL (Shift Left Logical) instruction

## Performance Optimization

The instruction system is optimized for performance:

- **Pre-built tables**: All instructions built once at startup
- **Direct microcode binding**: Each instruction directly references its implementation
- **Minimal lookup overhead**: Integer key lookup for instruction decode
- **Optimized operand handling**: Type-safe enum-based operand processing
- **Timing pre-calculation**: Machine cycle timing calculated once

## Thread Safety

Instruction objects are immutable after creation and thread-safe for reading. The InstructionSet class uses static collections that are populated once and then read-only.

## See Also

- [Processor](./Processor.md) - Processor class that executes instructions
- [Registers](./Registers.md) - Register system used by instructions
- [Memory](./Memory.md) - Memory system for instruction operands
- [ArithmeticExtensions](./ArithmeticExtensions.md) - Bit manipulation utilities used in microcode
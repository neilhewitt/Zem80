# Registers - Z80 Register Set Implementation

[? Back to Documentation Home](../README.md) | [? Core Components](../README.md#core-components)

The register system in Zem80 Core provides a complete implementation of the Z80's register set, including the main registers, shadow registers, index registers, and special-purpose registers.

## Overview

The Z80 processor features a rich register set that includes:
- **8-bit general-purpose registers** (A, B, C, D, E, H, L)
- **16-bit register pairs** (AF, BC, DE, HL)
- **Shadow register set** (AF', BC', DE', HL')
- **Index registers** (IX, IY) with high/low byte access
- **Special registers** (I, R, SP, PC)
- **Internal register** (WZ/MEMPTR)

## Interfaces

### IRegisters Interface

```csharp
public interface IRegisters
{
    // Indexer access
    byte this[ByteRegister register] { get; set; }
    ushort this[WordRegister registerPair] { get; set; }

    // 8-bit registers
    byte A { get; set; }    // Accumulator
    byte F { get; set; }    // Flags register
    byte B { get; set; }    // General purpose
    byte C { get; set; }    // General purpose
    byte D { get; set; }    // General purpose
    byte E { get; set; }    // General purpose
    byte H { get; set; }    // General purpose
    byte L { get; set; }    // General purpose

    // 16-bit register pairs
    ushort AF { get; set; } // Accumulator + Flags
    ushort BC { get; set; } // B + C
    ushort DE { get; set; } // D + E
    ushort HL { get; set; } // H + L

    // Index registers
    ushort IX { get; set; } // Index register X
    ushort IY { get; set; } // Index register Y
    byte IXh { get; set; }  // IX high byte
    byte IXl { get; set; }  // IX low byte
    byte IYh { get; set; }  // IY high byte
    byte IYl { get; set; }  // IY low byte

    // Special registers
    byte I { get; set; }    // Interrupt vector
    byte R { get; set; }    // Memory refresh
    ushort IR { get; }      // I + R as 16-bit (read-only)
    ushort PC { get; set; } // Program counter
    ushort SP { get; set; } // Stack pointer
    ushort WZ { get; set; } // Internal register (MEMPTR)

    // Shadow registers access
    IShadowRegisters Shadow { get; }

    // Operations
    void Clear();
    void ExchangeAF();
    void ExchangeBCDEHL();
    Registers Snapshot();
}
```

### IShadowRegisters Interface

```csharp
public interface IShadowRegisters
{
    ushort AF { get; set; } // Shadow AF register
    ushort BC { get; set; } // Shadow BC register
    ushort DE { get; set; } // Shadow DE register  
    ushort HL { get; set; } // Shadow HL register
}
```

## Register Enumerations

### ByteRegister Enumeration

```csharp
public enum ByteRegister
{
    None = 26,
    A = 0,      // Accumulator
    F = 1,      // Flags
    B = 2,      // General purpose
    C = 3,      // General purpose
    D = 4,      // General purpose
    E = 5,      // General purpose
    H = 6,      // General purpose
    L = 7,      // General purpose
    IXh = 16,   // IX high byte
    IXl = 17,   // IX low byte
    IYh = 18,   // IY high byte
    IYl = 19,   // IY low byte
    I = 22,     // Interrupt vector
    R = 23      // Memory refresh
}
```

### WordRegister Enumeration

```csharp
public enum WordRegister
{
    None = 26,
    AF = 0,     // Accumulator + Flags
    BC = 2,     // B + C pair
    DE = 4,     // D + E pair
    HL = 6,     // H + L pair
    IX = 16,    // Index register X
    IY = 18,    // Index register Y
    SP = 20,    // Stack pointer
    PC = 24     // Program counter
}
```

## Usage Examples

### Basic Register Access

```csharp
var processor = new Processor();
var registers = processor.Registers;

// 8-bit register access
registers.A = 0x42;          // Set accumulator
byte value = registers.B;    // Read B register

// 16-bit register pair access
registers.HL = 0x8000;       // Set HL pair
ushort address = registers.BC; // Read BC pair

// Index register access
registers.IX = 0x9000;       // Set IX register
registers.IXh = 0x90;        // Set IX high byte
registers.IXl = 0x00;        // Set IX low byte
```

### Enumeration-Based Access

```csharp
// Using byte register enumeration
registers[ByteRegister.A] = 0xFF;
byte accumulator = registers[ByteRegister.A];

// Using word register enumeration
registers[WordRegister.HL] = 0x1234;
ushort hlValue = registers[WordRegister.HL];

// Useful for dynamic register selection
void SetRegister(ByteRegister reg, byte value)
{
    registers[reg] = value;
}

SetRegister(ByteRegister.B, 0x42);
```

### Shadow Register Operations

```csharp
// Exchange AF with AF'
registers.AF = 0x1234;
registers.ExchangeAF();
// Now AF contains the previous AF' value
// and AF' contains 0x1234

// Exchange BC, DE, HL with BC', DE', HL'
registers.BC = 0x1111;
registers.DE = 0x2222;
registers.HL = 0x3333;
registers.ExchangeBCDEHL();
// Now BC, DE, HL contain their shadow values

// Direct shadow register access (for debugging)
ushort shadowAF = registers.Shadow.AF;
registers.Shadow.BC = 0x4444;
```

### Special Register Usage

```csharp
// Program counter manipulation
registers.PC = 0x8000;       // Jump to address
ushort currentPC = registers.PC;

// Stack pointer management
registers.SP = 0xFFFF;       // Set stack to top of memory
ushort stackTop = registers.SP;

// Interrupt vector register
registers.I = 0x80;          // Set interrupt vector base

// Memory refresh register
byte refreshValue = registers.R; // Read current refresh value
registers.R = 0x00;          // Reset refresh counter

// Internal WZ register (MEMPTR)
registers.WZ = 0x1234;       // Used internally by some instructions
```

## Register State Management

### Clearing Registers

```csharp
// Clear all registers to default values
registers.Clear();

// After clear:
// - All 8-bit registers = 0x00
// - All 16-bit registers = 0x0000
// - Shadow registers = 0x0000
// - WZ register = 0x0000
```

### Creating Snapshots

```csharp
// Create a snapshot of current register state
Registers snapshot = registers.Snapshot();

// The snapshot is a complete copy
snapshot.A = 0xFF;           // Doesn't affect original
byte originalA = registers.A; // Still original value

// Useful for save states or debugging
void SaveProcessorState()
{
    var registerState = processor.Registers.Snapshot();
    // Store registerState for later restoration
}
```

## Internal Implementation Details

### Memory Layout

The `Registers` class stores all register values in a single byte array:

```csharp
private byte[] _registers = new byte[26];

// Layout:
// [0-1]   AF (A=0, F=1)
// [2-3]   BC (B=2, C=3)
// [4-5]   DE (D=4, E=5)
// [6-7]   HL (H=6, L=7)
// [8-9]   AF' shadow
// [10-11] BC' shadow
// [12-13] DE' shadow
// [14-15] HL' shadow
// [16-17] IX (IXh=16, IXl=17)
// [18-19] IY (IYh=18, IYl=19)
// [20-21] SP
// [22]    I
// [23]    R
// [24-25] PC
```

### 16-bit Value Handling

```csharp
// 16-bit values are stored in big-endian format internally
// High byte first, then low byte
private ushort Get16BitValue(int offset)
{
    return (ushort)((_registers[offset] * 256) + _registers[offset + 1]);
}

private void Set16BitValue(int offset, ushort value)
{
    _registers[offset] = (byte)(value / 256);      // High byte
    _registers[offset + 1] = (byte)(value % 256);  // Low byte
}
```

## Register Behavior Notes

### Accumulator (A Register)
- Primary register for arithmetic and logical operations
- Target for most data manipulation instructions
- Automatically updated by ALU operations

### Flags Register (F Register)
- Contains processor status flags
- Updated automatically by most instructions
- Should typically be accessed via the Flags interface rather than directly

### Index Registers (IX, IY)
- Can be accessed as 16-bit values or separate high/low bytes
- Used for indexed addressing modes
- IXh/IXl and IYh/IYl provide byte-level access

### Memory Refresh Register (R)
- Automatically incremented during memory refresh cycles
- Only lower 7 bits increment, bit 7 is preserved
- Can be read and written by software

### Internal WZ Register
- Not visible to Z80 programs
- Used internally by the processor for temporary address storage
- Affects the behavior of some undocumented instructions
- Sometimes called MEMPTR in Z80 documentation

## Thread Safety

The register implementation is **not thread-safe**. External synchronization is required when:
- Accessing registers from multiple threads
- Reading/writing during processor execution
- Taking snapshots while processor is running

## Performance Considerations

- Register access is highly optimized with direct array indexing
- 16-bit operations use efficient bit manipulation
- Enumeration-based access has minimal overhead
- Snapshot creation performs a single array copy

## Example: Register Manipulation Program

```csharp
using Zem80.Core.CPU;

// Create processor and access registers
var processor = new Processor();
var regs = processor.Registers;

// Initialize registers
regs.A = 0x10;
regs.B = 0x20;
regs.HL = 0x8000;
regs.SP = 0xFFFF;

// Exchange with shadow registers
Console.WriteLine($"HL before exchange: {regs.HL:X4}");
regs.ExchangeBCDEHL();
Console.WriteLine($"HL after exchange: {regs.HL:X4}");

// Use index registers
regs.IX = 0x9000;
Console.WriteLine($"IX: {regs.IX:X4}");
Console.WriteLine($"IXh: {regs.IXh:X2}, IXl: {regs.IXl:X2}");

// Modify index register bytes
regs.IXh = 0xA0;
Console.WriteLine($"IX after IXh change: {regs.IX:X4}");

// Create and compare snapshots
var snapshot1 = regs.Snapshot();
regs.A = 0xFF;
var snapshot2 = regs.Snapshot();

Console.WriteLine($"Snapshot1 A: {snapshot1.A:X2}");
Console.WriteLine($"Snapshot2 A: {snapshot2.A:X2}");
```

## Related Topics

- **[Flags](Flags.md)** - Processor flags and status register
- **[Processor](Processor.md)** - Main processor implementation
- **[Instruction Set](../Instructions/InstructionSet.md)** - Instructions that manipulate registers
- **[Debug Interface](../Advanced/Debug.md)** - Register inspection and modification

---

*[? Back to Documentation Home](../README.md)*
# Register System

The Zem80_Core register system provides complete emulation of the Z80's register set, including main registers, shadow registers, index registers, and special registers. The system is designed for both performance and ease of use.

## Core Interfaces and Classes

### IRegisters Interface

The main interface for accessing Z80 registers.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IRegisters
```

#### Properties

##### Indexer Access
```csharp
byte this[ByteRegister register] { get; set; }
ushort this[WordRegister registerPair] { get; set; }
```
Provides indexed access to registers using enums for type safety.

**Examples:**
```csharp
// Set accumulator using indexer
registers[ByteRegister.A] = 0x42;

// Set HL register pair using indexer  
registers[WordRegister.HL] = 0x1234;
```

##### Main 8-bit Registers
```csharp
byte A { get; set; }    // Accumulator
byte B { get; set; }    // B register
byte C { get; set; }    // C register  
byte D { get; set; }    // D register
byte E { get; set; }    // E register
byte F { get; set; }    // Flags register
byte H { get; set; }    // H register (high byte of HL)
byte L { get; set; }    // L register (low byte of HL)
```

##### 16-bit Register Pairs
```csharp
ushort AF { get; set; }  // A and F as a pair
ushort BC { get; set; }  // B and C as a pair
ushort DE { get; set; }  // D and E as a pair  
ushort HL { get; set; }  // H and L as a pair
```

##### Index Registers
```csharp
ushort IX { get; set; }  // Index register X
ushort IY { get; set; }  // Index register Y
byte IXh { get; set; }   // High byte of IX
byte IXl { get; set; }   // Low byte of IX
byte IYh { get; set; }   // High byte of IY
byte IYl { get; set; }   // Low byte of IY
```

##### Special Registers
```csharp
ushort PC { get; set; }  // Program Counter
ushort SP { get; set; }  // Stack Pointer
byte I { get; set; }     // Interrupt Vector register
byte R { get; set; }     // Memory Refresh register
ushort IR { get; }       // I and R as a read-only pair
ushort WZ { get; set; }  // Internal temporary register (MEMPTR)
```

##### Shadow Registers
```csharp
IShadowRegisters Shadow { get; }
```
Access to shadow register set (AF', BC', DE', HL').

#### Methods

##### Clear
```csharp
void Clear()
```
Resets all registers to zero.

**Example:**
```csharp
registers.Clear();
// All registers are now 0x00 or 0x0000
```

##### ExchangeAF
```csharp
void ExchangeAF()
```
Exchanges AF with AF' (shadow registers).

**Example:**
```csharp
registers.AF = 0x1234;
registers.ExchangeAF();
// Now AF contains what was in AF', and AF' contains 0x1234
```

##### ExchangeBCDEHL
```csharp
void ExchangeBCDEHL()
```
Exchanges BC, DE, HL with BC', DE', HL' (shadow registers).

**Example:**
```csharp
registers.BC = 0x1111;
registers.DE = 0x2222; 
registers.HL = 0x3333;

registers.ExchangeBCDEHL();
// Register pairs now contain shadow values, shadows contain original values
```

##### Snapshot
```csharp
Registers Snapshot()
```
Creates a complete copy of the current register state.

**Example:**
```csharp
var snapshot = registers.Snapshot();
// snapshot contains a complete copy of all register values
```

## IShadowRegisters Interface

Provides access to the Z80's shadow register set.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IShadowRegisters
```

#### Properties
```csharp
ushort AF { get; set; }  // Shadow AF register pair
ushort BC { get; set; }  // Shadow BC register pair
ushort DE { get; set; }  // Shadow DE register pair
ushort HL { get; set; }  // Shadow HL register pair
```

**Example:**
```csharp
// Access shadow registers directly
registers.Shadow.AF = 0x5678;
registers.Shadow.HL = 0x9ABC;

// Exchange to make them active
registers.ExchangeAF();
registers.ExchangeBCDEHL();

// Now AF = 0x5678, HL = 0x9ABC
```

## Registers Class Implementation

The concrete implementation of the IRegisters interface.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public class Registers : IShadowRegisters, IRegisters
```

#### Internal Structure
- Uses internal byte array for efficient storage and copying
- Implements little-endian word access (low byte first)
- Provides both individual and paired register access
- Maintains shadow registers in same storage array

#### Constructor
```csharp
public Registers()
```
Creates a new register set with all values initialized to zero.

**Example:**
```csharp
var registers = new Registers();
// All registers initialized to 0
```

## Register Enumerations

### ByteRegister Enumeration

Enumeration for accessing 8-bit registers by name.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public enum ByteRegister
```

#### Values
```csharp
None = 26,  // No register (used for instructions that don't target a register)
A = 0,      // Accumulator
F = 1,      // Flags
B = 2,      // B register
C = 3,      // C register
D = 4,      // D register
E = 5,      // E register
H = 6,      // H register
L = 7,      // L register
IXh = 16,   // High byte of IX
IXl = 17,   // Low byte of IX
IYh = 18,   // High byte of IY
IYl = 19,   // Low byte of IY
I = 22,     // Interrupt vector
R = 23      // Memory refresh
```

### WordRegister Enumeration

Enumeration for accessing 16-bit register pairs by name.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public enum WordRegister
```

#### Values
```csharp
None = 26,  // No register pair
AF = 0,     // A and F pair
BC = 2,     // B and C pair
DE = 4,     // D and E pair
HL = 6,     // H and L pair
IX = 16,    // Index register X
IY = 18,    // Index register Y
SP = 20,    // Stack pointer
PC = 24     // Program counter
```

## Flag System

### IReadOnlyFlags Interface

Provides read-only access to Z80 flags for safe inspection.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IReadOnlyFlags
```

#### Properties
```csharp
bool Sign { get; }           // S flag (bit 7) - set if result is negative
bool Zero { get; }           // Z flag (bit 6) - set if result is zero
bool Y { get; }              // Undocumented flag (bit 5) - copy of bit 5 of result
bool HalfCarry { get; }      // H flag (bit 4) - set if carry from bit 3 to bit 4
bool X { get; }              // Undocumented flag (bit 3) - copy of bit 3 of result
bool ParityOverflow { get; } // P/V flag (bit 2) - parity or overflow
bool Subtract { get; }       // N flag (bit 1) - set if last operation was subtraction
bool Carry { get; }          // C flag (bit 0) - set if carry occurred
byte Value { get; }          // Raw flag byte value
FlagState State { get; }     // Flags as enumeration
```

#### Methods

##### SatisfyCondition
```csharp
bool SatisfyCondition(Condition condition)
```
Tests if flags satisfy a conditional instruction's requirements.

**Example:**
```csharp
// Check if conditional jump should be taken
if (flags.SatisfyCondition(Condition.Z))
{
    // Jump because Zero flag is set
}
```

##### Clone
```csharp
Flags Clone()
```
Creates a copy of the current flags state.

### Flags Class Implementation

Concrete implementation providing full flag manipulation.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public class Flags : IReadOnlyFlags
```

#### Additional Properties (Writable)
All IReadOnlyFlags properties are available as read-write in the concrete class.

#### Methods

##### Reset
```csharp
public void Reset()
```
Clears all flags to zero.

## Usage Examples

### Basic Register Operations
```csharp
var registers = new Registers();

// Set individual registers
registers.A = 0x42;
registers.B = 0x12;
registers.C = 0x34;

// Access as register pairs
ushort bc = registers.BC; // Returns 0x1234

// Modify pairs
registers.HL = 0x8000;
byte h = registers.H; // Returns 0x80
byte l = registers.L; // Returns 0x00
```

### Shadow Register Operations
```csharp
var registers = new Registers();

// Set main registers
registers.AF = 0x1234;
registers.BC = 0x5678;

// Set shadow registers directly
registers.Shadow.AF = 0xABCD;
registers.Shadow.BC = 0xEF00;

// Exchange to make shadows active
registers.ExchangeAF();       // AF now 0xABCD, AF' now 0x1234
registers.ExchangeBCDEHL();   // BC now 0xEF00, BC' now 0x5678
```

### Index Register Operations
```csharp
var registers = new Registers();

// Set index registers
registers.IX = 0x2000;
registers.IY = 0x3000;

// Access high and low bytes
byte ixHigh = registers.IXh; // 0x20
byte ixLow = registers.IXl;  // 0x00

// Modify individual bytes
registers.IYh = 0x40;        // IY becomes 0x4000
```

### Flag Operations
```csharp
var processor = new Processor();

// Access flags through processor
IReadOnlyFlags flags = processor.Flags;

// Check individual flags
if (flags.Zero)
{
    Console.WriteLine("Last operation result was zero");
}

if (flags.Carry)
{
    Console.WriteLine("Carry occurred in last operation");
}

// Check conditional execution
if (flags.SatisfyCondition(Condition.NZ))
{
    Console.WriteLine("Would execute conditional instruction");
}
```

### Indexed Register Access
```csharp
var registers = new Registers();

// Using enums for type safety
registers[ByteRegister.A] = 0xFF;
registers[ByteRegister.B] = 0x00;

// Register pair access
registers[WordRegister.HL] = 0x1000;
registers[WordRegister.SP] = 0xFFFF;

// Loop through all main registers
var mainRegs = new[] { ByteRegister.A, ByteRegister.B, ByteRegister.C, 
                      ByteRegister.D, ByteRegister.E, ByteRegister.H, ByteRegister.L };
                      
foreach (var reg in mainRegs)
{
    Console.WriteLine($"{reg}: 0x{registers[reg]:X2}");
}
```

### Register State Debugging
```csharp
var registers = new Registers();

// Create snapshot for comparison
var before = registers.Snapshot();

// Execute some operations...
registers.A = 0x42;
registers.HL = 0x1234;

// Compare states
Console.WriteLine($"A changed: {before.A:X2} -> {registers.A:X2}");
Console.WriteLine($"HL changed: {before.HL:X4} -> {registers.HL:X4}");
```

### Stack Operations Context
```csharp
var registers = new Registers();
var processor = new Processor();

// Set up stack
registers.SP = 0xFFFF;  // Top of memory

// Typical interrupt context save
ushort returnAddress = registers.PC;
processor.Stack.Push(returnAddress);  // Stack pointer automatically adjusted

// Later restore
registers.PC = processor.Stack.Pop();
```

### Memory Addressing Examples
```csharp
var registers = new Registers();

// Indirect addressing through HL
registers.HL = 0x8000;
byte value = processor.Memory.ReadByteAt(registers.HL);

// Indexed addressing
registers.IX = 0x2000;
sbyte displacement = 5;
ushort effectiveAddress = (ushort)(registers.IX + displacement);
byte indexedValue = processor.Memory.ReadByteAt(effectiveAddress);

// Auto-increment addressing (like (HL)+)
byte data = processor.Memory.ReadByteAt(registers.HL);
registers.HL++;  // Move to next address
```

## Performance Considerations

- Register access is highly optimized using direct array indexing
- 16-bit register pairs are stored as consecutive bytes for efficient access
- Shadow register exchange uses fast memory swapping
- Snapshots create new instances but share no references (safe for parallel access)

## Thread Safety

The register system is **not** inherently thread-safe. When the processor is running:

- **Safe operations**: Reading register values for debugging/inspection
- **Unsafe operations**: Writing to registers (except through processor events)

For thread-safe inspection, use the `Snapshot()` method to capture register state.

## Z80 Compatibility Notes

The register implementation maintains full compatibility with Z80 behavior:

- **Little-endian storage**: Low byte stored at lower address
- **Shadow registers**: Complete implementation of EX AF,AF' and EXX instructions
- **Index registers**: Full IX/IY support with high/low byte access
- **Undocumented registers**: WZ (MEMPTR) register for accurate BIT instruction behavior
- **Flag behavior**: All documented and undocumented flag behaviors supported

## See Also

- [Processor](./Processor.md) - Main processor class that uses the register system
- [ArithmeticExtensions](./ArithmeticExtensions.md) - Utilities for register manipulation
- [Instructions](./Instructions.md) - Instruction system that operates on registers
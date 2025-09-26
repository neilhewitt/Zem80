# ArithmeticExtensions Class

The `ArithmeticExtensions` class provides static extension methods for bit manipulation and arithmetic operations commonly used in Z80 emulation. These methods optimize performance-critical operations like bit manipulation, byte/word conversions, and parity calculations.

## Namespace
`Zem80.Core`

## Declaration
```csharp
public static class ArithmeticExtensions
```

## Methods

### Byte Extraction from Word

#### LowByte
```csharp
public static byte LowByte(this ushort input)
```
Extracts the low byte (bits 0-7) from a 16-bit word.

**Parameters:**
- `input`: The 16-bit value to extract from

**Returns:** The low 8 bits as a byte value

**Example:**
```csharp
ushort value = 0x1234;
byte low = value.LowByte(); // Returns 0x34
```

#### HighByte
```csharp
public static byte HighByte(this ushort input)
```
Extracts the high byte (bits 8-15) from a 16-bit word.

**Parameters:**
- `input`: The 16-bit value to extract from

**Returns:** The high 8 bits as a byte value

**Example:**
```csharp
ushort value = 0x1234;
byte high = value.HighByte(); // Returns 0x12
```

### Nybble (4-bit) Operations

#### GetHighNybble
```csharp
public static byte GetHighNybble(this byte input)
```
Extracts the high nybble (bits 4-7) and returns it as the low nybble of a new byte.

**Parameters:**
- `input`: The byte to extract from

**Returns:** The high 4 bits shifted to the low nybble position

**Example:**
```csharp
byte value = 0xAB;
byte highNybble = value.GetHighNybble(); // Returns 0x0A
```

#### GetLowNybble
```csharp
public static byte GetLowNybble(this byte input)
```
Extracts the low nybble (bits 0-3) from a byte.

**Parameters:**
- `input`: The byte to extract from

**Returns:** The low 4 bits as a byte (high nybble cleared)

**Example:**
```csharp
byte value = 0xAB;
byte lowNybble = value.GetLowNybble(); // Returns 0x0B
```

#### SetHighNybble
```csharp
public static byte SetHighNybble(this byte input, byte from)
```
Sets the high nybble of the input byte using the low nybble of the `from` byte.

**Parameters:**
- `input`: The target byte to modify
- `from`: Source byte (only low nybble is used)

**Returns:** New byte with modified high nybble

**Example:**
```csharp
byte target = 0x0F;
byte source = 0x3A;
byte result = target.SetHighNybble(source); // Returns 0xAF
```

#### SetLowNybble
```csharp
public static byte SetLowNybble(this byte input, byte from)
```
Sets the low nybble of the input byte using the low nybble of the `from` byte.

**Parameters:**
- `input`: The target byte to modify
- `from`: Source byte (only low nybble is used)

**Returns:** New byte with modified low nybble

**Example:**
```csharp
byte target = 0xF0;
byte source = 0x3A;
byte result = target.SetLowNybble(source); // Returns 0xFA
```

### Bit Field Operations

#### GetByteFromBits
```csharp
public static byte GetByteFromBits(this byte input, int startIndex, int numberOfBits)
```
Extracts a specific range of bits from a byte.

**Parameters:**
- `input`: The byte to extract bits from
- `startIndex`: Starting bit position (0-7)
- `numberOfBits`: Number of bits to extract (1-8)

**Returns:** The extracted bits shifted to the low positions

**Example:**
```csharp
byte value = 0b11010110; // 0xD6
byte extracted = value.GetByteFromBits(2, 3); // Extract bits 2-4
// Result: 0b101 (0x05) - bits 4,3,2 were 1,0,1
```

### Single Bit Operations

#### SetBit (byte)
```csharp
public static byte SetBit(this byte input, int bitIndex, bool state)
```
Sets or clears a specific bit in a byte.

**Parameters:**
- `input`: The byte to modify
- `bitIndex`: Bit position to modify (0-7)
- `state`: True to set bit, false to clear bit

**Returns:** New byte with the bit modified

**Example:**
```csharp
byte value = 0b00000000;
byte result = value.SetBit(3, true); // Returns 0b00001000 (0x08)

byte value2 = 0b11111111;
byte result2 = value2.SetBit(3, false); // Returns 0b11110111 (0xF7)
```

#### SetBit (ushort)
```csharp
public static ushort SetBit(this ushort input, int bitIndex, bool state)
```
Sets or clears a specific bit in a 16-bit word.

**Parameters:**
- `input`: The word to modify
- `bitIndex`: Bit position to modify (0-15)
- `state`: True to set bit, false to clear bit

**Returns:** New word with the bit modified

**Example:**
```csharp
ushort value = 0x0000;
ushort result = value.SetBit(12, true); // Returns 0x1000
```

#### GetBit (byte)
```csharp
public static bool GetBit(this byte input, int bitIndex)
```
Gets the state of a specific bit in a byte.

**Parameters:**
- `input`: The byte to test
- `bitIndex`: Bit position to test (0-7)

**Returns:** True if bit is set, false if clear

**Example:**
```csharp
byte value = 0b10110100; // 0xB4
bool bit5 = value.GetBit(5); // Returns true (bit 5 is set)
bool bit0 = value.GetBit(0); // Returns false (bit 0 is clear)
```

#### GetBit (ushort)
```csharp
public static bool GetBit(this ushort input, int bitIndex)
```
Gets the state of a specific bit in a 16-bit word.

**Parameters:**
- `input`: The word to test
- `bitIndex`: Bit position to test (0-15)

**Returns:** True if bit is set, false if clear

**Example:**
```csharp
ushort value = 0x8001;
bool bit15 = value.GetBit(15); // Returns true
bool bit1 = value.GetBit(1);   // Returns false
```

### Bitwise Operations

#### Invert
```csharp
public static byte Invert(this byte input)
```
Performs bitwise NOT operation on a byte (flips all bits).

**Parameters:**
- `input`: The byte to invert

**Returns:** Byte with all bits flipped

**Example:**
```csharp
byte value = 0b10101010; // 0xAA
byte inverted = value.Invert(); // Returns 0b01010101 (0x55)
```

#### EvenParity
```csharp
public static bool EvenParity(this byte input)
```
Calculates even parity for a byte (counts set bits). Uses optimized bit-counting algorithm for performance.

**Parameters:**
- `input`: The byte to check parity for

**Returns:** True if even number of bits are set, false for odd

**Example:**
```csharp
byte value1 = 0b00000011; // 2 bits set
bool parity1 = value1.EvenParity(); // Returns true (even)

byte value2 = 0b00000111; // 3 bits set  
bool parity2 = value2.EvenParity(); // Returns false (odd)
```

**Implementation Note:** This method uses an optimized bit-counting algorithm that avoids loops for better performance in instruction emulation.

### Word Construction

#### ToWord
```csharp
public static ushort ToWord(this (byte low, byte high) bytePair)
```
Combines two bytes into a 16-bit word using little-endian byte order.

**Parameters:**
- `bytePair`: Tuple containing low and high bytes

**Returns:** 16-bit word constructed from the byte pair

**Example:**
```csharp
var bytes = (low: (byte)0x34, high: (byte)0x12);
ushort word = bytes.ToWord(); // Returns 0x1234

// Can also be used with named tuples
(byte low, byte high) registers = (0xCD, 0xAB);
ushort result = registers.ToWord(); // Returns 0xABCD
```

## Usage Examples

### Z80 Register Pair Operations
```csharp
// Simulating Z80 register pair operations
ushort HL = 0x1234;
byte H = HL.HighByte(); // Get H register (0x12)
byte L = HL.LowByte();  // Get L register (0x34)

// Modify and reconstruct
H = 0x56;
L = 0x78;
HL = (L, H).ToWord(); // HL = 0x5678
```

### Flag Register Manipulation
```csharp
byte flags = 0x00;

// Set specific flags
flags = flags.SetBit(0, true);  // Set Carry flag
flags = flags.SetBit(6, true);  // Set Zero flag  
flags = flags.SetBit(7, true);  // Set Sign flag

// Check flags
bool carrySet = flags.GetBit(0);
bool zeroSet = flags.GetBit(6);
bool signSet = flags.GetBit(7);

// Calculate parity for Parity/Overflow flag
byte result = 0x42;
bool evenParity = result.EvenParity();
flags = flags.SetBit(2, evenParity); // Set P/V flag
```

### BCD (Binary Coded Decimal) Operations
```csharp
byte bcdValue = 0x59; // BCD representation of decimal 59

// Extract BCD digits
byte tensDigit = bcdValue.GetHighNybble(); // 5
byte onesDigit = bcdValue.GetLowNybble();  // 9

// Create new BCD value
byte newBcd = ((byte)0).SetHighNybble(7).SetLowNybble(3); // Creates 0x73 (BCD 73)
```

### Bit Field Extraction (for instruction decoding)
```csharp
byte instruction = 0b11001011; // CB prefix instruction

// Extract register field (bits 0-2)
byte register = instruction.GetByteFromBits(0, 3); // Gets 0b011 (3)

// Extract operation field (bits 3-5)  
byte operation = instruction.GetByteFromBits(3, 3); // Gets 0b001 (1)

// Extract bit number (bits 6-7 for bit instructions)
byte bitNum = instruction.GetByteFromBits(6, 2); // Gets 0b11 (3)
```

### Memory Address Calculation
```csharp
// Z80 indexed addressing: (IX + displacement)
ushort IX = 0x2000;
sbyte displacement = -5; // Signed displacement

// Calculate effective address
ushort effectiveAddress = (ushort)(IX + displacement);

// Extract address components
byte addressLow = effectiveAddress.LowByte();
byte addressHigh = effectiveAddress.HighByte();
```

## Performance Notes

- **EvenParity**: Uses optimized bit-counting algorithm instead of loops for better performance
- **Bit operations**: Use bitwise operations instead of mathematical operations where possible
- **Memory allocation**: All methods return values rather than allocating objects
- **Inlining**: Methods are candidates for JIT inlining due to their simple nature

## Z80 Specific Applications

These extensions are specifically designed for Z80 emulation needs:

- **LowByte/HighByte**: Used for register pair access (BC, DE, HL, IX, IY)
- **SetBit/GetBit**: Flag register manipulation and bit test instructions
- **GetByteFromBits**: Instruction field extraction during decode
- **EvenParity**: Parity flag calculation for arithmetic operations
- **Nybble operations**: BCD arithmetic and display applications
- **ToWord**: Little-endian word construction for memory and register operations

## See Also

- [Processor](./Processor.md) - Main processor class that uses these extensions
- [Registers](./Registers.md) - Register system that relies on byte/word conversions  
- [Instructions](./Instructions.md) - Instruction system that uses bit manipulation
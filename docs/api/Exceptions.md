# Exception System

The Zem80_Core exception system provides a comprehensive hierarchy of exceptions for different error conditions that can occur during Z80 emulation.

## Base Exception

### Z80Exception

Base exception class for all Zem80_Core exceptions.

#### Namespace
`Zem80.Core`

#### Declaration
```csharp
public class Z80Exception : Exception
```

Standard exception constructors are provided for message, inner exception, and serialization scenarios.

## Memory-Related Exceptions

### MemoryException
Base class for all memory-related errors.

### MemoryNotInitialisedException
Thrown when attempting to use memory bank before initialization.

### MemoryNotPresentException
Thrown when accessing unmapped memory addresses.

### MemoryNotWritableException
Thrown when attempting to write to read-only memory.

### MemoryMapException
Thrown for invalid memory mapping operations.

### MemorySegmentException
Thrown for invalid memory segment configurations.

### StackNotInitialisedException
Thrown when stack operations are attempted without proper stack initialization.

## Instruction-Related Exceptions

### InstructionNotFoundException
Thrown when processor encounters an unknown opcode.

### InstructionDecoderException
Thrown when instruction decoder encounters invalid instruction format.

## I/O and Hardware Exceptions

### InterruptException
Thrown for invalid interrupt operations or configurations.

### ClockException
Thrown for clock-related errors.

### TimingException
Thrown for timing configuration or synchronization errors.

## Usage Examples

### Exception Handling in Emulation
```csharp
var processor = new Processor();

try
{
    // Load potentially invalid program
    processor.Memory.WriteBytesAt(0x0000, suspiciousProgram);
    processor.Start(endOnHalt: true);
    processor.RunUntilStopped();
}
catch (MemoryNotWritableException ex)
{
    Console.WriteLine($"Attempted to write to ROM: {ex.Message}");
}
catch (InstructionNotFoundException ex)
{
    Console.WriteLine($"Invalid opcode encountered: {ex.Message}");
}
catch (Z80Exception ex)
{
    Console.WriteLine($"Z80 emulation error: {ex.Message}");
}
```

### Memory Access Validation
```csharp
try
{
    byte value = processor.Memory.ReadByteAt(0xFFFF);
}
catch (MemoryNotPresentException)
{
    Console.WriteLine("Address not mapped in memory");
}
```

### Safe Memory Operations
```csharp
public static bool TryWriteMemory(IMemoryBank memory, ushort address, byte value)
{
    try
    {
        memory.WriteByteAt(address, value);
        return true;
    }
    catch (MemoryNotWritableException)
    {
        return false; // ROM or unmapped
    }
    catch (MemoryNotPresentException)
    {
        return false; // Address not mapped
    }
}
```

## Error Recovery

The exception system is designed to allow graceful error handling and recovery:

```csharp
processor.BeforeExecuteInstruction += (sender, package) =>
{
    try
    {
        // Validate instruction before execution
        if (package.Instruction == null)
        {
            throw new InstructionNotFoundException($"Invalid opcode at {package.InstructionAddress:X4}");
        }
    }
    catch (InstructionNotFoundException ex)
    {
        Console.WriteLine($"Skipping invalid instruction: {ex.Message}");
        processor.Registers.PC++; // Skip bad opcode
    }
};
```

## See Also

- [Processor](./Processor.md) - Processor operations that may throw exceptions
- [Memory](./Memory.md) - Memory operations and related exceptions
- [Instructions](./Instructions.md) - Instruction system exceptions
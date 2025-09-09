# DebugProcessor & IDebugProcessor

[? Back to CPU API](README.md)

## DebugProcessor Class

- **Namespace**: `Zem80.Core.CPU`
- **Implements**: `IDebugProcessor`

Provides debugging support for the CPU, including breakpoints and direct instruction execution.

### Key Methods

- `void AddBreakpoint(ushort address)`: Adds a breakpoint at the specified address.
- `void RemoveBreakpoint(ushort address)`: Removes a breakpoint.
- `void EvaluateAndRunBreakpoint(ushort address, InstructionPackage executingPackage)`: Checks if a breakpoint is hit and triggers the event.
- `void ExecuteDirect(byte[] opcode)`: Executes an instruction directly by opcode bytes.
- `void ExecuteDirect(string mnemonic, byte? arg1, byte? arg2)`: Executes an instruction directly by mnemonic and arguments.

### Example Usage

```csharp
var debug = new DebugProcessor(cpu, cpu.ExecuteInstruction);
debug.AddBreakpoint(0x1234);
```

## IDebugProcessor Interface

Defines the contract for debugging support.

---

[? Back to CPU API](README.md) | [API Index](../README.md)

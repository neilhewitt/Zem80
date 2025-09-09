# Usage Examples

[? Back to Main Index](../README.md)

This page provides practical usage examples for Zem80 Core.

## Basic CPU Startup

```csharp
using Zem80.Core.CPU;

var cpu = new Processor();
cpu.Start(0x0000);
// ...
cpu.Stop();
```

## Setting a Breakpoint

```csharp
cpu.Debug.AddBreakpoint(0x1234);
```

## Custom Memory Implementation

```csharp
public class MyMemory : IMemoryBank { /* ... */ }
var cpu = new Processor(memory: new MyMemory());
```

## Direct Instruction Execution

```csharp
cpu.Debug.ExecuteDirect(new byte[] { 0x3E, 0x42 }); // LD A,42h
```

---

[? Back to Main Index](../README.md)

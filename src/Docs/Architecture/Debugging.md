# Debugging & Breakpoints

[? Back to Architecture & Design](README.md)

## Debugging Support
- **DebugProcessor**: Provides breakpoints and direct instruction execution
- **Events**: Processor exposes events for before/after instruction execution, halt, suspend, etc.
- **Breakpoints**: Can be set at any address; triggers event when hit

## Usage Example
```csharp
cpu.Debug.AddBreakpoint(0x1234);
cpu.Debug.ExecuteDirect("LD", 0x42, null);
```

## Integration
- Debugging is non-intrusive and can be enabled/disabled as needed
- Useful for test harnesses and development

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

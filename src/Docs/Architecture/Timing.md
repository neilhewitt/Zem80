# Timing & Clocks

[? Back to Architecture & Design](README.md)

## Timing in Z80 Emulation
Accurate emulation of the Z80 requires simulating its clock cycles (t-states) for each instruction, memory access, and IO operation.

## Zem80 Timing Model
- **ProcessorTiming**: Central class for managing t-states and wait cycles
- **Clock**: Pluggable clock implementations (e.g., DefaultClock, RealTimeClock, TimeSlicedClock)
- **Integration**: Timing is invoked during opcode fetch, memory/IO access, and internal operations

## Wait States
- Wait cycles can be inserted to simulate slow memory or peripherals
- Used for cycle-accurate emulation

## Example: Using a Real-Time Clock
```csharp
var cpu = new Processor(clock: new RealTimeClock(4.0f));
```

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

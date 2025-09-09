# Interrupt Handling

[? Back to Architecture & Design](README.md)

## Z80 Interrupts
- **Maskable Interrupts (INT)**: Can be enabled/disabled by software
- **Non-Maskable Interrupts (NMI)**: Always serviced when triggered
- **Interrupt Modes**: IM0, IM1, IM2 (each with different behaviors)

## Zem80 Implementation
- **Interrupts**: Class manages all interrupt logic and state
- **Processor**: Integrates interrupt handling into the main loop
- **Timing**: Interrupts are timed and can trigger instruction execution or jumps

## Example: Raising an Interrupt
```csharp
cpu.Interrupts.RaiseMaskable();
```

[? Back to Architecture & Design](README.md)
[? Back to Main Index](../README.md)

# Interrupts Class

[? Back to API Reference](README.md)

## Overview

The `Interrupts` class manages Z80 interrupt modes, maskable and non-maskable interrupts, and integrates with the CPU for correct interrupt handling and timing.

## Public API

### Properties
- `Mode` (`InterruptMode`): Current interrupt mode (IM0, IM1, IM2).
- `Enabled` (`bool`): Whether interrupts are enabled.
- `IFF1`, `IFF2` (`bool`): Internal flip-flops for interrupt state.

### Events
- `OnMaskableInterrupt`, `OnNonMaskableInterrupt`: Raised on interrupt events.

### Methods
- `SetMode(mode)`: Sets the interrupt mode.
- `RaiseMaskable(callback)`: Raises a maskable interrupt, optionally with a callback for IM0/IM2.
- `RaiseNonMaskable()`: Raises a non-maskable interrupt.
- `Disable()`, `Enable()`: Disables/enables interrupts.
- `RestoreAfterNMI()`: Restores state after NMI.
- `HandleAll()`: Handles pending interrupts.

### Constructor
- `Interrupts(Processor cpu, Action<InstructionPackage> executeInstruction)`: Binds to a CPU and execution delegate.

## Usage Example

```csharp
interrupts.SetMode(InterruptMode.IM1);
interrupts.RaiseMaskable();
```

## Internal Details
- Handles all Z80 interrupt modes and integrates with the processor's execution loop.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

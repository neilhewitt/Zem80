# IO Class

[? Back to API Reference](README.md)

## Overview

The `IO` class models the Z80's IO pin state, including address/data buses and control signals. It provides methods to set and clear pin states as required by the CPU and emulated hardware.

## Public API

### Properties
- `ADDRESS_BUS` (`ushort`): Current address bus value.
- `DATA_BUS` (`byte`): Current data bus value.
- `A0`-`A15`, `D0`-`D7`: Individual address/data lines.
- `MREQ`, `IORQ`, `RD`, `WR`, `M1`, `WAIT`, `INT`, `NMI`, `RESET`: Control signals.

### Methods
- `Clear()`: Resets all pins to default state.
- `SetOpcodeFetchState(address)`, `AddOpcodeFetchData(data)`, `EndOpcodeFetchState()`: Opcode fetch cycle.
- `SetMemoryReadState(address)`, `AddMemoryData(data)`, `EndMemoryReadState()`: Memory read cycle.
- `SetMemoryWriteState(address, data)`, `EndMemoryWriteState()`: Memory write cycle.
- `SetPortReadState(portAddress)`, `AddPortReadData(data)`, `EndPortReadState()`: Port read cycle.
- `SetPortWriteState(portAddress, data)`, `EndPortWriteState()`: Port write cycle.
- `SetInterruptState()`, `EndInterruptState()`: Interrupt cycle.
- `SetNMIState()`, `EndNMIState()`: Non-maskable interrupt cycle.
- `SetWaitState()`, `EndWaitState()`: Wait state control.
- `SetResetState()`: Reset pin.
- `SetAddressBusValue(value)`, `SetDataBusValue(value)`, `ResetDataBusValue()`, `SetDataBusDefaultValue(defaultValue)`: Bus control.

### Constructor
- `IO(Processor cpu)`: Binds IO to a CPU instance.

## Usage Example

```csharp
io.SetOpcodeFetchState(0x1000);
io.AddOpcodeFetchData(0x3E);
io.EndOpcodeFetchState();
```

## Internal Details
- Used by the processor and timing classes to simulate hardware-level IO.

[? Back to API Reference](README.md)
[? Back to Main Index](../README.md)

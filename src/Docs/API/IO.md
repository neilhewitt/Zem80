# IO & Ports API

[? Back to API Reference](README.md)

Zem80 Core models Z80 IO using the `IIO`, `IPorts`, and `IPort` interfaces.

## Overview
IO is modeled as a set of ports and a bus. Ports can be connected to handlers for custom IO behavior.

## IIO Interface
Represents the Z80's address and data bus, and control lines (RD, WR, MREQ, etc). Used for low-level emulation and debugging.

## IPorts & IPort
- `IPorts` provides indexed access to all ports.
- `IPort` allows connecting handlers for port IO.

## Example: Connecting a Port Handler
````csharp
var port = processor.Ports[0xFE];
port.Connect(
    reader: () => 0xFF,
    writer: b => Console.WriteLine($"OUT: {b}"),
    signalRead: null,
    signalWrite: null
);
````

---

[? Back to API Reference](README.md)

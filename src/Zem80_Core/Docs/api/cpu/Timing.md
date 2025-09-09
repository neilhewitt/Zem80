# Timing & Clock

[? Back to CPU API](README.md)

## IProcessorTiming Interface

- Manages instruction and internal operation timing.
- Methods for adding wait cycles, stack/port cycles, and internal operation cycles.

## IClock Interface

- Abstracts the CPU clock.
- Methods for stopping, waiting for ticks, and managing clock events.

## Implementations

- `ProcessorTiming`: Default timing implementation.
- `DefaultClock`, `TimeSlicedClock`, `RealTimeClock`: Different clock implementations for various timing needs.

---

[? Back to CPU API](README.md) | [API Index](../README.md)

# Timing and Clock System

The Zem80_Core timing system provides flexible clock management for Z80 emulation, supporting multiple timing modes from performance-optimized to cycle-accurate real-time emulation.

## Core Interfaces

### IClock Interface

Main interface for clock implementations providing timing control.

#### Namespace
`Zem80.Core.CPU`

#### Declaration
```csharp
public interface IClock
```

#### Properties

##### FrequencyInMHz
```csharp
float FrequencyInMHz { get; }
```
Clock frequency in MHz (e.g., 4.0 for 4MHz).

##### Ticks
```csharp
long Ticks { get; }
```
Total clock ticks elapsed since start.

#### Events

##### OnTick
```csharp
event EventHandler<long> OnTick
```
Fired on each clock tick for real-time synchronization.

#### Methods

##### Initialise
```csharp
void Initialise(Processor cpu)
```
Connects clock to processor instance.

##### Start / Stop
```csharp
void Start()
void Stop()
```
Controls clock execution.

##### Timer Methods
```csharp
int SetEvent(long ticks, Action onElapsed, bool repeats)
void UnsetEvent(int timerIndex)
```
Sets up timed events for interrupt generation or device simulation.

##### Wait Methods
```csharp
void WaitForNextClockTick()
void WaitForClockTicks(int ticks)
```
Synchronization methods for real-time clocks.

## Clock Implementations

### DefaultClock
Fast execution clock that runs at maximum CPU speed but reports specified frequency.

### RealTimeClock  
Attempts to maintain actual real-time frequency synchronization.

### TimeSlicedClock
Provides time-sliced execution for multi-threading scenarios.

## TimingMode Enumeration

```csharp
public enum TimingMode
{
    FastAndFurious,    // Maximum performance, no timing accuracy
    PseudoRealTime,    // Approximated real-time execution
    TimeSliced         // Time-sliced for threading
}
```

## Usage Examples

### Basic Clock Setup
```csharp
// Create fast clock for testing
var fastClock = ClockMaker.DefaultClock(4.0f); // 4MHz fast clock
var processor = new Processor(clock: fastClock);

// Create real-time clock for accurate emulation
var realTimeClock = ClockMaker.RealTimeClock(4.0f);
var accurateProcessor = new Processor(clock: realTimeClock);
```

### Clock Events for Interrupts
```csharp
var processor = new Processor();

// Set up interrupt every 1000 ticks (simulating 50Hz interrupt)
int interruptTimer = processor.Clock.SetEvent(1000, () =>
{
    processor.Interrupts.TriggerMaskableInterrupt();
}, repeats: true);

// Later, disable the interrupt
processor.Clock.UnsetEvent(interruptTimer);
```

### Performance vs Accuracy
```csharp
// Fast execution for testing
var testProcessor = new Processor(
    clock: ClockMaker.DefaultClock(4.0f)
);
// Runs as fast as possible

// Accurate execution for emulation
var emuProcessor = new Processor(
    clock: ClockMaker.RealTimeClock(3.5f)
);
// Maintains ~3.5MHz real-time speed
```

## See Also

- [Processor](./Processor.md) - Processor class that uses the timing system
- [IO](./IO.md) - I/O system for timed device interactions
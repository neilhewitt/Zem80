# ZX Spectrum LIST Command Analysis Report

## Executive Summary

This report analyzes code changes between tag 1.2.2 and the current HEAD in the Zem80_Core and ZXSpectrum_VM projects to identify why the ZX Spectrum emulation no longer returns to the prompt after executing the LIST command in ZX BASIC.

**Key Finding**: A critical change in interrupt handling behavior between versions 1.2.2 and HEAD is the most probable cause of the LIST command failure.

## Background: ZX Spectrum LIST Command Requirements

### How LIST Works on the ZX Spectrum

The LIST command in ZX Spectrum BASIC:
1. Displays BASIC program lines from memory to the screen
2. Uses the same display channel mechanism as PRINT
3. Requires continuous keyboard scanning to detect user input (especially BREAK/STOP)
4. Depends on interrupt-driven keyboard scanning for responsiveness

### Critical Dependency: Interrupt Mode 1 (IM1)

The ZX Spectrum operates in **Interrupt Mode 1 (IM1)** after boot:
- The ULA (Uncommitted Logic Array) triggers interrupts at vertical blank (50Hz)
- CPU jumps to address 0x0038 in ROM when interrupt occurs
- ROM interrupt service routine (ISR) performs:
  - Keyboard matrix scanning
  - System variable updates
  - Detection of BREAK key (Caps Shift + Space)

**Critical Point**: Without proper interrupt handling, the keyboard cannot be scanned, and the user cannot:
- Stop a listing with BREAK
- Return to the prompt
- Interact with the system during LIST execution

## Code Analysis: Changes Between 1.2.2 and HEAD

### 1. Major Architectural Changes in Zem80_Core

The codebase underwent significant refactoring between versions:

#### Version 1.2.2 Structure
- Processor class implemented timing directly via partial class `IInstructionTiming`
- Interrupts handled inline within `Processor.cs`
- Simple execution loop with consistent interrupt handling

#### HEAD Version Structure  
- Timing separated into `ProcessorTiming` class implementing `IProcessorTiming`
- Interrupts separated into `Interrupts` class implementing `IInterrupts`
- More modular architecture with dependency injection
- Instruction decoding refactored into `InstructionDecoder` class

### 2. Critical Change: Interrupt Handling in Execution Loop

#### Version 1.2.2 (WORKING)

```csharp
// From Processor.cs lines 213-269
private void InstructionCycle()
{
    _clock = new Stopwatch();
    _clock.Start();

    while (_running)
    {
        if (_suspended)
        {
            Thread.Sleep(1);
        }
        else
        {
            InstructionPackage package = null;
            ushort pc = Registers.PC;

            if (_halted || _runExtraNOP)
            {
                FetchNextOpcodeByte();
                package = new InstructionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);
                
                if (_runExtraNOP)
                {
                    Registers.PC++;
                    _runExtraNOP = false;
                }
            }
            else
            {
                package = DecodeInstructionAtProgramCounter();
                if (package == null)
                {
                    Stop();
                    return;
                }
            }

            Execute(package);
            HandleInterrupts();      // ← CALLED UNCONDITIONALLY AFTER EVERY INSTRUCTION
            RefreshMemory();

            if (_timeSliced && _ticksPerTimeSlice > 0)
            {
                _ticksThisTimeSlice += package.Instruction.Timing.TStates;
                if (_ticksThisTimeSlice >= _ticksPerTimeSlice)
                {
                    _ticksThisTimeSlice = 0;
                    Suspend();
                    OnTimeSliceEnded?.Invoke(this, _ticksThisTimeSlice);
                }
            }
        }
    }
}
```

**Key Point**: `HandleInterrupts()` is called **unconditionally** after every instruction execution.

#### HEAD Version (BROKEN)

```csharp
// From Processor.cs lines 145-188
private void InstructionCycle()
{
    Clock.Start();

    while (_running)
    {
        if (!_suspended)
        {
            bool skipNextByte = false;
            do
            {
                ushort address = Registers.PC;
                byte[] instructionBytes = (_halted || skipNextByte) ? new byte[4] : Memory.ReadBytesAt(address, 4);

                InstructionPackage package = _instructionDecoder.DecodeInstruction(instructionBytes, address, out skipNextByte, out bool opcodeErrorNOP);

                Timing.OpcodeFetchTiming(package.Instruction, address);
                Timing.OperandReadTiming(package.Instruction, address, package.Data.Argument1, package.Data.Argument2);

                Registers.PC += (ushort)package.Instruction.SizeInBytes;
                ExecuteInstruction(package);

                if (!opcodeErrorNOP)     // ← CONDITIONAL CHECK
                {
                    Interrupts.HandleAll();  // ← ONLY CALLED IF NOT AN OPCODE ERROR
                }
                
                RefreshMemory();
            }
            while (skipNextByte);
        }
        else
        {
            Thread.Sleep(1);
        }
    }
}
```

**Critical Difference**: `Interrupts.HandleAll()` is now **conditional** and only called when `!opcodeErrorNOP`.

### 3. Analysis of opcodeErrorNOP Flag

The `opcodeErrorNOP` flag is set by the instruction decoder when:
- An invalid opcode sequence is encountered
- The decoder needs to execute a NOP in place of invalid instructions

**Potential Issue**: If the decoder incorrectly sets `opcodeErrorNOP` for valid instructions, or if there's a pattern of instructions that triggers this flag, interrupts could be skipped.

### 4. Timing System Changes

#### Version 1.2.2
- Implemented as partial class methods directly in `Processor`
- Simple, inline timing with direct clock tick waits
- Method: `IInstructionTiming.OpcodeFetchCycle(ushort address, byte data)`

#### HEAD Version
- Separate `ProcessorTiming` class
- More sophisticated timing with separate opcode fetch and operand read methods
- New method structure: `OpcodeFetchTiming(Instruction, address)` and `OperandReadTiming(Instruction, address, operands)`
- Timing now calculated based on `MachineCycle` data from instructions

**Impact**: While cleaner architecturally, the timing changes could affect interrupt latency or timing-sensitive operations.

### 5. Memory Access Changes

#### Version 1.2.2
- Used `Memory.Untimed.ReadByteAt()` and `Memory.Timed.ReadByteAt()` with clear separation
- Direct control over when timing was applied

#### HEAD Version
- Unified `Memory.ReadByteAt()` interface
- Timing controlled via `ProcessorTiming` class
- More complex timing cycle management

### 6. ZXSpectrum_VM Changes

The Spectrum implementation also changed significantly:

#### Version 1.2.2
```csharp
_cpu.Initialise(timingMode: TimingMode.TimeSliced, ticksPerTimeSlice: 70000);
// ...
_timer.Elapsed += UpdateDisplay;
// ...
private void UpdateDisplay(object sender, EventArgs e)
{
    _cpu.Resume();
    _cpu.RaiseInterrupt();
    // ... screen update code
}
```

#### HEAD Version
```csharp
// ULA class now handles timing and interrupts
_ula = new ULA(_cpu);
// Interrupt raising moved into ULA class
```

The interrupt raising mechanism was refactored into the ULA class, which may introduce timing or synchronization issues.

## Probable Causes of LIST Command Failure

### Primary Suspects (Most Likely → Less Likely)

### 1. **Conditional Interrupt Handling (HIGHEST PROBABILITY)**

**Issue**: Interrupts are skipped when `opcodeErrorNOP` is true.

**Why This Breaks LIST**:
- If any instruction during LIST execution triggers the `opcodeErrorNOP` condition
- Keyboard scanning interrupts may be skipped
- User cannot break out of LIST or interact with the system
- System appears hung

**Evidence**:
- Direct comparison shows this is the only behavioral change in the main execution loop
- LIST is a long-running operation that executes many instructions
- Statistical probability that `opcodeErrorNOP` may be triggered during LIST execution

**Test Hypothesis**: 
Add logging to track when `opcodeErrorNOP` is true during LIST execution. If it triggers even once during the LIST operation, keyboard interrupts will be skipped.

### 2. **Instruction Decoder Issues**

**Issue**: The new `InstructionDecoder` class may incorrectly set `opcodeErrorNOP` flag.

**Why This Breaks LIST**:
- If decoder has bugs in certain opcode sequences
- Could incorrectly classify valid ZX Spectrum ROM instructions as errors
- Would trigger the interrupt skip condition

**Evidence**:
- Completely new decoder implementation
- Complex logic for handling prefixed instructions (DD, FD, CB, ED)
- Error handling for invalid opcodes changed

**Investigation Needed**:
- Compare decoder output between versions for ZX Spectrum ROM code
- Check if any ROM routines used by LIST trigger error conditions

### 3. **Timing Precision Changes**

**Issue**: New timing system may have subtle differences in interrupt timing.

**Why This Could Break LIST**:
- ZX Spectrum depends on precise 50Hz interrupts (20ms intervals)
- If interrupt timing is off, keyboard scan may be delayed
- Screen updates may desynchronize

**Evidence**:
- Complete rewrite of timing system
- New `ProcessorTiming` class with different T-state accounting
- Changes to how wait states are handled

**Likelihood**: Medium - timing issues could cause intermittent problems but less likely to cause complete hang

### 4. **ULA Integration Changes**

**Issue**: Moving interrupt generation into ULA class may introduce bugs.

**Why This Could Break LIST**:
- If ULA doesn't raise interrupts correctly
- If timing between ULA and CPU is incorrect
- If interrupt acknowledgment is broken

**Evidence**:
- Significant refactoring of ZXSpectrum_VM
- New ULA class implementation
- Changed interrupt raising mechanism

**Likelihood**: Medium - but would affect all operations, not just LIST

### 5. **Memory Timing Changes**

**Issue**: Changes to memory access timing could affect screen memory or ROM execution.

**Why This Could Break LIST**:
- LIST reads from program memory
- Outputs to screen memory (contended on real Spectrum)
- Timing changes could cause memory corruption or race conditions

**Likelihood**: Lower - memory corruption would cause crashes, not just hanging

## Recommended Investigation Steps

### Phase 1: Immediate Diagnosis

1. **Add Debug Logging**:
   ```csharp
   if (!opcodeErrorNOP)
   {
       Interrupts.HandleAll();
   }
   else
   {
       Debug.WriteLine($"Skipping interrupt at PC={Registers.PC:X4}, opcode={instructionBytes[0]:X2}");
   }
   ```

2. **Test Interrupt Frequency**:
   - Add counter to track how often `HandleAll()` is actually called
   - Should be called approximately every N instructions
   - Compare to 1.2.2 behavior

3. **Monitor opcodeErrorNOP Trigger**:
   - Log all cases where this flag is set
   - Check if it correlates with LIST execution
   - Identify which opcodes trigger this condition

### Phase 2: Focused Testing

1. **Bisect the Change**:
   - Create minimal test case that reproduces LIST hang
   - Test with interrupt handling made unconditional
   - If fixes issue, confirms root cause

2. **Compare Decoder Behavior**:
   - Run same opcode sequences through both decoders
   - Check for differences in error detection
   - Verify all ZX Spectrum ROM opcodes decode correctly

3. **Interrupt Timing Verification**:
   - Measure actual interrupt frequency
   - Should be ~50Hz (20ms intervals)
   - Check if timing drifts during LIST execution

### Phase 3: Validation

1. **Test Fix**:
   - Remove conditional interrupt handling (make it unconditional like 1.2.2)
   - Verify LIST command works
   - Test other BASIC commands for regression

2. **Root Cause Analysis**:
   - Identify why `opcodeErrorNOP` exists
   - Determine if it's necessary
   - If necessary, find alternative that doesn't skip interrupts

3. **Comprehensive Testing**:
   - Test with various BASIC programs
   - Test with snapshots (SNA, Z80)
   - Verify keyboard responsiveness
   - Confirm BREAK key works during long operations

## Speculative Root Causes

### Most Probable Scenario

The conditional interrupt handling was likely introduced to avoid running interrupts after handling invalid opcodes, which makes sense in theory. However, this creates a critical flaw:

1. **Invalid opcode detected** → `opcodeErrorNOP = true` → Interrupts skipped
2. **Next instruction** → Still in same execution cycle → Interrupts potentially skipped again
3. **During LIST** → Many instructions executed → Even small probability of error means interrupts eventually skipped
4. **Result** → Keyboard not scanned → User cannot break → Appears hung

### Why It Worked in 1.2.2

Version 1.2.2 always called `HandleInterrupts()` unconditionally, ensuring:
- Keyboard scanned after every instruction
- Interrupts never missed
- BREAK key always detected
- System always responsive

### Why the Change Was Made

The refactoring likely attempted to:
- Improve code organization (separate concerns)
- Handle edge cases more explicitly
- Prevent interrupts during error recovery
- Make timing more accurate

However, the implementation introduced a subtle but critical bug by making interrupts conditional.

## Recommendations

### Immediate Fix (Highest Priority)

**Remove conditional interrupt handling**:

```csharp
// In Processor.cs, InstructionCycle method
Registers.PC += (ushort)package.Instruction.SizeInBytes;
ExecuteInstruction(package);

// REMOVE THIS CONDITION:
// if (!opcodeErrorNOP)
// {
    Interrupts.HandleAll();  // Always call interrupts
// }

RefreshMemory();
```

**Rationale**: This restores 1.2.2 behavior and should fix the LIST hang immediately.

### Proper Solution (Long-term)

If skipping interrupts during error handling is truly necessary:

1. **Add separate error handling path** that doesn't skip interrupts
2. **Use a counter or flag** to skip interrupts only for the immediate error NOP, not subsequent instructions
3. **Re-enable interrupts explicitly** after error recovery
4. **Add comprehensive testing** for interrupt handling during all operation modes

### Testing Requirements

Before considering the fix complete:

1. **Verify LIST works** with various program sizes
2. **Test BREAK key** during LIST and other operations
3. **Test all BASIC commands** that require user interaction
4. **Load and test snapshots** to verify no regression
5. **Run for extended periods** to check for timing drift
6. **Test with different interrupt modes** (IM0, IM1, IM2)

## Conclusion

The most probable cause of the ZX Spectrum LIST command hanging is the **conditional interrupt handling** introduced in the HEAD version. By making interrupt handling conditional on `!opcodeErrorNOP`, the system can miss keyboard scan interrupts, preventing the user from breaking out of LIST or returning to the prompt.

This issue is particularly insidious because:
- It only manifests during long-running operations (like LIST)
- It may be intermittent depending on what instructions are executed
- The root cause is hidden in a seemingly innocuous conditional check
- The refactoring improved code organization but introduced a subtle behavioral change

**Recommended Action**: Remove the conditional check and always call `Interrupts.HandleAll()` after instruction execution, restoring the 1.2.2 behavior. This should immediately resolve the LIST command issue while a more comprehensive solution is developed if error-case interrupt skipping is determined to be necessary.

---

## Appendix: Detailed Code Comparison

### Interrupt Handling Comparison

| Aspect | Version 1.2.2 | HEAD Version | Impact |
|--------|---------------|--------------|--------|
| **Location** | Inline in Processor.cs | Separate Interrupts class | Organizational |
| **Calling** | Unconditional | Conditional on !opcodeErrorNOP | **CRITICAL** |
| **Frequency** | Every instruction | Most instructions (not errors) | **BREAKS LIST** |
| **Implementation** | HandleInterrupts() | Interrupts.HandleAll() | Functional equivalent |

### Timing System Comparison

| Aspect | Version 1.2.2 | HEAD Version |
|--------|---------------|--------------|
| **Architecture** | Partial class (IInstructionTiming) | Separate class (ProcessorTiming) |
| **Opcode Fetch** | OpcodeFetchCycle(address, data) | OpcodeFetchTiming(instruction, address) |
| **Memory Read** | MemoryReadCycle(address, data) | MemoryReadCycle(address, data, tStates) |
| **T-State Calculation** | Hardcoded in methods | Based on MachineCycle data |
| **Wait Cycles** | _pendingWaitCycles field | _waitCyclesPending field |

### Decoder Comparison

| Aspect | Version 1.2.2 | HEAD Version |
|--------|---------------|--------------|
| **Location** | Inline in Processor | Separate InstructionDecoder class |
| **Error Handling** | Returns null or NOP | Sets opcodeErrorNOP flag |
| **Prefix Handling** | Complex inline logic | Refactored into decoder |
| **Skip Logic** | _runExtraNOP flag | skipNextByte output parameter |

## Research References

This analysis was informed by research into ZX Spectrum interrupt handling:

1. **ZX Spectrum Interrupt Modes**: The Spectrum uses IM1 (Interrupt Mode 1) where interrupts jump to 0x0038 in ROM
2. **Keyboard Scanning**: Performed by ROM ISR triggered every 50Hz (20ms) during vertical blank
3. **BREAK Detection**: Requires regular interrupt-driven keyboard scanning to detect Caps Shift + Space
4. **LIST Command**: Depends on uninterrupted interrupt handling to remain responsive to user input

The failure mode matches exactly what would be expected if interrupts are being skipped during execution: the keyboard cannot be scanned, BREAK cannot be detected, and the system appears hung even though it's actively executing the LIST command.

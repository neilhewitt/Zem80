﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Zem80.Core.Instructions;
using Zem80.Core.IO;
using Zem80.Core.Memory;
using Stack = Zem80.Core.Memory.Stack;

namespace Zem80.Core
{
    public partial class Processor : IDisposable
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
        public const float DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ = 4;

        private bool _running;
        private bool _halted;
        private bool _suspended;
        private bool _realTime;
        private bool _timeSliced;
        
        private HaltReason _reasonForLastHalt;
        private bool _suspendMachineCycles;
        private int _pendingWaitCycles;
        private int _waitCyclesAdded;
        
        private long _emulatedTStates;
        private long _lastElapsedTicks;

        private int[] _cycleWaitPattern;
        private int _cycleWaitCount;
        private long _ticksPerTimeSlice;
        private long _ticksThisTimeSlice;

        private Stopwatch _clock;
        private Thread _instructionCycle;
        private InstructionPackage _executingInstructionPackage;
        
        private Func<byte> _interruptCallback;
        private bool _runExtraNOP;
        private bool _uninterruptableNOP;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; private set; }
        public IMemoryBank Memory { get; private set; }
        public Stack Stack { get; private set; }
        public Ports Ports { get; private set; }
        public ProcessorIO IO { get; private set; }

        public IReadOnlyFlags Flags => new Flags(Registers.F, true);

        public InterruptMode InterruptMode { get; private set; }
        public bool InterruptsEnabled { get; private set; }
        internal bool InterruptsWereEnabledBeforeNMI { get; private set; }

        public float FrequencyInMHz { get; private set; }
        public TimingMode TimingMode { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 
        public long EmulatedTStates => _emulatedTStates;

        public event EventHandler<InstructionPackage> OnClockTick;
        public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
        public event EventHandler<ExecutionResult> AfterExecuteInstruction;
        public event EventHandler<int> BeforeInsertWaitCycles;
        public event EventHandler BeforeStart;
        public event EventHandler OnStop;
        public event EventHandler<HaltReason> OnHalt;
        public event EventHandler<long> OnTimeSliceEnded;

        public void Dispose()
        {
            _running = false;
            _instructionCycle?.Interrupt(); // just in case
        }

        public void Initialise(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious, long ticksPerTimeSlice = 0, InterruptMode interruptMode = InterruptMode.IM0)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                TimingMode = timingMode;
                _ticksPerTimeSlice = ticksPerTimeSlice;
                InterruptMode = interruptMode;
                
                _realTime = (timingMode == TimingMode.PseudoRealTime && Stopwatch.IsHighResolution);
                _timeSliced = (timingMode == TimingMode.TimeSliced);
                _emulatedTStates = 0;

                DisableInterrupts();

                Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
            }
        }

        public void Start()
        {
            BeforeStart?.Invoke(null, null);
            _lastElapsedTicks = 0;
            _cycleWaitCount = 0;
            _running = true;

            IO.Clear();
            _instructionCycle = new Thread(InstructionCycle);
            _instructionCycle.IsBackground = true;
            _instructionCycle.Start();
        }

        public void Stop()
        {
            _running = false;
            _halted = false;

            _clock.Stop();
            _lastRunTimeInMilliseconds = _clock.ElapsedMilliseconds;

            OnStop?.Invoke(null, null);
        }

        public void Suspend()
        {
            _suspended = true;
        }

        public void Resume()
        {
            if (_halted)
            {
                _halted = false;
                // if coming back from a HALT instruction (at next interrupt or by API override here), move the Program Counter on to step over the HALT instruction
                // otherwise we'll HALT forever in a loop
                if (_reasonForLastHalt == HaltReason.HaltInstruction) Registers.PC++;
            }

            _suspended = false;
        }

        public void RunUntilStopped()
        {
            while (_running) Thread.Sleep(1); // main thread can sleep while instruction thread does its thing
        }

        public void ResetAndClearMemory(bool restartAfterReset = true, ushort startAddress = 0, InterruptMode interruptMode = InterruptMode.IM0)
        {
            IO.SetResetState();
            Stop();
            Memory.Clear();
            Registers.Clear();
            IO.Clear();
            Registers.SP = Stack.Top;
            if (restartAfterReset)
            {
                Initialise(startAddress, this.EndOnHalt, this.TimingMode, _ticksPerTimeSlice, interruptMode);
                Start();
            }
        }

        public void SetInterruptMode(InterruptMode mode)
        {
            InterruptMode = mode;
        }

        public void RaiseInterrupt(Func<byte> callback = null)
        {
            IO.SetInterruptState();
            _interruptCallback = callback;
        }

        public void RaiseNonMaskableInterrupt()
        {
            IO.SetNMIState();
        }

        public void DisableInterrupts()
        {
            InterruptsEnabled = false;
        }

        public void EnableInterrupts()
        {
            InterruptsEnabled = true;
        }

        public void AddWaitCycles(int waitCycles)
        {
            // Will add *waitCycles* wait states at the next insertion point.
            // Waits are only actually inserted at certain points in the instruction cycle.
            // If some waits are already pending, the new waits will be added to that total.
            _pendingWaitCycles += waitCycles;
        }

        public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
        {
            if (!_halted)
            {
                _halted = true;
                _reasonForLastHalt = reason;
                OnHalt?.Invoke(null, reason);

                if (EndOnHalt)
                {
                    Stop();
                }
            }
        }

        internal void RestoreInterruptsAfterNMI()
        {
            InterruptsEnabled = InterruptsWereEnabledBeforeNMI;
        }

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
                        // halted - run NOP until we resume (don't advance PC)
                        // runExtrNOP - run a single NOP and advance PC so we can continue on

                        FetchNextOpcodeByte(); // enable devices to run, interrupts etc
                        package = new InstructionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);
                        
                        if (_runExtraNOP)
                        {
                            Registers.PC++;
                            _runExtraNOP= false;
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
                    HandleInterrupts();
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

        private void HandleInterrupts()
        {
            // in some situations, a NOP may not allow any interrupts (including NMI) to run after it
            if (_uninterruptableNOP)
            {
                _uninterruptableNOP = false;
                return;
            }

            if (!HandleNMI())
            {
                HandleMaskableInterrupts();
            }
        }

        private void RefreshMemory()
        {
            Registers.R = (byte)(((Registers.R + 1) & 0x7F) | (Registers.R & 0x80)); // bits 0-6 of R are incremented as part of the memory refresh - bit 7 is preserved 
        }

        private ExecutionResult Execute(InstructionPackage package)
        {
            BeforeExecuteInstruction?.Invoke(this, package);
            _executingInstructionPackage = package;

            // check for breakpoints
            if (_breakpoints != null && _breakpoints.Contains(package.InstructionAddress))
            {
                _onBreakpoint?.Invoke(this, package);
            }

            // set the internal WZ register to an initial value based on whether this is an indexed instruction or not; the instruction that runs may alter/set WZ itself
            // the value in WZ (sometimes known as MEMPTR in Z80 enthusiast circles) is only ever used to control the behavior of the BIT instruction
            Registers.WZ = (ushort)(Registers[package.Instruction.IndexedRegister] + package.Data.Argument1);

            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);
            if (result.Flags != null) Registers.F = result.Flags.Value;
            result.WaitCyclesAdded = _waitCyclesAdded;
            AfterExecuteInstruction?.Invoke(this, result);

            _executingInstructionPackage = null;
            return result;
        }

        // execute an instruction directly (without the processor loop running), for example for directly testing instructions
        ExecutionResult IDebugProcessor.ExecuteDirect(byte[] opcode)
        {
            Memory.Untimed.WriteBytesAt(Registers.PC, opcode);
            InstructionPackage package = DecodeInstructionAtProgramCounter();
            if (package == null)
            {
                throw new InstructionDecoderException("Supplied opcode sequence does not decode to a valid instruction.");
            }

            return Execute(package);
        }

        // execute an instruction directly (specified by mnemonic, so no decoding necessary)
        ExecutionResult IDebugProcessor.ExecuteDirect(string mnemonic, byte? arg1, byte? arg2)
        {
            if (!InstructionSet.InstructionsByMnemonic.TryGetValue(mnemonic, out Instruction instruction))
            {
                throw new InstructionDecoderException("Supplied mnemonic does not correspond to a valid instruction");
            }

            InstructionData data = new InstructionData()
            {
                Argument1 = arg1 ?? 0,
                Argument2 = arg2 ?? 0
            };

            InstructionPackage package = new InstructionPackage(instruction, data, Registers.PC);
            Registers.PC += package.Instruction.SizeInBytes; // simulate the decode cycle effect on PC
            return Execute(package);
        }

        private InstructionPackage DecodeInstructionAtProgramCounter()
        {
            byte b0, b1, b3; // placeholders for upcoming instruction bytes
            Instruction instruction;
            InstructionData data = new InstructionData();
            ushort instructionAddress = Registers.PC;

            b0 = FetchNextOpcodeByte(); // always at least one opcode byte
            Registers.PC++;

            // was byte 0 a prefix code?
            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD) 
            {
                b1 = Memory.Untimed.ReadByteAt(Registers.PC); // peek ahead, we need this value but don't want to advance PC yet
                
                if ((b0 == 0xDD || b0 == 0xFD || b0 == 0xED) && (b1 == 0xDD || b1 == 0xFD || b1 == 0xED))
                {
                    // sequences of 0xDD / 0xFD / 0xED count as NOP until the final 0xDD / 0xFD / 0xED which is then the prefix byte
                    b1 = FetchNextOpcodeByte();
                    Registers.PC++;

                    instruction = InstructionSet.NOP;
                }
                else if ((b0 == 0xDD || b0 == 0xFD) && b1 == 0xCB)
                {
                    // DDCB / FDCB: four-byte opcode = two prefix bytes + one displacement byte + one opcode byte (no four-byte instruction has any actual operand values)
                    b1 = FetchNextOpcodeByte();
                    Registers.PC++;
                    
                    data.Argument1 = FetchNextOperandByte(); // displacement byte is the only 'operand'
                    Registers.PC++;
                    
                    b3 = FetchNextOpcodeByte();
                    Registers.PC++;
                    
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        // not a valid instruction - the Z80 spec says we should run a single NOP instead
                        instruction = InstructionSet.NOP;
                    }                    
                }
                else 
                {
                    // all other prefixed instructions (CB, ED, DD, FD): a two-byte opcode + up to 2 operand bytes
                    
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        // if prefix was 0xED and the instruction is invalid, the spec says run *two* NOPs
                        if (b0 == 0xED)
                        {
                            _runExtraNOP = true; // causes the processor to run an extra NOP *after* this one and skip over the invalid instruction byte
                        }
                        
                        // if the prefix was 0xDD or 0xFD and the instruction is invalid, the spec says we should run a NOP now but then run the equivalent
                        // unprefixed instruction next
                        if (b0 == 0xDD || b0 == 0xFD)
                        {
                            instructionAddress++;
                            Registers.PC = instructionAddress;
                        }

                        instruction = InstructionSet.NOP;
                    }
                    else
                    {
                        b1 = FetchNextOpcodeByte();
                        Registers.PC++;

                        WaitForClockTicks(instruction.Timing.Exceptions.ExtraOpcodeFetchTStates); // some instructions have prolonged opcode fetch

                        // fetch the operand bytes (as required)
                        if (instruction.Argument1 != InstructionElement.None)
                        {
                            data.Argument1 = FetchNextOperandByte();
                            Registers.PC++;
                            if (instruction.Argument2 != InstructionElement.None)
                            {
                                data.Argument2 = FetchNextOperandByte();
                                Registers.PC++;
                            }
                        }
                    }
                }
            }
            else
            {
                // unprefixed instruction - 1 byte opcode + up to 2 operand bytes
                if (!InstructionSet.Instructions.TryGetValue(b0, out instruction))
                {
                    instruction = InstructionSet.NOP;
                }

                WaitForClockTicks(instruction.Timing.Exceptions.ExtraOpcodeFetchTStates);

                // fetch the operand bytes (as required)
                if (instruction.Argument1 != InstructionElement.None)
                {
                    data.Argument1 = FetchNextOperandByte();
                    Registers.PC++;
                    if (instruction.Argument2 != InstructionElement.None)
                    {
                        // Special case: CALL has a prolonged high byte operand read with an additional cycle
                        // but ONLY if a) it's CALL nn OR b) if the condition (CALL NZ, for example) is satisfied (ie Flags.NZ is true)
                        if (instruction.Timing.Exceptions.HasProlongedConditionalOperandDataReadHigh)
                        {
                            if (instruction.Condition == Condition.None || Flags.SatisfyCondition(instruction.Condition))
                            {
                                WaitForClockTicks(1);
                            }
                        }

                        data.Argument2 = FetchNextOperandByte();
                        Registers.PC++;
                    }
                }
            }

            if (instruction == InstructionSet.NOP && b0 != 0x00)
            {
                _uninterruptableNOP = true; // this is a 'pseudo-NOP' caused by a special case / invalid opcode, after which interrupts (including NMI) must not run
            }

            return new InstructionPackage(instruction, data, instructionAddress);
        }

        private void WaitForClockTicks(int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                WaitForNextClockTick();
            }
        }

        private void WaitForNextClockTick()
        {
            if (_running)
            {
                if (_realTime)
                {
                    if (_cycleWaitCount == _cycleWaitPattern.Length) _cycleWaitCount = 0;
                    int ticksToWait = _cycleWaitPattern[_cycleWaitCount++];
                    long targetTicks = _lastElapsedTicks + ticksToWait;

                    while (_clock.ElapsedTicks < targetTicks) ; // wait until enough Windows ticks have elapsed
                    
                    _lastElapsedTicks = _clock.ElapsedTicks;
                }

                OnClockTick?.Invoke(this, _executingInstructionPackage);
                _emulatedTStates++;
            }
        }

        private void InsertWaitCycles()
        {
            int cyclesToAdd = _pendingWaitCycles;

            if (cyclesToAdd > 0)
            {
                BeforeInsertWaitCycles?.Invoke(this, cyclesToAdd);
            }

            while (cyclesToAdd > 0)
            {
                WaitForNextClockTick();
                cyclesToAdd--;
            }

            _waitCyclesAdded = _pendingWaitCycles;
            _pendingWaitCycles = 0;
        }

        private byte FetchNextOpcodeByte()
        {
            byte opcode = Memory.Untimed.ReadByteAt(Registers.PC);
            if (!_suspendMachineCycles) Timing.OpcodeFetchCycle(Registers.PC, opcode);
            return opcode;
        }

        private byte FetchNextOperandByte()
        {
            byte operand = Memory.Untimed.ReadByteAt(Registers.PC);
            if (!_suspendMachineCycles) Timing.MemoryReadCycle(Registers.PC, operand);
            return operand;
        }

        private bool HandleNMI()
        {
            bool handledNMI = false;

            if (IO.NMI)
            {
                if (_halted)
                {
                    Resume();
                }

                InterruptsWereEnabledBeforeNMI = InterruptsEnabled; // save IFF1 state ready for RETN
                InterruptsEnabled = false; // disable maskable interrupts until RETN

                Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.NMI_INTERRUPT_ACKNOWLEDGE_TSTATES);

                Stack.Push(WordRegister.PC);
                Registers.PC = 0x0066;
                Registers.WZ = Registers.PC;

                Timing.EndInterruptRequestAcknowledgeCycle();

                handledNMI = true;
            }

            IO.EndNMIState();

            return handledNMI;
        }

        private void HandleMaskableInterrupts()
        {
            if (IO.INT && InterruptsEnabled)
            {
                if (_interruptCallback == null && InterruptMode == InterruptMode.IM0)
                {
                    throw new InterruptException("Interrupt mode is IM0 which requires a callback for reading data from the interrupting device. Callback was null.");
                }

                if (_halted)
                {
                    Resume();
                }

                switch (InterruptMode)
                {
                    case InterruptMode.IM0:

                        // In this mode, we read up to 4 bytes that form an instruction which is then executed. Usually, this is an RST but it can in theory be 
                        // any Z80 instruction. The instruction bytes are read from the data bus, whose value is set by a hardware device during 4 clock cycles.

                        // To emulate a device functioning via IM0, your device code must call RaiseInterrupt on the CPU when it is ready to interrupt the system, supplying a
                        // Func<byte> that returns the opcode bytes of the instruction to run one by one as it is called 4 times (it will *always* be called 4 times). Trailing
                        // 0x00s will be ignored, but you must return 4 bytes even if the instruction is shorter than that. See DecodeIM0Interrupt below for details of how this works.

                        // The decoded instruction is executed in the current execution context with registers and program counter where they were when the interrupt was triggered.
                        // The program counter is pushed on the stack before executing the instruction and restored afterwards (but *not* popped), so instructions like JR, JP and CALL will have no effect. 
                        
                        // NOTE: I have not been able to test this mode extensively based on real-world examples. It may well be buggy compared to the real Z80. 
                        // TODO: verify the behaviour of the real Z80 and fix the code if necessary

                        Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.IM0_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        InstructionPackage package = DecodeIM0Interrupt();
                        ushort pc = Registers.PC;
                        Stack.Push(WordRegister.PC);
                        Execute(package);
                        Registers.PC = pc;
                        Registers.WZ = Registers.PC;
                        break;

                    case InterruptMode.IM1:

                        // This mode is simple. When IM1 is set (this is the default mode) then a jump to 0x0038 is performed when 
                        // the interrupt occurs. 

                        Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.IM1_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Stack.Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        Registers.WZ = Registers.PC;
                        break;

                    case InterruptMode.IM2:
                       
                        // In this mode, we synthesize an address from the contents of register I as the high byte and the 
                        // value on the data bus as the low byte. This address is a pointer into a table in RAM containing the *actual* interrupt
                        // routine addresses. We read the word at the address we calculated, and then jump to *that* address.

                        // When emulating a hardware device that uses IM2 to jump into its service routine/s, you must trigger the interrupt
                        // by calling RaiseInterrupt and supplying a Func<byte> that returns the data bus value to use when called.
                        // If the callback is null then the data bus value is assumed to be 0. 
                        
                        // It's actually quite common on some platforms (eg ZX Spectrum) to use IM2 this way to call a routine that needs to be synchronised 
                        // with the hardware (on the Spectrum, an interrupt is raised by the system after each display refresh, and setting IM2 allows 
                        // the programmer to divert that interrupt to a routine of their choice and then call down to the ROM routine [which handles the
                        // keyboard, sound etc] afterwards).

                        IO.SetDataBusValue(_interruptCallback?.Invoke() ?? 0); 
                        Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.IM2_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Stack.Push(WordRegister.PC);
                        ushort address = (IO.DATA_BUS, Registers.I).ToWord();
                        Registers.PC = Memory.Timed.ReadWordAt(address);
                        Registers.WZ = Registers.PC;
                        break;
                }

                // handling an interrupt always results in two extra wait cycles being added, so we'll add those here
                WaitForClockTicks(2);

                _interruptCallback = null;
                Timing.EndInterruptRequestAcknowledgeCycle();
            }
        }

        private InstructionPackage DecodeIM0Interrupt()
        {
            // In IM0, when an interrupt is generated, the CPU will ask the device to supply four bytes one at a time via
            // a callback method, which are then decoded into an instruction to be executed.
            
            // To emulate this without heavily re-writing the decode loop (which expects the instruction bytes to be 
            // in memory at the address pointed to by the program counter), we will temporarily copy 
            // the last 4 bytes of RAM to an array, move the program counter and use those 4 bytes for the interrupt decode, 
            // then restore them and fix up the program counter. Sneaky, eh?

            ushort address = (ushort)(Memory.SizeInBytes - 5);
            byte[] lastFour = Memory.Untimed.ReadBytesAt(address, 4);
            ushort pc = Registers.PC;
            Registers.PC = address;

            // we need to suspend all machine cycle timing so things don't get out of sync
            _suspendMachineCycles = true;
           
            byte[] opcode = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                // The callback will be called 4 times; it should return the opcode bytes of the instruction to run in sequence.
                // If there are fewer than 4 bytes in the opcode, return 0x00 for the 'extra' bytes
                opcode[i] = _interruptCallback();
                Memory.Untimed.WriteByteAt((ushort)(address + i), opcode[i]);
            }

            InstructionPackage package = DecodeInstructionAtProgramCounter();

            Registers.PC = pc;
            Memory.Untimed.WriteBytesAt(address, lastFour);
            
            _suspendMachineCycles = false;

            // The first opcode byte is read during the interrupt request acknowledge cycle, so no additional timing is required.
            // Any additional bytes read should be timed as a normal memory read (4 clock cycles each).
            for (int i = 1; i < 4; i++)
            {
                if (opcode[i] != 0) WaitForClockTicks(4);
            }

            return package;
        }

        private int[] MakeWaitPattern(float frequencyInMHz)
        {
            // HACK ALERT!!!
            // To simulate real-time operation, we need to work out how many Windows ticks of the Stopwatch class there are
            // per emulated Z80 tick, so we can wait for the right amount of time for each clock cycle... easy, right?
            // Well, no. Because it isn't always a whole number, and we can't wait for a fractional number of Stopwatch ticks.
            // So, we need to create a pattern of waiting for one fewer ticks every x ticks to even out the wait time, otherwise
            // timing-critical stuff in your emulated hardware may not work properly (example: Spectrum beeper sound)
            //
            // Client code can supply a custom WaitPattern at constructor time. Otherwise we have to generate one.

            int[] cycleWaitPattern = new int[1] { 1 }; // default if not high-resolution platform

            // high-res stopwatch frequency will be 10MHz - if we're on a non-high-res platform then real-time mode is not available anyway
            if (Stopwatch.IsHighResolution)
            {
                float windowsFrequency = Stopwatch.Frequency / (frequencyInMHz * 1000000);
                int windowsTicksPerZ80Tick = (int)Math.Ceiling(windowsFrequency);

                if (windowsTicksPerZ80Tick % 2 == 0) // even number, so the pattern is regular, for example at 5MHz 10/5 = 2
                {
                    cycleWaitPattern = new int[] { windowsTicksPerZ80Tick };
                }
                else // odd number - we're approximating here, some hardware emulation may require a custom pattern
                {
                    cycleWaitPattern = new int[windowsTicksPerZ80Tick + 1];
                    for (int i = 0; i < windowsTicksPerZ80Tick; i++)
                    {
                        cycleWaitPattern[i] = windowsTicksPerZ80Tick;
                    }
                    cycleWaitPattern[windowsTicksPerZ80Tick] = windowsTicksPerZ80Tick - 1;
                }
            }

            return cycleWaitPattern;
        }

        public Processor(
            IMemoryBank memory = null, 
            IMemoryMap map = null, 
            ushort topOfStackAddress = 0xFFFF, 
            float frequencyInMHz = DEFAULT_PROCESSOR_FREQUENCY_IN_MHZ, 
            int[] cycleWaitPattern = null
            )
        {
            FrequencyInMHz = frequencyInMHz;
            _cycleWaitPattern = cycleWaitPattern ?? MakeWaitPattern(frequencyInMHz);

            Registers = new Registers();
            Ports = new Ports(this);

            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));
            
            // stack pointer defaults to 0xFFFF - this is undocumented but verified behaviour of the Z80
            Stack = new Stack(topOfStackAddress, this);
            Registers.SP = Stack.Top;

            IO = new ProcessorIO(this);
            
            // The Z80 instruction set needs to be built (all Instruction objects are created, bound to the microcode instances, and indexed into a hashtable - undocumented 'overloads' are built here too)
            InstructionSet.Build();
        }
    }
}

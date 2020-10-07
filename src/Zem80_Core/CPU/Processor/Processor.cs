using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;
using Zem80.Core.IO;

namespace Zem80.Core
{
    public class Processor : IDebugProcessor, ITiming
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;

        public const int OPCODE_FETCH_TSTATES = 4;
        public const int NMI_INTERRUPT_ACKNOWLEDGE_TSTATES = 5;
        public const int IM0_INTERRUPT_ACKNOWLEDGE_TSTATES = 6;
        public const int IM1_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;
        public const int IM2_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;

        private ExecutionPackage _executingPackage;

        private bool _running;
        private bool _halted;
        private bool _suspendMachineCycles;
        private ushort _topOfStack;
        private long _emulatedTStates;

        private int _pendingWaitCycles;

        private ExternalClock _clock;
        private bool _clockSemaphore;
        private Stopwatch _startStopTimer = new Stopwatch();

        private Thread _instructionCycle;

        private EventHandler<ExecutionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler<HaltReason> _onHalt;
        private EventHandler<int> _onBeforeInsertWaitCycles;
        
        private Func<byte> _interruptCallback;

        public IDebugProcessor Debug => this;
        internal ITiming Timing => this;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; private set; }
        public MemorySpace Memory { get; private set; }
        public Ports Ports { get; private set; }
        public ExecutionPackage ExecutingPackage => _executingPackage;
        public ProcessorIO IO { get; private set; }
        
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; } = true;
        public double FrequencyInMHz { get; private set; }
        public TimingMode TimingMode { get; private set; } = TimingMode.FastAndFurious;
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 
        public long EmulatedTStates => _emulatedTStates;
        public TimeSpan Elapsed => _startStopTimer.Elapsed;

        public event EventHandler<ExecutionPackage> OnClockTick;

        // why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        // (basically, automatic properties don't work as events when they are explicitly on the interface, so we use a backing variable... old-school style)
        event EventHandler<ExecutionPackage> IDebugProcessor.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugProcessor.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler<int> IDebugProcessor.OnBeforeInsertWaitCycles { add { _onBeforeInsertWaitCycles += value; } remove { _onBeforeInsertWaitCycles -= value; } }
        event EventHandler IDebugProcessor.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugProcessor.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler<HaltReason> IDebugProcessor.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }

        public void Start(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                TimingMode = timingMode;
                _emulatedTStates = 0;

                _beforeStart?.Invoke(null, null);
                Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
                _running = true;

                _startStopTimer.Reset();
                _startStopTimer.Start();
                if (timingMode == TimingMode.PseudoRealTime) _clock.Start();

                _instructionCycle = new Thread(new ThreadStart(InstructionCycle));
                _instructionCycle.Start();
            }
        }

        public void Stop()
        {
            _running = false;
            _startStopTimer.Stop();
            if (_clock.Started) _clock.Stop();
            _halted = false;
            _instructionCycle?.Interrupt();

            _onStop?.Invoke(null, null);
        }

        public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
        {
            _halted = true;
            _onHalt?.Invoke(null, reason);
        }

        public void Resume()
        {
            _halted = false;
        }

        public void RunUntilStopped()
        {
            while (_running) Thread.Sleep(0);
        }

        public void ResetAndClearMemory(bool restartAfterReset = true)
        {
            Stop();
            Memory.Clear();
            Registers.Clear();
            Registers.SP = _topOfStack;
            if (restartAfterReset) Start(0, this.EndOnHalt, this.TimingMode);
        }

        public void Push(WordRegister register)
        {
            ushort value = Registers[register];
            Registers.SP--;
            Timing.StackWriteCycle(true, value.HighByte());
            Memory.WriteByteAt(Registers.SP, value.HighByte(), true);

            Registers.SP--;
            Timing.StackWriteCycle(false, value.LowByte());
            Memory.WriteByteAt(Registers.SP, value.LowByte(), true);
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            Timing.BeginStackReadCycle();
            low = Memory.ReadByteAt(Registers.SP, true);
            Timing.EndStackReadCycle(false, low);
            Registers.SP++;

            Timing.BeginStackReadCycle();
            high = Memory.ReadByteAt(Registers.SP, true);
            Timing.EndStackReadCycle(true, high);
            Registers.SP++;

            ushort value = (low, high).ToWord();
            Registers[register] = value;
        }

        public ushort Peek()
        {
            return Memory.ReadWordAt(Registers.SP, true);
        }

        public void SetInterruptMode(InterruptMode mode)
        {
            InterruptMode = mode;
        }

        public void RaiseInterrupt(Func<byte> instructionReadCallback = null)
        {
            IO.SetInterruptState();
            if (InterruptsEnabled)
            {
                _interruptCallback = instructionReadCallback;
            }
        }

        public void RaiseNonMasktableInterrupt()
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
            // Note that it's fine for client software to add wait cycles
            // and then add again *before* the wait has happened.
            // Waits are only actually inserted at certain points in the instruction cycle. 
            _pendingWaitCycles = waitCycles;
        }

        private void InstructionCycle()
        {
            while (_running)
            {
                ExecutionPackage package = null;
                ushort pc = Registers.PC;

                if (!_halted)
                {
                    // decode next instruction
                    package = DecodeInstruction();
                    if (package == null)
                    {
                        // only happens if we reach the end of memory mid-instruction, if so we bail out
                        Stop();
                        return;
                    }
                }
                else
                {
                    if (EndOnHalt)
                    {
                        Stop();
                        return;
                    }
                    // when halted, the CPU should execute NOP continuously
                    Timing.OpcodeFetchCycle(Registers.PC, InstructionSet.NOP.Opcode); // we still need to generate the right ticks + wait cycles for a NOP
                    package = new ExecutionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);
                }

                // run the decoded instruction and deal with timing / ticks
                ExecutionResult result = Execute(package);
                Registers.R++; // R is incremented after every instruction as a memory refresh initiator
                _executingPackage = null;

                HandleNonMaskableInterrupts();
                HandleMaskableInterrupts();
            }
        }

        private ExecutionResult Execute(ExecutionPackage package)
        {
            _beforeExecute?.Invoke(this, package);
            _executingPackage = package;

            // run the instruction microcode implementation
            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);
            if (result.Flags != null) Registers.Flags.Value = result.Flags.Value;
            _afterExecute?.Invoke(this, result);
            
            return result;
        } 
        
        ExecutionResult IDebugProcessor.Execute(byte[] opcode)
        {
            Memory.WriteBytesAt(Registers.PC, opcode, true);
            ExecutionPackage package = DecodeInstruction();
            if (package == null)
            {
                // only happens if we reach the end of memory mid-instruction, if so we bail out
                Stop();
                return null;
            }

            return Execute(package);
        }

        ExecutionResult IDebugProcessor.Execute(ExecutionPackage package)
        {
            Registers.PC += package.Instruction.SizeInBytes; // simulate the decode cycle effect on PC
            return Execute(package);
        }

        private ExecutionPackage DecodeInstruction()
        {
            byte b0, b1, b3;
            Instruction instruction;
            InstructionData data = new InstructionData();
            ushort instructionAddress = Registers.PC;

            b0 = OpcodeFetch(); // 4 ticks, always, but may add more below
            Registers.PC++;

            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD) // is byte 0 a prefix code?
            {
                b1 = Memory.ReadByteAt(Registers.PC, true); // peek ahead
                if ((b0 == 0xDD || b0 == 0xFD) && (b1 == 0xDD || b1 == 0xFD))
                {
                    // sequences of 0xDD / 0xFD / either / or count as NOP until the final 0xDD / 0xFD which is then the prefix byte
                    b1 = OpcodeFetch(); // to generate correct ticks
                    Registers.PC++;
                    return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                }
                else if ((b0 == 0xDD || b0 == 0xFD) && b1 == 0xCB)
                {
                    // DDCB / FDCB: four-byte opcode = two prefix bytes + one displacement byte + one opcode byte - no four-byte instruction has any actual operand values
                    b1 = OpcodeFetch();
                    Registers.PC++;
                    data.Argument1 = OperandFetch(); // displacement byte is the only 'operand'
                    Registers.PC++;
                    b3 = OpcodeFetch();
                    Registers.PC++;
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        return new ExecutionPackage(InstructionSet.NOP, data, (ushort)(instructionAddress));
                    }                    

                    return new ExecutionPackage(instruction, data, instructionAddress);
                }
                else // all other prefixed instructions: a two-byte opcode + up to 2 operand bytes
                {
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                    }
                    else
                    {
                        b1 = OpcodeFetch(); // Note: while we already have this value from peeking ahead above, we need to do this to generate the right IO state and ticks
                        Registers.PC++; // advance PC to position of b1
                        addTicksIfNeededForOpcodeFetch(); // some specific opcodes have longer opcode fetch cycles than normal

                        if (instruction.Argument1 != InstructionElement.None)
                        {
                            data.Argument1 = OperandFetch();
                            Registers.PC++;
                            if (instruction.Argument2 != InstructionElement.None)
                            {
                                data.Argument2 = OperandFetch();
                                Registers.PC++;
                            }
                        }

                        return new ExecutionPackage(instruction, data, instructionAddress);
                    }
                }
            }
            else
            {
                // unprefixed instruction - 1 byte opcode + up to 2 operand bytes
                if (!InstructionSet.Instructions.TryGetValue(b0, out instruction))
                {
                    return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                }
                    
                addTicksIfNeededForOpcodeFetch();

                if (instruction.Argument1 != InstructionElement.None)
                {
                    data.Argument1 = OperandFetch();
                    Registers.PC++;
                    if (instruction.Argument2 != InstructionElement.None)
                    {
                        // some instructions (OK, just CALL) have a prolonged high byte operand read 
                        // but ONLY if a) it's CALL nn or b) if the condition (CALL NZ, for example) is satisfied (ie Flags.NZ is true)
                        // otherwise it's the standard size. Timing oddities like this are noted in the TimingExceptions structure,
                        // but this is the only one that affects the instruction decode cycle (the others affect Memory Read cycles)
                        if (instruction.TimingExceptions.HasConditionalOperandDataReadHigh4)
                        {
                            if (instruction.Condition == Condition.None || Registers.Flags.SatisfyCondition(instruction.Condition))
                            {
                                WaitForNextClockTick();
                            }
                        }

                        data.Argument2 = OperandFetch();
                        Registers.PC++;
                    }
                }

                return new ExecutionPackage(instruction, data, instructionAddress);
            }

            void addTicksIfNeededForOpcodeFetch()
            {
                // normal opcode fetch cycle is 4 clock cycles, BUT some instructions have longer opcode fetch cycles - 
                // this is either a long *first* opcode fetch (followed by a normal *second* opcode fetch, if any)
                // OR a normal first opcode fetch and a long *second* opcode fetch, never both
                int extraTStates = extraTStatesFor(instruction.Timing.CyclesByType(MachineCycleType.OpcodeFetch).First());
                if (extraTStates == 0) extraTStates = extraTStatesFor(instruction.Timing.CyclesByType(MachineCycleType.OpcodeFetch).Last());

                if (extraTStates > 0)
                {
                    for (int i = 0; i < extraTStates; i++)
                    {
                        WaitForNextClockTick();
                    }
                }
            }

            int extraTStatesFor(MachineCycle machineCycle)
            {
                return (machineCycle.TStates - OPCODE_FETCH_TSTATES);
            }
        }

        private void WaitForNextClockTick()
        {
            if (TimingMode == TimingMode.FastAndFurious)
            {
                _emulatedTStates++;
                OnClockTick?.Invoke(this, _executingPackage);
                return;
            }
            else
            {
                while (_clockSemaphore == false);
                _clockSemaphore = false;
                _emulatedTStates++;
                OnClockTick?.Invoke(this, _executingPackage);
            }
        }

        private void WhenTheClockTicks(object sender, EventArgs e)
        {
            _clockSemaphore = true;
        }

        private void InsertWaitCycles()
        {
            if (_pendingWaitCycles > 0)
            {
                _onBeforeInsertWaitCycles?.Invoke(this, _pendingWaitCycles);
            }

            while (_pendingWaitCycles-- > 0)
            {
                WaitForNextClockTick();
            }
        }

        private byte OpcodeFetch()
        {
            byte opcode = Memory.ReadByteAt(Registers.PC, true);
            if (!_suspendMachineCycles) Timing.OpcodeFetchCycle(Registers.PC, opcode);
            return opcode;
        }

        private byte OperandFetch()
        {
            byte operand = Memory.ReadByteAt(Registers.PC, true);
            if (!_suspendMachineCycles) Timing.MemoryReadCycle(Registers.PC, operand);
            return operand;
        }

        private void HandleNonMaskableInterrupts()
        {
            if (IO.NMI)
            {
                if (_halted)
                {
                    Resume();
                }

                Timing.BeginInterruptRequestAcknowledgeCycle(NMI_INTERRUPT_ACKNOWLEDGE_TSTATES);

                Push(WordRegister.PC);
                Registers.PC = 0x0066;
                _halted = false;

                Timing.EndInterruptRequestAcknowledgeCycle();
            }

            IO.EndNMIState();
        }

        private ExecutionPackage DecodeIM0Interrupt()
        {
            // In IM0, when an interrupt is generated, the CPU will ask the device
            // to send one to four bytes which are decoded into an instruction, which will then be executed.
            // To emulate this without heavily re-writing the decode loop, we will temporarily copy 
            // the last 4 bytes of RAM to an array, use those 4 bytes for the interrupt decode, then restore them
            // and fix up the program counter. Sneaky, eh?

            ushort address = (ushort)(Memory.SizeInBytes - 5);
            byte[] lastFour = Memory.ReadBytesAt(address, 4, true);
            ushort pc = Registers.PC;
            Registers.PC = address;

            _suspendMachineCycles = true;
            for (int i = 0; i < 4; i++)
            {
                byte value = _interruptCallback();
                IO.SetDataBusValue(value); // set this directly as there are no machine cycles currently
                Memory.WriteByteAt((ushort)(address + i), value, true);
            }

            ExecutionPackage package = DecodeInstruction();
            _suspendMachineCycles = false;

            Registers.PC = pc;
            Memory.WriteBytesAt(address, lastFour, true);

            return package;
        }

        private void HandleMaskableInterrupts()
        {
            if (IO.INT && InterruptsEnabled)
            {
                if (_halted)
                {
                    Resume();
                }

                if (_interruptCallback == null && InterruptMode != InterruptMode.IM1)
                {
                    throw new InterruptException("Interrupt mode is " + InterruptMode.ToString() + " which requires a callback for reading data from the interrupting device. Callback was null.");
                }

                switch (InterruptMode)
                {
                    case InterruptMode.IM0: // read instruction data from data bus in 1-4 opcode fetch cycles and execute resulting instruction - flags are set but PC is unaffected
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM0_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        ExecutionPackage package = DecodeIM0Interrupt();
                        ushort pc = Registers.PC;
                        Execute(package);
                        Registers.PC = pc;
                        break;

                    case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM1_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        break;

                    case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                        _interruptCallback(); // device must populate data bus with low byte of address
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM2_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Push(WordRegister.PC);
                        ushort address = (ushort)((Registers.I * 256) + IO.DATA_BUS);
                        Registers.PC = Memory.ReadWordAt(address, false);
                        break;
                }

                _halted = false;

                Timing.EndInterruptRequestAcknowledgeCycle();
            }
        }

        #region ITiming
        void ITiming.OpcodeFetchCycle(ushort address, byte data)
        {
            IO.SetOpcodeFetchState(address);
            WaitForNextClockTick();
            IO.AddOpcodeFetchData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndOpcodeFetchState();
            IO.SetAddressBusValue(Registers.IR);
            IO.SetDataBusValue(0x00);

            WaitForNextClockTick();
            WaitForNextClockTick();
        }

        void ITiming.MemoryReadCycle(ushort address, byte data)
        {
            IO.SetMemoryReadState(address);
            WaitForNextClockTick();

            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();

            if (ExecutingPackage != null &&
                ExecutingPackage.Instruction.TimingExceptions.HasMemoryRead4)
            {
                WaitForNextClockTick();
            }
        }

        void ITiming.MemoryWriteCycle(ushort address, byte data)
        {
            IO.SetMemoryWriteState(address, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryWriteState();
            WaitForNextClockTick();

            if (ExecutingPackage != null &&
                ExecutingPackage.Instruction.TimingExceptions.HasMemoryWrite5)
            {
                WaitForNextClockTick();
                WaitForNextClockTick();
            }
        }

        void ITiming.BeginStackReadCycle()
        {
            IO.SetMemoryReadState(Registers.SP);
            WaitForNextClockTick();
        }


        void ITiming.EndStackReadCycle(bool highByte, byte data)
        {
            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();
        }

        void ITiming.StackWriteCycle(bool highByte, byte data)
        {
            IO.SetMemoryWriteState(Registers.SP, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryWriteState();
            WaitForNextClockTick();
        }

        void ITiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortReadState(address);
            WaitForNextClockTick();
        }

        void ITiming.CompletePortReadCycle(byte data)
        {
            IO.AddPortReadData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortReadState();
            WaitForNextClockTick();
        }

        void ITiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortWriteState(address, data);
            WaitForNextClockTick();
        }

        void ITiming.CompletePortWriteCycle()
        {
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortWriteState();
            WaitForNextClockTick();
        }

        void ITiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            IO.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }

        void ITiming.EndInterruptRequestAcknowledgeCycle()
        {
            IO.EndInterruptState();
        }

        void ITiming.InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }
        #endregion  

        public Processor(IMemoryMap map = null, ushort? topOfStackAddress = null, double frequencyInMHz = 4, bool enableFlagPrecalculation = true)
        {
            FrequencyInMHz = frequencyInMHz;

            Registers = new Registers();
            Ports = new Ports(this);
            Memory = new MemorySpace();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));
            IO = new ProcessorIO(this);

            _topOfStack = topOfStackAddress ?? (ushort)(Memory.SizeInBytes - 3);
            Registers.SP = _topOfStack;

            // if precalculation is enabled, all flag combinations for all input values for 8-bit ALU / bitwise operations are pre-built now (not 16-bit ALU operations, far too big!)
            // this is *slightly* faster than calculating them in real-time, but if you want to debug flag calculation you should
            // disable this and attach a debugger to the flag calculation methods in FlagLookup.cs
            FlagLookup.EnablePrecalculation = enableFlagPrecalculation;
            FlagLookup.BuildFlagLookupTables();
            
            InstructionSet.Build();

            // if running in Pseudo-Real Time mode, an external clock routine is used to sync instruction execution to real time. Sort of.
            // The timing is approximate due to unpredictability as to how long .NET will take to run the instruction code each time, so you do get clock misses, but all emulated devices run at the same
            // speed so the effect is as if time itself is slowing and accelerating (by a tiny %) inside the emulation, and everything remains consistent.
            _clock = new ExternalClock(frequencyInMHz);
            _clock.OnTick += WhenTheClockTicks;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Z80.Core
{
    public class Processor : IDebugMode
    {
        private ExecutionPackage _executingPackage;

        private bool _running;
        private bool _halted;
        private ushort _topOfStack;
        private long _emulatedClockCycles;

        private ExternalClock _clock;
        private bool _clockSemaphore;
        private Stopwatch _startStopTimer = new Stopwatch();

        private Thread _instructionCycle;

        private EventHandler<ExecutionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler<HaltReason> _onHalt;
        
        private Action _interruptCallback;

        public IDebugMode Debug => (IDebugMode)this;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; private set; }
        public Memory Memory { get; private set; }
        public Ports Ports { get; private set; }
        public ExecutionPackage ExecutingPackage => _executingPackage;
        public IO IO { get; private set; }
        
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; } = true;
        public int FrequencyInMHz { get; private set; }
        public TimingMode TimingMode { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 
        public long EmulatedClockCycles => _emulatedClockCycles;
        public TimeSpan Elapsed => _startStopTimer.Elapsed;

        public event EventHandler<ExecutionPackage> OnClockTick;
        
        // why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        // (basically, automatic properties don't work as events when they are explicitly on the interface, so we use a backing variable... old-school style)
        event EventHandler<ExecutionPackage> IDebugMode.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugMode.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler IDebugMode.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugMode.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler<HaltReason> IDebugMode.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }

        public void Start(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                TimingMode = timingMode;
                _emulatedClockCycles = 0;

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

        public void WaitUntilStopped()
        {
            while (_running) Thread.Sleep(0);
        }

        public void ResetAndClearMemory()
        {
            if (State != ProcessorState.Stopped) throw new Z80Exception("Cannot reset the processor unless it is stopped. Call Stop() first.");

            Memory.Clear();
            Registers.Clear();
            Registers.SP = _topOfStack;
        }

        public void Push(WordRegister register)
        {
            ushort value = Registers[register];
            Registers.SP--;
            StackWriteCycle(true, value.HighByte());
            Memory.WriteByteAt(Registers.SP, value.HighByte(), true);

            Registers.SP--;
            StackWriteCycle(false, value.LowByte());
            Memory.WriteByteAt(Registers.SP, value.LowByte(), true);
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            BeginStackReadCycle();
            low = Memory.ReadByteAt(Registers.SP, true);
            EndStackReadCycle(false, low);
            Registers.SP++;

            BeginStackReadCycle();
            high = Memory.ReadByteAt(Registers.SP, true);
            EndStackReadCycle(true, high);
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

        public void RaiseInterrupt(Action instructionReadCallback = null)
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

        internal void OpcodeFetchCycle(ushort address, byte data)
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

        internal void MemoryReadCycle(ushort address, byte data)
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

        internal void MemoryWriteCycle(ushort address, byte data)
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

        internal void BeginStackReadCycle()
        {
            IO.SetMemoryReadState(Registers.SP);
            WaitForNextClockTick();
        }


        internal void EndStackReadCycle(bool highByte, byte data)
        {
            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();
        }

        internal void StackWriteCycle(bool highByte, byte data)
        {
            IO.SetMemoryWriteState(Registers.SP, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryWriteState();
            WaitForNextClockTick();
        }

        internal void BeginPortReadCycle()
        {
            IO.SetPortReadState((Registers.B, Registers.C).ToWord());
            WaitForNextClockTick();
        }

        internal void CompletePortReadCycle(byte data)
        {
            IO.AddPortReadData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortReadState();
            WaitForNextClockTick();
        }

        internal void BeginPortWriteCycle(byte data)
        {
            IO.SetPortWriteState((Registers.B, Registers.C).ToWord(), data);
            WaitForNextClockTick();
        }

        internal void CompletePortWriteCycle()
        {
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortWriteState();
            WaitForNextClockTick();
        }

        internal void BeginInterruptRequestAcknowledgeCycle(int cycles)
        {
            IO.SetInterruptState();
            for (int i = 0; i < cycles; i++)
            {
                WaitForNextClockTick();
            }
        }

        internal void EndInterruptRequestAcknowledgeCycle()
        {
            IO.EndInterruptState();
        }

        internal void InternalOperationCycle(int clockCycles)
        {
            for (int i = 0; i < clockCycles; i++)
            {
                WaitForNextClockTick();
            }
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
                    package = new ExecutionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);
                }

                // run the decoded instruction and deal with timing / ticks
                ExecutionResult result = Execute(package);
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

        ExecutionResult IDebugMode.Execute(ExecutionPackage package)
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

            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD)
            {
                b1 = Memory.ReadByteAt(Registers.PC, true); // peek ahead
                if (b1 == 0xDD || b1 == 0xFD)
                {
                    // sequences of 0xDD / 0xFD / either / or count as NOP until the final 0xDD / 0xFD which is then the prefix byte
                    b1 = OpcodeFetch(); // to generate correct ticks
                    Registers.PC++;
                    return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                }
                else if (b1 == 0xCB)
                {
                    // four-byte opcode = two prefix bytes + one displacement byte + one opcode byte - *no* four-byte instruction has operand values
                    data.Argument1 = DisplacementByteFetch();
                    Registers.PC++;
                    b3 = OpcodeFetch();
                    Registers.PC++;
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        return new ExecutionPackage(InstructionSet.NOP, data, (ushort)(instructionAddress));
                    }

                    return new ExecutionPackage(instruction, data, instructionAddress);
                }
                else // all other prefixed instructions are a two-byte opcode + up to 2 operand bytes
                {
                    b1 = OpcodeFetch(); // Note: while we already have this value from peeking ahead above, we need to do this to generate the right IO state and ticks
                    Registers.PC++;
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        if (b0 == 0xED)
                        {
                            byte[] undocumentedED = new byte[] { 0x4C, 0x4E, 0x54, 0x55, 0x5C, 0x5D, 0x64, 0x65, 0x6C, 0x6D, 0x6E, 0x71, 0x74, 0x75, 0x76, 0x77, 0x7C, 0x7D, 0x7E };
                            // NOTES: these specific ED-prefix instructions are not implemented separately here, hence the attempt to look them up fails,
                            // but are functionally the equivalent of the *unprefixed* instruction without the ED prefix,
                            // just taking 4 ticks longer. In this case we should behave as per the unprefixed instruction, because we already added the 4 ticks getting here.
                            if (!undocumentedED.Contains(b1))
                            {
                                return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                            }
                            else
                            {
                                // ignore the prefix and use the unprefixed instruction: there are many holes in the ED instruction set
                                // and some code uses this technique to consume extra cycles for synchronisation but expects the unprefixed instruction to run as normal
                                Registers.PC++;
                                return DecodeInstruction();
                            }
                        }

                        // any other failed instruction lookup generates a 2-NOP
                        return new ExecutionPackage(InstructionSet.NOP, data, instructionAddress);
                    }
                    else
                    {
                        addTicksIfNeededForOpcodeFetch();

                        if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                        {
                            data.Argument1 = OperandFetch(false, false);
                            Registers.PC++;
                        }
                        else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                        {
                            data.Argument1 = OperandFetch(true, true);
                            Registers.PC++;
                            data.Argument2 = OperandFetch(true, false);
                            Registers.PC++;
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

                if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                {
                    data.Argument1 = OperandFetch(false, false);
                    Registers.PC++;
                }
                else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                {
                    data.Argument1 = OperandFetch(true, true);
                    Registers.PC++;
                    // some instructions (OK, just CALL) have a prolonged high byte operand read (4 clocks instead of 3)
                    // but ONLY if a) it's CALL nn or b) if the condition (CALL NZ, for example) is satisfied (ie Flags.NZ is true)
                    // otherwise it's the standard size. Timing oddities like this are noted in the TimingExceptions structure,
                    // but this is the only one that affects the instruction decode cycle (the others affect Memory Read cycles)
                    if (instruction.TimingExceptions.HasConditionalOperandDataReadHigh4)
                    {
                        if (instruction.Condition == null || Registers.Flags.SatisfyCondition(instruction.Condition.Value))
                        {
                            WaitForNextClockTick();
                        }
                    }

                    data.Argument2 = OperandFetch(true, false);
                    Registers.PC++;
                }

                return new ExecutionPackage(instruction, data, instructionAddress);
            }

            void addTicksIfNeededForOpcodeFetch()
            {
                // normal opcode fetch cycle is 4 clock cycles, BUT some instructions have longer opcode fetch cycles - 
                // this is either a long *first* opcode fetch (followed by a normal *second* opcode fetch, if any)
                // OR a normal first opcode fetch and a long *second* opcode fetch, never both
                int extraCycles = extraClockCyclesFor(instruction.Timing.CyclesByType(MachineCycleType.OpcodeFetch).First());
                if (extraCycles == 0) extraCycles = extraClockCyclesFor(instruction.Timing.CyclesByType(MachineCycleType.OpcodeFetch).Last());

                if (extraCycles > 0)
                {
                    for (int i = 0; i < extraCycles; i++)
                    {
                        WaitForNextClockTick();
                    }
                }
            }

            int extraClockCyclesFor(MachineCycle machineCycle)
            {
                return (MachineCycle.OPCODE_FETCH_STANDARD_CYCLES - machineCycle.ClockCycles);
            }
        }

        private void WaitForNextClockTick()
        {
            if (TimingMode == TimingMode.FastAndFurious)
            {
                _emulatedClockCycles++;
                OnClockTick?.Invoke(this, _executingPackage);
                return;
            }
            else
            {
                while (_clockSemaphore == false);
                _clockSemaphore = false;
                _emulatedClockCycles++;
                OnClockTick?.Invoke(this, _executingPackage);
            }
        }

        private void InsertWaitCycles()
        {
            while (IO.WAIT)
            {
                WaitForNextClockTick();
            }
        }

        private byte OpcodeFetch()
        {
            byte opcode = Memory.ReadByteAt(Registers.PC, true);
            OpcodeFetchCycle(Registers.PC, opcode);
            return opcode;
        }

        private byte MemoryFetch(ushort address)
        {
            byte data = Memory.ReadByteAt(address, true);
            MemoryReadCycle(address, data);
            return data;
        }

        private byte OperandFetch(bool wordOperand, bool highByte)
        {
            return MemoryFetch(Registers.PC);
        }

        private byte DisplacementByteFetch()
        {
            return MemoryFetch(Registers.PC);
        }

        private void HandleNonMaskableInterrupts()
        {
            if (IO.NMI)
            {
                BeginInterruptRequestAcknowledgeCycle(MachineCycle.NMI_INTA_CYCLES);

                Push(WordRegister.PC);
                Registers.PC = 0x0066;
                _halted = false;

                EndInterruptRequestAcknowledgeCycle();
            }

            IO.EndNMIState();
        }

        private void HandleMaskableInterrupts()
        {
            if (IO.INT && InterruptsEnabled)
            {
                if (_interruptCallback == null && InterruptMode != InterruptMode.IM1)
                {
                    throw new InterruptException("Interrupt mode is " + InterruptMode.ToString() + " which requires a callback for reading data from the interrupting device. Callback was null.");
                }

                switch (InterruptMode)
                {
                    case InterruptMode.IM0: // read instruction data from data bus in 1-4 cycles and execute resulting instruction - flags are set but PC is unaffected
                        BeginInterruptRequestAcknowledgeCycle(MachineCycle.IM0_INTA_CYCLES);
                        //ExecutionPackage package = DecodeInterrupt(() =>
                        //{
                        //    _interruptCallback(); // each time callback is called, device will set data bus with next instruction byte
                        //    return IO.DATA_BUS;
                        //});
                        //Execute(package); // instruction is *usually* RST which diverts execution, but can be any valid instruction
                        break;

                    case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                        BeginInterruptRequestAcknowledgeCycle(MachineCycle.IM1_INTA_CYCLES);
                        Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        break;

                    case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                        _interruptCallback(); // device must populate data bus with low byte of address
                        BeginInterruptRequestAcknowledgeCycle(MachineCycle.IM2_INTA_CYCLES);
                        Push(WordRegister.PC);
                        ushort address = (ushort)((Registers.I * 256) + IO.DATA_BUS);
                        Registers.PC = Memory.ReadWordAt(address, false);
                        break;
                }

                _halted = false;

                EndInterruptRequestAcknowledgeCycle();
            }
        }

        internal Processor(IMemoryMap map, ushort topOfStackAddress, int frequencyInMHz = 4, bool enableFlagPrecalculation = true)
        {
            FrequencyInMHz = frequencyInMHz;

            Registers = new Registers();
            Ports = new Ports(this);
            Memory = new Memory();
            Memory.Initialise(this, map);
            IO = new IO(this);

            _topOfStack = topOfStackAddress;
            Registers.SP = _topOfStack;

            // if precalculation is enabled, all flag combinations for all input values for 8-bit ALU / bitwise operations are pre-built now (*not* 16-bit ALU operations)
            // this is *slightly* faster than calculating them in real-time, but if you want to debug flag calculation you should
            // disable this and attach a debugger to the flag calculation methods in FlagLookup.cs
            FlagLookup.EnablePrecalculation = enableFlagPrecalculation;
            FlagLookup.BuildFlagLookupTables(); 
            var nop = InstructionSet.NOP; // this causes the whole instruction set to get built now, so it's not done on the first instruction run

            _clock = new ExternalClock(frequencyInMHz);
            _clock.OnTick += WhenTheClockTicks;
        }

        private void WhenTheClockTicks(object sender, EventArgs e)
        {
            _clockSemaphore = true;
        }
    }
}

using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;
using Zem80.Core.IO;
using System.Collections;
using System.Collections.Generic;

namespace Zem80.Core
{
    public class Processor : IDebugProcessor, IInstructionTiming
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;

        public const int OPCODE_FETCH_TSTATES = 4;
        public const int NMI_INTERRUPT_ACKNOWLEDGE_TSTATES = 5;
        public const int IM0_INTERRUPT_ACKNOWLEDGE_TSTATES = 6;
        public const int IM1_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;
        public const int IM2_INTERRUPT_ACKNOWLEDGE_TSTATES = 7;

        private bool _running;
        private bool _halted;
        private bool _suspended;
        private HaltReason _reasonForLastHalt;
        private bool _suspendMachineCycles;
        private int _pendingWaitCycles;
        private ushort _topOfStack;

        private ExternalClock _clock;
        private bool _clockSemaphore;
        private Stopwatch _startStopTimer;
        private long _emulatedTStates;

        private Thread _instructionCycle;
        private InstructionPackage _executingInstructionPackage;
        private Func<byte> _interruptCallback;
        private bool _iff1;
        private bool _iff2;

        private EventHandler<InstructionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler<HaltReason> _onHalt;
        private EventHandler<int> _beforeInsertWaitCycles;
        private EventHandler<InstructionPackage> _onBreakpoint;

        private IList<ushort> _breakpoints = new List<ushort>();
        
        public IDebugProcessor Debug => this;
        public IInstructionTiming Timing => this;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; private set; }
        public IMemoryBank Memory { get; private set; }
        public Ports Ports { get; private set; }
        public ProcessorIO IO { get; private set; }

        public InterruptMode InterruptMode { get; private set; }
        public bool InterruptsEnabled => _iff1;
        public bool InterruptsPaused => _iff2;
        public double FrequencyInMHz { get; private set; }
        public TimingMode TimingMode { get; private set; }
        public ProcessorState State => _running ? _suspended ? ProcessorState.Suspended : _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 
        public long EmulatedTStates => _emulatedTStates;
        public TimeSpan Elapsed => _startStopTimer.Elapsed;

        IEnumerable<ushort> IDebugProcessor.Breakpoints => _breakpoints;

        // client code should use this event to synchronise activity with the CPU clock cyles
        public event EventHandler<InstructionPackage> OnClockTick;

        // IDebugProcessor gives you access to these extra event handlers (and some methods) that are more... dangerous than the main API
        // the Processor.Debug property gives access to the current instance as IDebugProcessor

        // Note: why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        // (basically, automatic properties don't work as events when they are explicitly on the interface, so we use a backing variable... old-school style)
        event EventHandler<InstructionPackage> IDebugProcessor.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugProcessor.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler<int> IDebugProcessor.BeforeInsertWaitCycles { add { _beforeInsertWaitCycles += value; } remove { _beforeInsertWaitCycles -= value; } }
        event EventHandler IDebugProcessor.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugProcessor.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler<HaltReason> IDebugProcessor.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }
        event EventHandler<InstructionPackage> IDebugProcessor.OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public void Start(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                TimingMode = timingMode;
                _emulatedTStates = 0;

                DisableInterrupts();

                Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
                _beforeStart?.Invoke(null, null);
                _running = true;

                _startStopTimer.Reset();
                _startStopTimer.Start();
                if (timingMode == TimingMode.PseudoRealTime) _clock.Start(); // only use the clock thread if we need it

                IO.Clear();
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

        public void Suspend()
        {
            _suspended = true;
        }

        public void RunUntilStopped()
        {
            while (_running) Thread.Sleep(0);
        }

        public void ResetAndClearMemory(bool restartAfterReset = true)
        {
            IO.SetResetState();
            Stop();
            Memory.Clear();
            Registers.Clear();
            IO.Clear();
            Registers.SP = _topOfStack;
            if (restartAfterReset) Start(0, this.EndOnHalt, this.TimingMode);
        }

        public void Push(WordRegister register)
        {
            ushort value = Registers[register];
            Registers.SP--;
            Timing.BeginStackWriteCycle(true, value.HighByte());
            Memory.Untimed.WriteByteAt(Registers.SP, value.HighByte());
            Timing.EndStackWriteCycle();

            Registers.SP--;
            Timing.BeginStackWriteCycle(false, value.LowByte());
            Memory.Untimed.WriteByteAt(Registers.SP, value.LowByte());
            Timing.EndStackWriteCycle();
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            Timing.BeginStackReadCycle();
            low = Memory.Untimed.ReadByteAt(Registers.SP);
            Timing.EndStackReadCycle(false, low);
            Registers.SP++;

            Timing.BeginStackReadCycle();
            high = Memory.Untimed.ReadByteAt(Registers.SP);
            Timing.EndStackReadCycle(true, high);
            Registers.SP++;

            ushort value = (low, high).ToWord();
            Registers[register] = value;
        }

        public ushort Peek()
        {
            return Memory.Untimed.ReadWordAt(Registers.SP);
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

        public void RaiseNonMaskableInterrupt()
        {
            IO.SetNMIState();
        }

        public void DisableInterrupts()
        {
            _iff1 = false;
            _iff2 = false;
        }

        public void EnableInterrupts()
        {
            _iff1 = true;
            _iff2 = true;
        }

        public void RestoreInterruptsFromNMI()
        {
            _iff1 = _iff2;
        }

        public void AddWaitCycles(int waitCycles)
        {
            // Note that it's fine for client software to add wait cycles
            // and then reset and add again *before* the wait has happened.
            // Waits are only actually inserted at certain points in the instruction cycle. 
            _pendingWaitCycles = waitCycles;
        }

        public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
        {
            if (!_halted)
            {
                _halted = true;
                _reasonForLastHalt = reason;
                _onHalt?.Invoke(null, reason);
            }
        }

        public void Resume()
        {
            _suspended = false;
            if (_halted)
            {
                _halted = false;
                if (_reasonForLastHalt == HaltReason.HaltInstruction) Registers.PC++;
                // if coming back from a HALT instruction (at next interrupt or by API override here), move the Program Counter on to step over the HALT instruction
                // otherwise we'll HALT forever in a loop
            }
        }

        void IDebugProcessor.AddBreakpoint(ushort address)
        {
            // Note that the breakpoint functionality is *very* simple and not checked
            // so if you add a breakpoint for an address which is not the start
            // of an instruction in the code, it will never be triggered
            if (!_breakpoints.Contains(address))
            {
                _breakpoints.Add(address);
            }    

        }

        void IDebugProcessor.RemoveBreakpoint(ushort address)
        {
            if (_breakpoints.Contains(address))
            {
                _breakpoints.Remove(address);
            }
        }

        private void InstructionCycle()
        {
            while (_running)
            {
                if (!_suspended) // if suspended, we should do *nothing at all* until resumed
                {
                    InstructionPackage package = null;
                    ushort pc = Registers.PC;

                    if (!_halted)
                    {
                        // decode next instruction 
                        // (note that the decode cycle leaves the Program Counter at the byte *after* this instruction unless it's adjusted by the instruction code itself,
                        // this is how the program moves on to the next instruction)
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

                        // run a NOP - by not decoding anything we leave the Program Counter where it was when we HALTed
                        // the PC will be advanced on resume from the HALT instruction when the next interrupt is handled

                        // since we're not decoding the instruction (it's HALT), we need to run the opcode fetch cycle for NOP
                        // so that the clock ticks and attached devices can generate interrupts to bring the CPU out of HALT;
                        // the following call does that..
                        FetchOpcodeByte();

                        // now send a NOP package to be executed
                        package = new InstructionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);

                    }

                    // run the decoded instruction and deal with timing / ticks
                    ExecutionResult result = Execute(package);
                    _executingInstructionPackage = null;

                    HandleNonMaskableInterrupts();
                    if (!result.SkipInterruptAfterExecution) HandleMaskableInterrupts();

                    Registers.R = (byte)(((Registers.R + 1) & 0x7F) | (Registers.R & 0x80)); // bits 0-6 of R are incremented as part of the memory refresh - bit 7 is preserved    
                }
            }
        }

        private ExecutionResult Execute(InstructionPackage package)
        {
            _beforeExecute?.Invoke(this, package);
            _executingInstructionPackage = package;

            // check for breakpoints
            if (_breakpoints.Contains(package.InstructionAddress))
            {
                _onBreakpoint?.Invoke(this, package);
            }

            if (package.Instruction.IsIndexed)
            {
                ushort wz = package.Instruction switch
                {
                    var i when i.Source.IsAddressFromIndexAndOffset() => Registers[i.Source.AsWordRegister()],
                    var i when i.Target.IsAddressFromIndexAndOffset() => Registers[i.Target.AsWordRegister()],
                    _ => 0
                };
                wz = (ushort)(wz + package.Data.Argument1);
                Registers.WZ = wz;
            }

            // run the instruction microcode implementation
            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);

            if (result.Flags != null) Registers.SetFlags(result.Flags.Value);
            _afterExecute?.Invoke(this, result);
            
            return result;
        }

        // execute an instruction directly (without the instruction cycle running)
        ExecutionResult IDebugProcessor.ExecuteDirect(byte[] opcode)
        {
            Memory.Untimed.WriteBytesAt(Registers.PC, opcode);
            InstructionPackage package = DecodeInstruction();
            if (package == null)
            {
                throw new InstructionDecoderException("Supplied opcode sequence does not decode to a valid instruction.");
            }

            return Execute(package);
        }

        // execute an instruction directly (specified by mnemonic, so no decoding necessary)
        ExecutionResult IDebugProcessor.ExecuteDirect(string mnemonic, byte? arg1, byte? arg2)
        {
            if (!InstructionSet.InstructionsByMnemonic.TryGetValue(mnemonic.ToUpper(), out Instruction instruction))
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

        private InstructionPackage DecodeInstruction()
        {
            byte b0, b1, b3; // placeholders for upcoming instruction bytes
            Instruction instruction;
            InstructionData data = new InstructionData();
            ushort instructionAddress = Registers.PC;

            b0 = FetchOpcodeByte(); // always at least one opcode byte
            Registers.PC++;

            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD) // was byte 0 a prefix code?
            {
                b1 = Memory.Untimed.ReadByteAt(Registers.PC); // peek ahead
                if ((b0 == 0xDD || b0 == 0xFD || b0 == 0xED) && (b1 == 0xDD || b1 == 0xFD || b1 == 0xED))
                {
                    // sequences of 0xDD / 0xFD / 0xED either / or count as NOP until the final 0xDD / 0xFD / 0xED which is then the prefix byte
                    b1 = FetchOpcodeByte(); // need to call this even though we have the value, to generate correct timing
                    Registers.PC++;
                    return new InstructionPackage(InstructionSet.NOP, data, instructionAddress);
                }
                else if ((b0 == 0xDD || b0 == 0xFD) && b1 == 0xCB)
                {
                    // DDCB / FDCB: four-byte opcode = two prefix bytes + one displacement byte + one opcode byte - no four-byte instruction has any actual operand values
                    b1 = FetchOpcodeByte();
                    Registers.PC++;
                    data.Argument1 = FetchOperandByte(); // displacement byte is the only 'operand'
                    Registers.PC++;
                    b3 = FetchOpcodeByte();
                    Registers.PC++;
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        // not a valid instruction - the Z80 spec says we should run a NOP instead
                        return new InstructionPackage(InstructionSet.NOP, data, (ushort)(instructionAddress));
                    }                    

                    return new InstructionPackage(instruction, data, instructionAddress);
                }
                else 
                {
                    // all other prefixed instructions: a two-byte opcode + up to 2 operand bytes
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        // not a valid instruction - the Z80 spec says we should run a NOP instead
                        return new InstructionPackage(InstructionSet.NOP, data, instructionAddress);
                    }
                    else
                    {
                        b1 = FetchOpcodeByte(); // need to call this even though we have the value, to generate correct timing
                        Registers.PC++;
                        addTicksIfNeededForOpcodeFetch(); // (some specific opcodes have longer opcode fetch cycles than normal)

                        // fetch the operand bytes (as required)
                        if (instruction.Argument1 != InstructionElement.None)
                        {
                            data.Argument1 = FetchOperandByte();
                            Registers.PC++;
                            if (instruction.Argument2 != InstructionElement.None)
                            {
                                data.Argument2 = FetchOperandByte();
                                Registers.PC++;
                            }
                        }

                        return new InstructionPackage(instruction, data, instructionAddress);
                    }
                }
            }
            else
            {
                // unprefixed instruction - 1 byte opcode + up to 2 operand bytes
                if (!InstructionSet.Instructions.TryGetValue(b0, out instruction))
                {
                    return new InstructionPackage(InstructionSet.NOP, data, instructionAddress);
                }
                    
                addTicksIfNeededForOpcodeFetch();

                // fetch the operand bytes (as required)
                if (instruction.Argument1 != InstructionElement.None)
                {
                    data.Argument1 = FetchOperandByte();
                    Registers.PC++;
                    if (instruction.Argument2 != InstructionElement.None)
                    {
                        // some instructions (OK, just CALL) have a prolonged high byte operand read 
                        // but ONLY if a) it's CALL nn or b) if the condition (CALL NZ, for example) is satisfied (ie Flags.NZ is true)
                        // otherwise it's the standard size. Timing oddities like this are noted in the TimingExceptions structure,
                        // but this is the only one that affects the instruction decode cycle (the others affect Memory Read cycles)
                        if (instruction.Timing.Exceptions.HasConditionalOperandDataReadHigh4)
                        {
                            if (instruction.Condition == Condition.None || Registers.Flags.SatisfyCondition(instruction.Condition))
                            {
                                WaitForNextClockTick();
                            }
                        }

                        data.Argument2 = FetchOperandByte();
                        Registers.PC++;
                    }
                }

                return new InstructionPackage(instruction, data, instructionAddress);
            }

            void addTicksIfNeededForOpcodeFetch()
            {
                // normal opcode fetch cycle is 4 clock cycles, BUT some instructions have longer opcode fetch cycles - 
                // this is either a long *first* opcode fetch (followed by a normal *second* opcode fetch, if any)
                // OR a normal first opcode fetch and a long *second* opcode fetch, never both
                int extraTStates = extraTStatesFor(instruction.Timing.ByType(MachineCycleType.OpcodeFetch).First());
                if (extraTStates == 0) extraTStates = extraTStatesFor(instruction.Timing.ByType(MachineCycleType.OpcodeFetch).Last());

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
            if (TimingMode == TimingMode.PseudoRealTime)
            {
                while (_clockSemaphore == false) ;
            }

            _emulatedTStates++;
            OnClockTick?.Invoke(this, _executingInstructionPackage);
        }

        private void WhenTheClockTicks(object sender, EventArgs e)
        {
            // this is called by the external clock thread, but it's not synchronised (no need)
            _clockSemaphore = true;
        }

        private void InsertWaitCycles()
        {
            int cyclesToAdd = _pendingWaitCycles;

            if (cyclesToAdd > 0)
            {
                _beforeInsertWaitCycles?.Invoke(this, cyclesToAdd);
            }

            while (cyclesToAdd > 0)
            {
                WaitForNextClockTick();
                cyclesToAdd--;
            }

            _pendingWaitCycles = 0;
        }

        private byte FetchOpcodeByte()
        {
            byte opcode = Memory.Untimed.ReadByteAt(Registers.PC);
            if (!_suspendMachineCycles) Timing.OpcodeFetchCycle(Registers.PC, opcode);
            return opcode;
        }

        private byte FetchOperandByte()
        {
            byte operand = Memory.Untimed.ReadByteAt(Registers.PC);
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

                _iff2 = _iff1; // save IFF1 state ready for RETN
                _iff1 = false; // disable maskable interrupts until RETN

                Timing.BeginInterruptRequestAcknowledgeCycle(NMI_INTERRUPT_ACKNOWLEDGE_TSTATES);

                Push(WordRegister.PC);
                Registers.PC = 0x0066;
                Registers.WZ = Registers.PC;

                Timing.EndInterruptRequestAcknowledgeCycle();
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

                if (_halted)
                {
                    Resume();
                }

                switch (InterruptMode)
                {
                    case InterruptMode.IM0: // read instruction data from data bus in 1-4 opcode fetch cycles and execute resulting instruction - flags are set but PC is unaffected
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM0_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        InstructionPackage package = DecodeIM0Interrupt();
                        ushort pc = Registers.PC;
                        Push(WordRegister.PC);
                        Execute(package);
                        Registers.PC = pc;
                        Registers.WZ = Registers.PC;
                        break;

                    case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM1_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        Registers.WZ = Registers.PC;
                        break;

                    case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                        _interruptCallback(); // device must populate data bus with low byte of address
                        Timing.BeginInterruptRequestAcknowledgeCycle(IM2_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Push(WordRegister.PC);
                        ushort address = (ushort)((Registers.I * 256) + IO.DATA_BUS);
                        Registers.PC = Memory.Timed.ReadWordAt(address);
                        Registers.WZ = Registers.PC;
                        break;
                }

                Timing.EndInterruptRequestAcknowledgeCycle();
            }
        }

        private InstructionPackage DecodeIM0Interrupt()
        {
            // In IM0, when an interrupt is generated, the CPU will ask the device
            // to send one to four bytes which are decoded into an instruction, which will then be executed.
            // To emulate this without heavily re-writing the decode loop which expects the instruction bytes to be 
            // in memory at the address pointed to by the program counter, we will temporarily copy 
            // the last 4 bytes of RAM to an array, move the program counter and use those 4 bytes for the interrupt decode, 
            // then restore them and fix up the program counter. Sneaky, eh?

            ushort address = (ushort)(Memory.SizeInBytes - 5);
            byte[] lastFour = Memory.Untimed.ReadBytesAt(address, 4);
            ushort pc = Registers.PC;
            Registers.PC = address;

            _suspendMachineCycles = true;
            
            for (int i = 0; i < 4; i++)
            {
                byte value = _interruptCallback();
                IO.SetDataBusValue(value); // set this directly as it would normally by set by the interrupting device
                Memory.Untimed.WriteByteAt((ushort)(address + i), value);
            }

            InstructionPackage package = DecodeInstruction();

            Registers.PC = pc;
            Memory.Untimed.WriteBytesAt(address, lastFour);
            
            _suspendMachineCycles = false;

            return package;
        }


        // IInstructionTiming contains methods to execute the different types of machine cycle that the Z80 supports.
        // These will be called mostly by the instruction decoder, stack operations and interrupt handlers, but some instruction
        // microcode uses these methods directly to generate timing (eg IN/OUT) or add 'internal operation' ticks. 
        // I segregated these onto an interface just to keep them logically partioned from the main API but without moving them out to a class.
        // Calling code can get at these methods using the Processor.Timing property (or by casting to the interface type, but don't do that, it's ugly).

        #region IInstructionTiming
        void IInstructionTiming.OpcodeFetchCycle(ushort address, byte data)
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

        void IInstructionTiming.MemoryReadCycle(ushort address, byte data)
        {
            IO.SetMemoryReadState(address);
            WaitForNextClockTick();

            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasMemoryRead4)
                {
                    WaitForNextClockTick();
                }
            }
        }

        void IInstructionTiming.MemoryWriteCycle(ushort address, byte data)
        {
            IO.SetMemoryWriteState(address, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryWriteState();
            WaitForNextClockTick();

            Instruction instruction = _executingInstructionPackage?.Instruction;
            if (instruction != null)
            {
                if (instruction.Timing.Exceptions.HasMemoryWrite5)
                {
                    WaitForNextClockTick();
                    WaitForNextClockTick();
                }
            }
        }

        void IInstructionTiming.BeginStackReadCycle()
        {
            IO.SetMemoryReadState(Registers.SP);
            WaitForNextClockTick();
        }


        void IInstructionTiming.EndStackReadCycle(bool highByte, byte data)
        {
            IO.AddMemoryData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            IO.EndMemoryReadState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginStackWriteCycle(bool highByte, byte data)
        {
            IO.SetMemoryWriteState(Registers.SP, data);
            WaitForNextClockTick();
            WaitForNextClockTick();
            InsertWaitCycles();
        }

        void IInstructionTiming.EndStackWriteCycle()
        {
            IO.EndMemoryWriteState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortReadCycle(byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortReadState(address);
            WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortReadCycle(byte data)
        {
            IO.AddPortReadData(data);
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortReadState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginPortWriteCycle(byte data, byte n, bool bc)
        {
            ushort address = bc ? (Registers.C, Registers.B).ToWord() : (n, Registers.A).ToWord();

            IO.SetPortWriteState(address, data);
            WaitForNextClockTick();
        }

        void IInstructionTiming.EndPortWriteCycle()
        {
            WaitForNextClockTick();
            InsertWaitCycles();

            WaitForNextClockTick();
            IO.EndPortWriteState();
            WaitForNextClockTick();
        }

        void IInstructionTiming.BeginInterruptRequestAcknowledgeCycle(int tStates)
        {
            IO.SetInterruptState();
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }

        void IInstructionTiming.EndInterruptRequestAcknowledgeCycle()
        {
            IO.EndInterruptState();
        }

        void IInstructionTiming.InternalOperationCycle(int tStates)
        {
            for (int i = 0; i < tStates; i++)
            {
                WaitForNextClockTick();
            }
        }
        #endregion  

        public Processor(IMemoryBank memory = null, IMemoryMap map = null, ushort? topOfStackAddress = null, double frequencyInMHz = 4, bool enableFlagPrecalculation = true)
        {
            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.

            FrequencyInMHz = frequencyInMHz;

            Registers = new Registers();
            Ports = new Ports(this);
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));
            IO = new ProcessorIO(this);

            TimingMode = TimingMode.FastAndFurious;
            InterruptMode = InterruptMode.IM0;

            _topOfStack = topOfStackAddress ?? 0;
            Registers.SP = _topOfStack;

            // If precalculation is enabled, all flag combinations for all input values for 8-bit ALU / bitwise operations are pre-built now 
            // (but not the 16-bit ALU operations, the number space is far too big).
            // This is *slightly* faster than calculating them in real-time, but if you need to debug flag calculation you should
            // disable this and attach a debugger to the flag calculation methods in FlagLookup.cs.
            FlagLookup.EnablePrecalculation = enableFlagPrecalculation;
            FlagLookup.BuildFlagLookupTables();
            
            // The Z80 instruction set needs to be built (all Instruction objects are created, bound to the microcode instances, and indexed into a hashtable - undocumented 'overloads' are built here too)
            InstructionSet.Build();

            // If running in Pseudo-RealTime mode, an external clock routine is used to sync instruction execution to real time. Sort of.
            // The timing is approximate due to fundamental unpredictability as to how long .NET will take to run the instruction code each time, so you do get clock misses, but all emulated devices run at the same
            // speed so the effect is as if time itself is slowing and accelerating (by a tiny %) inside the emulation, and everything remains consistent, though visual effects based purely on the
            // instruction timing / CRT timing may not work 100% as intended.
            _clock = new ExternalClock(frequencyInMHz);
            _clock.OnTick += WhenTheClockTicks;
            _startStopTimer = new Stopwatch();
        }
    }
}

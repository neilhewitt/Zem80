using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Zem80.Core.Instructions;
using Zem80.Core.Memory;
using Zem80.Core.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Zem80.Core
{
    public class Processor : IDebugProcessor, IInstructionTiming, IDisposable
    {
        public const int MAX_MEMORY_SIZE_IN_BYTES = 65536;
        public const int DEFAULT_PROCESSOR_FREQUENCY = 4;

        private bool _running;
        private bool _halted;
        private bool _realTime;
        private HaltReason _reasonForLastHalt;
        private bool _suspendMachineCycles;
        private int _pendingWaitCycles;
        private int _waitCyclesAdded;
        private int _windowsTicksPerZ80Tick;
        private ushort _topOfStack;
        private long _emulatedTStates;

        private Stopwatch _clock;
        private Thread _instructionCycle;
        private InstructionPackage _executingInstructionPackage;
        
        private Func<byte> _interruptCallback;
        private bool _interruptLatch;

        private EventHandler<InstructionPackage> _onBreakpoint;

        private IList<ushort> _breakpoints;
        
        public IDebugProcessor Debug => this;
        public IInstructionTiming Timing => this;
       
        public bool EndOnHalt { get; private set; }

        public Registers Registers { get; private set; }
        public IMemoryBank Memory { get; private set; }
        public Ports Ports { get; private set; }
        public ProcessorIO IO { get; private set; }

        public IReadOnlyFlags Flags => new Flags(Registers.F, true);

        public InterruptMode InterruptMode { get; private set; }
        public bool InterruptsEnabled { get; private set; }
        public bool InterruptsEnabledAfterNMI { get; private set; }
        public float FrequencyInMHz { get; private set; }
        public TimingMode TimingMode { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; 
        public long EmulatedTStates => _emulatedTStates;

        IEnumerable<ushort> IDebugProcessor.Breakpoints => _breakpoints;

        public event EventHandler<InstructionPackage> OnClockTick;
        public event EventHandler<InstructionPackage> BeforeExecuteInstruction;
        public event EventHandler<ExecutionResult> AfterExecuteInstruction;
        public event EventHandler<int> BeforeInsertWaitCycles;
        public event EventHandler BeforeStart;
        public event EventHandler OnStop;
        public event EventHandler<HaltReason> OnHalt;

        event EventHandler<InstructionPackage> IDebugProcessor.OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public void Dispose()
        {
            _running = false;
            _instructionCycle?.Interrupt(); // just in case
        }

        public void Initialise(ushort address = 0x0000, bool endOnHalt = false, TimingMode timingMode = TimingMode.FastAndFurious, InterruptMode interruptMode = InterruptMode.IM0)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                TimingMode = timingMode;
                InterruptMode = interruptMode;
                
                _realTime = (timingMode == TimingMode.PseudoRealTime); // we use a bool field to hold this flag because comparing against an enum value is, curiously, expensive, and we do this EVERY cycle
                _emulatedTStates = 0;

                DisableInterrupts();

                Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
            }
        }

        public void Start()
        {
            BeforeStart?.Invoke(null, null);
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

            OnStop?.Invoke(null, null);
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
        }

        public void RunUntilStopped()
        {
            while (_running) Thread.Sleep(1); // main thread can sleep while instruction thread does its thing
        }

        public void ResetAndClearMemory(bool restartAfterReset = true)
        {
            IO.SetResetState();
            Stop();
            Memory.Clear();
            Registers.Clear();
            IO.Clear();
            Registers.SP = _topOfStack;
            if (restartAfterReset)
            {
                Initialise(0, this.EndOnHalt, this.TimingMode);
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
            InterruptsEnabledAfterNMI = false;
            _interruptLatch = false;
        }

        public void EnableInterrupts()
        {
            InterruptsEnabled = true;
            InterruptsEnabledAfterNMI = true;
            _interruptLatch = true;
        }

        public void RestoreInterruptsAfterNMI()
        {
            InterruptsEnabled = InterruptsEnabledAfterNMI;
            _interruptLatch = InterruptsEnabled;
        }

        public void AddWaitCycles(int waitCycles)
        {
            // Will add *waitCycles* wait states at the next insertion point.
            // Waits are only actually inserted at certain points in the instruction cycle. 
            _pendingWaitCycles = waitCycles;
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

        void IDebugProcessor.AddBreakpoint(ushort address)
        {
            if (_breakpoints == null) _breakpoints = new List<ushort>();

            // Note that the breakpoint functionality is *very* simple and not checked
            // so if you add a breakpoint for an address which is not the start
            // of an instruction in the code, it will never be triggered as PC will never get there
            if (!_breakpoints.Contains(address))
            {
                _breakpoints.Add(address);
            }    
        }

        void IDebugProcessor.RemoveBreakpoint(ushort address)
        {
            if (_breakpoints != null && _breakpoints.Contains(address))
            {
                _breakpoints.Remove(address);
            }
        }

        internal void Push(WordRegister register)
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

        internal void Pop(WordRegister register)
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

        void IDebugProcessor.PushStackDirect(ushort value)
        {
            Registers.SP--;
            Memory.Untimed.WriteByteAt(Registers.SP, value.HighByte());

            Registers.SP--;
            Memory.Untimed.WriteByteAt(Registers.SP, value.LowByte());
        }

        ushort IDebugProcessor.PopStackDirect()
        {
            byte high, low;

            low = Memory.Untimed.ReadByteAt(Registers.SP);
            Registers.SP++;

            high = Memory.Untimed.ReadByteAt(Registers.SP);
            Registers.SP++;

            return (low, high).ToWord();
        }

        ushort IDebugProcessor.PeekStack()
        {
            return Memory.Untimed.ReadWordAt(Registers.SP);
        }

        private void InstructionCycle()
        {
            _clock = new Stopwatch();
            _clock.Start();

            while (_running)
            {
                InstructionPackage package = null;
                ushort pc = Registers.PC;

                if (_halted)
                {
                    package = DecodeNOP();
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
            }
        }

        private void HandleInterrupts()
        {
            bool handledNMI = HandleNMI();

            // when interrupts are enabled during an instruction cycle, handling should not resume until the *next* cycle, hence the latch here
            if (!_interruptLatch && !handledNMI)
            {
                HandleMaskableInterrupts();
            }
            else
            {
                _interruptLatch = false;
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

            // set the internal WZ register to an initial value based on whether this is an indexed instruction or not;
            // the instruction that runs may alter/set WZ itself.
            // (the value in WZ [sometimes known as MEMPTR in Z80 enthusiast circles] is only ever used to control the behavior of the BIT instruction)
            ushort wz = package.Instruction switch
            {
                var i when i.Source.IsAddressFromIndexAndOffset() => Registers[i.Source.AsWordRegister()],
                var i when i.Target.IsAddressFromIndexAndOffset() => Registers[i.Target.AsWordRegister()],
                _ => 0
            };
            wz = (ushort)(wz + package.Data.Argument1);
            Registers.WZ = wz;

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

        private InstructionPackage DecodeNOP()
        {
            FetchOpcodeByte(); // enable devices to run, interrupts etc
            return new InstructionPackage(InstructionSet.NOP, new InstructionData(), Registers.PC);
        }

        private InstructionPackage DecodeInstructionAtProgramCounter()
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
                        return new InstructionPackage(InstructionSet.NOP, data, instructionAddress);
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
                        // some instructions (OK, just CALL) have a prolonged high byte operand read with an additional cycle
                        // but ONLY if a) it's CALL nn OR b) if the condition (CALL NZ, for example) is satisfied (ie Flags.NZ is true)
                        // otherwise it's the standard size. Timing oddities like this are noted in the TimingExceptions,
                        // but this is the only one that affects the instruction decode cycle (the others affect Memory Read cycles)
                        if (instruction.Timing.Exceptions.HasProlongedConditionalOperandDataReadHigh)
                        {
                            if (instruction.Condition == Condition.None || Flags.SatisfyCondition(instruction.Condition))
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

                for (int i = 0; i < instruction.Timing.Exceptions.ExtraOpcodeFetchTStates; i++)
                {
                    WaitForNextClockTick();
                }
            }
        }

        private void WaitForNextClockTick()
        {
            if (_running)
            {
                if (_realTime)
                {
                    long currentTicks = _clock.ElapsedTicks;
                    long targetTicks = currentTicks + _windowsTicksPerZ80Tick;
                    while (_clock.ElapsedTicks < targetTicks) ;
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

        private bool HandleNMI()
        {
            bool handledNMI = false;

            if (IO.NMI)
            {
                if (_halted)
                {
                    Resume();
                }

                InterruptsEnabledAfterNMI = InterruptsEnabled; // save IFF1 state ready for RETN
                InterruptsEnabled = false; // disable maskable interrupts until RETN

                Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.NMI_INTERRUPT_ACKNOWLEDGE_TSTATES);

                Push(WordRegister.PC);
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
                        // TODO: verify the behaviour of the real Z80 and fix the code!

                        Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.IM0_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        InstructionPackage package = DecodeIM0Interrupt();
                        ushort pc = Registers.PC;
                        Push(WordRegister.PC);
                        Execute(package);
                        Registers.PC = pc;
                        Registers.WZ = Registers.PC;
                        break;

                    case InterruptMode.IM1:

                        // This mode is simple. When IM1 is set (this is the default mode) then a jump to 0x0038 is performed when 
                        // the interrupt occurs. 

                        Timing.BeginInterruptRequestAcknowledgeCycle(InstructionTiming.IM1_INTERRUPT_ACKNOWLEDGE_TSTATES);
                        Push(WordRegister.PC);
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
                        Push(WordRegister.PC);
                        ushort address = (IO.DATA_BUS, Registers.I).ToWord();
                        Registers.PC = Memory.Timed.ReadWordAt(address);
                        Registers.WZ = Registers.PC;
                        break;
                }

                _interruptCallback = null;
                Timing.EndInterruptRequestAcknowledgeCycle();
            }
        }

        private InstructionPackage DecodeIM0Interrupt()
        {
            // In IM0, when an interrupt is generated, the CPU will ask the device
            // to supply four bytes one at a time via a callback method, which are then decoded into an instruction to be executed.
            
            // To emulate this without heavily re-writing the decode loop (which expects the instruction bytes to be 
            // in memory at the address pointed to by the program counter), we will temporarily copy 
            // the last 4 bytes of RAM to an array, move the program counter and use those 4 bytes for the interrupt decode, 
            // then restore them and fix up the program counter. Sneaky, eh?

            ushort address = (ushort)(Memory.SizeInBytes - 5);
            byte[] lastFour = Memory.Untimed.ReadBytesAt(address, 4);
            ushort pc = Registers.PC;
            Registers.PC = address;

            _suspendMachineCycles = true;
            
            for (int i = 0; i < 4; i++)
            {
                // The callback will be called 4 times; it should return the opcode bytes of the instruction to run in sequence.
                // If there are fewer than 4 bytes in the opcode, return 0x00 for the 'extra' bytes
                byte value = _interruptCallback();
                IO.SetDataBusValue(value);
                Memory.Untimed.WriteByteAt((ushort)(address + i), value);
            }

            InstructionPackage package = DecodeInstructionAtProgramCounter();

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
                if (instruction.Timing.Exceptions.HasProlongedMemoryRead)
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
                if (instruction.Timing.Exceptions.HasProlongedMemoryWrite)
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

        public Processor(IMemoryBank memory = null, IMemoryMap map = null, ushort? topOfStackAddress = null, float frequencyInMHz = DEFAULT_PROCESSOR_FREQUENCY, bool enableFlagPrecalculation = true)
        {
            // You can supply your own memory implementations, for example if you need to do RAM paging for >64K implementations.
            // Since there are several different methods for doing this and no 'official' method, there is no paged RAM implementation in the core code.

            FrequencyInMHz = frequencyInMHz;
            _windowsTicksPerZ80Tick = (int)Math.Ceiling(10 / frequencyInMHz);

            Registers = new Registers();
            Ports = new Ports(this);
            Memory = memory ?? new MemoryBank();
            Memory.Initialise(this, map ?? new MemoryMap(MAX_MEMORY_SIZE_IN_BYTES, true));
            IO = new ProcessorIO(this);

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
        }
    }
}

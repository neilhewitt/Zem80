using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;

namespace Z80.Core
{
    public class Processor : IDebugMode
    {
        private Instruction _executingInstruction;
        private bool _running;
        private bool _halted;
        private bool _pendingInterrupt;
        private bool _debugEnabled;
        private Action _interruptCallback;
        private bool _pendingNonMaskableInterrupt;
        private Thread _instructionCycle;
        private ushort _topOfStack;
        private EventHandler<ExecutionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler<HaltReason> _onHalt;
        private EventHandler<TickEvent> _onBeforeTick;
        private EventHandler<TickEvent> _onAfterTick;
        private IDictionary<string, Func<TickEvent, Processor, bool>> _devices;
        private Stopwatch _stopwatch;
        private double _windowsTicksPerCycle;
        private long _timingErrors;
        private IList<TimingErrorLog> _timingLog;

        public IReadOnlyDictionary<string, Func<TickEvent, Processor, bool>> Devices => (IReadOnlyDictionary<string, Func<TickEvent, Processor, bool>>)_devices;

        public IDebugMode Debug
        {
            get
            {
                if (_debugEnabled) return (IDebugMode)this;
                else throw new Z80Exception("Debugging is not enabled and debug stats will be empty.");
            }
        }

        public bool Synchronous { get; private set; }
        public bool EndOnHalt { get; private set; }
        public bool StableRealTime { get; private set; }

        public Registers Registers { get; private set; }
        public Memory Memory { get; private set; }
        public Ports Ports { get; private set; }
        public IO IO { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; } = true;
        public int SpeedInMhz { get; private set; }
        public bool RealTimeExecution { get; private set; }
        public long Ticks { get; private set; }
        public long ClockCycles { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; // yay for tri-states!
        
        // why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        // (basically, automatic properties don't work as events when they are explicitly on the interface, so we use a backing variable... old-school style)
        event EventHandler<ExecutionPackage> IDebugMode.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugMode.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler IDebugMode.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugMode.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler<HaltReason> IDebugMode.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }
        event EventHandler<TickEvent> IDebugMode.OnBeforeTick { add { _onBeforeTick += value; } remove { _onBeforeTick -= value; } }
        event EventHandler<TickEvent> IDebugMode.OnAfterTick { add { _onAfterTick += value; } remove { _onAfterTick -= value; } }

        long IDebugMode.TimingErrors => _timingErrors;
        IEnumerable<TimingErrorLog> IDebugMode.TimingErrorLogs => _timingLog;

        public void Start(bool synchronous = false, ushort address = 0x0000, bool endOnHalt = false, bool realTimeExecution = false, bool enableDebug = false)
        {
            if (!_running)
            {
                EndOnHalt = endOnHalt; // if set, will summarily end execution at the first HALT instruction. This is mostly for test / debug scenarios.
                _debugEnabled = enableDebug; // if this is not set, debug events will not fire even if subscribed to, timing errors will not be logged, and access to this.Debug will throw an exception
                ClockCycles = 0;
                Ticks = 0;
                _timingErrors = 0;
                _timingLog.Clear();
                RealTimeExecution = realTimeExecution;

                if (_debugEnabled) _beforeStart?.Invoke(null, null);
                Registers.PC = address; // ordinarily, execution will start at 0x0000, but this can be overridden
                _running = true;

                if (!synchronous) // run the CPU on a thread and return to the calling code immediately
                {
                    Synchronous = false;
                    _instructionCycle = new Thread(new ThreadStart(InstructionCycle));
                    _instructionCycle.Start();
                }
                else
                {
                    Synchronous = true;
                    InstructionCycle(); // run the CPU as a synchronous task until stopped
                }
            }
        }

        public void Stop()
        {
            _running = false;
            _halted = false;
            _instructionCycle?.Abort();

            if (_debugEnabled) _onStop?.Invoke(null, null);
        }

        public void Halt(HaltReason reason = HaltReason.HaltCalledDirectly)
        {
            _halted = true;
            if (_debugEnabled) _onHalt?.Invoke(null, reason);
        }

        public void Resume()
        {
            _halted = false;
        }

        public void EnableDebug()
        {
            _debugEnabled = true;
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

        public void RaiseInterrupt(Action callback)
        {
            IO.INT.Value = true; // knock knock
            if (InterruptsEnabled)
            {
                _pendingInterrupt = true;
                _interruptCallback = callback;
            }
        }

        public void RaiseNonMasktableInterrupt()
        {
            IO.NMI.Value = true;
            _pendingNonMaskableInterrupt = true;
        }

        public void DisableInterrupts()
        {
            InterruptsEnabled = false;
        }

        public void EnableInterrupts()
        {
            InterruptsEnabled = true;
        }

        public void RegisterDevice(string name, Func<TickEvent, Processor, bool> callback)
        {
            _devices.Add(name, callback);
        }

        public void DeRegisterDevice(string name)
        {
            if (_devices.ContainsKey(name)) _devices.Remove(name);
        }

        #region machine cycle handling
        internal void NotifyMemoryReadCycle(ushort address, byte data, MachineCycleType cycleType)
        {
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;

            int cyclesToAdd = 3;
            // some instructions have MR(4) but there's only ever one MR per instruction
            if (_executingInstruction != null && _executingInstruction.HasMemoryRead4)
            {
                cyclesToAdd = 4;
            }
            ClockCycles += cyclesToAdd;

            TickAll(new TickEvent(_executingInstruction, cycleType, cyclesToAdd));
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        internal void NotifyMemoryWriteCycle(ushort address, byte data, MachineCycleType cycleType)
        {
            IO.MREQ.Value = true;
            IO.WR.Value = true;
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;

            int cyclesToAdd = 3;
            // some instructions have MW(5) but there's only ever one MW per instruction
            if (_executingInstruction != null && _executingInstruction.HasMemoryWrite5)
            {
                cyclesToAdd = 5;
            }
            ClockCycles += cyclesToAdd;

            TickAll(new TickEvent(_executingInstruction, cycleType, cyclesToAdd));
            IO.MREQ.Value = false;
            IO.WR.Value = false;
        }

        internal void BeginPortReadCycle()
        {
            IO.IORQ.Value = true;
            IO.RD.Value = true;
            IO.ADDRESS_BUS.Value = (Registers.B, Registers.C).ToWord();
        }

        internal void CompletePortReadCycle(byte data)
        {
            IO.DATA_BUS.Value = data;
            ClockCycles += 4;
            TickAll(new TickEvent(_executingInstruction, MachineCycleType.PortRead, 4));
            IO.IORQ.Value = false;
            IO.RD.Value = false;
        }

        internal void BeginPortWriteCycle(byte data)
        {
            IO.IORQ.Value = true;
            IO.WR.Value = true;
            IO.ADDRESS_BUS.Value = (Registers.B, Registers.C).ToWord();
            IO.DATA_BUS.Value = data;
        }

        internal void CompletePortWriteCycle()
        {
            ClockCycles += 4;
            TickAll(new TickEvent(_executingInstruction, MachineCycleType.PortWrite, 4));
            IO.IORQ.Value = false;
            IO.WR.Value = false;
        }

        internal void NotifyInternalOperationCycle(int clockCycles)
        {
            ClockCycles += clockCycles;
            TickAll(new TickEvent(_executingInstruction, MachineCycleType.InternalOperation, clockCycles));
        }
        #endregion

        private void InstructionCycle()
        {
            while (_running)
            {
                ExecutionPackage package = null;

                if (!_halted)
                {
                    // if the CPU is running in real time, it will attempt to time instructions
                    // to what they should be on a real Z80 running at SpeedInMhz value
                    // We'll use a stopwatch to count elapsed time (much better performance than TimeSpans or DateTime)
                    StableRealTime = true;
                    if (RealTimeExecution)
                    {
                        _stopwatch.Start();
                    }

                    // read next 4 bytes and decode to instruction
                    IO.M1.Value = true;
                    byte[] instructionBytes = Memory.ReadBytesAt(Registers.PC, 4, true); 
                    package = Decode(instructionBytes, Registers.PC);
                    if (_debugEnabled) _beforeExecute?.Invoke(this, package);
                    if (package.Instruction == null)
                    {
                        // in this case we should execute two consecutive NOPs to move the program counter on correctly and generate the correct ticks
                        package.Instruction = InstructionSet.NOP; // set the instruction to NOP
                        Execute(package); // run the first NOP here and then let the second run below as normal
                    }
                    else if (package == null)
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
                    package = new ExecutionPackage(InstructionSet.NOP, new InstructionData());
                }

                // run the decoded instruction and deal with timing / ticks
                ExecutionResult result = Execute(package);

                IO.M1.Value = false;
                _executingInstruction = null;

                if (_debugEnabled) _afterExecute?.Invoke(this, result);

                // check if the instruction has run in less than the 'real' time, and if so, spin in a while loop until enough time has passed
                // alternatively, if we've already run too long, up the timing error counter and log it for debug purposes
                if (RealTimeExecution)
                {
                    _stopwatch.Stop();
                    long expectedTicks = (long)(_windowsTicksPerCycle * result.ClockCycles);
                    if (_stopwatch.ElapsedTicks > expectedTicks)
                    {
                        if (_debugEnabled)
                        {
                            _timingErrors++;
                            _timingLog.Add(new TimingErrorLog(package.Instruction, _stopwatch.ElapsedTicks, expectedTicks));
                        }
                        StableRealTime = false;
                    }
                    else
                    {
                        _stopwatch.Restart();
                        while (_stopwatch.ElapsedTicks < expectedTicks) ;
                    }
                    _stopwatch.Stop();
                    _stopwatch.Reset();
                }

                HandleNonMaskableInterrupts();
                HandleMaskableInterrupts();
            }
        }

        private ExecutionResult Execute(ExecutionPackage package)
        {
            _executingInstruction = package.Instruction;

            // run the instruction microcode implementation
            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);

            if (result.Flags != null) Registers.Flags.Value = result.Flags.Value;
            // move the program counter on if not already set by the instruction, and not halted
            Registers.PC += (ushort)((_halted || result.InstructionSetsProgramCounter) ? 0x00 : result.Instruction.SizeInBytes);

            return result;
        }        

        ExecutionResult IDebugMode.Execute(ExecutionPackage package)
        {
            return Execute(package);
        }

        private void TickAll(TickEvent tickEvent)
        {
            if (_debugEnabled) _onBeforeTick?.Invoke(this, tickEvent);
            Ticks++;
            foreach(var callback in _devices.Values)
            {
                callback(tickEvent, this);
            }
            if (_debugEnabled) _onAfterTick?.Invoke(this, tickEvent);
        }

        private void BeginStackReadCycle()
        {
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            IO.ADDRESS_BUS.Value = Registers.SP;
        }

        private void EndStackReadCycle(bool highByte, byte data)
        {
            ClockCycles += 3;
            IO.DATA_BUS.Value = data;
            TickAll(new TickEvent(_executingInstruction, highByte ? MachineCycleType.StackReadHigh : MachineCycleType.StackReadLow, 3));
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        private void StackWriteCycle(bool highByte, byte data)
        {
            IO.MREQ.Value = true;
            IO.WR.Value = true;
            IO.ADDRESS_BUS.Value = Registers.SP;
            IO.DATA_BUS.Value = data;
            ClockCycles += 3;
            TickAll(new TickEvent(_executingInstruction, highByte ? MachineCycleType.StackWriteHigh : MachineCycleType.StackWriteLow, 3));
            IO.MREQ.Value = false;
            IO.WR.Value = false;
        }

        private void OpcodeFetchCycle(ushort address, byte data, int clockCycles)
        {
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            ClockCycles += clockCycles;
            TickAll(new TickEvent(_executingInstruction, MachineCycleType.OpcodeFetch, clockCycles));
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        private void OperandDataCycle(ushort address, byte data, int clockCycles, bool wordOperand, bool high)
        {
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            ClockCycles += clockCycles;
            MachineCycleType cycleType = wordOperand ? high ? MachineCycleType.OperandReadHigh : MachineCycleType.OperandReadLow : MachineCycleType.OperandRead;
            TickAll(new TickEvent(_executingInstruction, cycleType, clockCycles));
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        private ExecutionPackage Decode(byte[] instructionBytes, ushort address)
        {
            byte b0 = instructionBytes[0];
            byte b1 = instructionBytes[1];
            byte b2 = instructionBytes[2];
            byte b3 = instructionBytes[3];
            InstructionData data = new InstructionData();

            try
            {
                // handle NOP + special case - sequences of DD or FD (uniform or mixed) count as NOP until the final DD / FD
                if (b0 == 0x00 || ((b0 == 0xDD || b0 == 0xFD) && (b1 == 0xDD || b1 == 0xFD)))
                {
                    OpcodeFetchCycle(address, 0x00, 4);
                    return new ExecutionPackage(InstructionSet.NOP, data);
                }

                // b0 may be a prefix byte (or the first byte of two prefix bytes), and can be any of:
                // CB, ED, DD, FD, DD+CB or DD+FD

                byte prefix = b0;
                Instruction instruction = null;

                if (((prefix == 0xDD || prefix == 0xFD) && b1 == 0xCB))
                {
                    // extended instructions (double-prefix 'DD CB' and 'FD CB')
                    // prefix is always followed by a displacement byte or 0x00, opcode is then in *b3*
                    instruction = InstructionSet.Instructions[(b3 | b1 << 8 | b0 << 16)];
                    
                    // there will be two opcode fetch cycles
                    OpcodeFetchCycle(address, prefix, instruction.MachineCycles[0].ClockCycles);
                    OpcodeFetchCycle((ushort)(address + 3), b3, instruction.MachineCycles[1].ClockCycles);

                    // only displacement arguments valid for DDCB/FDCB instructions
                    if (instruction.Argument1 == ArgumentType.Displacement)
                    {
                        OperandDataCycle((ushort)(address + 1), b2, 3, false, false);
                        data.Argument1 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else if (prefix == 0xCB || prefix == 0xED || prefix == 0xDD || prefix == 0xFD)
                {
                    // other extended instructions
                    instruction = InstructionSet.Instructions[(b1 | b0 << 8)];
                    if (instruction == null)
                    {
                        if (prefix == 0xED)
                        {
                            byte[] undocumentedED = new byte[] { 0x4C, 0x4E, 0x54, 0x55, 0x5C, 0x5D, 0x64, 0x65, 0x6C, 0x6D, 0x6E, 0x71, 0x74, 0x75, 0x76, 0x77, 0x7C, 0x7D, 0x7E };
                            // some ED instructions are undocumented but are the equivalent of the unprefixed instruction without the prefix
                            // in which case we should behave as per the unprefixed instruction,
                            // but if not on the list above, any missing ED instruction should be treated as 2 x NOP
                            // in this case we return a package with the instruction set to null and the CPU will sort it out
                            if (!undocumentedED.Contains(b1))
                            {
                                return new ExecutionPackage(null, data);
                            }
                        }

                        // ignore the prefix and use the unprefixed instruction, there are holes in the extended instruction set
                        // and some (naughty!) code uses this technique to consume extra cycles for synchronisation
                        return Decode(new byte[] { b1, b2, b3, 0x00 }, (ushort)(address + 1));
                    }

                    // there will be two opcode fetch cycles
                    OpcodeFetchCycle(address, b0, instruction.MachineCycles[0].ClockCycles);
                    OpcodeFetchCycle((ushort)(address + 1), b1, instruction.MachineCycles[1].ClockCycles);

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        OperandDataCycle((ushort)(address + 1), b1, 3, false, false);
                        data.Argument1 = b2;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        OperandDataCycle((ushort)(address + 1), b1, 3, true, false);
                        data.Argument1 = b2;
                        OperandDataCycle((ushort)(address + 2), b2, 3, true, true);
                        data.Argument2 = b3;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else
                {
                    // unprefixed instructions (can be 1-3 bytes only)
                    instruction = InstructionSet.Instructions[(b0)];
                    OpcodeFetchCycle(address, b0, instruction.MachineCycles[0].ClockCycles); // only ever 1 opcode fetch cycle

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        OperandDataCycle((ushort)(address + 1), b1, 3, false, false);
                        data.Argument1 = b1;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        OperandDataCycle((ushort)(address + 1), b1, 3, true, false);
                        data.Argument1 = b1;
                        // OperandReadHigh can be 4 clock cycles for CALL conditional instructions, but only if the condition is satisfied
                        // need to signal to the handler that this is the case, hence the check here
                        int clockCycles = 3;
                        if (instruction.HasOperandDataReadHigh4)
                        {
                            if (instruction.Condition == null || (instruction.Condition.HasValue && Registers.Flags.SatisfyCondition(instruction.Condition.Value)))
                            {
                                clockCycles = 4;
                            }
                        }
                        OperandDataCycle((ushort)(address + 2), b2, clockCycles, true, true);
                        data.Argument2 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
            }
            catch (InstructionNotFoundException)
            {
                // handle special case where instruction buffer is short / corrupt (read beyond end of memory) in which case, return null to the caller
                // all other exceptions will throw as normal
                return null;
            }
        }

        private ExecutionPackage DecodeInterrupt(Func<byte> dataRead)
        {
            if (dataRead == null) throw new InterruptException("You must supply a valid delegate / Func<bool> to support reading device data.");

            byte[] instructionBytes = new byte[4];
            for (int i = 0; i < 4; i++) instructionBytes[i] = dataRead();

            return Decode(instructionBytes, Registers.PC);
        }

        private void HandleNonMaskableInterrupts()
        {
            if (_pendingNonMaskableInterrupt)
            {
                OpcodeFetchCycle(0x0000, 0x00, 5); // ignored but needed for timing
                Push(WordRegister.PC);
                Registers.PC = 0x0066;
                _pendingNonMaskableInterrupt = false;
                _halted = false;
            }

            IO.NMI.Value = false;
        }

        private void HandleMaskableInterrupts()
        {
            if (_pendingInterrupt && InterruptsEnabled)
            {
                if (_interruptCallback == null && InterruptMode != InterruptMode.IM1)
                {
                    throw new InterruptException("Interrupt mode is " + InterruptMode.ToString() + " which requires a callback for reading data from the interrupting device. Callback was null.");
                }

                switch (InterruptMode)
                {
                    case InterruptMode.IM0: // read instruction data from data bus in 1-4 cycles and execute resulting instruction - flags are set but PC is unaffected
                        NotifyInternalOperationCycle(6);
                        ExecutionPackage package = DecodeInterrupt(() =>
                        {
                            _interruptCallback(); // each time callback is called, device will set data bus with next instruction byte
                            return IO.DATA_BUS.Value;
                        });
                        Execute(package); // instruction is *usually* RST which diverts execution, but can be any valid instruction
                        break;

                    case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                        NotifyInternalOperationCycle(7);
                        Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        break;

                    case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                        _interruptCallback(); // device must populate data bus with low byte of address
                        NotifyInternalOperationCycle(7);
                        Push(WordRegister.PC);
                        ushort address = (ushort)((Registers.I * 256) + IO.DATA_BUS.Value);
                        Registers.PC = Memory.ReadWordAt(address, false);
                        break;
                }

                _pendingInterrupt = false;
                _halted = false;

                IO.NMI.Value = false;
            }
        }

        internal Processor(IMemoryMap map, ushort topOfStackAddress, int speedInMHz, bool enableFlagPrecalculation = true)
        {
            Registers = new Registers();
            Ports = new Ports(this);
            SpeedInMhz = speedInMHz;

            Memory = new Memory();
            Memory.Initialise(this, map);

            _topOfStack = topOfStackAddress;
            Registers.SP = _topOfStack;

            // if precalculation is enabled, all flag combinations for all input values for 8-bit operations are pre-built now (*not* 16-bit ALU operations)
            // this is *slightly* faster than calculating them in real-time, but if you want to debug flag calculation you should
            // disable this and attach a debugger to the flag calculation methods in FlagLookup.cs
            FlagLookup.EnablePrecalculation = enableFlagPrecalculation;
            FlagLookup.BuildFlagLookupTables(); 
            var nop = InstructionSet.NOP; // this causes the whole instruction set to get built now, so it's not done on the first instruction run

            IO = new IO(this);
            _devices = new Dictionary<string, Func<TickEvent, Processor, bool>>();

            _stopwatch = new Stopwatch();
            _windowsTicksPerCycle = (double)(10 / SpeedInMhz); // @1MHz = 1 cycle per microsecond, 10 ticks per microsecond
            _timingErrors = 0;
            _timingLog = new List<TimingErrorLog>();
        }
    }
}

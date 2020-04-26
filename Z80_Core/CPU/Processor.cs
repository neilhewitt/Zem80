using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Z80.Core
{
    public class Processor : IDebugProcessor
    {
        private Instruction _executingInstruction;

        private bool _running;
        private bool _halted;
        private bool _pendingInterrupt;
        private Action _interruptCallback;
        private bool _pendingNonMaskableInterrupt;
        private Thread _instructionCycle;
        private ushort _topOfStack;
        private EventHandler<ExecutionPackage> _beforeExecute;
        private EventHandler<ExecutionResult> _afterExecute;
        private EventHandler _beforeStart;
        private EventHandler _onStop;
        private EventHandler _onHalt;

        public IDebugProcessor Debug => (IDebugProcessor)this;
        public bool Synchronous { get; private set; }

        public Registers Registers { get; private set; }
        public Memory Memory { get; private set; }
        public Ports Ports { get; private set; }
        public IO IO { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.IM0;
        public bool InterruptsEnabled { get; private set; } = true;
        public double SpeedInMhz { get; private set; }
        public long InstructionTicks { get; private set; }
        public long ClockCycles { get; private set; }
        public ProcessorState State => _running ? _halted ? ProcessorState.Halted : ProcessorState.Running : ProcessorState.Stopped; // yay for tri-states!

        // why yes, you do have to do event handlers this way if you want them to be on the interface and not on the type... no idea why
        // (basically, automatic properties don't work as events when they are explicitly on the interface, so we use a backing variable... old-school style)
        event EventHandler<ExecutionPackage> IDebugProcessor.BeforeExecute { add { _beforeExecute += value; } remove { _beforeExecute -= value; } }
        event EventHandler<ExecutionResult> IDebugProcessor.AfterExecute { add { _afterExecute += value; } remove { _afterExecute -= value; } }
        event EventHandler IDebugProcessor.BeforeStart { add { _beforeStart += value; } remove { _beforeStart -= value; } }
        event EventHandler IDebugProcessor.OnStop { add { _onStop += value; } remove { _onStop -= value; } }
        event EventHandler IDebugProcessor.OnHalt { add { _onHalt += value; } remove { _onHalt -= value; } }

        public void Start(bool synchronous = false, ushort address = 0x0000)
        {
            if (!_running)
            {
                _beforeStart?.Invoke(null, null);
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

            _onStop?.Invoke(null, null);
        }

        public void Halt()
        {
            _halted = true;
            _onHalt?.Invoke(null, null);
        }

        public void Resume()
        {
            _halted = false;
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
            StackWriteCycle();
            Memory.WriteByteAt(Registers.SP, value.HighByte(), true);
            Registers.SP--;
            StackWriteCycle();
            Memory.WriteByteAt(Registers.SP, value.LowByte(), true);
        }

        public void Pop(WordRegister register)
        {
            byte high, low;

            StackReadCycle();
            low = Memory.ReadByteAt(Registers.SP, true);
            Registers.SP++;
            StackReadCycle();
            high = Memory.ReadByteAt(Registers.SP, true);
            Registers.SP++;

            ushort value = (low, high).ToWord();
            Registers[register] = value;
        }

        public ushort Peek()
        {
            return Memory.ReadWordAt(Registers.SP);
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

        #region machine cycle handling
        internal void MemoryReadCycle(ushort address, byte data)
        {
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;

            int cyclesToAdd = 3;
            // some instructions have MR(4) but there's only ever one MR per instruction
            if (_executingInstruction != null && 
                _executingInstruction.MachineCycles.Any(x => x.Type == MachineCycleType.MemoryRead && x.ClockCycles == 4))
            {
                cyclesToAdd = 4;
            }
            ClockCycles += cyclesToAdd;

            TickAll();
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        internal void MemoryWriteCycle(ushort address, byte data)
        {
            IO.MREQ.Value = true;
            IO.WR.Value = true;
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;

            int cyclesToAdd = 3;
            // some instructions have MW(5) but there's only ever one MW per instruction
            if (_executingInstruction != null && 
                _executingInstruction.MachineCycles.Any(x => x.Type == MachineCycleType.MemoryWrite && x.ClockCycles == 5))
            {
                cyclesToAdd = 5;
            }
            ClockCycles += cyclesToAdd;

            TickAll();
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
            TickAll();
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
            TickAll();
            IO.IORQ.Value = false;
            IO.WR.Value = false;
        }

        internal void StackReadCycle()
        {
            ClockCycles += 3;
            TickAll();
        }

        internal void StackWriteCycle()
        {
            ClockCycles += 3;
            TickAll();
        }

        internal void InternalOperationCycle(int clockCycles)
        {
            ClockCycles += clockCycles;
            TickAll();
        }
        #endregion

        private void InstructionCycle()
        {
            while (_running)
            {
                ExecutionPackage package = null;
                if (!_halted)
                {
                    // read next 4 bytes and decode to instruction
                    IO.M1.Value = true;
                    byte[] instructionBytes = Memory.ReadBytesAt(Registers.PC, 4, true); // we suppress the machine cycles for the memory reads as the Decode method generates these
                    package = Decode(instructionBytes, Registers.PC);
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
                    // when halted, the CPU should execute NOP continuously
                    package = new ExecutionPackage(InstructionSet.NOP, new InstructionData());
                }

                // run the decoded instruction and deal with timing / ticks
                Execute(package);

                IO.M1.Value = false;

                HandleNonMaskableInterrupts();
                HandleMaskableInterrupts();
            }
        }

        private void Execute(ExecutionPackage package)
        {
            this.Debug.Execute(package);
        }

        ExecutionResult IDebugProcessor.Execute(ExecutionPackage package)
        {
            _executingInstruction = package.Instruction;

            // run the instruction microcode implementation
            _beforeExecute?.Invoke(this, package);
            ExecutionResult result = package.Instruction.Microcode.Execute(this, package);
            _afterExecute?.Invoke(this, result);

            if (result.Flags != null) Registers.Flags.Value = result.Flags.Value;
            // move the program counter on if not already set by the instruction, and not halted
            Registers.PC += (ushort)((_halted || result.InstructionSetsProgramCounter) ? 0x00 : result.Instruction.SizeInBytes);

            return result;
        }

        private void TickAll()
        {
            // TODO:
            // signal attached devices that a machine cycle has ended - provide details of the cycle
            // and provide access to CPU state and IO pins

            InstructionTicks++;
        }

        private void OpcodeFetchCycle(ushort address, byte data, int clockCycles)
        {
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            ClockCycles += clockCycles;
            TickAll();
            IO.MREQ.Value = false;
            IO.RD.Value = false;
        }

        private void OperandDataCycle(ushort address, byte data, bool hasFourCycles = false)
        {
            IO.ADDRESS_BUS.Value = address;
            IO.DATA_BUS.Value = data;
            IO.MREQ.Value = true;
            IO.RD.Value = true;
            ClockCycles += 3;
            if (hasFourCycles) ClockCycles++; // only for CALL nn and MODE 0 interrupts
            TickAll();
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
                    instruction = InstructionSet.Find(b3, (InstructionPrefix)(prefix, b1).ToWord());
                    
                    // there will be two opcode fetch cycles
                    OpcodeFetchCycle(address, prefix, instruction.MachineCycles[0].ClockCycles);
                    OpcodeFetchCycle((ushort)(address + 3), b3, instruction.MachineCycles[1].ClockCycles);

                    // only displacement arguments valid for DDCB/FDCB instructions
                    if (instruction.Argument1 == ArgumentType.Displacement)
                    {
                        OperandDataCycle((ushort)(address + 1), b2);
                        data.Argument1 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else if (prefix == 0xCB || prefix == 0xED || prefix == 0xDD || prefix == 0xFD)
                {
                    // other extended instructions
                    instruction = InstructionSet.Find(b1, (InstructionPrefix)prefix);
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
                        OperandDataCycle((ushort)(address + 1), b1);
                        data.Argument1 = b2;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        OperandDataCycle((ushort)(address + 1), b1);
                        data.Argument1 = b2;
                        OperandDataCycle((ushort)(address + 2), b2);
                        data.Argument2 = b3;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else
                {
                    // unprefixed instructions (can be 1-3 bytes only)
                    instruction = InstructionSet.Find(b0, InstructionPrefix.Unprefixed);
                    OpcodeFetchCycle(address, b0, instruction.MachineCycles[0].ClockCycles); // only ever 1 opcode fetch cycle

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        OperandDataCycle((ushort)(address + 1), b1);
                        data.Argument1 = b1;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        OperandDataCycle((ushort)(address + 1), b1);
                        data.Argument1 = b1;
                        // OperandReadHigh can be 4 clock cycles, need to signal to the handler that this is the case, hence the check here
                        OperandDataCycle((ushort)(address + 2), b2, instruction.MachineCycles.Any(x => x.Type == MachineCycleType.OperandReadHigh && x.ClockCycles == 4));
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
                        InternalOperationCycle(6);
                        ExecutionPackage package = DecodeInterrupt(() =>
                        {
                            _interruptCallback(); // each time callback is called, device will set data bus with next instruction byte
                            return IO.DATA_BUS.Value;
                        });
                        Execute(package); // instruction is *usually* RST which diverts execution, but can be any valid instruction
                        break;

                    case InterruptMode.IM1: // just redirect to 0x0038 where interrupt handler must begin
                        InternalOperationCycle(7);
                        Push(WordRegister.PC);
                        Registers.PC = 0x0038;
                        break;

                    case InterruptMode.IM2: // redirect to address pointed to by register I + data bus value - gives 128 possible addresses
                        _interruptCallback(); // device must populate data bus with low byte of address
                        InternalOperationCycle(7);
                        Push(WordRegister.PC);
                        ushort address = (ushort)((Registers.I * 256) + IO.DATA_BUS.Value);
                        Registers.PC = Memory.ReadWordAt(address);
                        break;
                }

                _pendingInterrupt = false;
                _halted = false;

                IO.NMI.Value = false;
            }
        }

        internal Processor(IMemoryMap map, ushort topOfStackAddress, double speedInMHz, bool enableFlagPrecalculation = true)
        {
            Registers = new Registers();
            Ports = new Ports(this);
            SpeedInMhz = speedInMHz;

            Memory = new Memory();
            Memory.Initialise(this, map);

            _topOfStack = topOfStackAddress;
            Registers.SP = _topOfStack;

            FlagLookup.EnablePrecalculation = enableFlagPrecalculation;
            FlagLookup.BuildFlagLookupTables();

            IO = new IO(this);
        }
    }
}

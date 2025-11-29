using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.Debugger;

namespace Zem80.Core.CPU
{
    public class DebugProcessor
    {
        private Processor _cpu;
        private Action<InstructionPackage> _executeInstruction;
        private List<ushort> _breakpoints;

        private DebugSession _debugSession;

        public IReadOnlyCollection<ushort> Breakpoints => (IReadOnlyCollection<ushort>)_breakpoints;
        public bool IsDebugging => _debugSession != null;

        public event EventHandler<DebugSession> OnBreakpointReached;
        public event EventHandler OnDebugSessionEnded;

        // execute an instruction directly (without the processor loop running), for example for directly testing instructions
        public void ExecuteDirect(byte[] opcode)
        {
            Array.Resize(ref opcode, 4); // must be 4 bytes to decode
            _cpu.Memory.WriteBytesAt(_cpu.Registers.PC, opcode);
            InstructionPackage package = InstructionDecoder.DecodeInstruction(opcode, _cpu.Registers.PC, out bool _, out bool _);
            _cpu.Registers.PC += package.Instruction.SizeInBytes;

            _executeInstruction(package);
        }

        // execute an instruction directly (specified by mnemonic, so no decoding necessary)
        public void ExecuteDirect(string mnemonic, byte? arg1, byte? arg2)
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

            InstructionPackage package = new InstructionPackage(instruction, data, _cpu.Registers.PC);
            _cpu.Registers.PC += package.Instruction.SizeInBytes; // simulate the decode cycle effect on PC
            _executeInstruction(package);
        }

        public void AddBreakpoint(ushort address)
        {
            if (_breakpoints == null) _breakpoints = new List<ushort>();

            // Note that the breakpoint is not checked,
            // so if you add a breakpoint for an address which is not the start
            // of an instruction in the code, it will never be triggered as PC will never get there
            if (!_breakpoints.Contains(address))
            {
                _breakpoints.Add(address);
            }
        }

        public void RemoveBreakpoint(ushort address)
        {
            if (_breakpoints != null && _breakpoints.Contains(address))
            {
                _breakpoints.Remove(address);
            }
        }

        internal void NotifyExecute(InstructionPackage package)
        {
            if (_breakpoints.Contains(package.InstructionAddress))
            {
                if (_debugSession == null) 
                {
                    _debugSession = new DebugSession(_cpu, package);
                }

                OnBreakpointReached?.Invoke(this, _debugSession);
                _debugSession.NotifyBreakpointHit(package.InstructionAddress);
            }
        }

        internal void NotifySessionEnded()
        {
            OnDebugSessionEnded?.Invoke(this, EventArgs.Empty);
            _debugSession = null;
        }

        public DebugProcessor(Processor cpu, Action<InstructionPackage> executeInstruction)
        {
            _cpu = cpu;

            _executeInstruction = executeInstruction;
            _breakpoints = new List<ushort>();
        }
    }
}
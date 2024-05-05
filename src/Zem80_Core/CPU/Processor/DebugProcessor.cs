using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core.CPU
{
    public class DebugProcessor : IDebugProcessor
    {
        private Processor _cpu;
        private InstructionDecoder _instructionDecoder;
        private EventHandler<InstructionPackage> _onBreakpoint;
        private Action<InstructionPackage> _executeInstruction;
        private IList<ushort> _breakpoints;

        IEnumerable<ushort> Breakpoints => _breakpoints;

        event EventHandler<InstructionPackage> OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public void AddBreakpoint(ushort address)
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

        public void RemoveBreakpoint(ushort address)
        {
            if (_breakpoints != null && _breakpoints.Contains(address))
            {
                _breakpoints.Remove(address);
            }
        }

        public void EvaluateAndRunBreakpoint(ushort address, InstructionPackage executingPackage)
        {
            if (_breakpoints.Contains(address))
            {
                _onBreakpoint.Invoke(_cpu, executingPackage);
            }
        }

        // execute an instruction directly (without the processor loop running), for example for directly testing instructions
        public void ExecuteDirect(byte[] opcode)
        {
            Array.Resize(ref opcode, 4); // must be 4 bytes to decode
            _cpu.Memory.Untimed.WriteBytesAt(_cpu.Registers.PC, opcode);
            InstructionPackage package = _instructionDecoder.DecodeInstruction(opcode, _cpu.Registers.PC, out bool _, out bool _);
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

        public DebugProcessor(Processor cpu, Action<InstructionPackage> executeInstruction)
        {
            _cpu = cpu;
            _instructionDecoder = new InstructionDecoder(cpu);
            _executeInstruction = executeInstruction;
            _breakpoints = new List<ushort>();
        }
    }
}

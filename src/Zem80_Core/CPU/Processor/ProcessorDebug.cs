using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public partial class Processor : IDebugProcessor
    {
        private EventHandler<InstructionPackage> _onBreakpoint;
        private IList<ushort> _breakpoints;

        long IDebugProcessor.LastRunTimeInMilliseconds => (_lastEnd - _lastStart).Milliseconds;
        IEnumerable<ushort> IDebugProcessor.Breakpoints => _breakpoints;

        event EventHandler<InstructionPackage> IDebugProcessor.OnBreakpoint { add { _onBreakpoint += value; } remove { _onBreakpoint -= value; } }

        public IDebugProcessor Debug => this;

        public void SetDataBusDefaultValue(byte defaultValue)
        {
            IO.SetDataBusDefault(defaultValue);
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

        // execute an instruction directly (without the processor loop running), for example for directly testing instructions
        ExecutionResult IDebugProcessor.ExecuteDirect(byte[] opcode)
        {
            Memory.Untimed.WriteBytesAt(Registers.PC, opcode);
            DecodeResult result = _instructionDecoder.DecodeInstructionAt(Registers.PC);
            Registers.PC += result.InstructionPackage.Instruction.SizeInBytes;

            return ExecuteInstruction(result.InstructionPackage);
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
            return ExecuteInstruction(package);
        }
    }
}

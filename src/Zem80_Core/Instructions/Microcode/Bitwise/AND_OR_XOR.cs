using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class AND : AND_OR_XOR { public AND() : base("AND") { } }
    public class OR : AND_OR_XOR { public OR() : base("OR") { } }
    public class XOR : AND_OR_XOR { public XOR() : base("XOR") { } }

    public class AND_OR_XOR : MicrocodeBase
    {
        // AND/OR/XOR r
        // AND/OR/XOR n
        // AND/OR/XOR (HL)
        // AND/OR/XOR (IX+o)
        // AND/OR/XOR (IY+o)

        private LogicalOperation _operation;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = Resolver.GetSourceByte(instruction, data, cpu, 3);
            (int result, Flags flags) = _operation switch
            {
                LogicalOperation.And => Logical.And(r.A, operand),
                LogicalOperation.Or => Logical.Or(r.A, operand),
                LogicalOperation.Xor => Logical.Xor(r.A, operand),
                _ => throw new InvalidOperationException("Unsupported logical operation.")
            };

            r.A = (byte)result;            
            return new ExecutionResult(package, flags);
        }

        public AND_OR_XOR(string z80Mnemonic)
        {
            _operation = z80Mnemonic switch
            {
                "AND" => LogicalOperation.And,
                "OR" => LogicalOperation.Or,
                "XOR" => LogicalOperation.Xor,
                _ => throw new ArgumentException($"Invalid mnemonic for AND_OR_XOR: {z80Mnemonic}")
            };
        }
    }
}

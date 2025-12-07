using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class ADC : IMicrocode
    {
        // ADC A,r
        // ADC A,n
        // ADC A,(HL)
        // ADC A,(IX+o)
        // ADC A,(IY+o)
        // ADC HL,rr

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                ushort left = r.HL;
                cpu.Timing.InternalOperationCycles(4, 3);
                ushort right = Resolver.GetSourceWord(instruction, data, cpu, 3);
                (r.HL, flags) = Arithmetic.Add(left, right, flags.Carry, true, flags);
                r.WZ = (ushort)(left + 1);
            }
            else
            {
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
                (r.A, flags) = Arithmetic.Add(left, right, flags.Carry);
            }
          
            return new ExecutionResult(package, flags);
        }

        public ADC()
        {
        }
    }
}

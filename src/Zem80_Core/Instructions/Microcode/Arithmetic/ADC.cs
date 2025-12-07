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
                // it's ADC HL,rr
                ushort left = r.HL;
                cpu.Timing.InternalOperationCycles(4, 3);
                ushort right = r[instruction.Source.AsWordRegister()];
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

    public class ADD_ADC_SUB_SBC : MicrocodeBase, IMicrocode
    {
        private bool _withCarry;
        private bool _subtracts;

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                // it's one of the 16-bit adds / sbc (HL,DE etc)
                // ADC, SBC only allow HL. Add allows BC, DE, HL, SP

                WordRegister destination = instruction.Target.AsWordRegister();
                ushort left = r[destination];
                ushort right = 0;// GetSourceWordAndAddTimingAndEvents();
                (r.HL, flags) = Arithmetic.Add(left, right, flags.Carry, true, flags);
                r.WZ = (ushort)(left + 1);
            }
            else
            {
                // it's one of the 8-bit adds / subs

                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
                (r.A, flags) = Arithmetic.Add(left, right, flags.Carry);
            }

            return new ExecutionResult(package, flags);
        }

        public ADD_ADC_SUB_SBC(string z80Mnemonic)
        {
            _withCarry = z80Mnemonic == "ADC" || z80Mnemonic == "SBC";
            _subtracts = z80Mnemonic == "SUB" || z80Mnemonic == "SBC";
        }
    }
}

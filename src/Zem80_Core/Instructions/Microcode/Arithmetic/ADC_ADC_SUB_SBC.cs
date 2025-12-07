using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class ADC : ADD_ADC_SUB_SBC { public ADC() : base("ADC") { } }
    public class ADD : ADD_ADC_SUB_SBC { public ADD() : base("ADD") { } }
    public class SBC : ADD_ADC_SUB_SBC { public SBC() : base("SBC") { } }
    public class SUB : ADD_ADC_SUB_SBC { public SUB() : base("SUB") { } }

    public class ADD_ADC_SUB_SBC : MicrocodeBase
    {
        private bool _withCarry;
        private bool _subtracts;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                // it's one of the 16-bit adds / sbc (HL,DE etc)
                // ADC, SBC only allow HL. Add allows BC, DE, HL, SP

                ushort left = r[instruction.Target.AsWordRegister()];
                ushort right = r[instruction.Source.AsWordRegister()];
                (r[instruction.Target.AsWordRegister()], flags) = (_withCarry, _subtracts) switch
                {
                    (false, false) => Arithmetic.Add(left, right, false, false, flags),
                    (true, false) => Arithmetic.Add(left, right, flags.Carry, true, flags),
                    (false, true) => Arithmetic.Subtract(left, right, false, false, flags),
                    (true, true) => Arithmetic.Subtract(left, right, flags.Carry, true, flags),
                };

                r.WZ = (ushort)(left + 1);
            }
            else
            {
                // it's one of the 8-bit adds / subs

                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
                (r.A, flags) = (_withCarry, _subtracts) switch
                {
                    (false, false) => Arithmetic.Add(left, right, false),
                    (true, false) => Arithmetic.Add(left, right, flags.Carry),
                    (false, true) => Arithmetic.Subtract(left, right, false),
                    (true, true) => Arithmetic.Subtract(left, right, flags.Carry),
                };
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

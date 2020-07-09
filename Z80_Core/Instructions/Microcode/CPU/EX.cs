using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Z80.Core
{
    public class EX : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;

            if (instruction.Opcode == 0x08)
            {
                // EX AF,AF'
                r.ExchangeAF();
            }
            else
            {
                if (instruction.Target == InstructionElement.AddressFromSP)
                {
                    // EX (SP),HL/IX+o/IY+o
                    ushort value = instruction.MarshalSourceWord(data, cpu, out ushort address);
                    r.HL = value;
                    cpu.Memory.WriteWordAt(r.SP, value, false);
                }
                else
                {
                    // EX DE,HL
                    ushort de = r.DE;
                    r.DE = r.HL;
                    r.HL = de;
                }
            }

            return new ExecutionResult(package, null);;
        }

        public EX()
        {
        }
    }
}

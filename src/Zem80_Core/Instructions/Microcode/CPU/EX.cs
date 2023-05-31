using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class EX : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
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
                    // EX (SP),HL/IX/IY
                    ushort value = instruction.MarshalSourceWord(data, cpu);
                    ushort valueAtSP = cpu.Memory.Untimed.ReadWordAt(r.SP);

                    r[instruction.Source.AsWordRegister()] = valueAtSP;
                    cpu.Memory.TimedFor(package.Instruction).WriteWordAt(r.SP, value);

                    r.WZ = valueAtSP;
                }
                else
                {
                    // EX DE,HL
                    ushort de = r.DE;
                    r.DE = r.HL;
                    r.HL = de;
                }
            }

            return new ExecutionResult(package, null);
        }

        public EX()
        {
        }
    }
}

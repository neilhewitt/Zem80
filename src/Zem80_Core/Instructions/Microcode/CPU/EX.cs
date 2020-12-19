using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
                    // get value of source register (HL or IX or IY) 
                    ushort value = instruction.MarshalSourceWord(data, cpu, out ushort address);
                    ushort valueAtSP = cpu.Memory.ReadWordAt(r.SP, true);

                    // get word pointed to by the stack pointer into the source register
                    r[instruction.Source.AsWordRegister()] = valueAtSP;

                    // now store value taken originally to address in SP
                    cpu.Memory.WriteWordAt(r.SP, value, false);

                    // set WZ internal register
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

            return new ExecutionResult(package, null);;
        }

        public EX()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Zem80.Core.CPU
{
    public class EX : MicrocodeBase
    {
        // EX AF,AF'
        // EX DE,HL
        // EX (SP),HL
        // EX (SP),IX
        // EX (SP),IY

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            if (instruction.LastOpcodeByte == 0x08)
            {
                // EX AF,AF'
                r.ExchangeAF();
            }
            else
            {
                if (instruction.Target == InstructionElement.AddressFromSP)
                {
                    // EX (SP),HL/IX/IY
                    ushort value = r[instruction.Source.AsWordRegister()];
                    ushort valueAtSP = cpu.Memory.ReadWordAt(r.SP);
                    r[instruction.Source.AsWordRegister()] = valueAtSP;
                    cpu.Memory.WriteWordAt(r.SP, value, 0);

                    // timing is weird here - 4,3,4,3,5 (OF,SRL,SRH,SWL,SWH)
                    // whereas we're reading from and writing to memory rather than 
                    // pushing and popping the stack, so we'll operate untimed and
                    // then add the correct timing in below...

                    cpu.Timing.BeginStackReadCycle();
                    cpu.Timing.EndStackReadCycle(valueAtSP.LowByte());
                    cpu.Timing.BeginStackReadCycle();
                    cpu.Timing.EndStackReadCycle(valueAtSP.HighByte());

                    cpu.Timing.BeginStackWriteCycle(value.LowByte());
                    cpu.Timing.EndStackWriteCycle();
                    cpu.Timing.BeginStackWriteCycle(value.HighByte());
                    cpu.Timing.EndStackWriteCycle();

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

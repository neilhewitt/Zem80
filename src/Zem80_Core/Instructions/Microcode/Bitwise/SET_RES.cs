using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class SET : SET_RES { public SET() : base("SET") {} }
    public class RES : SET_RES { public RES() : base("RES") {} }

    public class SET_RES : MicrocodeBase
    {
        // SET / RES b,r
        // SET / RES b,(HL)
        // SET / RES b,(IX+o)
        // SET / RES b,(IY+o)

        bool _set;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte bitIndex = instruction.BitIndex;
            sbyte offset = (sbyte)(data.Argument1);
            
            ByteRegister register = instruction.Source.AsByteRegister();
            if (register != ByteRegister.None)
            {
                byte value = r[register].SetBit(bitIndex, _set);
                r[register] = value;
            }
            else
            {
                ushort address = Resolver.GetSourceAddress(instruction, cpu, offset);
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                
                byte value = cpu.Memory.ReadByteAt(address, 4);
                value = value.SetBit(bitIndex, _set);
                cpu.Memory.WriteByteAt(address, value, 3);
                if (instruction.CopiesResultToRegister)
                {
                    r[instruction.CopyResultTo] = value;
                }
            }

            return new ExecutionResult(package, null);
        }

        public SET_RES(string z80Mnemonic)
        {
            _set = z80Mnemonic == "SET";
        }
    }
}

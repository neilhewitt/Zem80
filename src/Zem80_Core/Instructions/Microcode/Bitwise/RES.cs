using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RES : IMicrocode
    {
        // RES b,r
        // RES b,(HL)
        // RES b,(IX+o)
        // RES b,(IY+o)

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte bitIndex = instruction.BitIndex;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Source.AsByteRegister();

            if (register != ByteRegister.None)
            {
                byte value = r[register].SetBit(bitIndex, false);
                r[register] = value;
            }
            else
            {
                ushort address = Resolver.GetSourceAddress(instruction, cpu, offset);
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte value = cpu.Memory.ReadByteAt(address, 4);
                value = value.SetBit(bitIndex, false);
                cpu.Memory.WriteByteAt(address, value, 3);
                if (instruction.CopiesResultToRegister)
                {
                    r[instruction.CopyResultTo] = value;
                }
            }

            return new ExecutionResult(package, null);
        }

        public RES()
        {
        }
    }
}

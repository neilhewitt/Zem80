using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();
            bool previousCarry = flags.Carry;

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                (shifted, flags) = BitwiseOperations.RotateLeftThroughCarry(original, flags);
                r[register] = shifted;
            }
            else
            {
                ushort address = instruction.Prefix switch
                {
                    0xCB => r.HL,
                    0xDDCB => (ushort)(r.IX + offset),
                    0xFDCB => (ushort)(r.IY + offset),
                    _ => (ushort)0xFFFF
                };
                
                original = cpu.Memory.ReadByteAt(address, 4);
                (shifted, flags) = BitwiseOperations.RotateLeftThroughCarry(original, flags);

                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(4);
                cpu.Memory.WriteByteAt(address, shifted, 3);
                
                if (instruction.CopiesResultToRegister)
                {
                    r[instruction.CopyResultTo] = shifted;
                }
            }

            return new ExecutionResult(package, flags);
        }

        public RL()
        {
        }
    }
}

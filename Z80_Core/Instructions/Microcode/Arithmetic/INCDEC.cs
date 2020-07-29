//using System;
//using System.Collections.Generic;
//using System.Diagnostics.SymbolStore;
//using System.Text;

//namespace Z80.Core
//{
//    public static class INCDEC
//    {
//        public static ExecutionResult Adjust(Processor cpu, ExecutionPackage package, bool subtracts)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Registers r = cpu.Registers;
//            byte offset = data.Argument1;
//            Flags flags = cpu.Registers.Flags;

//            int inc = subtracts ? -1 : 1;
//            byte compare = (byte)(subtracts ? 0x80: 0x7F);

//            if (instruction.TargetsWordRegister)
//            {
//                // inc 16-bit
//                WordRegister register = instruction.Target.AsWordRegister();
//                ushort value = r[register];
//                r[register] = (ushort)(value + inc);
//            }
//            else
//            {
//                byte value = 0;
//                if (instruction.TargetsByteInMemory)
//                {
//                    // inc byte in memory
//                    if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
//                    value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
//                    cpu.Memory.WriteByteAt(address, (byte)(value + inc), false);
//                }
//                else
//                {
//                    // it's an 8-bit inc
//                    ByteRegister register = instruction.Target.AsByteRegister();
//                    value = r[register];
//                    r[register] = (byte)(value + inc);
//                }

//                bool carry = flags.Carry;
//                flags = FlagLookup.ByteArithmeticFlags(value, 1, false, subtracts);
//                flags.ParityOverflow = (value == compare);
//                flags.Carry = carry; // always unaffected
//                flags.Subtract = subtracts;
//            }

//            return new ExecutionResult(package, flags);
//        }

//        public class INC : IMicrocode
//        {
//            public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//            {
//                return INCDEC.Adjust(cpu, package, true);
//            }

//            public INC()
//            {
//            }
//        }

//        public class DEC : IMicrocode
//        {
//            public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//            {
//                return INCDEC.Adjust(cpu, package, false);
//            }

//            public DEC()
//            {
//            }
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Z80.Core
//{
//    public static class SETRES
//    {
//        public static ExecutionResult SetReset(Processor cpu, ExecutionPackage package, bool setsTheBit)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Registers r = cpu.Registers;

//            byte bitIndex = instruction.GetBitIndex();
//            byte value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister register);
//            value = value.SetBit(bitIndex, setsTheBit);

//            if (register != ByteRegister.None)
//            {
//                r[register] = value;
//            }
//            else
//            {
//                cpu.Memory.WriteByteAt(address, value, false);
//            }

//            return new ExecutionResult(package, cpu.Registers.Flags);
//        }
//    }

//    public class SET : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return SETRES.SetReset(cpu, package, true);
//        }
//    }

//    public class RES : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return SETRES.SetReset(cpu, package, false);
//        }
//    }
//}

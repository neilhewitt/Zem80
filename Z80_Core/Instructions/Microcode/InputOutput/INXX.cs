//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Z80.Core
//{
//    public class INXX
//    {
//        public static ExecutionResult Input(Processor cpu, ExecutionPackage package, bool increments, bool repeats)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Flags flags = cpu.Registers.Flags;
//            Registers r = cpu.Registers;

//            Port port = cpu.Ports[r.C];
//            port.SignalRead();
//            byte input = port.ReadByte();
//            cpu.Memory.WriteByteAt(r.HL, input, false);

//            if (increments)
//            {
//                r.HL++;
//            }
//            else
//            {
//                r.HL--;
//            }
            
//            r.B--;

//            if (repeats) flags.Sign = false;
//            flags.Zero = (repeats || ((ushort)(r.B - 1) == 0));
//            flags.Subtract = true;

//            if (repeats && r.B != 0)
//            {
//                cpu.Registers.PC = package.InstructionAddress;
//                cpu.Timing.InternalOperationCycle(5);
//            }

//            return new ExecutionResult(package, flags);
//        }
//    }

//    public class IND : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return INXX.Input(cpu, package, false, false);
//        }
//    }

//    public class INI : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return INXX.Input(cpu, package, true, false);
//        }
//    }

//    public class INDR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return INXX.Input(cpu, package, false, true);
//        }
//    }

//    public class INIR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return INXX.Input(cpu, package, true, true);
//        }
//    }
//}

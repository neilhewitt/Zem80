//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Z80.Core
//{
//    public class OUTXX
//    {
//        public static ExecutionResult Output(Processor cpu, ExecutionPackage package, bool increments, bool repeats)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Flags flags = cpu.Registers.Flags;
//            Registers r = cpu.Registers;

//            Port port = cpu.Ports[r.C];
//            byte output = cpu.Memory.ReadByteAt(r.HL, false);
//            port.SignalWrite();
//            port.WriteByte(output);

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

//    public class OUTD : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return OUTXX.Output(cpu, package, false, false);
//        }
//    }

//    public class OUTI : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return OUTXX.Output(cpu, package, true, false);
//        }
//    }

//    public class OTDR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return OUTXX.Output(cpu, package, false, true);
//        }
//    }

//    public class OTIR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            return OUTXX.Output(cpu, package, true, true);
//        }
//    }
//}


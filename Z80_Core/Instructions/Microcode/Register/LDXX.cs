//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Z80.Core
//{
//    public class LDXX
//    {
//        public static ExecutionResult Load(Processor cpu, ExecutionPackage package, bool increments, bool repeats)
//        {
//            Flags flags = cpu.Registers.Flags;
//            Registers r = cpu.Registers;

//            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL, false), false);
            
//            if (increments)
//            {
//                r.HL++;
//                r.DE++;
//            }
//            else
//            {
//                r.HL--;
//                r.DE--;
//            }

//            r.BC--;

//            flags.HalfCarry = false;
//            flags.Subtract = false;
//            flags.ParityOverflow = r.BC != 0;

//            if (repeats)
//            {
//                if (r.BC != 0)
//                {
//                    cpu.Timing.InternalOperationCycle(5);
//                    r.PC = package.InstructionAddress;
//                }
//            }

//            return new ExecutionResult(package, flags);
//        }
//    }

//    //public class LDD : IMicrocode
//    //{
//    //    public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//    //    {
//    //        return LDXX.Load(cpu, package, false, false);
//    //    }
//    //}

//    //public class LDI : IMicrocode
//    //{
//    //    public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//    //    {
//    //        return LDXX.Load(cpu, package, true, false);
//    //    }
//    //}

//    //public class LDDR : IMicrocode
//    //{
//    //    public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//    //    {
//    //        return LDXX.Load(cpu, package, false, true);
//    //    }
//    //}

//    //public class LDIR : IMicrocode
//    //{
//    //    public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//    //    {
//    //        return LDXX.Load(cpu, package, true, true);
//    //    }
//    //}
//}


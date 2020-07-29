//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Z80.Core
//{
//    public static class ANDORXOR
//    {
//        public static ExecutionResult Logic(Processor cpu, ExecutionPackage package, LogicalOperation operation)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Registers r = cpu.Registers;

//            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
//            byte operand = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister register);
//            var logic = LogicalOperations.Op(operation, r.A, operand);
//            r.A = logic.Result;

//            return new ExecutionResult(package, logic.Flags);
//        }
//    }
    
//    public class AND : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Registers r = cpu.Registers;
//            Flags flags = cpu.Registers.Flags;
//            sbyte offset = (sbyte)data.Argument1;

//            byte and(byte operand)
//            {
//                int result = (r.A & operand);
//                flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.And);
//                return (byte)result;
//            }

//            switch (instruction.Prefix)
//            {
//                case InstructionPrefix.Unprefixed:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xA0: // AND B
//                            r.A = and(r.B);
//                            break;
//                        case 0xA1: // AND C
//                            r.A = and(r.C);
//                            break;
//                        case 0xA2: // AND D
//                            r.A = and(r.D);
//                            break;
//                        case 0xA3: // AND E
//                            r.A = and(r.E);
//                            break;
//                        case 0xA4: // AND H
//                            r.A = and(r.H);
//                            break;
//                        case 0xA5: // AND L
//                            r.A = and(r.L);
//                            break;
//                        case 0xA7: // AND A
//                            r.A = and(r.A);
//                            break;
//                        case 0xA6: // AND (HL)
//                            r.A = and(cpu.Memory.ReadByteAt(r.HL, false));
//                            break;
//                        case 0xE6: // AND n
//                            r.A = and(data.Argument1);
//                            break;

//                    }
//                    break;

//                case InstructionPrefix.DD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xA4: // AND IXh
//                            r.A = and(r.IXh);
//                            break;
//                        case 0xA5: // AND IXl
//                            r.A = and(r.IXl);
//                            break;
//                        case 0xA6: // AND (IX+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IX + offset), false));
//                            break;
//                    }
//                    break;

//                case InstructionPrefix.FD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xA4: // AND IYh
//                            r.A = and(r.IYh);
//                            break;
//                        case 0xA5: // AND IYl
//                            r.A = and(r.IYl);
//                            break;
//                        case 0xA6: // AND (IY+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IY + offset), false));
//                            break;
//                    }
//                    break;
//            }

//            return new ExecutionResult(package, flags);

//            //return ANDORXOR.Logic(cpu, package, LogicalOperation.And);
//        }
//    }

//    public class OR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Flags flags = cpu.Registers.Flags;
//            Registers r = cpu.Registers;

//            void or(byte operand)
//            {
//                int result = (r.A | operand);
//                flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.Or);
//                r.A = (byte)result;
//            }

//            switch (instruction.Prefix)
//            {
//                case InstructionPrefix.Unprefixed:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xB0: // OR B
//                            or(r.B);
//                            break;
//                        case 0xB1: // OR C
//                            or(r.C);
//                            break;
//                        case 0xB2: // OR D
//                            or(r.D);
//                            break;
//                        case 0xB3: // OR E
//                            or(r.E);
//                            break;
//                        case 0xB4: // OR H
//                            or(r.H);
//                            break;
//                        case 0xB5: // OR L
//                            or(r.L);
//                            break;
//                        case 0xB7: // OR A
//                            or(r.A);
//                            break;
//                        case 0xB6: // OR (HL)
//                            or(cpu.Memory.ReadByteAt(r.HL, false));
//                            break;
//                        case 0xF6: // OR n
//                            or(data.Argument1);
//                            break;
//                    }
//                    break;

//                case InstructionPrefix.DD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xB4: // OR IXh
//                            or(r.IXh);
//                            break;
//                        case 0xB5: // OR IXl
//                            or(r.IXl);
//                            break;
//                        case 0xB6: // OR (IX+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            or(cpu.Memory.ReadByteAt((ushort)(r.IX + (sbyte)data.Argument1), false));
//                            break;
//                    }
//                    break;

//                case InstructionPrefix.FD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xB4: // OR IYh
//                            or(r.IYh);
//                            break;
//                        case 0xB5: // OR IYl
//                            or(r.IYl);
//                            break;
//                        case 0xB6: // OR (IY+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            or(cpu.Memory.ReadByteAt((ushort)(r.IY + (sbyte)data.Argument1), false));
//                            break;
//                    }
//                    break;
//            }

//            return new ExecutionResult(package, flags);

//            //return ANDORXOR.Logic(cpu, package, LogicalOperation.Or);
//        }
//    }

//    public class XOR : IMicrocode
//    {
//        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
//        {
//            Instruction instruction = package.Instruction;
//            InstructionData data = package.Data;
//            Flags flags = cpu.Registers.Flags;
//            Registers r = cpu.Registers;

//            void xor(byte operand)
//            {
//                int result = (byte)(r.A ^ operand);
//                flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.Xor);
//                r.A = (byte)result;
//            }

//            switch (instruction.Prefix)
//            {
//                case InstructionPrefix.Unprefixed:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xA8: // XOR B
//                            xor(r.B);
//                            break;
//                        case 0xA9: // XOR C
//                            xor(r.C);
//                            break;
//                        case 0xAA: // XOR D
//                            xor(r.D);
//                            break;
//                        case 0xAB: // XOR E
//                            xor(r.E);
//                            break;
//                        case 0xAC: // XOR H
//                            xor(r.H);
//                            break;
//                        case 0xAD: // XOR L
//                            xor(r.L);
//                            break;
//                        case 0xAF: // XOR A
//                            xor(r.A);
//                            break;
//                        case 0xAE: // XOR (HL)
//                            xor(cpu.Memory.ReadByteAt(r.HL, false));
//                            break;
//                        case 0xEE: // XOR n
//                            xor(data.Argument1);
//                            break;
//                    }
//                    break;

//                case InstructionPrefix.DD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xAC: // XOR IXh
//                            xor(r.IXh);
//                            break;
//                        case 0xAD: // XOR IXl
//                            xor(r.IXl);
//                            break;
//                        case 0xAE: // XOR (IX+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            xor(cpu.Memory.ReadByteAt((ushort)(r.IX + (sbyte)data.Argument1), false));
//                            break;
//                    }
//                    break;

//                case InstructionPrefix.FD:
//                    switch (instruction.Opcode)
//                    {
//                        case 0xAC: // YOR IYh
//                            xor(r.IYh);
//                            break;
//                        case 0xAD: // YOR IYl
//                            xor(r.IYl);
//                            break;
//                        case 0xAE: // YOR (IY+o)
//                            cpu.Timing.InternalOperationCycle(5);
//                            xor(cpu.Memory.ReadByteAt((ushort)(r.IY + (sbyte)data.Argument1), false));
//                            break;
//                    }
//                    break;
//            }

//            return new ExecutionResult(package, flags);

//            //return ANDORXOR.Logic(cpu, package, LogicalOperation.Xor);
//        }
//    }
//}

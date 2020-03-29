using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CALL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            bool conditionTrue = false;
            bool callRequired = false;

            void call()
            {
                cpu.Registers.PC += 3;
                cpu.Push(WordRegister.PC);
                cpu.Registers.PC = data.ArgumentsAsWord;
                conditionTrue = instruction.Opcode != 0xCD;
                callRequired = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC4: // CALL NZ,nn
                            if (!flags.Zero) call();
                            break;
                        case 0xCC: // CALL Z,nn
                            if (flags.Zero) call();
                            break;
                        case 0xCD: // CALL nn
                            call();
                            break;
                        case 0xD4: // CALL NC,nn
                            if (!flags.Carry) call();
                            break;
                        case 0xDC: // CALL C,nn
                            if (flags.Carry) call();
                            break;
                        case 0xE4: // CALL PO,nn
                            if (!flags.ParityOverflow) call();
                            break;
                        case 0xEC: // CALL PE,nn
                            if (flags.ParityOverflow) call();
                            break;
                        case 0xF4: // CALL P,nn
                            if (!flags.Sign) call();
                            break;
                        case 0xFC: // CALL M,nn
                            if (flags.Sign) call();
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, conditionTrue, callRequired);
        }

        public CALL()
        {
        }
    }
}

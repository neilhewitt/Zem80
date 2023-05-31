using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public class InstructionDecoder
    {
        private Processor _cpu;

        public DecodeResult DecodeNOPAt(ushort instructionAddress)
        {
            ushort currentAddress = instructionAddress;
            Instruction instruction = InstructionSet.NOP;

            _cpu.Timing.OpcodeFetchCycle(currentAddress++, 0, 0);

            InstructionPackage package = new InstructionPackage(instruction, new InstructionData(), instructionAddress);
            DecodeResult result = new DecodeResult(package, false, false);
            return result;
        }

        public DecodeResult DecodeInstructionAt(ushort instructionAddress)
        {
            ushort currentAddress = instructionAddress;
            byte[] instructionBytes = _cpu.Memory.Untimed.ReadBytesAt(instructionAddress, 4);
            Instruction instruction = DecodeInstruction(instructionBytes, out bool skipNextByte, out bool opcodeError, out byte? indexDisplacementByte);

            int opcodeIndex = 0;
            foreach (MachineCycle cycle in instruction.Timing.MachineCycles.ByType(MachineCycleType.OpcodeFetch))
            {
                byte opcodeByte = instruction.FullOpcodeAsByteArray[opcodeIndex++];
                _cpu.Timing.OpcodeFetchCycle(currentAddress++, opcodeByte, (byte)(cycle.TStates - InstructionTiming.OPCODE_FETCH_NORMAL_TSTATES));
            }

            InstructionData data = DecodeOperands(currentAddress, instruction, indexDisplacementByte);
            
            InstructionPackage package = new InstructionPackage(instruction, data, instructionAddress);
            DecodeResult result = new DecodeResult(package, opcodeError, skipNextByte);
            return result;
        }

        private Instruction DecodeInstruction(byte[] instructionBytes, out bool skipNextByte, out bool opcodeErrorNOP, out byte? indexDisplacementByte)
        { 
            byte b0, b1, b3; // placeholders for upcoming instruction bytes
            Instruction instruction;
            InstructionData data = new InstructionData();

            skipNextByte = false;
            opcodeErrorNOP = false;

            b0 = GetByte(0); // always at least one opcode byte
            b1 = GetByte(1);
            indexDisplacementByte = GetByte(2);
            b3 = GetByte(3);

            byte GetByte(int index)
            {
                if (instructionBytes.Length >= (index + 1))
                {
                    return instructionBytes[index];
                }
                else
                {
                    return 0;
                }    
            }

            // was byte 0 a prefix code?
            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD)
            {
                if ((b0 == 0xDD || b0 == 0xFD || b0 == 0xED) && (b1 == 0xDD || b1 == 0xFD || b1 == 0xED))
                {
                    // sequences of 0xDD / 0xFD / 0xED count as NOP until the final 0xDD / 0xFD / 0xED which is then the prefix byte
                    instruction = InstructionSet.NOP;
                }
                else if ((b0 == 0xDD || b0 == 0xFD) && b1 == 0xCB)
                {
                    // DDCB / FDCB: four-byte opcode = two prefix bytes + one displacement byte + one opcode byte (no four-byte instruction has any actual operand values)
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        // not a valid instruction - the Z80 spec says we should run a single NOP instead
                        instruction = InstructionSet.NOP;
                    }
                }
                else
                {
                    // all other prefixed instructions (CB, ED, DD, FD): a two-byte opcode + up to 2 operand bytes
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        // if prefix was 0xED and the instruction is invalid, the spec says run *two* NOPs
                        if (b0 == 0xED)
                        {
                            skipNextByte = true; // causes the processor to run an extra NOP *after* this one and skip over the invalid instruction byte
                        }

                        // otherwise, if the prefix was 0xDD or 0xFD and the instruction is invalid, the spec says we should run a NOP now but then run the equivalent
                        // unprefixed instruction next - this will happen automatically when PC advances past the synthetic NOP

                        instruction = InstructionSet.NOP;
                    }
                }
            }
            else
            {
                // unprefixed instruction - 1 byte opcode + up to 2 operand bytes
                if (!InstructionSet.Instructions.TryGetValue(b0, out instruction))
                {
                    instruction = InstructionSet.NOP;
                }
            }

            if (instruction == InstructionSet.NOP && b0 != 0x00)
            {
                opcodeErrorNOP = true; // this is a 'pseudo-NOP' caused by an invalid opcode, after which interrupts (including NMI) must not run
            }

            return instruction;
        }

        private InstructionData DecodeOperands(ushort address, Instruction instruction, byte? indexDisplacementByte)
        {
            InstructionData data = new InstructionData();

            bool setArgument1 = false;
            // now we can decode the operands and read their values (0-2 bytes depending on the instruction)
            foreach (MachineCycle cycle in instruction.Timing.MachineCycles.ByType(new[] { MachineCycleType.OperandRead, MachineCycleType.OperandReadHigh, MachineCycleType.OperandReadLow }))
            {
                byte operand = _cpu.Memory.Untimed.ReadByteAt(address);
                _cpu.Timing.MemoryReadCycle(address++, operand, (byte)(cycle.TStates - InstructionTiming.MEMORY_READ_NORMAL_TSTATES));

                if (!setArgument1)
                {
                    if (cycle.Type == MachineCycleType.OperandRead && instruction.IsIndexed && instruction.HasIntermediateDisplacementByte)
                    {
                        data.Argument1 = indexDisplacementByte.Value;
                        setArgument1 = true;
                    }
                    else if (cycle.Type == MachineCycleType.OperandRead || cycle.Type == MachineCycleType.OperandReadLow)
                    {
                        // single byte operand or first byte of two-byte operand
                        data.Argument1 = operand;
                        setArgument1 = true;
                    }
                }
                else if (cycle.Type == MachineCycleType.OperandRead || cycle.Type == MachineCycleType.OperandReadHigh)
                {
                    // second byte of two-byte operand
                    data.Argument2 = operand;

                    // special case - CALL XX nn has a prolonged operand read but only if it's CALL nn or the flag condition is true
                    if (instruction.Microcode is CALL)
                    {
                        if (instruction.Condition == Condition.None || _cpu.Flags.SatisfyCondition(instruction.Condition))
                        {
                            _cpu.Clock.WaitForNextClockTick();
                        }
                    }
                }
            }

            return data;
        }

        public InstructionDecoder(Processor cpu)
        {
            _cpu = cpu;
        }
    }
}

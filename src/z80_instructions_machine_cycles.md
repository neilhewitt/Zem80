# Z80 Instruction Set - Machine Cycles Reference

This document provides a complete listing of all Z80 instructions organized by the number of machine cycles they require.

## Machine Cycle Types

- **OF** = OpcodeFetch
- **OR** = OperandRead  
- **ORL** = OperandReadLow
- **ORH** = OperandReadHigh
- **MR** = MemoryRead
- **MW** = MemoryWrite
- **MRL** = MemoryReadLow
- **MRH** = MemoryReadHigh
- **MWL** = MemoryWriteLow
- **MWH** = MemoryWriteHigh
- **SRL** = StackReadLow
- **SRH** = StackReadHigh
- **SWL** = StackWriteLow
- **SWH** = StackWriteHigh
- **IO** = InternalOperation
- **PR** = PortRead
- **PW** = PortWrite

## Legend

- T-states shown in parentheses: `(4)`
- Conditional cycles marked with `*` (only execute if condition is true)
- Instructions grouped by number of machine cycles (M-cycles)
- **Notify Calls**: Number of `NotifyMachineCycle` calls in instruction code (M1 is auto-called in Setup, not counted here)

---

## 1 Machine Cycle Instructions

| Mnemonic | M1 | Total T-States | Notify Calls | Notes |
|----------|----|----|-------|-------|
| NOP | OF(4) | 4 | 0 | |
| INC BC | OF(6) | 6 | 0 | |
| INC B | OF(4) | 4 | 0 | |
| DEC B | OF(4) | 4 | 0 | |
| RLCA | OF(4) | 4 | 0 | |
| EX AF,AF' | OF(4) | 4 | 0 | |
| DEC BC | OF(6) | 6 | 0 | |
| INC C | OF(4) | 4 | 0 | |
| DEC C | OF(4) | 4 | 0 | |
| RRCA | OF(4) | 4 | 0 | |
| INC DE | OF(6) | 6 | 0 | |
| INC D | OF(4) | 4 | 0 | |
| DEC D | OF(4) | 4 | 0 | |
| RLA | OF(4) | 4 | 0 | |
| DEC DE | OF(6) | 6 | 0 | |
| INC E | OF(4) | 4 | 0 | |
| DEC E | OF(4) | 4 | 0 | |
| RRA | OF(4) | 4 | 0 | |
| INC HL | OF(6) | 6 | 0 | |
| INC H | OF(4) | 4 | 0 | |
| DEC H | OF(4) | 4 | 0 | |
| DAA | OF(4) | 4 | 0 | |
| DEC HL | OF(6) | 6 | 0 | |
| INC L | OF(4) | 4 | 0 | |
| DEC L | OF(4) | 4 | 0 | |
| CPL | OF(4) | 4 | 0 | |
| INC SP | OF(6) | 6 | 0 | |
| SCF | OF(4) | 4 | 0 | |
| DEC SP | OF(6) | 6 | 0 | |
| INC A | OF(4) | 4 | 0 | |
| DEC A | OF(4) | 4 | 0 | |
| CCF | OF(4) | 4 | 0 | |
| LD B,B - LD A,A | OF(4) | 4 | 0 | Register to register loads |
| HALT | OF(4) | 4 | 0 | |
| ADD A,r | OF(4) | 4 | 0 | |
| ADC A,r | OF(4) | 4 | 0 | |
| SUB r | OF(4) | 4 | 0 | |
| SBC A,r | OF(4) | 4 | 0 | |
| AND r | OF(4) | 4 | 0 | |
| XOR r | OF(4) | 4 | 0 | |
| OR r | OF(4) | 4 | 0 | |
| CP r | OF(4) | 4 | 0 | |
| RET NZ/Z/NC/C/PO/PE/P/M | OF(5) | 5 | 0 | When condition is false |
| EXX | OF(4) | 4 | 0 | |
| JP HL | OF(4) | 4 | 0 | |
| EX DE,HL | OF(4) | 4 | 0 | |
| DI | OF(4) | 4 | 0 | |
| EI | OF(4) | 4 | 0 | |
| LD SP,HL | OF(6) | 6 | 0 | |

---

## 2 Machine Cycle Instructions

| Mnemonic | M1 | M2 | Total T-States | Notify Calls | Notes |
|----------|----|----|-------|-------|-------|
| LD (BC),A | OF(4) | MW(3) | 7 | 1 | |
| LD B,n | OF(4) | OR(3) | 7 | 0 | |
| LD A,(BC) | OF(4) | MR(3) | 7 | 1 | |
| LD C,n | OF(4) | OR(3) | 7 | 0 | |
| DJNZ o | OF(5) | OR(3) | 8 | 0 | When condition is false |
| LD (DE),A | OF(4) | MW(3) | 7 | 1 | |
| LD D,n | OF(4) | OR(3) | 7 | 0 | |
| LD A,(DE) | OF(4) | MR(3) | 7 | 1 | |
| LD E,n | OF(4) | OR(3) | 7 | 0 | |
| JR NZ/Z/NC/C,o | OF(4) | OR(3) | 7 | 0 | When condition is false |
| LD H,n | OF(4) | OR(3) | 7 | 0 | |
| LD L,n | OF(4) | OR(3) | 7 | 0 | |
| LD A,n | OF(4) | OR(3) | 7 | 0 | |
| LD r,(HL) | OF(4) | MR(3) | 7 | 1 | |
| LD (HL),r | OF(4) | MW(3) | 7 | 1 | |
| ADD A,(HL) | OF(4) | MR(3) | 7 | 1 | |
| ADC A,(HL) | OF(4) | MR(3) | 7 | 1 | |
| SUB (HL) | OF(4) | MR(3) | 7 | 1 | |
| SBC A,(HL) | OF(4) | MR(3) | 7 | 1 | |
| AND (HL) | OF(4) | MR(3) | 7 | 1 | |
| XOR (HL) | OF(4) | MR(3) | 7 | 1 | |
| OR (HL) | OF(4) | MR(3) | 7 | 1 | |
| CP (HL) | OF(4) | MR(3) | 7 | 1 | |
| ADD A,n | OF(4) | OR(3) | 7 | 1 | |
| ADC A,n | OF(4) | OR(3) | 7 | 1 | |
| SUB n | OF(4) | OR(3) | 7 | 1 | |
| SBC A,n | OF(4) | OR(3) | 7 | 1 | |
| AND n | OF(4) | OR(3) | 7 | 1 | |
| XOR n | OF(4) | OR(3) | 7 | 1 | |
| OR n | OF(4) | OR(3) | 7 | 1 | |
| CP n | OF(4) | OR(3) | 7 | 1 | |
| **CB-Prefixed** | | | | | |
| RLC/RRC/RL/RR/SLA/SRA/SLL/SRL r | OF(4) | OF(4) | 8 | 0 | |
| BIT b,r | OF(4) | OF(4) | 8 | 0 | |
| RES/SET b,r | OF(4) | OF(4) | 8 | 0 | |
| **ED-Prefixed** | | | | | |
| NEG | OF(4) | OF(4) | 8 | 1 | |
| IM 0/1/2 | OF(4) | OF(4) | 8 | 0 | |
| LD I/R,A | OF(4) | OF(4/5) | 9 | 0 | |
| LD A,I/R | OF(4) | OF(5) | 9 | 0 | |
| **DD/FD-Prefixed** | | | | | |
| JP IX/IY | OF(4) | OF(4) | 8 | 0 | |
| INC/DEC IX/IY | OF(4) | OF(6) | 10 | 0 | |
| LD SP,IX/IY | OF(4) | OF(6) | 10 | 0 | |

---

## 3 Machine Cycle Instructions

| Mnemonic | M1 | M2 | M3 | Total T-States | Notify Calls | Notes |
|----------|----|----|----|----|-------|-------|
| LD BC,nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| LD DE,nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| JR o | OF(4) | OR(3) | IO(5) | 12 | 0 | |
| JR NZ/Z/NC/C,o | OF(4) | OR(3) | IO(5)* | 12 | 0 | When condition is true |
| LD HL,nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| LD SP,nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| INC (HL) | OF(4) | MR(4) | MW(3) | 11 | 2 | |
| DEC (HL) | OF(4) | MR(4) | MW(3) | 11 | 2 | |
| LD (HL),n | OF(4) | OR(3) | MW(3) | 10 | 1 | |
| DJNZ o | OF(5) | OR(3) | IO(5)* | 13 | 0 | When condition is true |
| ADD HL,BC/DE/HL/SP | OF(4) | IO(4) | IO(3) | 11 | 1 | |
| POP BC/DE/HL/AF | OF(4) | SRL(3) | SRH(3) | 10 | 2 | |
| JP NZ/Z/NC/C/PO/PE/P/M,nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| JP nn | OF(4) | ORL(3) | ORH(3) | 10 | 0 | |
| PUSH BC/DE/HL/AF | OF(5) | SWL(3) | SWH(3) | 11 | 2 | |
| RST 00H-38H | OF(5) | SWL(3) | SWH(3) | 11 | 2 | |
| RET | OF(4) | SRL(3) | SRH(3) | 10 | 2 | |
| RET NZ/Z/NC/C/PO/PE/P/M | OF(5) | SRL(3)* | SRH(3)* | 11 | 0/2 | When condition is true |
| **CB-Prefixed** | | | | | | |
| BIT b,(HL) | OF(4) | OF(4) | MR(4) | 12 | 1 | |
| **ED-Prefixed** | | | | | | |
| IN r,(C) | OF(4) | OF(4) | PR(4) | 12 | 1 | |
| OUT (C),r | OF(4) | OF(4) | PW(4) | 12 | 1 | |

---

## 4 Machine Cycle Instructions

| Mnemonic | M1 | M2 | M3 | M4 | Total T-States | Notify Calls | Notes |
|----------|----|----|----|----|-------|-------|-------|
| LD (nn),A | OF(4) | ORL(3) | ORH(3) | MW(3) | 13 | 1 | |
| LD A,(nn) | OF(4) | ORL(3) | ORH(3) | MR(3) | 13 | 1 | |
| CALL NZ/Z/NC/C/PO/PE/P/M,nn | OF(4) | ORL(3) | ORH(4) | - | 10 | 0 | When condition is false |
| **CB-Prefixed** | | | | | | | |
| RLC/RRC/RL/RR/SLA/SRA/SLL/SRL (HL) | OF(4) | OF(4) | MR(4) | MW(3) | 15 | 2 | |
| RES/SET b,(HL) | OF(4) | OF(4) | MR(4) | MW(3) | 15 | 2 | |
| **ED-Prefixed** | | | | | | | |
| SBC HL,rr | OF(4) | OF(4) | IO(4) | IO(3) | 15 | 1 | |
| ADC HL,rr | OF(4) | OF(4) | IO(4) | IO(3) | 15 | 1 | |
| RETN/RETI | OF(4) | OF(4) | SRL(3) | SRH(3) | 14 | 2 | |
| LDI/LDD | OF(4) | OF(4) | MR(3) | MW(5) | 16 | 2 | |
| CPI/CPD | OF(4) | OF(4) | MR(3) | MW(5) | 16 | 1 | |
| INI/IND | OF(4) | OF(5) | PR(4) | MW(3) | 16 | 2 | |
| OUTI/OUTD | OF(4) | OF(5) | MR(3) | PW(4) | 16 | 2 | |
| **DD/FD-Prefixed** | | | | | | | |
| ADD IX/IY,rr | OF(4) | OF(4) | IO(4) | IO(3) | 15 | 1 | |
| LD IX/IY,nn | OF(4) | OF(4) | ORL(3) | ORH(3) | 14 | 0 | |
| POP IX/IY | OF(4) | OF(4) | SRL(3) | SRH(3) | 14 | 2 | |
| PUSH IX/IY | OF(4) | OF(5) | SWL(3) | SWH(3) | 15 | 2 | |

---

## 5 Machine Cycle Instructions

| Mnemonic | M1 | M2 | M3 | M4 | M5 | Total T-States | Notify Calls | Notes |
|----------|----|----|----|----|----|----|-------|-------|
| LD (nn),HL | OF(4) | ORL(3) | ORH(3) | MW(3) | MW(3) | 16 | 2 | |
| LD HL,(nn) | OF(4) | ORL(3) | ORH(3) | MRL(3) | MRH(3) | 16 | 2 | |
| EX (SP),HL | OF(4) | SRL(3) | SRH(4) | SWL(3) | SWH(5) | 19 | 4 | |
| CALL nn | OF(4) | ORL(3) | ORH(4) | SWL(3) | SWH(3) | 17 | 2 | |
| CALL NZ/Z/NC/C/PO/PE/P/M,nn | OF(4) | ORL(3) | ORH(4) | SWL(3)* | SWH(3)* | 17 | 0/2 | When condition is true |
| **ED-Prefixed** | | | | | | | | |
| RLD | OF(4) | OF(4) | MR(3) | IO(4) | MW(3) | 18 | 2 | |
| LDIR/LDDR | OF(4) | OF(4) | MR(3) | MW(5) | IO(5)* | 21/16 | 2 | Repeating, when BC?0 |
| CPIR/CPDR | OF(4) | OF(4) | MR(3) | MW(5) | IO(5)* | 21/16 | 1 | Repeating, when BC?0 and not found |
| INIR/INDR | OF(4) | OF(5) | PR(4) | MW(3) | IO(5) | 21 | 2 | |
| OTIR/OTDR | OF(4) | OF(5) | MR(3) | PW(4) | IO(5) | 21 | 2 | |
| **DD/FD-Prefixed** | | | | | | | | |
| LD r,(IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(3) | 19 | 1 | |
| LD (IX/IY+o),r | OF(4) | OF(4) | OR(3) | IO(5) | MW(3) | 19 | 1 | |
| ADD/ADC/SUB/SBC/AND/XOR/OR/CP A,(IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(3) | 19 | 1 | |
| **DDCB/FDCB-Prefixed** | | | | | | | | |
| BIT b,(IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | 20 | 1 | |

---

## 6 Machine Cycle Instructions

| Mnemonic | M1 | M2 | M3 | M4 | M5 | M6 | Total T-States | Notify Calls | Notes |
|----------|----|----|----|----|----|----|-------|-------|-------|
| **ED-Prefixed** | | | | | | | | | |
| LD (nn),BC/DE/HL/SP | OF(4) | OF(4) | ORL(3) | ORH(3) | MW(3) | MW(3) | 20 | 2 | |
| LD BC/DE/HL/SP,(nn) | OF(4) | OF(4) | ORL(3) | ORH(3) | MRL(3) | MRH(3) | 20 | 2 | |
| RRD | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 18 | 2 | |
| **DD/FD-Prefixed** | | | | | | | | | |
| LD (nn),IX/IY | OF(4) | OF(4) | ORL(3) | ORH(3) | MW(3) | MW(3) | 20 | 2 | |
| LD IX/IY,(nn) | OF(4) | OF(4) | ORL(3) | ORH(3) | MRL(3) | MRH(3) | 20 | 2 | |
| LD (IX/IY+o),n | OF(4) | OF(4) | OR(3) | OR(3) | IO(5) | MW(3) | 19 | 1 | |
| INC/DEC (IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 23 | 2 | |
| EX (SP),IX/IY | OF(4) | OF(4) | SRL(3) | SRH(4) | SWL(3) | SWH(5) | 23 | 4 | |
| **DDCB/FDCB-Prefixed** | | | | | | | | | |
| RLC/RRC/RL/RR/SLA/SRA/SLL/SRL (IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 23 | 2 | |
| RES/SET b,(IX/IY+o) | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 23 | 2 | |
| RLC/RRC/RL/RR/SLA/SRA/SLL/SRL (IX/IY+o),r | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 23 | 2 | Undocumented |
| RES/SET b,(IX/IY+o),r | OF(4) | OF(4) | OR(3) | IO(5) | MR(4) | MW(3) | 23 | 2 | Undocumented |

---

## Summary by Machine Cycle Count

| M-Cycles | Instruction Count (approx) | T-State Range | Notify Call Range |
|----------|---------------------------|---------------|-------------------|
| 1 | 50+ | 4-6 | 0 |
| 2 | 40+ | 7-10 | 0-1 |
| 3 | 25+ | 10-13 | 0-2 |
| 4 | 20+ | 13-16 | 0-2 |
| 5 | 15+ | 16-21 | 1-4 |
| 6 | 15+ | 18-23 | 1-4 |

---

## Notes

1. **Conditional Cycles**: Machine cycles marked with `*` only execute if the condition is met. T-state totals show both cases (condition true/false).

2. **Undocumented Instructions**: The DDCB and FDCB prefixed bit operations have undocumented variants that copy results to registers. These use the same machine cycle timings as their documented counterparts.

3. **Total T-States**: Sum of all T-states from all machine cycles that execute.

4. **Index Register Instructions**: IX and IY instructions follow identical patterns with the same timing.

5. **Repeating Instructions**: Instructions like LDIR, CPIR, etc. show timing for both when they repeat (BC?0 or match not found) and when they terminate.

6. **Notify Calls**: The number shown does not include the automatic M1 notification in `MicrocodeBase.Setup()`. Instructions with conditional paths show both counts (e.g., "0/2" means 0 calls when condition is false, 2 when true).

---

*Generated from Zem80 Core InstructionSet.cs - Updated with NotifyMachineCycle call counts*

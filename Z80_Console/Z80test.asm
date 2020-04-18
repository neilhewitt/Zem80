 format binary as "p"

xx     db 10h dup(0FFh)
       db 2

yy     EQU 3

       OUTI
       OUTD
       OTIR
       OTDR

       OUT (C),A
       OUT (C),B
       OUT (C),C
       OUT (C),D
       OUT (C),E
       OUT (C),H
       OUT (C),L

       OUT (0),A
       OUT (40h),A
       OUT (0F0h),A
       OUT ( -80 ) , A

       INI
       IND
       INIR
       INDR

       IN A,(C)
       IN B,(C)
       IN C,(C)
       IN D,(C)
       IN E,(C)
       IN H,(C)
       IN L,(C)

       IN A , (0h)
       IN A,(7fh)
       IN A,(0ffh)
       IN A,(-80)

       RST 0
       RST 8
       RST 10h
       RST 18h
       RST 20h
       RST 28h
       RST 30h
       RST 38h

       RET
       RETI
       RETN

       RET NZ
       RET Z
       RET NC
       RET C
       RET PO
       RET PE
       RET P
       RET M

       CALL NZ,100h
       CALL NZ,100h
       CALL Z,200h
       CALL NC,300h
       CALL C,400h
       CALL PO,500h
       CALL PE,600h
       CALL P,700h
       CALL M,800h

       CALL 0h
       CALL testlabel2

       DJNZ testlabel2

       JR C,@f
       JR NC,@f
       JR Z,@f
       JR NZ,@f

testlabel2:

@@:
       db 7ch dup(4)
       JR @f
       JR @b

       db 7dh dup(5)
@@:

       JP NZ,0
       JP Z,1
       JP NC,11h
       JP C,5678h
       JP PO,-1
       JP PE,100h
       JP P,99h
       JP M,testlabel

       JP [HL]
       JP [IX]
       JP [IY]
       JP @f
       JP 4321h

testlabel:

       RES 7,[HL]
       RES 5,[IX+yy]
       RES 4,[IY]

@@:
       RES 7,A
       RES 6,B
       RES 5,C
       RES 4,D
       RES 3,E
       RES 2,H
       RES 1,L

       SET 7,[HL]
       SET 0,[IX+1]
       SET 1,[IY-1]

       SET 7,A
       SET 6,B
       SET 5,C
       SET 4,D
       SET 3,E
       SET 2,H
       SET 1,L

       BIT 7,[HL]
       BIT 0,[IX+127]
       BIT 1,[IY-128]

       BIT 7,A
       BIT 6,B
       BIT 5,C
       BIT 4,D
       BIT 3,E
       BIT 2,H
       BIT 1,L

       RLD
       RRD

       SRL [HL]
       SRL [IX+1]
       SRL [IY-1]
       SRL A
       SRL B
       SRL C
       SRL D
       SRL E
       SRL H
       SRL L

       SRA [HL]
       SRA [IX+1]
       SRA [IY-1]
       SRA A
       SRA B
       SRA C
       SRA D
       SRA E
       SRA H
       SRA L

       SLA [HL]
       SLA [IX+1]
       SLA [IY-1]
       SLA A
       SLA B
       SLA C
       SLA D
       SLA E
       SLA H
       SLA L

       RR [HL]
       RR [IX+1]
       RR [IY-1]
       RR A
       RR B
       RR C
       RR D
       RR E
       RR H
       RR L

       RL [HL]
       RL [IX+1]
       RL [IY-1]
       RL A
       RL B
       RL C
       RL D
       RL E
       RL H
       RL L

       RRC [HL]
       RRC [IX+127]
       RRC [IY-128]
       RRC A
       RRC B
       RRC C
       RRC D
       RRC E
       RRC H
       RRC L

       RLC [HL]
       RLC [IX+1]
       RLC [IY-1]
       RLC A
       RLC B
       RLC C
       RLC D
       RLC E
       RLC H
       RLC L

       RLA
       RRA
       RLCA
       RRCA

       DEC BC
       DEC DE
       DEC HL
       DEC SP
       DEC IX
       DEC IY

       INC BC
       INC DE
       INC HL
       INC SP
       INC IX
       INC IY

       ADD IX,BC
       ADD IX,DE
       ADD IX,IX
       ADD IX,SP

       ADD IY,BC
       ADD IY,DE
       ADD IY,IY
       ADD IY,SP

       SBC HL,BC
       SBC HL,DE
       SBC HL,HL
       SBC HL,SP

       ADC HL,BC
       ADC HL,DE
       ADC HL,HL
       ADC HL,SP

       ADD HL,BC
       ADD HL,DE
       ADD HL,HL
       ADD HL,SP

       DAA
       CPL
       NEG
       CCF
       SCF
       NOP
       HALT
       DI
       EI
       IM 0
       IM 1
       IM 2

       DEC [IX-128]
       DEC [IY+127]
       DEC [HL]
       DEC A
       DEC B
       DEC C
       DEC D
       DEC E
       DEC H
       DEC L

       INC [IX-2]
       INC [IY+127]
       INC [HL]
       INC A
       INC B
       INC C
       INC D
       INC E
       INC H
       INC L

       CP [IX+5]
       CP [IY]
       CP [HL]
       CP -1
       CP A
       CP B
       CP C
       CP D
       CP E
       CP H
       CP L

       XOR [IX+5]
       XOR [IY]
       XOR [HL]
       XOR -1
       XOR A
       XOR B
       XOR C
       XOR D
       XOR E
       XOR H
       XOR L

       OR [IX+5]
       OR [IY]
       OR [HL]
       OR -1
       OR A
       OR B
       OR C
       OR D
       OR E
       OR H
       OR L

       AND [IX+5]
       AND [IY]
       AND [HL]
       AND -1
       AND A
       AND B
       AND C
       AND D
       AND E
       AND H
       AND L

       SBC A,[IX+5]
       SBC A,[IY]
       SBC A,[HL]
       SBC A,-1
       SBC A,A
       SBC A,B
       SBC A,C
       SBC A,D
       SBC A,E
       SBC A,H
       SBC A,L

       SUB [IX+5]
       SUB [IY]
       SUB [HL]
       SUB 0ffh
;       SUB (L1ADF+1) MOD 100h
       SUB A
       SUB B
       SUB C
       SUB D
       SUB E
       SUB H
       SUB L

	org 1ADFh
L1ADF:

       ADC A,[IX+5]
       ADC A,[IY]
       ADC A,[HL]
       ADC A,-1
       ADC A,A
       ADC A,B
       ADC A,C
       ADC A,D
       ADC A,E
       ADC A,H
       ADC A,L

       ADD A,[IX+5]
       ADD A,[IY]
       ADD A,[HL]
       ADD A,-1
       ADD A,A
       ADD A,B
       ADD A,C
       ADD A,D
       ADD A,E
       ADD A,H
       ADD A,L

       LDI
       LDIR
       LDD
       LDDR

       CPI
       CPIR
       CPD
       CPDR

       EX [SP],HL
       EX [SP],IX
       EX [SP],IY

       EXX

       EX AF,AF
       EX DE,HL

       POP IX
       POP IY

       POP BC
       POP DE
       POP HL
       POP AF

       PUSH IX
       PUSH IY

       PUSH BC
       PUSH DE
       PUSH HL
       PUSH AF


       LD SP,HL
       LD SP,IX
       LD SP,IY

       LD [9876h],IX
       LD [55AAh],IY

       LD [-1],HL
       LD [0FFFFh],BC
       LD [99h],DE
       LD [0],SP

       LD IX,[34h]
       LD IY,[5678h]

       LD BC,[0]
       LD DE,(-1)
       LD SP,[3333h]

       LD HL,[1178h]

       LD IX,(1010h+yy)*2
       LD IY,2244h

       LD BC,0FFFFh
       LD DE,0h
       LD HL,8080h
       LD SP,0F000h

; 16 bit load instructions
       LD [0h],A

       LD [BC],A
       LD [DE],A

       LD A,[-1]
       LD A,[BC]
       LD A,[DE]

       LD [IX+5],6
       LD [IY],0eeh
       LD [HL],-2

       LD [IX+1],A
       LD [IY],B
       LD [IX+7Fh],C
       LD [IY-80h],D
       LD [IX+20h*2],E
       LD [IY+80h/8h],H
       LD [IX],L

       LD [HL],A
       LD [HL],B
       LD [HL],C
       LD [HL],D
       LD [HL],E
       LD [HL],H
       LD [HL],L

       LD A,[IX+7Fh]
       LD A,[IX-80h]

       LD A,[IX]
       LD B,[IX+2]
       LD C,[IX+5*2]
       LD D,[IX+11h]
       LD E,[IX+77h]
       LD H,[IX-2]
       LD L,[IX-100]

       LD A,[IY]
       LD B,[IY+2]
       LD C,[IY+5*2]
       LD D,[IY+11h]
       LD E,[IY+77h]
       LD H,[IY-2]
       LD L,[IY-100]

       LD A,(HL)
       LD B,(HL)
       LD C,(HL)
       LD D,[HL]
       LD E,[HL]
       LD H,[HL]
       LD L,[HL]

       LD A,R
       LD R,A

       LD A,I
       LD I,A

       LD A,0FFh
       LD B,01h
       LD C,02h
       LD D,04h
       LD E,08h
       LD H,10h
       LD L,12h

       LD A,A
       LD A,B
       LD A,C
       LD A,D
       LD A,E
       LD A,H
       LD A,L

       LD B,A
       LD B,B
       LD B,C
       LD B,D
       LD B,E
       LD B,H
       LD B,L

       LD C,A
       LD C,B
       LD C,C
       LD C,D
       LD C,E
       LD C,H
       LD C,L

       LD D,A
       LD D,B
       LD D,C
       LD D,D
       LD D,E
       LD D,H
       LD D,L

       LD E,A
       LD E,B
       LD E,C
       LD E,D
       LD E,E
       LD E,H
       LD E,L

       LD H,A
       LD H,B
       LD H,C
       LD H,D
       LD H,E
       LD H,H
       LD H,L

       LD L,A
       LD L,B
       LD L,C
       LD L,D
       LD L,E
       LD L,H
       LD L,L

;       TRACE
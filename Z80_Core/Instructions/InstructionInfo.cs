using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class InstructionInfo
    {
        private static Dictionary<string, Dictionary<string, InstructionInfo>> _opcodeSets =
                new Dictionary<string, Dictionary<string, InstructionInfo>>() {
                {
                    "00", new Dictionary<string, InstructionInfo>()
                    {
                        { "8E", new InstructionInfo("00","ADC A,(HL)", "8E", "", "", "", "1", "7") },
                        { "CE", new InstructionInfo("00","ADC A,n", "CE", "n", "", "", "2", "7") },
                        { "88", new InstructionInfo("00","ADC A,r", "88", "", "", "+r", "1", "4") },
                        { "86", new InstructionInfo("00","ADD A,(HL)", "86", "", "", "", "1", "7") },
                        { "C6", new InstructionInfo("00","ADD A,n", "C6", "n", "", "", "2", "7") },
                        { "80", new InstructionInfo("00","ADD A,r", "80", "", "", "+r", "1", "4") },
                        { "09", new InstructionInfo("00","ADD HL,BC", "09", "", "", "", "1", "11") },
                        { "19", new InstructionInfo("00","ADD HL,DE", "19", "", "", "", "1", "11") },
                        { "29", new InstructionInfo("00","ADD HL,HL", "29", "", "", "", "1", "11") },
                        { "39", new InstructionInfo("00","ADD HL,SP", "39", "", "", "", "1", "11") },
                        { "A6", new InstructionInfo("00","AND (HL)", "A6", "", "", "", "1", "7") },
                        { "E6", new InstructionInfo("00","AND n", "E6", "n", "", "", "2", "7") },
                        { "A0", new InstructionInfo("00","AND r", "A0", "", "", "+r", "1", "4") },
                        { "DC", new InstructionInfo("00","CALL C,nn", "DC", "nn", "nn", "", "3", "17/10") },
                        { "FC", new InstructionInfo("00","CALL M,nn", "FC", "nn", "nn", "", "3", "17/10") },
                        { "D4", new InstructionInfo("00","CALL NC,nn", "D4", "nn", "nn", "", "3", "17/10") },
                        { "CD", new InstructionInfo("00","CALL nn", "CD", "nn", "nn", "", "3", "17") },
                        { "C4", new InstructionInfo("00","CALL NZ,nn", "C4", "nn", "nn", "", "3", "17/10") },
                        { "F4", new InstructionInfo("00","CALL P,nn", "F4", "nn", "nn", "", "3", "17/10") },
                        { "EC", new InstructionInfo("00","CALL PE,nn", "EC", "nn", "nn", "", "3", "17/10") },
                        { "E4", new InstructionInfo("00","CALL PO,nn", "E4", "nn", "nn", "", "3", "17/10") },
                        { "CC", new InstructionInfo("00","CALL Z,nn", "CC", "nn", "nn", "", "3", "17/10") },
                        { "3F", new InstructionInfo("00","CCF", "3F", "", "", "", "1", "4") },
                        { "BE", new InstructionInfo("00","CP (HL)", "BE", "", "", "", "1", "7") },
                        { "FE", new InstructionInfo("00","CP n", "FE", "n", "", "", "2", "7") },
                        { "B8", new InstructionInfo("00","CP r", "B8", "", "", "+r", "1", "4") },
                        { "2F", new InstructionInfo("00","CPL", "2F", "", "", "", "1", "4") },
                        { "27", new InstructionInfo("00","DAA", "27", "", "", "", "1", "4") },
                        { "35", new InstructionInfo("00","DEC (HL)", "35", "", "", "", "1", "11") },
                        { "3D", new InstructionInfo("00","DEC A", "3D", "", "", "", "1", "4") },
                        { "05", new InstructionInfo("00","DEC B", "05", "", "", "", "1", "4") },
                        { "0B", new InstructionInfo("00","DEC BC", "0B", "", "", "", "1", "6") },
                        { "0D", new InstructionInfo("00","DEC C", "0D", "", "", "", "1", "4") },
                        { "15", new InstructionInfo("00","DEC D", "15", "", "", "", "1", "4") },
                        { "1B", new InstructionInfo("00","DEC DE", "1B", "", "", "", "1", "6") },
                        { "1D", new InstructionInfo("00","DEC E", "1D", "", "", "", "1", "4") },
                        { "25", new InstructionInfo("00","DEC H", "25", "", "", "", "1", "4") },
                        { "2B", new InstructionInfo("00","DEC HL", "2B", "", "", "", "1", "6") },
                        { "2D", new InstructionInfo("00","DEC L", "2D", "", "", "", "2", "4") },
                        { "3B", new InstructionInfo("00","DEC SP", "3B", "", "", "", "1", "6") },
                        { "F3", new InstructionInfo("00","DI", "F3", "", "", "", "1", "4") },
                        { "10", new InstructionInfo("00","DJNZ o", "10", "o", "", "", "2", "13/8") },
                        { "FB", new InstructionInfo("00","EI", "FB", "", "", "", "1", "4") },
                        { "E3", new InstructionInfo("00","EX (SP),HL", "E3", "", "", "", "1", "19") },
                        { "08", new InstructionInfo("00","EX AF,AF'", "08", "", "", "", "1", "4") },
                        { "EB", new InstructionInfo("00","EX DE,HL", "EB", "", "", "", "1", "4") },
                        { "D9", new InstructionInfo("00","EXX", "D9", "", "", "", "1", "4") },
                        { "76", new InstructionInfo("00","HALT", "76", "", "", "", "1", "4") },
                        { "DB", new InstructionInfo("00","IN A,(n)", "DB", "n", "", "", "2", "11") },
                        { "34", new InstructionInfo("00","INC (HL)", "34", "", "", "", "1", "11") },
                        { "3C", new InstructionInfo("00","INC A", "3C", "", "", "", "1", "4") },
                        { "04", new InstructionInfo("00","INC B", "04", "", "", "", "1", "4") },
                        { "03", new InstructionInfo("00","INC BC", "03", "", "", "", "1", "6") },
                        { "0C", new InstructionInfo("00","INC C", "0C", "", "", "", "1", "4") },
                        { "14", new InstructionInfo("00","INC D", "14", "", "", "", "1", "4") },
                        { "13", new InstructionInfo("00","INC DE", "13", "", "", "", "1", "6") },
                        { "1C", new InstructionInfo("00","INC E", "1C", "", "", "", "1", "4") },
                        { "24", new InstructionInfo("00","INC H", "24", "", "", "", "1", "4") },
                        { "23", new InstructionInfo("00","INC HL", "23", "", "", "", "1", "6") },
                        { "2C", new InstructionInfo("00","INC L", "2C", "", "", "", "1", "4") },
                        { "33", new InstructionInfo("00","INC SP", "33", "", "", "", "1", "6") },
                        { "E9", new InstructionInfo("00","JP (HL)", "E9", "", "", "", "1", "4") },
                        { "DA", new InstructionInfo("00","JP C,nn", "DA", "nn", "nn", "", "3", "10") },
                        { "FA", new InstructionInfo("00","JP M,nn", "FA", "nn", "nn", "", "3", "10") },
                        { "D2", new InstructionInfo("00","JP NC,nn", "D2", "nn", "nn", "", "3", "10") },
                        { "C3", new InstructionInfo("00","JP nn", "C3", "nn", "nn", "", "3", "10") },
                        { "C2", new InstructionInfo("00","JP NZ,nn", "C2", "nn", "nn", "", "3", "10") },
                        { "F2", new InstructionInfo("00","JP P,nn", "F2", "nn", "nn", "", "3", "10") },
                        { "EA", new InstructionInfo("00","JP PE,nn", "EA", "nn", "nn", "", "3", "10") },
                        { "E2", new InstructionInfo("00","JP PO,nn", "E2", "nn", "nn", "", "3", "10") },
                        { "CA", new InstructionInfo("00","JP Z,nn", "CA", "nn", "nn", "", "3", "10") },
                        { "38", new InstructionInfo("00","JR C,o", "38", "o", "", "", "2", "12/7") },
                        { "30", new InstructionInfo("00","JR NC,o", "30", "o", "", "", "2", "12/7") },
                        { "20", new InstructionInfo("00","JR NZ,o", "20", "o", "", "", "2", "12/7") },
                        { "18", new InstructionInfo("00","JR o", "18", "o", "", "", "2", "12") },
                        { "28", new InstructionInfo("00","JR Z,o", "28", "o", "", "", "2", "12/7") },
                        { "02", new InstructionInfo("00","LD (BC),A", "02", "", "", "", "1", "7") },
                        { "12", new InstructionInfo("00","LD (DE),A", "12", "", "", "", "1", "7") },
                        { "36", new InstructionInfo("00","LD (HL),n", "36", "n", "", "", "2", "10") },
                        { "70", new InstructionInfo("00","LD (HL),r", "70", "", "", "+r", "1", "7") },
                        { "32", new InstructionInfo("00","LD (nn),A", "32", "nn", "nn", "", "3", "13") },
                        { "22", new InstructionInfo("00","LD (nn),HL", "22", "nn", "nn", "", "3", "16") },
                        { "0A", new InstructionInfo("00","LD A,(BC)", "0A", "", "", "", "1", "7") },
                        { "1A", new InstructionInfo("00","LD A,(DE)", "1A", "", "", "", "1", "7") },
                        { "7E", new InstructionInfo("00","LD A,(HL)", "7E", "", "", "", "1", "7") },
                        { "3A", new InstructionInfo("00","LD A,(nn)", "3A", "nn", "nn", "", "3", "13") },
                        { "3E", new InstructionInfo("00","LD A,n", "3E", "n", "", "", "2", "7") },
                        { "78", new InstructionInfo("00","LD A,r", "78", "", "", "+r", "1", "4") },
                        { "46", new InstructionInfo("00","LD B,(HL)", "46", "", "", "", "1", "7") },
                        { "06", new InstructionInfo("00","LD B,n", "06", "n", "", "", "2", "7") },
                        { "40", new InstructionInfo("00","LD B,r", "40", "", "", "+r", "1", "4") },
                        { "01", new InstructionInfo("00","LD BC,nn", "01", "nn", "nn", "", "3", "10") },
                        { "4E", new InstructionInfo("00","LD C,(HL)", "4E", "", "", "", "1", "7") },
                        { "0E", new InstructionInfo("00","LD C,n", "0E", "n", "", "", "2", "7") },
                        { "48", new InstructionInfo("00","LD C,r", "48", "", "", "+r", "1", "4") },
                        { "56", new InstructionInfo("00","LD D,(HL)", "56", "", "", "", "1", "7") },
                        { "16", new InstructionInfo("00","LD D,n", "16", "n", "", "", "2", "7") },
                        { "50", new InstructionInfo("00","LD D,r", "50", "", "", "+r", "1", "4") },
                        { "11", new InstructionInfo("00","LD DE,nn", "11", "nn", "nn", "", "3", "10") },
                        { "5E", new InstructionInfo("00","LD E,(HL)", "5E", "", "", "", "1", "7") },
                        { "1E", new InstructionInfo("00","LD E,n", "1E", "n", "", "", "2", "7") },
                        { "58", new InstructionInfo("00","LD E,r", "58", "", "", "+r", "1", "4") },
                        { "66", new InstructionInfo("00","LD H,(HL)", "66", "", "", "", "1", "7") },
                        { "26", new InstructionInfo("00","LD H,n", "26", "n", "", "", "2", "7") },
                        { "60", new InstructionInfo("00","LD H,r", "60", "", "", "+r", "1", "4") },
                        { "2A", new InstructionInfo("00","LD HL,(nn)", "2A", "nn", "nn", "", "5", "16") },
                        { "21", new InstructionInfo("00","LD HL,nn", "21", "nn", "nn", "", "3", "10") },
                        { "6E", new InstructionInfo("00","LD L,(HL)", "6E", "", "", "", "1", "7") },
                        { "2E", new InstructionInfo("00","LD L,n", "2E", "n", "", "", "2", "7") },
                        { "68", new InstructionInfo("00","LD L,r", "68", "", "", "+r", "1", "4") },
                        { "F9", new InstructionInfo("00","LD SP,HL", "F9", "", "", "", "1", "6") },
                        { "31", new InstructionInfo("00","LD SP,nn", "31", "nn", "nn", "", "3", "10") },
                        { "00", new InstructionInfo("00","NOP", "00", "", "", "", "1", "4") },
                        { "B6", new InstructionInfo("00","OR (HL)", "B6", "", "", "", "1", "7") },
                        { "F6", new InstructionInfo("00","OR n", "F6", "n", "", "", "2", "7") },
                        { "B0", new InstructionInfo("00","OR r", "B0", "", "", "+r", "1", "4") },
                        { "D3", new InstructionInfo("00","OUT (n),A", "D3", "n", "", "", "2", "11") },
                        { "F1", new InstructionInfo("00","POP AF", "F1", "", "", "", "1", "10") },
                        { "C1", new InstructionInfo("00","POP BC", "C1", "", "", "", "1", "10") },
                        { "D1", new InstructionInfo("00","POP DE", "D1", "", "", "", "1", "10") },
                        { "E1", new InstructionInfo("00","POP HL", "E1", "", "", "", "1", "10") },
                        { "F5", new InstructionInfo("00","PUSH AF", "F5", "", "", "", "1", "11") },
                        { "C5", new InstructionInfo("00","PUSH BC", "C5", "", "", "", "1", "11") },
                        { "D5", new InstructionInfo("00","PUSH DE", "D5", "", "", "", "1", "11") },
                        { "E5", new InstructionInfo("00","PUSH HL", "E5", "", "", "", "1", "11") },
                        { "C9", new InstructionInfo("00","RET", "C9", "", "", "", "1", "10") },
                        { "D8", new InstructionInfo("00","RET C", "D8", "", "", "", "1", "11/5") },
                        { "F8", new InstructionInfo("00","RET M", "F8", "", "", "", "1", "11/5") },
                        { "D0", new InstructionInfo("00","RET NC", "D0", "", "", "", "1", "11/5") },
                        { "C0", new InstructionInfo("00","RET NZ", "C0", "", "", "", "1", "11/5") },
                        { "F0", new InstructionInfo("00","RET P", "F0", "", "", "", "1", "11/5") },
                        { "E8", new InstructionInfo("00","RET PE", "E8", "", "", "", "1", "11/5") },
                        { "E0", new InstructionInfo("00","RET PO", "E0", "", "", "", "1", "11/5") },
                        { "C8", new InstructionInfo("00","RET Z", "C8", "", "", "", "1", "11/5") },
                        { "17", new InstructionInfo("00","RLA", "17", "", "", "", "1", "4") },
                        { "07", new InstructionInfo("00","RLCA", "07", "", "", "", "1", "4") },
                        { "1F", new InstructionInfo("00","RRA", "1F", "", "", "", "1", "4") },
                        { "0F", new InstructionInfo("00","RRCA", "0F", "", "", "", "1", "4") },
                        { "C7", new InstructionInfo("00","RST 0", "C7", "", "", "", "1", "11") },
                        { "D7", new InstructionInfo("00","RST 10H", "D7", "", "", "", "1", "11") },
                        { "DF", new InstructionInfo("00","RST 18H", "DF", "", "", "", "1", "11") },
                        { "E7", new InstructionInfo("00","RST 20H", "E7", "", "", "", "1", "11") },
                        { "EF", new InstructionInfo("00","RST 28H", "EF", "", "", "", "1", "11") },
                        { "F7", new InstructionInfo("00","RST 30H", "F7", "", "", "", "1", "11") },
                        { "FF", new InstructionInfo("00","RST 38H", "FF", "", "", "", "1", "11") },
                        { "CF", new InstructionInfo("00","RST 8H", "CF", "", "", "", "1", "11") },
                        { "9E", new InstructionInfo("00","SBC A,(HL)", "9E", "", "", "", "1", "7") },
                        { "DE", new InstructionInfo("00","SBC A,n", "DE", "n", "", "", "2", "7") },
                        { "98", new InstructionInfo("00","SBC A,r", "98", "", "", "+r", "1", "4") },
                        { "37", new InstructionInfo("00","SCF", "37", "", "", "", "1", "4") },
                        { "96", new InstructionInfo("00","SUB (HL)", "96", "", "", "", "1", "7") },
                        { "D6", new InstructionInfo("00","SUB n", "D6", "n", "", "", "2", "7") },
                        { "90", new InstructionInfo("00","SUB r", "90", "", "", "+r", "1", "4") },
                        { "AE", new InstructionInfo("00","XOR (HL)", "AE", "", "", "", "1", "7") },
                        { "EE", new InstructionInfo("00","XOR n", "EE", "n", "", "", "2", "7") },
                        { "A8", new InstructionInfo("00","XOR r", "A8", "", "", "+r", "1", "4") }
                    }
                },
                {
                    "CB", new Dictionary<string, InstructionInfo>()
                    {
                        { "46", new InstructionInfo("CB","BIT b,(HL)", "46", "", "", "+8*b", "2", "12") },
                        { "40", new InstructionInfo("CB","BIT b,r", "40", "", "", "+8*b+r", "2", "8") },
                        { "86", new InstructionInfo("CB","RES b,(HL)", "86", "", "", "+8*b", "2", "15") },
                        { "80", new InstructionInfo("CB","RES b,r", "80", "", "", "+8*b+r", "2", "8") },
                        { "16", new InstructionInfo("CB","RL (HL)", "16", "", "", "", "2", "15") },
                        { "10", new InstructionInfo("CB","RL r", "10", "", "", "+r", "2", "8") },
                        { "06", new InstructionInfo("CB","RLC (HL)", "06", "", "", "", "2", "15") },
                        { "00", new InstructionInfo("CB","RLC r", "00", "", "", "+r", "2", "8") },
                        { "1E", new InstructionInfo("CB","RR (HL)", "1E", "", "", "", "2", "15") },
                        { "18", new InstructionInfo("CB","RR r", "18", "", "", "+r", "2", "8") },
                        { "0E", new InstructionInfo("CB","RRC (HL)", "0E", "", "", "", "2", "15") },
                        { "08", new InstructionInfo("CB","RRC r", "08", "", "", "+r", "2", "8") },
                        { "C6", new InstructionInfo("CB","SET b,(HL)", "C6", "", "", "+8*b", "2", "15") },
                        { "C0", new InstructionInfo("CB","SET b,r", "C0", "", "", "+8*b+r", "2", "8") },
                        { "26", new InstructionInfo("CB","SLA (HL)", "26", "", "", "", "2", "15") },
                        { "20", new InstructionInfo("CB","SLA r", "20", "", "", "+r", "2", "8") },
                        { "2E", new InstructionInfo("CB","SRA (HL)", "2E", "", "", "", "2", "15") },
                        { "28", new InstructionInfo("CB","SRA r", "28", "", "", "+r", "2", "8") },
                        { "3E", new InstructionInfo("CB","SRL (HL)", "3E", "", "", "", "2", "15") },
                        { "38", new InstructionInfo("CB","SRL r", "38", "", "", "+r", "2", "8") }
                    }
                },
                {
                    "DD", new Dictionary<string, InstructionInfo>()
                    {
                        { "8E", new InstructionInfo("DD","ADC A,(IX+o)", "8E", "o", "", "", "3", "19") },
                        { "88", new InstructionInfo("DD","ADC A,IXp", "88", "", "", "+p", "2", "8") },
                        { "86", new InstructionInfo("DD","ADD A,(IX+o)", "86", "o", "", "", "3", "19") },
                        { "80", new InstructionInfo("DD","ADD A,IXp", "80", "", "", "+p", "2", "8") },
                        { "09", new InstructionInfo("DD","ADD IX,BC", "09", "", "", "", "2", "15") },
                        { "19", new InstructionInfo("DD","ADD IX,DE", "19", "", "", "", "2", "15") },
                        { "29", new InstructionInfo("DD","ADD IX,IX", "29", "", "", "", "2", "15") },
                        { "39", new InstructionInfo("DD","ADD IX,SP", "39", "", "", "", "2", "15") },
                        { "A6", new InstructionInfo("DD","AND (IX+o)", "A6", "o", "", "", "3", "19") },
                        { "A0", new InstructionInfo("DD","AND IXp", "A0", "", "", "+p", "2", "8") },
                        { "BE", new InstructionInfo("DD","CP (IX+o)", "BE", "o", "", "", "3", "19") },
                        { "B8", new InstructionInfo("DD","CP IXp", "B8", "", "", "+p", "2", "8") },
                        { "35", new InstructionInfo("DD","DEC (IX+o)", "35", "o", "", "", "3", "23") },
                        { "2B", new InstructionInfo("DD","DEC IX", "2B", "", "", "", "2", "10") },
                        { "05", new InstructionInfo("DD","DEC IXp", "05", "", "", "+8*p", "2", "8") },
                        { "E3", new InstructionInfo("DD","EX (SP),IX", "E3", "", "", "", "2", "23") },
                        { "34", new InstructionInfo("DD","INC (IX+o)", "34", "o", "", "", "3", "23") },
                        { "23", new InstructionInfo("DD","INC IX", "23", "", "", "", "2", "10") },
                        { "04", new InstructionInfo("DD","INC IXp", "04", "", "", "+8*p", "2", "8") },
                        { "E9", new InstructionInfo("DD","JP (IX)", "E9", "", "", "", "2", "8") },
                        { "36", new InstructionInfo("DD","LD (IX+o),n", "36", "o", "n", "", "4", "19") },
                        { "70", new InstructionInfo("DD","LD (IX+o),r", "70", "o", "", "+r", "3", "19") },
                        { "22", new InstructionInfo("DD","LD (nn),IX", "22", "nn", "nn", "", "4", "20") },
                        { "7E", new InstructionInfo("DD","LD A,(IX+o)", "7E", "o", "", "", "3", "19") },
                        { "78", new InstructionInfo("DD","LD A,IXp", "78", "", "", "+p", "2", "8") },
                        { "46", new InstructionInfo("DD","LD B,(IX+o)", "46", "o", "", "", "3", "19") },
                        { "40", new InstructionInfo("DD","LD B,IXp", "40", "", "", "+p", "2", "8") },
                        { "4E", new InstructionInfo("DD","LD C,(IX+o)", "4E", "o", "", "", "3", "19") },
                        { "48", new InstructionInfo("DD","LD C,IXp", "48", "", "", "+p", "2", "8") },
                        { "56", new InstructionInfo("DD","LD D,(IX+o)", "56", "o", "", "", "3", "19") },
                        { "50", new InstructionInfo("DD","LD D,IXp", "50", "", "", "+p", "2", "8") },
                        { "5E", new InstructionInfo("DD","LD E,(IX+o)", "5E", "o", "", "", "3", "19") },
                        { "58", new InstructionInfo("DD","LD E,IXp", "58", "", "", "+p", "2", "8") },
                        { "66", new InstructionInfo("DD","LD H,(IX+o)", "66", "o", "", "", "3", "19") },
                        { "2A", new InstructionInfo("DD","LD IX,(nn)", "2A", "nn", "nn", "", "4", "20") },
                        { "21", new InstructionInfo("DD","LD IX,nn", "21", "nn", "nn", "", "4", "14") },
                        { "26", new InstructionInfo("DD","LD IXh,n", "26", "n", "", "", "2", "11") },
                        { "60", new InstructionInfo("DD","LD IXh,p", "60", "", "", "+p", "2", "8") },
                        { "2E", new InstructionInfo("DD","LD IXl,n", "2E", "n", "", "", "2", "11") },
                        { "68", new InstructionInfo("DD","LD IXl,p", "68", "", "", "+p", "2", "8") },
                        { "6E", new InstructionInfo("DD","LD L,(IX+o)", "6E", "o", "", "", "3", "19") },
                        { "F9", new InstructionInfo("DD","LD SP,IX", "F9", "", "", "", "2", "10") },
                        { "B6", new InstructionInfo("DD","OR (IX+o)", "B6", "o", "", "", "3", "19") },
                        { "B0", new InstructionInfo("DD","OR IXp", "B0", "", "", "+p", "2", "8") },
                        { "E1", new InstructionInfo("DD","POP IX", "E1", "", "", "", "2", "14") },
                        { "E5", new InstructionInfo("DD","PUSH IX", "E5", "", "", "", "2", "15") },
                        { "9E", new InstructionInfo("DD","SBC A,(IX+o)", "9E", "o", "", "", "3", "19") },
                        { "98", new InstructionInfo("DD","SBC A,IXp", "98", "", "", "+p", "2", "8") },
                        { "96", new InstructionInfo("DD","SUB (IX+o)", "96", "o", "", "", "3", "19") },
                        { "90", new InstructionInfo("DD","SUB IXp", "90", "", "", "+p", "2", "8") },
                        { "AE", new InstructionInfo("DD","XOR (IX+o)", "AE", "o", "", "", "3", "19") },
                        { "A8", new InstructionInfo("DD","XOR IXp", "A8", "", "", "+p", "2", "8") }
                    }
                },
                {
                    "FD", new Dictionary<string, InstructionInfo>()
                    {
                        { "8E", new InstructionInfo("FD","ADC A,(IY+o)", "8E", "o", "", "", "3", "19") },
                        { "88", new InstructionInfo("FD","ADC A,IYq", "88", "", "", "+q", "2", "8") },
                        { "86", new InstructionInfo("FD","ADD A,(IY+o)", "86", "o", "", "", "3", "19") },
                        { "80", new InstructionInfo("FD","ADD A,IYq", "80", "", "", "+q", "2", "8") },
                        { "09", new InstructionInfo("FD","ADD IY,BC", "09", "", "", "", "2", "15") },
                        { "19", new InstructionInfo("FD","ADD IY,DE", "19", "", "", "", "2", "15") },
                        { "29", new InstructionInfo("FD","ADD IY,IY", "29", "", "", "", "2", "15") },
                        { "39", new InstructionInfo("FD","ADD IY,SP", "39", "", "", "", "2", "15") },
                        { "A6", new InstructionInfo("FD","AND (IY+o)", "A6", "o", "", "", "3", "19") },
                        { "A0", new InstructionInfo("FD","AND IYq", "A0", "", "", "+q", "2", "8") },
                        { "BE", new InstructionInfo("FD","CP (IY+o)", "BE", "o", "", "", "3", "19") },
                        { "B8", new InstructionInfo("FD","CP IYq", "B8", "", "", "+q", "2", "8") },
                        { "35", new InstructionInfo("FD","DEC (IY+o)", "35", "o", "", "", "3", "23") },
                        { "2B", new InstructionInfo("FD","DEC IY", "2B", "", "", "", "2", "10") },
                        { "05", new InstructionInfo("FD","DEC IYq", "05", "", "", "+8*q", "2", "8") },
                        { "E3", new InstructionInfo("FD","EX (SP),IY", "E3", "", "", "", "2", "23") },
                        { "34", new InstructionInfo("FD","INC (IY+o)", "34", "o", "", "", "3", "23") },
                        { "23", new InstructionInfo("FD","INC IY", "23", "", "", "", "2", "10") },
                        { "04", new InstructionInfo("FD","INC IYq", "04", "", "", "+8*q", "2", "8") },
                        { "E9", new InstructionInfo("FD","JP (IY)", "E9", "", "", "", "2", "8") },
                        { "36", new InstructionInfo("FD","LD (IY+o),n", "36", "o", "n", "", "4", "19") },
                        { "70", new InstructionInfo("FD","LD (IY+o),r", "70", "o", "", "+r", "3", "19") },
                        { "22", new InstructionInfo("FD","LD (nn),IY", "22", "nn", "nn", "", "4", "20") },
                        { "7E", new InstructionInfo("FD","LD A,(IY+o)", "7E", "o", "", "", "3", "19") },
                        { "78", new InstructionInfo("FD","LD A,IYq", "78", "", "", "+q", "2", "8") },
                        { "46", new InstructionInfo("FD","LD B,(IY+o)", "46", "o", "", "", "3", "19") },
                        { "40", new InstructionInfo("FD","LD B,IYq", "40", "", "", "+q", "2", "8") },
                        { "4E", new InstructionInfo("FD","LD C,(IY+o)", "4E", "o", "", "", "3", "19") },
                        { "48", new InstructionInfo("FD","LD C,IYq", "48", "", "", "+q", "2", "8") },
                        { "56", new InstructionInfo("FD","LD D,(IY+o)", "56", "o", "", "", "3", "19") },
                        { "50", new InstructionInfo("FD","LD D,IYq", "50", "", "", "+q", "2", "8") },
                        { "5E", new InstructionInfo("FD","LD E,(IY+o)", "5E", "o", "", "", "3", "19") },
                        { "58", new InstructionInfo("FD","LD E,IYq", "58", "", "", "+q", "2", "8") },
                        { "66", new InstructionInfo("FD","LD H,(IY+o)", "66", "o", "", "", "3", "19") },
                        { "2A", new InstructionInfo("FD","LD IY,(nn)", "2A", "nn", "nn", "", "4", "20") },
                        { "21", new InstructionInfo("FD","LD IY,nn", "21", "nn", "nn", "", "4", "14") },
                        { "26", new InstructionInfo("FD","LD IYh,n", "26", "n", "", "", "2", "11") },
                        { "60", new InstructionInfo("FD","LD IYh,q", "60", "", "", "+q", "2", "8") },
                        { "2E", new InstructionInfo("FD","LD IYl,n", "2E", "n", "", "", "2", "11") },
                        { "68", new InstructionInfo("FD","LD IYl,q", "68", "", "", "+q", "2", "8") },
                        { "6E", new InstructionInfo("FD","LD L,(IY+o)", "6E", "o", "", "", "3", "19") },
                        { "F9", new InstructionInfo("FD","LD SP,IY", "F9", "", "", "", "2", "10") },
                        { "B6", new InstructionInfo("FD","OR (IY+o)", "B6", "o", "", "", "3", "19") },
                        { "B0", new InstructionInfo("FD","OR IYq", "B0", "", "", "+q", "2", "8") },
                        { "E1", new InstructionInfo("FD","POP IY", "E1", "", "", "", "2", "14") },
                        { "E5", new InstructionInfo("FD","PUSH IY", "E5", "", "", "", "2", "15") },
                        { "9E", new InstructionInfo("FD","SBC A,(IY+o)", "9E", "o", "", "", "3", "19") },
                        { "98", new InstructionInfo("FD","SBC A,IYq", "98", "", "", "+q", "2", "8") },
                        { "96", new InstructionInfo("FD","SUB (IY+o)", "96", "o", "", "", "3", "19") },
                        { "90", new InstructionInfo("FD","SUB IYq", "90", "", "", "+q", "2", "8") },
                        { "AE", new InstructionInfo("FD","XOR (IY+o)", "AE", "o", "", "", "3", "19") },
                        { "A8", new InstructionInfo("FD","XOR IYq", "A8", "", "", "+q", "2", "8") }
                    }
                },
                {
                    "ED", new Dictionary<string, InstructionInfo>()
                    {
                        { "4A", new InstructionInfo("ED","ADC HL,BC", "4A", "", "", "", "2", "15") },
                        { "5A", new InstructionInfo("ED","ADC HL,DE", "5A", "", "", "", "2", "15") },
                        { "6A", new InstructionInfo("ED","ADC HL,HL", "6A", "", "", "", "2", "15") },
                        { "7A", new InstructionInfo("ED","ADC HL,SP", "7A", "", "", "", "2", "15") },
                        { "A9", new InstructionInfo("ED","CPD", "A9", "", "", "", "2", "16") },
                        { "B9", new InstructionInfo("ED","CPDR", "B9", "", "", "", "2", "21/16") },
                        { "A1", new InstructionInfo("ED","CPI", "A1", "", "", "", "2", "16") },
                        { "B1", new InstructionInfo("ED","CPIR", "B1", "", "", "", "2", "21/16") },
                        { "46", new InstructionInfo("ED","IM 0", "46", "", "", "", "2", "8") },
                        { "56", new InstructionInfo("ED","IM 1", "56", "", "", "", "2", "8") },
                        { "5E", new InstructionInfo("ED","IM 2", "5E", "", "", "", "2", "8") },
                        { "78", new InstructionInfo("ED","IN A,(C)", "78", "", "", "", "2", "12") },
                        { "40", new InstructionInfo("ED","IN B,(C)", "40", "", "", "", "2", "12") },
                        { "48", new InstructionInfo("ED","IN C,(C)", "48", "", "", "", "2", "12") },
                        { "50", new InstructionInfo("ED","IN D,(C)", "50", "", "", "", "2", "12") },
                        { "58", new InstructionInfo("ED","IN E,(C)", "58", "", "", "", "2", "12") },
                        { "70", new InstructionInfo("ED","IN F,(C)", "70", "", "", "", "3", "12") },
                        { "60", new InstructionInfo("ED","IN H,(C)", "60", "", "", "", "2", "12") },
                        { "68", new InstructionInfo("ED","IN L,(C)", "68", "", "", "", "2", "12") },
                        { "AA", new InstructionInfo("ED","IND", "AA", "", "", "", "2", "16") },
                        { "BA", new InstructionInfo("ED","INDR", "BA", "", "", "", "2", "21/16") },
                        { "A2", new InstructionInfo("ED","INI", "A2", "", "", "", "2", "16") },
                        { "B2", new InstructionInfo("ED","INIR", "B2", "", "", "", "2", "21/16") },
                        { "43", new InstructionInfo("ED","LD (nn),BC", "43", "nn", "nn", "", "4", "20") },
                        { "53", new InstructionInfo("ED","LD (nn),DE", "53", "nn", "nn", "", "4", "20") },
                        { "73", new InstructionInfo("ED","LD (nn),SP", "73", "nn", "nn", "", "4", "20") },
                        { "57", new InstructionInfo("ED","LD A,I", "57", "", "", "", "2", "9") },
                        { "5F", new InstructionInfo("ED","LD A,R", "5F", "", "", "", "2", "9") },
                        { "4B", new InstructionInfo("ED","LD BC,(nn)", "4B", "nn", "nn", "", "4", "20") },
                        { "5B", new InstructionInfo("ED","LD DE,(nn)", "5B", "nn", "nn", "", "4", "20") },
                        { "47", new InstructionInfo("ED","LD I,A", "47", "", "", "", "2", "9") },
                        { "4F", new InstructionInfo("ED","LD R,A", "4F", "", "", "", "2", "9") },
                        { "7B", new InstructionInfo("ED","LD SP,(nn)", "7B", "nn", "nn", "", "4", "20") },
                        { "A8", new InstructionInfo("ED","LDD", "A8", "", "", "", "2", "16") },
                        { "B8", new InstructionInfo("ED","LDDR", "B8", "", "", "", "2", "21/16") },
                        { "A0", new InstructionInfo("ED","LDI", "A0", "", "", "", "2", "16") },
                        { "B0", new InstructionInfo("ED","LDIR", "B0", "", "", "", "2", "21/16") },
                        { "44", new InstructionInfo("ED","NEG", "44", "", "", "", "2", "8") },
                        { "BB", new InstructionInfo("ED","OTDR", "BB", "", "", "", "2", "21/16") },
                        { "B3", new InstructionInfo("ED","OTIR", "B3", "", "", "", "2", "21/16") },
                        { "79", new InstructionInfo("ED","OUT (C),A", "79", "", "", "", "2", "12") },
                        { "41", new InstructionInfo("ED","OUT (C),B", "41", "", "", "", "2", "12") },
                        { "49", new InstructionInfo("ED","OUT (C),C", "49", "", "", "", "2", "12") },
                        { "51", new InstructionInfo("ED","OUT (C),D", "51", "", "", "", "2", "12") },
                        { "59", new InstructionInfo("ED","OUT (C),E", "59", "", "", "", "2", "12") },
                        { "61", new InstructionInfo("ED","OUT (C),H", "61", "", "", "", "2", "12") },
                        { "69", new InstructionInfo("ED","OUT (C),L", "69", "", "", "", "2", "12") },
                        { "AB", new InstructionInfo("ED","OUTD", "AB", "", "", "", "2", "16") },
                        { "A3", new InstructionInfo("ED","OUTI", "A3", "", "", "", "2", "16") },
                        { "4D", new InstructionInfo("ED","RETI", "4D", "", "", "", "2", "14") },
                        { "45", new InstructionInfo("ED","RETN", "45", "", "", "", "2", "14") },
                        { "6F", new InstructionInfo("ED","RLD", "6F", "", "", "", "2", "18") },
                        { "67", new InstructionInfo("ED","RRD", "67", "", "", "", "2", "18") },
                        { "42", new InstructionInfo("ED","SBC HL,BC", "42", "", "", "", "2", "15") },
                        { "52", new InstructionInfo("ED","SBC HL,DE", "52", "", "", "", "2", "15") },
                        { "62", new InstructionInfo("ED","SBC HL,HL", "62", "", "", "", "2", "15") },
                        { "72", new InstructionInfo("ED","SBC HL,SP", "72", "", "", "", "2", "15") }
                    }
                },
                {
                    "DDCB", new Dictionary<string, InstructionInfo>()
                    {
                        { "DDCB 46", new InstructionInfo("DDCB","BIT b,(IX+o)", "DDCB 46", "o", "46", "+8*b", "4", "20") },
                        { "DDCB 86", new InstructionInfo("DDCB","RES b,(IX+o)", "DDCB 86", "o", "86", "+8*b", "4", "23") },
                        { "DDCB 16", new InstructionInfo("DDCB","RL (IX+o)", "DDCB 16", "o", "16", "", "4", "23") },
                        { "DDCB 06", new InstructionInfo("DDCB","RLC (IX+o)", "DDCB 06", "o", "06", "", "4", "23") },
                        { "DDCB 1E", new InstructionInfo("DDCB","RR (IX+o)", "DDCB 1E", "o", "1E", "", "4", "23") },
                        { "DDCB 0E", new InstructionInfo("DDCB","RRC (IX+o)", "DDCB 0E", "o", "0E", "", "4", "23") },
                        { "DDCB C6", new InstructionInfo("DDCB","SET b,(IX+o)", "DDCB C6", "o", "C6", "+8*b", "4", "23") },
                        { "DDCB 26", new InstructionInfo("DDCB","SLA (IX+o)", "DDCB 26", "o", "26", "", "4", "23") },
                        { "DDCB 2E", new InstructionInfo("DDCB","SRA (IX+o)", "DDCB 2E", "o", "2E", "", "4", "23") },
                        { "DDCB 3E", new InstructionInfo("DDCB","SRL (IX+o)", "DDCB 3E", "o", "3E", "", "4", "23") }
                    }
                },
                {
                    "FDCB", new Dictionary<string, InstructionInfo>()
                    {
                        { "FDCB 46", new InstructionInfo("FDCB","BIT b,(IY+o)", "FDCB 46", "o", "46", "+8*b", "4", "20") },
                        { "FDCB 86", new InstructionInfo("FDCB","RES b,(IY+o)", "FDCB 86", "o", "86", "+8*b", "4", "23") },
                        { "FDCB 16", new InstructionInfo("FDCB","RL (IY+o)", "FDCB 16", "o", "16", "", "4", "23") },
                        { "FDCB 06", new InstructionInfo("FDCB","RLC (IY+o)", "FDCB 06", "o", "06", "", "4", "23") },
                        { "FDCB 1E", new InstructionInfo("FDCB","RR (IY+o)", "FDCB 1E", "o", "1E", "", "4", "23") },
                        { "FDCB 0E", new InstructionInfo("FDCB","RRC (IY+o)", "FDCB 0E", "o", "0E", "", "4", "23") },
                        { "FDCB C6", new InstructionInfo("FDCB","SET b,(IY+o)", "FDCB C6", "o", "C6", "+8*b", "4", "23") },
                        { "FDCB 26", new InstructionInfo("FDCB","SLA (IY+o)", "FDCB 26", "o", "26", "", "4", "23") },
                        { "FDCB 2E", new InstructionInfo("FDCB","SRA (IY+o)", "FDCB 2E", "o", "2E", "", "4", "23") },
                        { "FDCB 3E", new InstructionInfo("FDCB","SRL (IY+o)", "FDCB 3E", "o", "3E", "", "4", "23") },
                    }
                }
        };

        public static InstructionInfo Find(byte opcode, params byte[] prefix)
        {
            string prefixKey = String.Concat(prefix.Select(p => p.ToHexString()));
            opcode = Core.OpcodeFamily.RootOpcodeFrom(opcode, prefix);
            string opcodeKey = opcode.ToHexString();
            if (prefixKey.Length > 2)
            {
                opcodeKey = prefixKey + " " + opcodeKey; // special case for DD CB or FD CB
            }

            if (_opcodeSets.ContainsKey(prefixKey))
            {
                if (_opcodeSets[prefixKey].ContainsKey(opcodeKey))
                {
                    InstructionInfo info = _opcodeSets[prefixKey][opcodeKey];
                    info.Opcode = opcode; // have to set this here since we don't know the *actual* opcode until this point
                    return info;
                }
            }

            throw new Exception("Instruction not found. Are you in a data block?"); // TODO: custom exception handling
        }

        public static InstructionInfo NOP => _opcodeSets["00"]["00"];

        public InstructionPrefix Prefix { get; private set; }
        public string Mnemonic { get; private set; }
        public byte Opcode { get; private set; }
        public byte OpcodeFamily { get; private set; }
        public ArgumentType Argument1 { get; private set; }
        public ArgumentType Argument2 { get; private set; }
        public ModifierType Modifier { get; private set; }
        public byte SizeInBytes { get; private set; }
        public byte ClockCycles { get; private set; }
        public byte? ClockCyclesWhenTrue { get; private set; }

        private ArgumentType GetArgumentFromString(string description)
        {
            return description switch
            {
                "o" => ArgumentType.Displacement,
                "n" => ArgumentType.Immediate,
                "nn" => ArgumentType.ImmediateWord,
                _ => ArgumentType.None
            };
        }

        private ModifierType GetModifierFromString(string description)
        {
            return description switch
            {
                "+r" => ModifierType.Register,
                "+8*b" => ModifierType.Bit,
                "+8*b+r" => ModifierType.Bit | ModifierType.Register,
                "+p" => ModifierType.IndexRegister,
                "+q" => ModifierType.IndexRegister,
                "+8*p" => ModifierType.IndexRegisterHigh,
                "+8*q" => ModifierType.IndexRegisterHigh,
                _ => ModifierType.None
            };
        }

        private InstructionInfo(string prefix, string name, string opcodeFamily, string argument1, string argument2, string modifier, string size, string timing)
        {
            Prefix = prefix == "00" ? InstructionPrefix.Unprefixed : Enum.Parse<InstructionPrefix>(prefix);
            if (opcodeFamily.StartsWith("DDCB") || opcodeFamily.StartsWith("FDCB")) opcodeFamily = opcodeFamily.Substring(5);
            
            Mnemonic = name;
            OpcodeFamily = byte.Parse(opcodeFamily, System.Globalization.NumberStyles.HexNumber);
            Argument1 = GetArgumentFromString(argument1);
            Argument2 = GetArgumentFromString(argument2);
            Modifier = GetModifierFromString(modifier);
            SizeInBytes = byte.Parse(size, System.Globalization.NumberStyles.HexNumber);

            string[] timingParts = timing.Split('/');
            ClockCycles = byte.Parse(timingParts[0]);
            if (timingParts.Length > 1)
            {
                ClockCyclesWhenTrue = byte.Parse(timingParts[1]);
            }
        }
    }

    public enum ArgumentType
    {
        None,
        Displacement,
        Immediate,
        ImmediateWord
    }

    [Flags]
    public enum ModifierType
    {
        None,
        Register,
        Bit,
        IndexRegister,
        IndexRegisterHigh
    }
}

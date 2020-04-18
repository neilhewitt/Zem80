using System;
using System.Collections.Generic;
using System.Text;

namespace ZexallCSharp
{
    public class ZexdocTests : IZexTestSource
    {
		public byte[][] CRCTable { get; } = 
		{
			new byte[4] { 0x00,0x00,0x00,0x00 },
			new byte[4] { 0x77,0x07,0x30,0x96 },
			new byte[4] { 0xee,0x0e,0x61,0x2c },
			new byte[4] { 0x99,0x09,0x51,0xba },
			new byte[4] { 0x07,0x6d,0xc4,0x19 },
			new byte[4] { 0x70,0x6a,0xf4,0x8f },
			new byte[4] { 0xe9,0x63,0xa5,0x35 },
			new byte[4] { 0x9e,0x64,0x95,0xa3 },
			new byte[4] { 0x0e,0xdb,0x88,0x32 },
			new byte[4] { 0x79,0xdc,0xb8,0xa4 },
			new byte[4] { 0xe0,0xd5,0xe9,0x1e },
			new byte[4] { 0x97,0xd2,0xd9,0x88 },
			new byte[4] { 0x09,0xb6,0x4c,0x2b },
			new byte[4] { 0x7e,0xb1,0x7c,0xbd },
			new byte[4] { 0xe7,0xb8,0x2d,0x07 },
			new byte[4] { 0x90,0xbf,0x1d,0x91 },
			new byte[4] { 0x1d,0xb7,0x10,0x64 },
			new byte[4] { 0x6a,0xb0,0x20,0xf2 },
			new byte[4] { 0xf3,0xb9,0x71,0x48 },
			new byte[4] { 0x84,0xbe,0x41,0xde },
			new byte[4] { 0x1a,0xda,0xd4,0x7d },
			new byte[4] { 0x6d,0xdd,0xe4,0xeb },
			new byte[4] { 0xf4,0xd4,0xb5,0x51 },
			new byte[4] { 0x83,0xd3,0x85,0xc7 },
			new byte[4] { 0x13,0x6c,0x98,0x56 },
			new byte[4] { 0x64,0x6b,0xa8,0xc0 },
			new byte[4] { 0xfd,0x62,0xf9,0x7a },
			new byte[4] { 0x8a,0x65,0xc9,0xec },
			new byte[4] { 0x14,0x01,0x5c,0x4f },
			new byte[4] { 0x63,0x06,0x6c,0xd9 },
			new byte[4] { 0xfa,0x0f,0x3d,0x63 },
			new byte[4] { 0x8d,0x08,0x0d,0xf5 },
			new byte[4] { 0x3b,0x6e,0x20,0xc8 },
			new byte[4] { 0x4c,0x69,0x10,0x5e },
			new byte[4] { 0xd5,0x60,0x41,0xe4 },
			new byte[4] { 0xa2,0x67,0x71,0x72 },
			new byte[4] { 0x3c,0x03,0xe4,0xd1 },
			new byte[4] { 0x4b,0x04,0xd4,0x47 },
			new byte[4] { 0xd2,0x0d,0x85,0xfd },
			new byte[4] { 0xa5,0x0a,0xb5,0x6b },
			new byte[4] { 0x35,0xb5,0xa8,0xfa },
			new byte[4] { 0x42,0xb2,0x98,0x6c },
			new byte[4] { 0xdb,0xbb,0xc9,0xd6 },
			new byte[4] { 0xac,0xbc,0xf9,0x40 },
			new byte[4] { 0x32,0xd8,0x6c,0xe3 },
			new byte[4] { 0x45,0xdf,0x5c,0x75 },
			new byte[4] { 0xdc,0xd6,0x0d,0xcf },
			new byte[4] { 0xab,0xd1,0x3d,0x59 },
			new byte[4] { 0x26,0xd9,0x30,0xac },
			new byte[4] { 0x51,0xde,0x00,0x3a },
			new byte[4] { 0xc8,0xd7,0x51,0x80 },
			new byte[4] { 0xbf,0xd0,0x61,0x16 },
			new byte[4] { 0x21,0xb4,0xf4,0xb5 },
			new byte[4] { 0x56,0xb3,0xc4,0x23 },
			new byte[4] { 0xcf,0xba,0x95,0x99 },
			new byte[4] { 0xb8,0xbd,0xa5,0x0f },
			new byte[4] { 0x28,0x02,0xb8,0x9e },
			new byte[4] { 0x5f,0x05,0x88,0x08 },
			new byte[4] { 0xc6,0x0c,0xd9,0xb2 },
			new byte[4] { 0xb1,0x0b,0xe9,0x24 },
			new byte[4] { 0x2f,0x6f,0x7c,0x87 },
			new byte[4] { 0x58,0x68,0x4c,0x11 },
			new byte[4] { 0xc1,0x61,0x1d,0xab },
			new byte[4] { 0xb6,0x66,0x2d,0x3d },
			new byte[4] { 0x76,0xdc,0x41,0x90 },
			new byte[4] { 0x01,0xdb,0x71,0x06 },
			new byte[4] { 0x98,0xd2,0x20,0xbc },
			new byte[4] { 0xef,0xd5,0x10,0x2a },
			new byte[4] { 0x71,0xb1,0x85,0x89 },
			new byte[4] { 0x06,0xb6,0xb5,0x1f },
			new byte[4] { 0x9f,0xbf,0xe4,0xa5 },
			new byte[4] { 0xe8,0xb8,0xd4,0x33 },
			new byte[4] { 0x78,0x07,0xc9,0xa2 },
			new byte[4] { 0x0f,0x00,0xf9,0x34 },
			new byte[4] { 0x96,0x09,0xa8,0x8e },
			new byte[4] { 0xe1,0x0e,0x98,0x18 },
			new byte[4] { 0x7f,0x6a,0x0d,0xbb },
			new byte[4] { 0x08,0x6d,0x3d,0x2d },
			new byte[4] { 0x91,0x64,0x6c,0x97 },
			new byte[4] { 0xe6,0x63,0x5c,0x01 },
			new byte[4] { 0x6b,0x6b,0x51,0xf4 },
			new byte[4] { 0x1c,0x6c,0x61,0x62 },
			new byte[4] { 0x85,0x65,0x30,0xd8 },
			new byte[4] { 0xf2,0x62,0x00,0x4e },
			new byte[4] { 0x6c,0x06,0x95,0xed },
			new byte[4] { 0x1b,0x01,0xa5,0x7b },
			new byte[4] { 0x82,0x08,0xf4,0xc1 },
			new byte[4] { 0xf5,0x0f,0xc4,0x57 },
			new byte[4] { 0x65,0xb0,0xd9,0xc6 },
			new byte[4] { 0x12,0xb7,0xe9,0x50 },
			new byte[4] { 0x8b,0xbe,0xb8,0xea },
			new byte[4] { 0xfc,0xb9,0x88,0x7c },
			new byte[4] { 0x62,0xdd,0x1d,0xdf },
			new byte[4] { 0x15,0xda,0x2d,0x49 },
			new byte[4] { 0x8c,0xd3,0x7c,0xf3 },
			new byte[4] { 0xfb,0xd4,0x4c,0x65 },
			new byte[4] { 0x4d,0xb2,0x61,0x58 },
			new byte[4] { 0x3a,0xb5,0x51,0xce },
			new byte[4] { 0xa3,0xbc,0x00,0x74 },
			new byte[4] { 0xd4,0xbb,0x30,0xe2 },
			new byte[4] { 0x4a,0xdf,0xa5,0x41 },
			new byte[4] { 0x3d,0xd8,0x95,0xd7 },
			new byte[4] { 0xa4,0xd1,0xc4,0x6d },
			new byte[4] { 0xd3,0xd6,0xf4,0xfb },
			new byte[4] { 0x43,0x69,0xe9,0x6a },
			new byte[4] { 0x34,0x6e,0xd9,0xfc },
			new byte[4] { 0xad,0x67,0x88,0x46 },
			new byte[4] { 0xda,0x60,0xb8,0xd0 },
			new byte[4] { 0x44,0x04,0x2d,0x73 },
			new byte[4] { 0x33,0x03,0x1d,0xe5 },
			new byte[4] { 0xaa,0x0a,0x4c,0x5f },
			new byte[4] { 0xdd,0x0d,0x7c,0xc9 },
			new byte[4] { 0x50,0x05,0x71,0x3c },
			new byte[4] { 0x27,0x02,0x41,0xaa },
			new byte[4] { 0xbe,0x0b,0x10,0x10 },
			new byte[4] { 0xc9,0x0c,0x20,0x86 },
			new byte[4] { 0x57,0x68,0xb5,0x25 },
			new byte[4] { 0x20,0x6f,0x85,0xb3 },
			new byte[4] { 0xb9,0x66,0xd4,0x09 },
			new byte[4] { 0xce,0x61,0xe4,0x9f },
			new byte[4] { 0x5e,0xde,0xf9,0x0e },
			new byte[4] { 0x29,0xd9,0xc9,0x98 },
			new byte[4] { 0xb0,0xd0,0x98,0x22 },
			new byte[4] { 0xc7,0xd7,0xa8,0xb4 },
			new byte[4] { 0x59,0xb3,0x3d,0x17 },
			new byte[4] { 0x2e,0xb4,0x0d,0x81 },
			new byte[4] { 0xb7,0xbd,0x5c,0x3b },
			new byte[4] { 0xc0,0xba,0x6c,0xad },
			new byte[4] { 0xed,0xb8,0x83,0x20 },
			new byte[4] { 0x9a,0xbf,0xb3,0xb6 },
			new byte[4] { 0x03,0xb6,0xe2,0x0c },
			new byte[4] { 0x74,0xb1,0xd2,0x9a },
			new byte[4] { 0xea,0xd5,0x47,0x39 },
			new byte[4] { 0x9d,0xd2,0x77,0xaf },
			new byte[4] { 0x04,0xdb,0x26,0x15 },
			new byte[4] { 0x73,0xdc,0x16,0x83 },
			new byte[4] { 0xe3,0x63,0x0b,0x12 },
			new byte[4] { 0x94,0x64,0x3b,0x84 },
			new byte[4] { 0x0d,0x6d,0x6a,0x3e },
			new byte[4] { 0x7a,0x6a,0x5a,0xa8 },
			new byte[4] { 0xe4,0x0e,0xcf,0x0b },
			new byte[4] { 0x93,0x09,0xff,0x9d },
			new byte[4] { 0x0a,0x00,0xae,0x27 },
			new byte[4] { 0x7d,0x07,0x9e,0xb1 },
			new byte[4] { 0xf0,0x0f,0x93,0x44 },
			new byte[4] { 0x87,0x08,0xa3,0xd2 },
			new byte[4] { 0x1e,0x01,0xf2,0x68 },
			new byte[4] { 0x69,0x06,0xc2,0xfe },
			new byte[4] { 0xf7,0x62,0x57,0x5d },
			new byte[4] { 0x80,0x65,0x67,0xcb },
			new byte[4] { 0x19,0x6c,0x36,0x71 },
			new byte[4] { 0x6e,0x6b,0x06,0xe7 },
			new byte[4] { 0xfe,0xd4,0x1b,0x76 },
			new byte[4] { 0x89,0xd3,0x2b,0xe0 },
			new byte[4] { 0x10,0xda,0x7a,0x5a },
			new byte[4] { 0x67,0xdd,0x4a,0xcc },
			new byte[4] { 0xf9,0xb9,0xdf,0x6f },
			new byte[4] { 0x8e,0xbe,0xef,0xf9 },
			new byte[4] { 0x17,0xb7,0xbe,0x43 },
			new byte[4] { 0x60,0xb0,0x8e,0xd5 },
			new byte[4] { 0xd6,0xd6,0xa3,0xe8 },
			new byte[4] { 0xa1,0xd1,0x93,0x7e },
			new byte[4] { 0x38,0xd8,0xc2,0xc4 },
			new byte[4] { 0x4f,0xdf,0xf2,0x52 },
			new byte[4] { 0xd1,0xbb,0x67,0xf1 },
			new byte[4] { 0xa6,0xbc,0x57,0x67 },
			new byte[4] { 0x3f,0xb5,0x06,0xdd },
			new byte[4] { 0x48,0xb2,0x36,0x4b },
			new byte[4] { 0xd8,0x0d,0x2b,0xda },
			new byte[4] { 0xaf,0x0a,0x1b,0x4c },
			new byte[4] { 0x36,0x03,0x4a,0xf6 },
			new byte[4] { 0x41,0x04,0x7a,0x60 },
			new byte[4] { 0xdf,0x60,0xef,0xc3 },
			new byte[4] { 0xa8,0x67,0xdf,0x55 },
			new byte[4] { 0x31,0x6e,0x8e,0xef },
			new byte[4] { 0x46,0x69,0xbe,0x79 },
			new byte[4] { 0xcb,0x61,0xb3,0x8c },
			new byte[4] { 0xbc,0x66,0x83,0x1a },
			new byte[4] { 0x25,0x6f,0xd2,0xa0 },
			new byte[4] { 0x52,0x68,0xe2,0x36 },
			new byte[4] { 0xcc,0x0c,0x77,0x95 },
			new byte[4] { 0xbb,0x0b,0x47,0x03 },
			new byte[4] { 0x22,0x02,0x16,0xb9 },
			new byte[4] { 0x55,0x05,0x26,0x2f },
			new byte[4] { 0xc5,0xba,0x3b,0xbe },
			new byte[4] { 0xb2,0xbd,0x0b,0x28 },
			new byte[4] { 0x2b,0xb4,0x5a,0x92 },
			new byte[4] { 0x5c,0xb3,0x6a,0x04 },
			new byte[4] { 0xc2,0xd7,0xff,0xa7 },
			new byte[4] { 0xb5,0xd0,0xcf,0x31 },
			new byte[4] { 0x2c,0xd9,0x9e,0x8b },
			new byte[4] { 0x5b,0xde,0xae,0x1d },
			new byte[4] { 0x9b,0x64,0xc2,0xb0 },
			new byte[4] { 0xec,0x63,0xf2,0x26 },
			new byte[4] { 0x75,0x6a,0xa3,0x9c },
			new byte[4] { 0x02,0x6d,0x93,0x0a },
			new byte[4] { 0x9c,0x09,0x06,0xa9 },
			new byte[4] { 0xeb,0x0e,0x36,0x3f },
			new byte[4] { 0x72,0x07,0x67,0x85 },
			new byte[4] { 0x05,0x00,0x57,0x13 },
			new byte[4] { 0x95,0xbf,0x4a,0x82 },
			new byte[4] { 0xe2,0xb8,0x7a,0x14 },
			new byte[4] { 0x7b,0xb1,0x2b,0xae },
			new byte[4] { 0x0c,0xb6,0x1b,0x38 },
			new byte[4] { 0x92,0xd2,0x8e,0x9b },
			new byte[4] { 0xe5,0xd5,0xbe,0x0d },
			new byte[4] { 0x7c,0xdc,0xef,0xb7 },
			new byte[4] { 0x0b,0xdb,0xdf,0x21 },
			new byte[4] { 0x86,0xd3,0xd2,0xd4 },
			new byte[4] { 0xf1,0xd4,0xe2,0x42 },
			new byte[4] { 0x68,0xdd,0xb3,0xf8 },
			new byte[4] { 0x1f,0xda,0x83,0x6e },
			new byte[4] { 0x81,0xbe,0x16,0xcd },
			new byte[4] { 0xf6,0xb9,0x26,0x5b },
			new byte[4] { 0x6f,0xb0,0x77,0xe1 },
			new byte[4] { 0x18,0xb7,0x47,0x77 },
			new byte[4] { 0x88,0x08,0x5a,0xe6 },
			new byte[4] { 0xff,0x0f,0x6a,0x70 },
			new byte[4] { 0x66,0x06,0x3b,0xca },
			new byte[4] { 0x11,0x01,0x0b,0x5c },
			new byte[4] { 0x8f,0x65,0x9e,0xff },
			new byte[4] { 0xf8,0x62,0xae,0x69 },
			new byte[4] { 0x61,0x6b,0xff,0xd3 },
			new byte[4] { 0x16,0x6c,0xcf,0x45 },
			new byte[4] { 0xa0,0x0a,0xe2,0x78 },
			new byte[4] { 0xd7,0x0d,0xd2,0xee },
			new byte[4] { 0x4e,0x04,0x83,0x54 },
			new byte[4] { 0x39,0x03,0xb3,0xc2 },
			new byte[4] { 0xa7,0x67,0x26,0x61 },
			new byte[4] { 0xd0,0x60,0x16,0xf7 },
			new byte[4] { 0x49,0x69,0x47,0x4d },
			new byte[4] { 0x3e,0x6e,0x77,0xdb },
			new byte[4] { 0xae,0xd1,0x6a,0x4a },
			new byte[4] { 0xd9,0xd6,0x5a,0xdc },
			new byte[4] { 0x40,0xdf,0x0b,0x66 },
			new byte[4] { 0x37,0xd8,0x3b,0xf0 },
			new byte[4] { 0xa9,0xbc,0xae,0x53 },
			new byte[4] { 0xde,0xbb,0x9e,0xc5 },
			new byte[4] { 0x47,0xb2,0xcf,0x7f },
			new byte[4] { 0x30,0xb5,0xff,0xe9 },
			new byte[4] { 0xbd,0xbd,0xf2,0x1c },
			new byte[4] { 0xca,0xba,0xc2,0x8a },
			new byte[4] { 0x53,0xb3,0x93,0x30 },
			new byte[4] { 0x24,0xb4,0xa3,0xa6 },
			new byte[4] { 0xba,0xd0,0x36,0x05 },
			new byte[4] { 0xcd,0xd7,0x06,0x93 },
			new byte[4] { 0x54,0xde,0x57,0x29 },
			new byte[4] { 0x23,0xd9,0x67,0xbf },
			new byte[4] { 0xb3,0x66,0x7a,0x2e },
			new byte[4] { 0xc4,0x61,0x4a,0xb8 },
			new byte[4] { 0x5d,0x68,0x1b,0x02 },
			new byte[4] { 0x2a,0x6f,0x2b,0x94 },
			new byte[4] { 0xb4,0x0b,0xbe,0x37 },
			new byte[4] { 0xc3,0x0c,0x8e,0xa1 },
			new byte[4] { 0x5a,0x05,0xdf,0x1b },
			new byte[4] { 0x2d,0x02,0xef,0x8d }
		};

		public IDictionary<string, TestDescriptor> Tests { get; } = new Dictionary<string, TestDescriptor>()
		{
			{
				"adc16",
				new TestDescriptor()
				{
					Name = "adc16",
					Message = "<ADC,SBC> HL,<BC,DE,HL,SP>....",
					Cycles = 38912,
					Mask = 0xC7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x42, (byte)0x00, (byte)0x00),
						MemOp = 0x832C,
						IY = 0x4F88,
						IX = 0xF22B,
						HL = 0xB339,
						DE = 0x7E1F,
						BC = 0x1563,
						F = 0xD3,
						A = 0x89,
						SP = 0x465E
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x38, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xF821,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xFFFF,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0xFFFF
					},
					CRC = (0xF8,0xB4,0xEA,0xA9)
				}
			},
			{
				"add16",
				new TestDescriptor()
				{
					Name = "add16",
					Message = "ADD HL,<BC,DE,HL,SP>..........",
					Cycles = 19456,
					Mask = 0xC7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x09, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xC4A5,
						IY = 0xC4C7,
						IX = 0xD226,
						HL = 0xA050,
						DE = 0x58EA,
						BC = 0x8566,
						F = 0xC6,
						A = 0xDE,
						SP = 0x9BC9
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x30, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xF821,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xFFFF,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0xFFFF
					},
					CRC = (0x89,0xFD,0xB6,0x35)
				}
			},
			{
				"add16x",
				new TestDescriptor()
				{
					Name = "add16x",
					Message = "ADD IX,<BC,DE,IX,SP>..........",
					Cycles = 19456,
					Mask = 0xC7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x09, (byte)0x00, (byte)0x00),
						MemOp = 0xDDAC,
						IY = 0xC294,
						IX = 0x635B,
						HL = 0x33D3,
						DE = 0x6A76,
						BC = 0xFA20,
						F = 0x94,
						A = 0x68,
						SP = 0x36F5
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x30, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0xF821,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0xFFFF,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0xFFFF
					},
					CRC = (0xC1,0x33,0x79,0x0B)
				}
			},
			{
				"add16y",
				new TestDescriptor()
				{
					Name = "add16y",
					Message = "ADD IY,<BC,DE,IY,SP>..........",
					Cycles = 19456,
					Mask = 0xC7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xFD, (byte)0x09, (byte)0x00, (byte)0x00),
						MemOp = 0xC7C2,
						IY = 0xF407,
						IX = 0x51C1,
						HL = 0x3E96,
						DE = 0x0BF4,
						BC = 0x510F,
						F = 0x92,
						A = 0x1E,
						SP = 0x71EA
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x30, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0xF821,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0xFFFF,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0xFFFF
					},
					CRC = (0xE8,0x81,0x7B,0x9E)
				}
			},
			{
				"alu8i",
				new TestDescriptor()
				{
					Name = "alu8i",
					Message = "ALUOP A,NN....................",
					Cycles = 28672,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xC6, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x9140,
						IY = 0x7E3C,
						IX = 0x7A67,
						HL = 0xDF6D,
						DE = 0x5B61,
						BC = 0x0B29,
						F = 0x10,
						A = 0x66,
						SP = 0x85B2
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x38, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0xFF, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x48,0x79,0x93,0x60)
				}
			},
			{
				"alu8r",
				new TestDescriptor()
				{
					Name = "alu8r",
					Message = "ALUOP A,<B,C,D,E,H,L,(HL),A>..",
					Cycles = 753664,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x80, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xC53E,
						IY = 0x573A,
						IX = 0x4C4D,
						HL = 0x0014,
						DE = 0xE309,
						BC = 0xA666,
						F = 0xD0,
						A = 0x3B,
						SP = 0xADBB
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x3F, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xFE,0x43,0xB0,0x16)
				}
			},
			{
				"alu8rx",
				new TestDescriptor()
				{
					Name = "alu8rx",
					Message = "ALUOP A,<IXH,IXL,IYH,IYL>.....",
					Cycles = 376832,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x84, (byte)0x00, (byte)0x00),
						MemOp = 0xD6F7,
						IY = 0xC76E,
						IX = 0xACCF,
						HL = 0x2847,
						DE = 0x22DD,
						BC = 0xC035,
						F = 0xC5,
						A = 0x38,
						SP = 0x234B
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x39, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA4,0x02,0x6D,0x5A)
				}
			},
			{
				"alu8x",
				new TestDescriptor()
				{
					Name = "alu8x",
					Message = "ALUOP A,(<IX,IY>+1)...........",
					Cycles = 229376,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x86, (byte)0x01, (byte)0x00),
						MemOp = 0x90B7,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x32FD,
						DE = 0x406E,
						BC = 0xC1DC,
						F = 0x45,
						A = 0x6E,
						SP = 0xE5FA
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x38, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xE8,0x49,0x67,0x6E)
				}
			},
			{
				"bitx",
				new TestDescriptor()
				{
					Name = "bitx",
					Message = "BIT N,(<IX,IY>+1).............",
					Cycles = 2048,
					Mask = 0x53,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0xCB, (byte)0x01, (byte)0x46),
						MemOp = 0x2075,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x3CFC,
						DE = 0xA79A,
						BC = 0x3D74,
						F = 0x51,
						A = 0x27,
						SP = 0xCA14
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x38),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x53,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA8,0xEE,0x08,0x67)
				}
			},
			{
				"bitz80",
				new TestDescriptor()
				{
					Name = "bitz80",
					Message = "BIT N,<B,C,D,E,H,L,(HL),A>....",
					Cycles = 49152,
					Mask = 0x53,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xCB, (byte)0x40, (byte)0x00, (byte)0x00),
						MemOp = 0x3EF1,
						IY = 0x9DFC,
						IX = 0x7ACC,
						HL = 0x0014,
						DE = 0xBE61,
						BC = 0x7A86,
						F = 0x50,
						A = 0x24,
						SP = 0x1998
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x3F, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x53,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x7B,0x55,0xE6,0xC8)
				}
			},
			{
				"cpd1",
				new TestDescriptor()
				{
					Name = "cpd1",
					Message = "CPD<R>........................",
					Cycles = 6144,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA9, (byte)0x00, (byte)0x00),
						MemOp = 0xC7B6,
						IY = 0x72B4,
						IX = 0x18F6,
						HL = 0x002B,
						DE = 0x8DBD,
						BC = 0x0001,
						F = 0xC0,
						A = 0x30,
						SP = 0x94A3
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0010,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA8,0x7E,0x6C,0xFA)
				}
			},
			{
				"cpi1",
				new TestDescriptor()
				{
					Name = "cpi1",
					Message = "CPI<R>........................",
					Cycles = 6144,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA1, (byte)0x00, (byte)0x00),
						MemOp = 0x4D48,
						IY = 0xAF4A,
						IX = 0x906B,
						HL = 0x0014,
						DE = 0x4E71,
						BC = 0x0001,
						F = 0x93,
						A = 0x6A,
						SP = 0x907C
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0010,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x06,0xDE,0xB3,0x56)
				}
			},
			{
				"daaop",
				new TestDescriptor()
				{
					Name = "daaop",
					Message = "<DAA,CPL,SCF,CCF>.............",
					Cycles = 0,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x27, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x2141,
						IY = 0x09FA,
						IX = 0x1D60,
						HL = 0xA559,
						DE = 0x8D5B,
						BC = 0x9079,
						F = 0x04,
						A = 0x8E,
						SP = 0x299D
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x18, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x9B,0x4B,0xA6,0x75)
				}
			},
			{
				"inca",
				new TestDescriptor()
				{
					Name = "inca",
					Message = "<INC,DEC> A...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x3C, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x4ADF,
						IY = 0xD5D8,
						IX = 0xE598,
						HL = 0x8A2B,
						DE = 0xA7B0,
						BC = 0x431B,
						F = 0x44,
						A = 0x5A,
						SP = 0xD030
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xD1,0x88,0x15,0xA4)
				}
			},
			{
				"incb",
				new TestDescriptor()
				{
					Name = "incb",
					Message = "<INC,DEC> B...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x04, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xD623,
						IY = 0x432D,
						IX = 0x7A61,
						HL = 0x8180,
						DE = 0x5A86,
						BC = 0x1E85,
						F = 0x86,
						A = 0x58,
						SP = 0x9BBB
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0xFF00,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x5F,0x68,0x22,0x64)
				}
			},
			{
				"incbc",
				new TestDescriptor()
				{
					Name = "incbc",
					Message = "<INC,DEC> BC..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x03, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xCD97,
						IY = 0x44AB,
						IX = 0x8DC9,
						HL = 0xE3E3,
						DE = 0x11CC,
						BC = 0xE8A4,
						F = 0x02,
						A = 0x49,
						SP = 0x2A4D
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x08, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0xF821,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xD2,0xAE,0x3B,0xEC)
				}
			},
			{
				"incc",
				new TestDescriptor()
				{
					Name = "incc",
					Message = "<INC,DEC> C...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x0C, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xD789,
						IY = 0x0935,
						IX = 0x055B,
						HL = 0x9F85,
						DE = 0x8B27,
						BC = 0xD208,
						F = 0x95,
						A = 0x05,
						SP = 0x0660
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x00FF,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xC2,0x84,0x55,0x4C)
				}
			},
			{
				"incd",
				new TestDescriptor()
				{
					Name = "incd",
					Message = "<INC,DEC> D...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x14, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xA0EA,
						IY = 0x5FBA,
						IX = 0x65FB,
						HL = 0x981C,
						DE = 0x38CC,
						BC = 0xDEBC,
						F = 0x43,
						A = 0x5C,
						SP = 0x03BD
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFF00,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x45,0x23,0xDE,0x10)
				}
			},
			{
				"incde",
				new TestDescriptor()
				{
					Name = "incde",
					Message = "<INC,DEC> DE..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x13, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x342E,
						IY = 0x131D,
						IX = 0x28C9,
						HL = 0x0ACA,
						DE = 0x9967,
						BC = 0x3A2E,
						F = 0x92,
						A = 0xF6,
						SP = 0x9D54
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x08, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xF821,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xAE,0xC6,0xD4,0x2C)
				}
			},
			{
				"ince",
				new TestDescriptor()
				{
					Name = "ince",
					Message = "<INC,DEC> E...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x1C, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x602F,
						IY = 0x4C0D,
						IX = 0x2402,
						HL = 0xE2F5,
						DE = 0xA0F4,
						BC = 0xA10A,
						F = 0x13,
						A = 0x32,
						SP = 0x5925
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x00FF,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xE1,0x75,0xAF,0xCC)
				}
			},
			{
				"inch",
				new TestDescriptor()
				{
					Name = "inch",
					Message = "<INC,DEC> H...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x24, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x1506,
						IY = 0xF2EB,
						IX = 0xE8DD,
						HL = 0x262B,
						DE = 0x11A6,
						BC = 0xBC1A,
						F = 0x17,
						A = 0x06,
						SP = 0x2818
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xFF00,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x1C,0xED,0x84,0x7D)
				}
			},
			{
				"inchl",
				new TestDescriptor()
				{
					Name = "inchl",
					Message = "<INC,DEC> HL..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x23, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xC3F4,
						IY = 0x07A5,
						IX = 0x1B6D,
						HL = 0x4F04,
						DE = 0xE2C2,
						BC = 0x822A,
						F = 0x57,
						A = 0xE0,
						SP = 0xC3E1
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x08, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xF821,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xFC,0x0D,0x6D,0x4A)
				}
			},
			{
				"incix",
				new TestDescriptor()
				{
					Name = "incix",
					Message = "<INC,DEC> IX..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x23, (byte)0x00, (byte)0x00),
						MemOp = 0xBC3C,
						IY = 0x0D9B,
						IX = 0xE081,
						HL = 0xADFD,
						DE = 0x9A7F,
						BC = 0x96E5,
						F = 0x13,
						A = 0x85,
						SP = 0x0BE2
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x08, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0xF821,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA5,0x4D,0xBE,0x31)
				}
			},
			{
				"inciy",
				new TestDescriptor()
				{
					Name = "inciy",
					Message = "<INC,DEC> IY..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xFD, (byte)0x23, (byte)0x00, (byte)0x00),
						MemOp = 0x9402,
						IY = 0x637A,
						IX = 0x3182,
						HL = 0xC65A,
						DE = 0xB2E9,
						BC = 0xABB4,
						F = 0x16,
						A = 0xF2,
						SP = 0x6D05
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x08, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0xF821,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x50,0x5D,0x51,0xA3)
				}
			},
			{
				"incl",
				new TestDescriptor()
				{
					Name = "incl",
					Message = "<INC,DEC> L...................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x2C, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x8031,
						IY = 0xA520,
						IX = 0x4356,
						HL = 0xB409,
						DE = 0xF4C1,
						BC = 0xDFA2,
						F = 0xD1,
						A = 0x3C,
						SP = 0x3EA2
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x00FF,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x56,0xCD,0x06,0xF3)
				}
			},
			{
				"incm",
				new TestDescriptor()
				{
					Name = "incm",
					Message = "<INC,DEC> (HL)................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x34, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xB856,
						IY = 0x0C7C,
						IX = 0xE53E,
						HL = 0x0014,
						DE = 0x877E,
						BC = 0xDA58,
						F = 0x15,
						A = 0x5C,
						SP = 0x1F37
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xB8,0x3A,0xDC,0xEF)
				}
			},
			{
				"incsp",
				new TestDescriptor()
				{
					Name = "incsp",
					Message = "<INC,DEC> SP..................",
					Cycles = 1536,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x33, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x346F,
						IY = 0xD482,
						IX = 0xD169,
						HL = 0xDEB6,
						DE = 0xA494,
						BC = 0xF476,
						F = 0x53,
						A = 0x02,
						SP = 0x855B
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x08, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0xF821
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x5D,0xAC,0xD5,0x27)
				}
			},
			{
				"incx",
				new TestDescriptor()
				{
					Name = "incx",
					Message = "<INC,DEC> (<IX,IY>+1).........",
					Cycles = 6144,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x34, (byte)0x01, (byte)0x00),
						MemOp = 0xFA6E,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x2C28,
						DE = 0x8894,
						BC = 0x5057,
						F = 0x16,
						A = 0x33,
						SP = 0x286F
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x20,0x58,0x14,0x70)
				}
			},
			{
				"incxh",
				new TestDescriptor()
				{
					Name = "incxh",
					Message = "<INC,DEC> IXH.................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x24, (byte)0x00, (byte)0x00),
						MemOp = 0xB838,
						IY = 0x316C,
						IX = 0xC6D4,
						HL = 0x3E01,
						DE = 0x8358,
						BC = 0x15B4,
						F = 0x81,
						A = 0xDE,
						SP = 0x4259
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0xFF00,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x6F,0x46,0x36,0x62)
				}
			},
			{
				"incxl",
				new TestDescriptor()
				{
					Name = "incxl",
					Message = "<INC,DEC> IXL.................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x2C, (byte)0x00, (byte)0x00),
						MemOp = 0x4D14,
						IY = 0x7460,
						IX = 0x76D4,
						HL = 0x06E7,
						DE = 0x32A2,
						BC = 0x213C,
						F = 0xD6,
						A = 0xD7,
						SP = 0x99A5
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x00FF,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x02,0x7B,0xEF,0x2C)
				}
			},
			{
				"incyh",
				new TestDescriptor()
				{
					Name = "incyh",
					Message = "<INC,DEC> IYH.................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x24, (byte)0x00, (byte)0x00),
						MemOp = 0x2836,
						IY = 0x9F6F,
						IX = 0x9116,
						HL = 0x61B9,
						DE = 0x82CB,
						BC = 0xE219,
						F = 0x92,
						A = 0x73,
						SP = 0xA98C
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0xFF00,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x2D,0x96,0x6C,0xF3)
				}
			},
			{
				"incyl",
				new TestDescriptor()
				{
					Name = "incyl",
					Message = "<INC,DEC> IYL.................",
					Cycles = 3072,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x2C, (byte)0x00, (byte)0x00),
						MemOp = 0xD7C6,
						IY = 0x62D5,
						IX = 0xA09E,
						HL = 0x7039,
						DE = 0x3E7E,
						BC = 0x9F12,
						F = 0x90,
						A = 0xD9,
						SP = 0x220F
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xFB,0xCB,0xBA,0x95)
				}
			},
			{
				"ld161",
				new TestDescriptor()
				{
					Name = "ld161",
					Message = "LD <BC,DE>,(NNNN).............",
					Cycles = 32,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x4B, (byte)0x14, (byte)0x14),
						MemOp = 0xF9A8,
						IY = 0xF559,
						IX = 0x93A4,
						HL = 0xF5ED,
						DE = 0x6F96,
						BC = 0xD968,
						F = 0x86,
						A = 0xE6,
						SP = 0x4BD8
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x4D,0x45,0xA9,0xAC)
				}
			},
			{
				"ld162",
				new TestDescriptor()
				{
					Name = "ld162",
					Message = "LD HL,(NNNN)..................",
					Cycles = 16,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x2A, (byte)0x14, (byte)0x14, (byte)0x00),
						MemOp = 0x9863,
						IY = 0x7830,
						IX = 0x2077,
						HL = 0xB1FE,
						DE = 0xB9FA,
						BC = 0xABB8,
						F = 0x04,
						A = 0x06,
						SP = 0x6015
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x5F,0x97,0x24,0x87)
				}
			},
			{
				"ld163",
				new TestDescriptor()
				{
					Name = "ld163",
					Message = "LD SP,(NNNN)..................",
					Cycles = 16,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x7B, (byte)0x14, (byte)0x14),
						MemOp = 0x8DFC,
						IY = 0x57D7,
						IX = 0x2161,
						HL = 0xCA18,
						DE = 0xC185,
						BC = 0x27DA,
						F = 0x83,
						A = 0x1E,
						SP = 0xF460
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x7A,0xCE,0xA1,0x1B)
				}
			},
			{
				"ld164",
				new TestDescriptor()
				{
					Name = "ld164",
					Message = "LD <IX,IY>,(NNNN).............",
					Cycles = 32,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x2A, (byte)0x14, (byte)0x14),
						MemOp = 0xDED7,
						IY = 0xA6FA,
						IX = 0xF780,
						HL = 0x244C,
						DE = 0x87DE,
						BC = 0xBCC2,
						F = 0x16,
						A = 0x63,
						SP = 0x4C96
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x85,0x8B,0xF1,0x6D)
				}
			},
			{
				"ld165",
				new TestDescriptor()
				{
					Name = "ld165",
					Message = "LD (NNNN),<BC,DE>.............",
					Cycles = 64,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x43, (byte)0x14, (byte)0x14),
						MemOp = 0x1F98,
						IY = 0x844D,
						IX = 0xE8AC,
						HL = 0xC9ED,
						DE = 0xC95D,
						BC = 0x8F61,
						F = 0x80,
						A = 0x3F,
						SP = 0xC7BF
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x64,0x1E,0x87,0x15)
				}
			},
			{
				"ld166",
				new TestDescriptor()
				{
					Name = "ld166",
					Message = "LD (NNNN),HL..................",
					Cycles = 16,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x22, (byte)0x14, (byte)0x14, (byte)0x00),
						MemOp = 0xD003,
						IY = 0x7772,
						IX = 0x7F53,
						HL = 0x3F72,
						DE = 0x64EA,
						BC = 0xE180,
						F = 0x10,
						A = 0x2D,
						SP = 0x35E9
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xFFFF,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA3,0x60,0x8B,0x47)
				}
			},
			{
				"ld167",
				new TestDescriptor()
				{
					Name = "ld167",
					Message = "LD (NNNN),SP..................",
					Cycles = 16,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x73, (byte)0x14, (byte)0x14),
						MemOp = 0xC0DC,
						IY = 0xD1D6,
						IX = 0xED5A,
						HL = 0xF356,
						DE = 0xAFDA,
						BC = 0x6CA7,
						F = 0x44,
						A = 0x9F,
						SP = 0x3F0A
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0xFFFF
					},
					CRC = (0x16,0x58,0x5F,0xD7)
				}
			},
			{
				"ld168",
				new TestDescriptor()
				{
					Name = "ld168",
					Message = "LD (NNNN),<IX,IY>.............",
					Cycles = 64,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x22, (byte)0x14, (byte)0x14),
						MemOp = 0x6CC3,
						IY = 0x0D91,
						IX = 0x6900,
						HL = 0x8EF8,
						DE = 0xE3D6,
						BC = 0xC3F7,
						F = 0xC6,
						A = 0xD9,
						SP = 0xC2DF
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0xFFFF,
						IX = 0xFFFF,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xBA,0x10,0x2A,0x6B)
				}
			},
			{
				"ld16im",
				new TestDescriptor()
				{
					Name = "ld16im",
					Message = "LD <BC,DE,HL,SP>,NNNN.........",
					Cycles = 64,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x01, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x5C1C,
						IY = 0x2D46,
						IX = 0x8EB9,
						HL = 0x6078,
						DE = 0x74B1,
						BC = 0xB30E,
						F = 0x46,
						A = 0xD1,
						SP = 0x30CC
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x30, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xDE,0x39,0x19,0x69)
				}
			},
			{
				"ld16ix",
				new TestDescriptor()
				{
					Name = "ld16ix",
					Message = "LD <IX,IY>,NNNN...............",
					Cycles = 32,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x21, (byte)0x00, (byte)0x00),
						MemOp = 0x87E8,
						IY = 0x2006,
						IX = 0xBD12,
						HL = 0xB69B,
						DE = 0x7253,
						BC = 0xA1E5,
						F = 0x51,
						A = 0x13,
						SP = 0xF1BD
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x22,0x7D,0xD5,0x25)
				}
			},
			{
				"ld8bd",
				new TestDescriptor()
				{
					Name = "ld8bd",
					Message = "LD A,<(BC),(DE)>..............",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x0A, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xB3A8,
						IY = 0x1D2A,
						IX = 0x7F8E,
						HL = 0x42AC,
						DE = 0x0014,
						BC = 0x0014,
						F = 0xC6,
						A = 0xB1,
						SP = 0xEF8E
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x10, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0xB0,0x81,0x89,0x35)
				}
			},
			{
				"ld8im",
				new TestDescriptor()
				{
					Name = "ld8im",
					Message = "LD <B,C,D,E,H,L,(HL),A>,NN....",
					Cycles = 64,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x06, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xC407,
						IY = 0xF49D,
						IX = 0xD13D,
						HL = 0x0339,
						DE = 0xDE89,
						BC = 0x7455,
						F = 0x53,
						A = 0xC0,
						SP = 0x5509
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x38, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0xF1,0xDA,0xB5,0x56)
				}
			},
			{
				"ld8imx",
				new TestDescriptor()
				{
					Name = "ld8imx",
					Message = "LD (<IX,IY>+1),NN.............",
					Cycles = 32,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x36, (byte)0x01, (byte)0x00),
						MemOp = 0x1B45,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xD5C1,
						DE = 0x61C7,
						BC = 0xBDC4,
						F = 0xC0,
						A = 0x85,
						SP = 0xCD16
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0xFF),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x26,0x0,0x47,0x7E)
				}
			},
			{
				"ld8ix1",
				new TestDescriptor()
				{
					Name = "ld8ix1",
					Message = "LD <B,C,D,E>,(<IX,IY>+1)......",
					Cycles = 512,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x46, (byte)0x01, (byte)0x00),
						MemOp = 0xD016,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x4260,
						DE = 0x7F39,
						BC = 0x0404,
						F = 0x97,
						A = 0x4A,
						SP = 0xD085
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x18, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xCC,0x11,0x06,0xA8)
				}
			},
			{
				"ld8ix2",
				new TestDescriptor()
				{
					Name = "ld8ix2",
					Message = "LD <H,L>,(<IX,IY>+1)..........",
					Cycles = 256,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x66, (byte)0x01, (byte)0x00),
						MemOp = 0x84E0,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x9C52,
						DE = 0xA799,
						BC = 0x49B6,
						F = 0x93,
						A = 0x00,
						SP = 0xEEAD
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x08, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xFA,0x2A,0x4D,0x03)
				}
			},
			{
				"ld8ix3",
				new TestDescriptor()
				{
					Name = "ld8ix3",
					Message = "LD A,(<IX,IY>+1)..............",
					Cycles = 128,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x7E, (byte)0x01, (byte)0x00),
						MemOp = 0xD8B6,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xC612,
						DE = 0xDF07,
						BC = 0x9CD0,
						F = 0x43,
						A = 0xA6,
						SP = 0xA0E5
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xA5,0xE9,0xAC,0x64)
				}
			},
			{
				"ld8ixy",
				new TestDescriptor()
				{
					Name = "ld8ixy",
					Message = "LD <IXH,IXL,IYH,IYL>,NN.......",
					Cycles = 32,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x26, (byte)0x00, (byte)0x00),
						MemOp = 0x3C53,
						IY = 0x4640,
						IX = 0xE179,
						HL = 0x7711,
						DE = 0xC107,
						BC = 0x1AFA,
						F = 0x81,
						A = 0xAD,
						SP = 0x5D9B
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x08, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x24,0xE8,0x82,0x8B)
				}
			},
			{
				"ld8rr",
				new TestDescriptor()
				{
					Name = "ld8rr",
					Message = "LD <BCDEHLA>,<BCDEHLA>........",
					Cycles = 3456,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x40, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x72A4,
						IY = 0xA024,
						IX = 0x61AC,
						HL = 0x0014,
						DE = 0x82C7,
						BC = 0x718F,
						F = 0x97,
						A = 0x8F,
						SP = 0xEF8E
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x3F, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x74,0x4B,0x01,0x18)
				}
			},
			{
				"ld8rrx",
				new TestDescriptor()
				{
					Name = "ld8rrx",
					Message = "LD <BCDEXYA>,<BCDEXYA>........",
					Cycles = 6912,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x40, (byte)0x00, (byte)0x00),
						MemOp = 0xBCC5,
						IY = 0x0014,
						IX = 0x0014,
						HL = 0x0014,
						DE = 0x2FC2,
						BC = 0x98C0,
						F = 0x83,
						A = 0x1F,
						SP = 0x3BCD
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x3F, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x47,0x8B,0xA3,0x6B)
				}
			},
			{
				"lda",
				new TestDescriptor()
				{
					Name = "lda",
					Message = "LD A,(NNNN) / LD (NNNN),A.....",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x32, (byte)0x14, (byte)0x14, (byte)0x00),
						MemOp = 0xFD68,
						IY = 0xF4EC,
						IX = 0x44A0,
						HL = 0xB543,
						DE = 0x0653,
						BC = 0xCDBA,
						F = 0xD2,
						A = 0x4F,
						SP = 0x1FD8
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x08, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0xC9,0x26,0x2D,0xE5)
				}
			},
			{
				"ldd1",
				new TestDescriptor()
				{
					Name = "ldd1",
					Message = "LDD<R> (1)....................",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA8, (byte)0x00, (byte)0x00),
						MemOp = 0x9852,
						IY = 0x68FA,
						IX = 0x66A1,
						HL = 0x0017,
						DE = 0x0015,
						BC = 0x0001,
						F = 0xC1,
						A = 0x68,
						SP = 0x20B7
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x94,0xF4,0x27,0x69)
				}
			},
			{
				"ldd2",
				new TestDescriptor()
				{
					Name = "ldd2",
					Message = "LDD<R> (2)....................",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA8, (byte)0x00, (byte)0x00),
						MemOp = 0xF12E,
						IY = 0xEB2A,
						IX = 0xD5BA,
						HL = 0x0017,
						DE = 0x0015,
						BC = 0x0002,
						F = 0x47,
						A = 0xFF,
						SP = 0xFBE4
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x5A,0x90,0x7E,0xD4)
				}
			},
			{
				"ldi1",
				new TestDescriptor()
				{
					Name = "ldi1",
					Message = "LDI<R> (1)....................",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA0, (byte)0x00, (byte)0x00),
						MemOp = 0xFE30,
						IY = 0x03CD,
						IX = 0x6058,
						HL = 0x0016,
						DE = 0x0014,
						BC = 0x0001,
						F = 0x04,
						A = 0x60,
						SP = 0x2688
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x9A,0xBD,0xF6,0xB5)
				}
			},
			{
				"ldi2",
				new TestDescriptor()
				{
					Name = "ldi2",
					Message = "LDI<R> (2)....................",
					Cycles = 44,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0xA0, (byte)0x00, (byte)0x00),
						MemOp = 0x4ACE,
						IY = 0xC26E,
						IX = 0xB188,
						HL = 0x0016,
						DE = 0x0014,
						BC = 0x0002,
						F = 0x14,
						A = 0x2D,
						SP = 0xA39F
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x10, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xEB,0x59,0x89,0x1B)
				}
			},
			{
				"negop",
				new TestDescriptor()
				{
					Name = "negop",
					Message = "NEG...........................",
					Cycles = 16384,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x44, (byte)0x00, (byte)0x00),
						MemOp = 0x38A2,
						IY = 0x5F6B,
						IX = 0xD934,
						HL = 0x57E4,
						DE = 0xD2D6,
						BC = 0x4642,
						F = 0x43,
						A = 0x5A,
						SP = 0x09CC
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x6A,0x3C,0x3B,0xBD)
				}
			},
			{
				"rldop",
				new TestDescriptor()
				{
					Name = "rldop",
					Message = "<RRD,RLD>.....................",
					Cycles = 7168,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xED, (byte)0x67, (byte)0x00, (byte)0x00),
						MemOp = 0x91CB,
						IY = 0xC48B,
						IX = 0xFA62,
						HL = 0x0014,
						DE = 0xE720,
						BC = 0xB479,
						F = 0x40,
						A = 0x06,
						SP = 0x8AE2
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x08, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x95,0x5B,0xA3,0x26)
				}
			},
			{
				"rot8080",
				new TestDescriptor()
				{
					Name = "rot8080",
					Message = "<RLCA,RRCA,RLA,RRA>...........",
					Cycles = 6144,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x07, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xCB92,
						IY = 0x6D43,
						IX = 0x0A90,
						HL = 0xC284,
						DE = 0x0C53,
						BC = 0xF50E,
						F = 0x91,
						A = 0xEB,
						SP = 0x40FC
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x18, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x25,0x13,0x30,0xAE)
				}
			},
			{
				"rotxy",
				new TestDescriptor()
				{
					Name = "rotxy",
					Message = "SHF/ROT (<IX,IY>+1)...........",
					Cycles = 416,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0xCB, (byte)0x01, (byte)0x06),
						MemOp = 0xDDAF,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xFF3C,
						DE = 0xDBF6,
						BC = 0x94F4,
						F = 0x82,
						A = 0x80,
						SP = 0x61D9
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x38),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x80,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x57,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x71,0x3A,0xCD,0x81)
				}
			},
			{
				"rotz80",
				new TestDescriptor()
				{
					Name = "rotz80",
					Message = "SHF/ROT <B,C,D,E,H,L,(HL),A>..",
					Cycles = 6784,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xCB, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xCCEB,
						IY = 0x5D4A,
						IX = 0xE007,
						HL = 0x0014,
						DE = 0x1395,
						BC = 0x30EE,
						F = 0x43,
						A = 0x78,
						SP = 0x3DAD
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x3F, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x80,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x00FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0x57,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0xEB,0x60,0x4D,0x58)
				}
			},
			{
				"srz80",
				new TestDescriptor()
				{
					Name = "srz80",
					Message = "<SET,RES> N,<BCDEHL(HL)A>.....",
					Cycles = 7936,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xCB, (byte)0x80, (byte)0x00, (byte)0x00),
						MemOp = 0x2CD5,
						IY = 0x97AB,
						IX = 0x39FF,
						HL = 0x0014,
						DE = 0xD14B,
						BC = 0x6AB2,
						F = 0x53,
						A = 0x27,
						SP = 0xB538
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x7F, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0xD7,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x8B,0x57,0xF0,0x08)
				}
			},
			{
				"srzx",
				new TestDescriptor()
				{
					Name = "srzx",
					Message = "<SET,RES> N,(<IX,IY>+1).......",
					Cycles = 448, // it said 1792, but increment vector is out by 2 bits...
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0xCB, (byte)0x01, (byte)0x86),
						MemOp = 0xFB44,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xBA09,
						DE = 0x68BE,
						BC = 0x32D8,
						F = 0x10,
						A = 0x5E,
						SP = 0xA867
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x78),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0FF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0xD7,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0xCC,0x63,0xF9,0x8A)
				}
			},
			{
				"st8ix1",
				new TestDescriptor()
				{
					Name = "st8ix1",
					Message = "LD (<IX,IY>+1),<B,C,D,E>......",
					Cycles = 1024,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x70, (byte)0x01, (byte)0x00),
						MemOp = 0x270D,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xB73A,
						DE = 0x887B,
						BC = 0x99EE,
						F = 0x86,
						A = 0x70,
						SP = 0xCA07
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x03, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0xFFFF,
						BC = 0xFFFF,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x04,0x62,0x6A,0xBF)
				}
			},
			{
				"st8ix2",
				new TestDescriptor()
				{
					Name = "st8ix2",
					Message = "LD (<IX,IY>+1),<H,L>..........",
					Cycles = 256,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x74, (byte)0x01, (byte)0x00),
						MemOp = 0xB664,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0xE8AC,
						DE = 0xB5F5,
						BC = 0xAAFE,
						F = 0x12,
						A = 0x10,
						SP = 0x9566
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x01, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0xFFFF,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					CRC = (0x6A,0x1A,0x88,0x31)
				}
			},
			{
				"st8ix3",
				new TestDescriptor()
				{
					Name = "st8ix3",
					Message = "LD (<IX,IY>+1),A..............",
					Cycles = 64,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0xDD, (byte)0x77, (byte)0x01, (byte)0x00),
						MemOp = 0x67AF,
						IY = 0x0013,
						IX = 0x0013,
						HL = 0x4F13,
						DE = 0x0644,
						BC = 0xBCD7,
						F = 0x50,
						A = 0xAC,
						SP = 0x5FAF
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x20, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0001,
						IX = 0x0001,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0xCC,0xBE,0x5A,0x96)
				}
			},
			{
				"stabd",
				new TestDescriptor()
				{
					Name = "stabd",
					Message = "LD (<BC,DE>),A................",
					Cycles = 96,
					Mask = 0xD7,
					Base = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x02, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0C3B,
						IY = 0xB592,
						IX = 0x6CFF,
						HL = 0x959E,
						DE = 0x0014,
						BC = 0x0015,
						F = 0xC1,
						A = 0x21,
						SP = 0xBDE7
					},
					Increment = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x18, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0x0000,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0x00,
						SP = 0x0000
					},
					Shift = new TestVector()
						{
						Instruction = new InstructionBytes((byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00),
						MemOp = 0xFFFF,
						IY = 0x0000,
						IX = 0x0000,
						HL = 0x0000,
						DE = 0x0000,
						BC = 0x0000,
						F = 0x00,
						A = 0xFF,
						SP = 0x0000
					},
					CRC = (0x7A,0x4C,0x11,0x4F)
				}
			}
		};
    }
}

; Disassembly of the file "C:\projects\z80\Z80_Console\zexdoc.bin"
; 
; CPU Type: Z80
; 
; Created with dZ80 2.0
; 
; on Sunday, 17 of May 2020 at 02:14 PM
; 
0000 c31301    jp      0113h
0003 00        nop     
0004 00        nop     
0005 00        nop     
0006 00        nop     
0007 00        nop     
0008 00        nop     
0009 00        nop     
000a 00        nop     
000b 00        nop     
000c 00        nop     
000d 00        nop     
000e 00        nop     
000f 00        nop     
0010 00        nop     
0011 00        nop     
0012 00        nop     
0013 2a0600    ld      hl,(0006h)
0016 f9        ld      sp,hl
0017 11ca1d    ld      de,1dcah
001a 0e09      ld      c,09h
001c cdbe1d    call    1dbeh
001f 213801    ld      hl,0138h
0022 7e        ld      a,(hl)
0023 23        inc     hl
0024 b6        or      (hl)
0025 ca2f01    jp      z,012fh
0028 2b        dec     hl
0029 cde01a    call    1ae0h
002c c32201    jp      0122h
002f 11e91d    ld      de,1de9h
0032 0e09      ld      c,09h
0034 cdbe1d    call    1dbeh
0037 76        halt    
0038 c0        ret     nz

0039 012002    ld      bc,0220h
003c 80        add     a,b
003d 02        ld      (bc),a
003e e0        ret     po

003f 02        ld      (bc),a
0040 40        ld      b,b
0041 03        inc     bc
0042 a0        and     b
0043 03        inc     bc
0044 00        nop     
0045 04        inc     b
0046 60        ld      h,b
0047 04        inc     b
0048 c0        ret     nz

0049 04        inc     b
004a 2005      jr      nz,0051h
004c 80        add     a,b
004d 05        dec     b
004e e0        ret     po

004f 05        dec     b
0050 40        ld      b,b
0051 06a0      ld      b,0a0h
0053 0600      ld      b,00h
0055 07        rlca    
0056 60        ld      h,b
0057 07        rlca    
0058 c0        ret     nz

0059 07        rlca    
005a 2008      jr      nz,0064h
005c 80        add     a,b
005d 08        ex      af,af'
005e e0        ret     po

005f 08        ex      af,af'
0060 40        ld      b,b
0061 09        add     hl,bc
0062 a0        and     b
0063 09        add     hl,bc
0064 00        nop     
0065 0a        ld      a,(bc)
0066 60        ld      h,b
0067 0a        ld      a,(bc)
0068 c0        ret     nz

0069 0a        ld      a,(bc)
006a 200b      jr      nz,0077h
006c 80        add     a,b
006d 0b        dec     bc
006e e0        ret     po

006f 0b        dec     bc
0070 40        ld      b,b
0071 0c        inc     c
0072 a0        and     b
0073 0c        inc     c
0074 00        nop     
0075 0d        dec     c
0076 60        ld      h,b
0077 0d        dec     c
0078 c0        ret     nz

0079 0d        dec     c
007a 200e      jr      nz,008ah
007c 80        add     a,b
007d 0ee0      ld      c,0e0h
007f 0e40      ld      c,40h
0081 0f        rrca    
0082 a0        and     b
0083 0f        rrca    
0084 00        nop     
0085 1060      djnz    00e7h
0087 10c0      djnz    0049h
0089 1020      djnz    00abh
008b 118011    ld      de,1180h
008e e0        ret     po

008f 114012    ld      de,1240h
0092 a0        and     b
0093 12        ld      (de),a
0094 00        nop     
0095 13        inc     de
0096 60        ld      h,b
0097 13        inc     de
0098 c0        ret     nz

0099 13        inc     de
009a 2014      jr      nz,00b0h
009c 80        add     a,b
009d 14        inc     d
009e e0        ret     po

009f 14        inc     d
00a0 40        ld      b,b
00a1 15        dec     d
00a2 a0        and     b
00a3 15        dec     d
00a4 00        nop     
00a5 1660      ld      d,60h
00a7 16c0      ld      d,0c0h
00a9 1620      ld      d,20h
00ab 17        rla     
00ac 80        add     a,b
00ad 17        rla     
00ae e0        ret     po

00af 17        rla     
00b0 40        ld      b,b
00b1 18a0      jr      0053h
00b3 1800      jr      00b5h
00b5 19        add     hl,de
00b6 60        ld      h,b
00b7 19        add     hl,de
00b8 c0        ret     nz

00b9 19        add     hl,de
00ba 201a      jr      nz,00d6h
00bc 80        add     a,b
00bd 1a        ld      a,(de)
00be 00        nop     
00bf 00        nop     
00c0 c7        rst     00h
00c1 ed42      sbc     hl,bc
00c3 00        nop     
00c4 00        nop     
00c5 2c        inc     l
00c6 83        add     a,e
00c7 88        adc     a,b
00c8 4f        ld      c,a
00c9 2b        dec     hl
00ca f239b3    jp      p,0b339h
00cd 1f        rra     
00ce 7e        ld      a,(hl)
00cf 63        ld      h,e
00d0 15        dec     d
00d1 d389      out     (89h),a
00d3 5e        ld      e,(hl)
00d4 46        ld      b,(hl)
00d5 00        nop     
00d6 3800      jr      c,00d8h
00d8 00        nop     
00d9 00        nop     
00da 00        nop     
00db 00        nop     
00dc 00        nop     
00dd 00        nop     
00de 00        nop     
00df 21f800    ld      hl,00f8h
00e2 00        nop     
00e3 00        nop     
00e4 00        nop     
00e5 00        nop     
00e6 00        nop     
00e7 00        nop     
00e8 00        nop     
00e9 00        nop     
00ea 00        nop     
00eb 00        nop     
00ec 00        nop     
00ed 00        nop     
00ee 00        nop     
00ef 00        nop     
00f0 00        nop     
00f1 00        nop     
00f2 00        nop     
00f3 ff        rst     38h
00f4 ff        rst     38h
00f5 ff        rst     38h
00f6 ff        rst     38h
00f7 ff        rst     38h
00f8 ff        rst     38h
00f9 d7        rst     10h
00fa 00        nop     
00fb ff        rst     38h
00fc ff        rst     38h
00fd f8        ret     m

00fe b4        or      h
00ff eaa93c    jp      pe,3ca9h
0102 61        ld      h,c
0103 64        ld      h,h
0104 63        ld      h,e
0105 2c        inc     l
0106 73        ld      (hl),e
0107 62        ld      h,d
0108 63        ld      h,e
0109 3e20      ld      a,20h
010b 68        ld      l,b
010c 6c        ld      l,h
010d 2c        inc     l
010e 3c        inc     a
010f 62        ld      h,d
0110 63        ld      h,e
0111 2c        inc     l
0112 64        ld      h,h
0113 65        ld      h,l
0114 2c        inc     l
0115 68        ld      l,b
0116 6c        ld      l,h
0117 2c        inc     l
0118 73        ld      (hl),e
0119 70        ld      (hl),b
011a 3e2e      ld      a,2eh
011c 2e2e      ld      l,2eh
011e 2e24      ld      l,24h
0120 c7        rst     00h
0121 09        add     hl,bc
0122 00        nop     
0123 00        nop     
0124 00        nop     
0125 a5        and     l
0126 c4c7c4    call    nz,0c4c7h
0129 26d2      ld      h,0d2h
012b 50        ld      d,b
012c a0        and     b
012d ea5866    jp      pe,6658h
0130 85        add     a,l
0131 c6de      add     a,0deh
0133 c9        ret     

0134 9b        sbc     a,e
0135 3000      jr      nc,0137h
0137 00        nop     
0138 00        nop     
0139 00        nop     
013a 00        nop     
013b 00        nop     
013c 00        nop     
013d 00        nop     
013e 00        nop     
013f 21f800    ld      hl,00f8h
0142 00        nop     
0143 00        nop     
0144 00        nop     
0145 00        nop     
0146 00        nop     
0147 00        nop     
0148 00        nop     
0149 00        nop     
014a 00        nop     
014b 00        nop     
014c 00        nop     
014d 00        nop     
014e 00        nop     
014f 00        nop     
0150 00        nop     
0151 00        nop     
0152 00        nop     
0153 ff        rst     38h
0154 ff        rst     38h
0155 ff        rst     38h
0156 ff        rst     38h
0157 ff        rst     38h
0158 ff        rst     38h
0159 d7        rst     10h
015a 00        nop     
015b ff        rst     38h
015c ff        rst     38h
015d 89        adc     a,c
015e fdb635    or      (iy+35h)
0161 61        ld      h,c
0162 64        ld      h,h
0163 64        ld      h,h
0164 2068      jr      nz,01ceh
0166 6c        ld      l,h
0167 2c        inc     l
0168 3c        inc     a
0169 62        ld      h,d
016a 63        ld      h,e
016b 2c        inc     l
016c 64        ld      h,h
016d 65        ld      h,l
016e 2c        inc     l
016f 68        ld      l,b
0170 6c        ld      l,h
0171 2c        inc     l
0172 73        ld      (hl),e
0173 70        ld      (hl),b
0174 3e2e      ld      a,2eh
0176 2e2e      ld      l,2eh
0178 2e2e      ld      l,2eh
017a 2e2e      ld      l,2eh
017c 2e2e      ld      l,2eh
017e 2e24      ld      l,24h
0180 c7        rst     00h
0181 dd09      add     ix,bc
0183 00        nop     
0184 00        nop     
0185 ac        xor     h
0186 dd94      sub     ixh
0188 c25b63    jp      nz,635bh
018b d333      out     (33h),a
018d 76        halt    
018e 6a        ld      l,d
018f 20fa      jr      nz,018bh
0191 94        sub     h
0192 68        ld      l,b
0193 f5        push    af
0194 3600      ld      (hl),00h
0196 3000      jr      nc,0198h
0198 00        nop     
0199 00        nop     
019a 00        nop     
019b 00        nop     
019c 00        nop     
019d 21f800    ld      hl,00f8h
01a0 00        nop     
01a1 00        nop     
01a2 00        nop     
01a3 00        nop     
01a4 00        nop     
01a5 00        nop     
01a6 00        nop     
01a7 00        nop     
01a8 00        nop     
01a9 00        nop     
01aa 00        nop     
01ab 00        nop     
01ac 00        nop     
01ad 00        nop     
01ae 00        nop     
01af 00        nop     
01b0 00        nop     
01b1 ff        rst     38h
01b2 ff        rst     38h
01b3 00        nop     
01b4 00        nop     
01b5 ff        rst     38h
01b6 ff        rst     38h
01b7 ff        rst     38h
01b8 ff        rst     38h
01b9 d7        rst     10h
01ba 00        nop     
01bb ff        rst     38h
01bc ff        rst     38h
01bd c1        pop     bc
01be 33        inc     sp
01bf 79        ld      a,c
01c0 0b        dec     bc
01c1 61        ld      h,c
01c2 64        ld      h,h
01c3 64        ld      h,h
01c4 2069      jr      nz,022fh
01c6 78        ld      a,b
01c7 2c        inc     l
01c8 3c        inc     a
01c9 62        ld      h,d
01ca 63        ld      h,e
01cb 2c        inc     l
01cc 64        ld      h,h
01cd 65        ld      h,l
01ce 2c        inc     l
01cf 69        ld      l,c
01d0 78        ld      a,b
01d1 2c        inc     l
01d2 73        ld      (hl),e
01d3 70        ld      (hl),b
01d4 3e2e      ld      a,2eh
01d6 2e2e      ld      l,2eh
01d8 2e2e      ld      l,2eh
01da 2e2e      ld      l,2eh
01dc 2e2e      ld      l,2eh
01de 2e24      ld      l,24h
01e0 c7        rst     00h
01e1 fd09      add     iy,bc
01e3 00        nop     
01e4 00        nop     
01e5 c2c707    jp      nz,07c7h
01e8 f4c151    call    p,51c1h
01eb 96        sub     (hl)
01ec 3ef4      ld      a,0f4h
01ee 0b        dec     bc
01ef 0f        rrca    
01f0 51        ld      d,c
01f1 92        sub     d
01f2 1eea      ld      e,0eah
01f4 71        ld      (hl),c
01f5 00        nop     
01f6 3000      jr      nc,01f8h
01f8 00        nop     
01f9 00        nop     
01fa 00        nop     
01fb 21f800    ld      hl,00f8h
01fe 00        nop     
01ff 00        nop     
0200 00        nop     
0201 00        nop     
0202 00        nop     
0203 00        nop     
0204 00        nop     
0205 00        nop     
0206 00        nop     
0207 00        nop     
0208 00        nop     
0209 00        nop     
020a 00        nop     
020b 00        nop     
020c 00        nop     
020d 00        nop     
020e 00        nop     
020f ff        rst     38h
0210 ff        rst     38h
0211 00        nop     
0212 00        nop     
0213 00        nop     
0214 00        nop     
0215 ff        rst     38h
0216 ff        rst     38h
0217 ff        rst     38h
0218 ff        rst     38h
0219 d7        rst     10h
021a 00        nop     
021b ff        rst     38h
021c ff        rst     38h
021d e8        ret     pe

021e 81        add     a,c
021f 7b        ld      a,e
0220 9e        sbc     a,(hl)
0221 61        ld      h,c
0222 64        ld      h,h
0223 64        ld      h,h
0224 2069      jr      nz,028fh
0226 79        ld      a,c
0227 2c        inc     l
0228 3c        inc     a
0229 62        ld      h,d
022a 63        ld      h,e
022b 2c        inc     l
022c 64        ld      h,h
022d 65        ld      h,l
022e 2c        inc     l
022f 69        ld      l,c
0230 79        ld      a,c
0231 2c        inc     l
0232 73        ld      (hl),e
0233 70        ld      (hl),b
0234 3e2e      ld      a,2eh
0236 2e2e      ld      l,2eh
0238 2e2e      ld      l,2eh
023a 2e2e      ld      l,2eh
023c 2e2e      ld      l,2eh
023e 2e24      ld      l,24h
0240 d7        rst     10h
0241 c600      add     a,00h
0243 00        nop     
0244 00        nop     
0245 40        ld      b,b
0246 91        sub     c
0247 3c        inc     a
0248 7e        ld      a,(hl)
0249 67        ld      h,a
024a 7a        ld      a,d
024b 6d        ld      l,l
024c df        rst     18h
024d 61        ld      h,c
024e 5b        ld      e,e
024f 29        add     hl,hl
0250 0b        dec     bc
0251 1066      djnz    02b9h
0253 b2        or      d
0254 85        add     a,l
0255 3800      jr      c,0257h
0257 00        nop     
0258 00        nop     
0259 00        nop     
025a 00        nop     
025b 00        nop     
025c 00        nop     
025d 00        nop     
025e 00        nop     
025f 00        nop     
0260 00        nop     
0261 00        nop     
0262 00        nop     
0263 00        nop     
0264 00        nop     
0265 00        nop     
0266 ff        rst     38h
0267 00        nop     
0268 00        nop     
0269 00        nop     
026a ff        rst     38h
026b 00        nop     
026c 00        nop     
026d 00        nop     
026e 00        nop     
026f 00        nop     
0270 00        nop     
0271 00        nop     
0272 00        nop     
0273 00        nop     
0274 00        nop     
0275 00        nop     
0276 00        nop     
0277 00        nop     
0278 00        nop     
0279 d7        rst     10h
027a 00        nop     
027b 00        nop     
027c 00        nop     
027d 48        ld      c,b
027e 79        ld      a,c
027f 93        sub     e
0280 60        ld      h,b
0281 61        ld      h,c
0282 6c        ld      l,h
0283 75        ld      (hl),l
0284 6f        ld      l,a
0285 70        ld      (hl),b
0286 2061      jr      nz,02e9h
0288 2c        inc     l
0289 6e        ld      l,(hl)
028a 6e        ld      l,(hl)
028b 2e2e      ld      l,2eh
028d 2e2e      ld      l,2eh
028f 2e2e      ld      l,2eh
0291 2e2e      ld      l,2eh
0293 2e2e      ld      l,2eh
0295 2e2e      ld      l,2eh
0297 2e2e      ld      l,2eh
0299 2e2e      ld      l,2eh
029b 2e2e      ld      l,2eh
029d 2e2e      ld      l,2eh
029f 24        inc     h
02a0 d7        rst     10h
02a1 80        add     a,b
02a2 00        nop     
02a3 00        nop     
02a4 00        nop     
02a5 3ec5      ld      a,0c5h
02a7 3a574d    ld      a,(4d57h)
02aa 4c        ld      c,h
02ab 03        inc     bc
02ac 0109e3    ld      bc,0e309h
02af 66        ld      h,(hl)
02b0 a6        and     (hl)
02b1 d0        ret     nc

02b2 3b        dec     sp
02b3 bb        cp      e
02b4 ad        xor     l
02b5 3f        ccf     
02b6 00        nop     
02b7 00        nop     
02b8 00        nop     
02b9 00        nop     
02ba 00        nop     
02bb 00        nop     
02bc 00        nop     
02bd 00        nop     
02be 00        nop     
02bf 00        nop     
02c0 00        nop     
02c1 00        nop     
02c2 00        nop     
02c3 00        nop     
02c4 00        nop     
02c5 00        nop     
02c6 ff        rst     38h
02c7 00        nop     
02c8 00        nop     
02c9 00        nop     
02ca 00        nop     
02cb 00        nop     
02cc 00        nop     
02cd ff        rst     38h
02ce 00        nop     
02cf 00        nop     
02d0 00        nop     
02d1 00        nop     
02d2 00        nop     
02d3 00        nop     
02d4 00        nop     
02d5 ff        rst     38h
02d6 ff        rst     38h
02d7 ff        rst     38h
02d8 ff        rst     38h
02d9 d7        rst     10h
02da 00        nop     
02db 00        nop     
02dc 00        nop     
02dd fe43      cp      43h
02df b0        or      b
02e0 1661      ld      d,61h
02e2 6c        ld      l,h
02e3 75        ld      (hl),l
02e4 6f        ld      l,a
02e5 70        ld      (hl),b
02e6 2061      jr      nz,0349h
02e8 2c        inc     l
02e9 3c        inc     a
02ea 62        ld      h,d
02eb 2c        inc     l
02ec 63        ld      h,e
02ed 2c        inc     l
02ee 64        ld      h,h
02ef 2c        inc     l
02f0 65        ld      h,l
02f1 2c        inc     l
02f2 68        ld      l,b
02f3 2c        inc     l
02f4 6c        ld      l,h
02f5 2c        inc     l
02f6 2868      jr      z,0360h
02f8 6c        ld      l,h
02f9 29        add     hl,hl
02fa 2c        inc     l
02fb 61        ld      h,c
02fc 3e2e      ld      a,2eh
02fe 2e24      ld      l,24h
0300 d7        rst     10h
0301 dd84      add     a,ixh
0303 00        nop     
0304 00        nop     
0305 f7        rst     30h
0306 d66e      sub     6eh
0308 c7        rst     00h
0309 cf        rst     08h
030a ac        xor     h
030b 47        ld      b,a
030c 28dd      jr      z,02ebh
030e 2235c0    ld      (0c035h),hl
0311 c5        push    bc
0312 384b      jr      c,035fh
0314 23        inc     hl
0315 2039      jr      nz,0350h
0317 00        nop     
0318 00        nop     
0319 00        nop     
031a 00        nop     
031b 00        nop     
031c 00        nop     
031d 00        nop     
031e 00        nop     
031f 00        nop     
0320 00        nop     
0321 00        nop     
0322 00        nop     
0323 00        nop     
0324 00        nop     
0325 00        nop     
0326 ff        rst     38h
0327 00        nop     
0328 00        nop     
0329 00        nop     
032a 00        nop     
032b 00        nop     
032c 00        nop     
032d ff        rst     38h
032e 00        nop     
032f 00        nop     
0330 00        nop     
0331 00        nop     
0332 00        nop     
0333 00        nop     
0334 00        nop     
0335 ff        rst     38h
0336 ff        rst     38h
0337 ff        rst     38h
0338 ff        rst     38h
0339 d7        rst     10h
033a 00        nop     
033b 00        nop     
033c 00        nop     
033d a4        and     h
033e 02        ld      (bc),a
033f 6d        ld      l,l
0340 5a        ld      e,d
0341 61        ld      h,c
0342 6c        ld      l,h
0343 75        ld      (hl),l
0344 6f        ld      l,a
0345 70        ld      (hl),b
0346 2061      jr      nz,03a9h
0348 2c        inc     l
0349 3c        inc     a
034a 69        ld      l,c
034b 78        ld      a,b
034c 68        ld      l,b
034d 2c        inc     l
034e 69        ld      l,c
034f 78        ld      a,b
0350 6c        ld      l,h
0351 2c        inc     l
0352 69        ld      l,c
0353 79        ld      a,c
0354 68        ld      l,b
0355 2c        inc     l
0356 69        ld      l,c
0357 79        ld      a,c
0358 6c        ld      l,h
0359 3e2e      ld      a,2eh
035b 2e2e      ld      l,2eh
035d 2e2e      ld      l,2eh
035f 24        inc     h
0360 d7        rst     10h
0361 dd8601    add     a,(ix+01h)
0364 00        nop     
0365 b7        or      a
0366 90        sub     b
0367 02        ld      (bc),a
0368 010201    ld      bc,0102h
036b fd326e40  ld      (406eh),a
036f dcc145    call    c,45c1h
0372 6e        ld      l,(hl)
0373 fae520    jp      m,20e5h
0376 3800      jr      c,0378h
0378 00        nop     
0379 00        nop     
037a 00        nop     
037b 010001    ld      bc,0100h
037e 00        nop     
037f 00        nop     
0380 00        nop     
0381 00        nop     
0382 00        nop     
0383 00        nop     
0384 00        nop     
0385 00        nop     
0386 ff        rst     38h
0387 00        nop     
0388 00        nop     
0389 00        nop     
038a 00        nop     
038b 00        nop     
038c 00        nop     
038d ff        rst     38h
038e 00        nop     
038f 00        nop     
0390 00        nop     
0391 00        nop     
0392 00        nop     
0393 00        nop     
0394 00        nop     
0395 00        nop     
0396 00        nop     
0397 00        nop     
0398 00        nop     
0399 d7        rst     10h
039a 00        nop     
039b 00        nop     
039c 00        nop     
039d e8        ret     pe

039e 49        ld      c,c
039f 67        ld      h,a
03a0 6e        ld      l,(hl)
03a1 61        ld      h,c
03a2 6c        ld      l,h
03a3 75        ld      (hl),l
03a4 6f        ld      l,a
03a5 70        ld      (hl),b
03a6 2061      jr      nz,0409h
03a8 2c        inc     l
03a9 283c      jr      z,03e7h
03ab 69        ld      l,c
03ac 78        ld      a,b
03ad 2c        inc     l
03ae 69        ld      l,c
03af 79        ld      a,c
03b0 3e2b      ld      a,2bh
03b2 31292e    ld      sp,2e29h
03b5 2e2e      ld      l,2eh
03b7 2e2e      ld      l,2eh
03b9 2e2e      ld      l,2eh
03bb 2e2e      ld      l,2eh
03bd 2e2e      ld      l,2eh
03bf 24        inc     h
03c0 53        ld      d,e
03c1 ddcb0146  bit     0,(ix+01h)
03c5 75        ld      (hl),l
03c6 2002      jr      nz,03cah
03c8 010201    ld      bc,0102h
03cb fc3c9a    call    m,9a3ch
03ce a7        and     a
03cf 74        ld      (hl),h
03d0 3d        dec     a
03d1 51        ld      d,c
03d2 27        daa     
03d3 14        inc     d
03d4 ca2000    jp      z,0020h
03d7 00        nop     
03d8 3800      jr      c,03dah
03da 00        nop     
03db 00        nop     
03dc 00        nop     
03dd 00        nop     
03de 00        nop     
03df 00        nop     
03e0 00        nop     
03e1 00        nop     
03e2 00        nop     
03e3 00        nop     
03e4 00        nop     
03e5 53        ld      d,e
03e6 00        nop     
03e7 00        nop     
03e8 00        nop     
03e9 00        nop     
03ea 00        nop     
03eb 00        nop     
03ec 00        nop     
03ed ff        rst     38h
03ee 00        nop     
03ef 00        nop     
03f0 00        nop     
03f1 00        nop     
03f2 00        nop     
03f3 00        nop     
03f4 00        nop     
03f5 00        nop     
03f6 00        nop     
03f7 00        nop     
03f8 00        nop     
03f9 00        nop     
03fa 00        nop     
03fb 00        nop     
03fc 00        nop     
03fd a8        xor     b
03fe ee08      xor     08h
0400 67        ld      h,a
0401 62        ld      h,d
0402 69        ld      l,c
0403 74        ld      (hl),h
0404 206e      jr      nz,0474h
0406 2c        inc     l
0407 283c      jr      z,0445h
0409 69        ld      l,c
040a 78        ld      a,b
040b 2c        inc     l
040c 69        ld      l,c
040d 79        ld      a,c
040e 3e2b      ld      a,2bh
0410 31292e    ld      sp,2e29h
0413 2e2e      ld      l,2eh
0415 2e2e      ld      l,2eh
0417 2e2e      ld      l,2eh
0419 2e2e      ld      l,2eh
041b 2e2e      ld      l,2eh
041d 2e2e      ld      l,2eh
041f 24        inc     h
0420 53        ld      d,e
0421 cb40      bit     0,b
0423 00        nop     
0424 00        nop     
0425 f1        pop     af
0426 3efc      ld      a,0fch
0428 9d        sbc     a,l
0429 cc7a03    call    z,037ah
042c 0161be    ld      bc,0be61h
042f 86        add     a,(hl)
0430 7a        ld      a,d
0431 50        ld      d,b
0432 24        inc     h
0433 98        sbc     a,b
0434 19        add     hl,de
0435 00        nop     
0436 3f        ccf     
0437 00        nop     
0438 00        nop     
0439 00        nop     
043a 00        nop     
043b 00        nop     
043c 00        nop     
043d 00        nop     
043e 00        nop     
043f 00        nop     
0440 00        nop     
0441 00        nop     
0442 00        nop     
0443 00        nop     
0444 00        nop     
0445 53        ld      d,e
0446 00        nop     
0447 00        nop     
0448 00        nop     
0449 00        nop     
044a 00        nop     
044b 00        nop     
044c 00        nop     
044d ff        rst     38h
044e 00        nop     
044f 00        nop     
0450 00        nop     
0451 00        nop     
0452 00        nop     
0453 00        nop     
0454 00        nop     
0455 ff        rst     38h
0456 ff        rst     38h
0457 ff        rst     38h
0458 ff        rst     38h
0459 00        nop     
045a ff        rst     38h
045b 00        nop     
045c 00        nop     
045d 7b        ld      a,e
045e 55        ld      d,l
045f e6c8      and     0c8h
0461 62        ld      h,d
0462 69        ld      l,c
0463 74        ld      (hl),h
0464 206e      jr      nz,04d4h
0466 2c        inc     l
0467 3c        inc     a
0468 62        ld      h,d
0469 2c        inc     l
046a 63        ld      h,e
046b 2c        inc     l
046c 64        ld      h,h
046d 2c        inc     l
046e 65        ld      h,l
046f 2c        inc     l
0470 68        ld      l,b
0471 2c        inc     l
0472 6c        ld      l,h
0473 2c        inc     l
0474 2868      jr      z,04deh
0476 6c        ld      l,h
0477 29        add     hl,hl
0478 2c        inc     l
0479 61        ld      h,c
047a 3e2e      ld      a,2eh
047c 2e2e      ld      l,2eh
047e 2e24      ld      l,24h
0480 d7        rst     10h
0481 eda9      cpd     
0483 00        nop     
0484 00        nop     
0485 b6        or      (hl)
0486 c7        rst     00h
0487 b4        or      h
0488 72        ld      (hl),d
0489 f618      or      18h
048b 14        inc     d
048c 01bd8d    ld      bc,8dbdh
048f 0100c0    ld      bc,0c000h
0492 30a3      jr      nc,0437h
0494 94        sub     h
0495 00        nop     
0496 1000      djnz    0498h
0498 00        nop     
0499 00        nop     
049a 00        nop     
049b 00        nop     
049c 00        nop     
049d 00        nop     
049e 00        nop     
049f 00        nop     
04a0 00        nop     
04a1 00        nop     
04a2 00        nop     
04a3 0a        ld      a,(bc)
04a4 00        nop     
04a5 00        nop     
04a6 ff        rst     38h
04a7 00        nop     
04a8 00        nop     
04a9 00        nop     
04aa 00        nop     
04ab 00        nop     
04ac 00        nop     
04ad 00        nop     
04ae 00        nop     
04af 00        nop     
04b0 00        nop     
04b1 00        nop     
04b2 00        nop     
04b3 00        nop     
04b4 00        nop     
04b5 00        nop     
04b6 00        nop     
04b7 00        nop     
04b8 00        nop     
04b9 d7        rst     10h
04ba 00        nop     
04bb 00        nop     
04bc 00        nop     
04bd a8        xor     b
04be 7e        ld      a,(hl)
04bf 6c        ld      l,h
04c0 fa6370    jp      m,7063h
04c3 64        ld      h,h
04c4 3c        inc     a
04c5 72        ld      (hl),d
04c6 3e2e      ld      a,2eh
04c8 2e2e      ld      l,2eh
04ca 2e2e      ld      l,2eh
04cc 2e2e      ld      l,2eh
04ce 2e2e      ld      l,2eh
04d0 2e2e      ld      l,2eh
04d2 2e2e      ld      l,2eh
04d4 2e2e      ld      l,2eh
04d6 2e2e      ld      l,2eh
04d8 2e2e      ld      l,2eh
04da 2e2e      ld      l,2eh
04dc 2e2e      ld      l,2eh
04de 2e24      ld      l,24h
04e0 d7        rst     10h
04e1 eda1      cpi     
04e3 00        nop     
04e4 00        nop     
04e5 48        ld      c,b
04e6 4d        ld      c,l
04e7 4a        ld      c,d
04e8 af        xor     a
04e9 6b        ld      l,e
04ea 90        sub     b
04eb 03        inc     bc
04ec 01714e    ld      bc,4e71h
04ef 010093    ld      bc,9300h
04f2 6a        ld      l,d
04f3 7c        ld      a,h
04f4 90        sub     b
04f5 00        nop     
04f6 1000      djnz    04f8h
04f8 00        nop     
04f9 00        nop     
04fa 00        nop     
04fb 00        nop     
04fc 00        nop     
04fd 00        nop     
04fe 00        nop     
04ff 00        nop     
0500 00        nop     
0501 00        nop     
0502 00        nop     
0503 0a        ld      a,(bc)
0504 00        nop     
0505 00        nop     
0506 ff        rst     38h
0507 00        nop     
0508 00        nop     
0509 00        nop     
050a 00        nop     
050b 00        nop     
050c 00        nop     
050d 00        nop     
050e 00        nop     
050f 00        nop     
0510 00        nop     
0511 00        nop     
0512 00        nop     
0513 00        nop     
0514 00        nop     
0515 00        nop     
0516 00        nop     
0517 00        nop     
0518 00        nop     
0519 d7        rst     10h
051a 00        nop     
051b 00        nop     
051c 00        nop     
051d 06de      ld      b,0deh
051f b3        or      e
0520 56        ld      d,(hl)
0521 63        ld      h,e
0522 70        ld      (hl),b
0523 69        ld      l,c
0524 3c        inc     a
0525 72        ld      (hl),d
0526 3e2e      ld      a,2eh
0528 2e2e      ld      l,2eh
052a 2e2e      ld      l,2eh
052c 2e2e      ld      l,2eh
052e 2e2e      ld      l,2eh
0530 2e2e      ld      l,2eh
0532 2e2e      ld      l,2eh
0534 2e2e      ld      l,2eh
0536 2e2e      ld      l,2eh
0538 2e2e      ld      l,2eh
053a 2e2e      ld      l,2eh
053c 2e2e      ld      l,2eh
053e 2e24      ld      l,24h
0540 d7        rst     10h
0541 27        daa     
0542 00        nop     
0543 00        nop     
0544 00        nop     
0545 41        ld      b,c
0546 21fa09    ld      hl,09fah
0549 60        ld      h,b
054a 1d        dec     e
054b 59        ld      e,c
054c a5        and     l
054d 5b        ld      e,e
054e 8d        adc     a,l
054f 79        ld      a,c
0550 90        sub     b
0551 04        inc     b
0552 8e        adc     a,(hl)
0553 9d        sbc     a,l
0554 29        add     hl,hl
0555 1800      jr      0557h
0557 00        nop     
0558 00        nop     
0559 00        nop     
055a 00        nop     
055b 00        nop     
055c 00        nop     
055d 00        nop     
055e 00        nop     
055f 00        nop     
0560 00        nop     
0561 00        nop     
0562 00        nop     
0563 00        nop     
0564 00        nop     
0565 d7        rst     10h
0566 ff        rst     38h
0567 00        nop     
0568 00        nop     
0569 00        nop     
056a 00        nop     
056b 00        nop     
056c 00        nop     
056d 00        nop     
056e 00        nop     
056f 00        nop     
0570 00        nop     
0571 00        nop     
0572 00        nop     
0573 00        nop     
0574 00        nop     
0575 00        nop     
0576 00        nop     
0577 00        nop     
0578 00        nop     
0579 00        nop     
057a 00        nop     
057b 00        nop     
057c 00        nop     
057d 9b        sbc     a,e
057e 4b        ld      c,e
057f a6        and     (hl)
0580 75        ld      (hl),l
0581 3c        inc     a
0582 64        ld      h,h
0583 61        ld      h,c
0584 61        ld      h,c
0585 2c        inc     l
0586 63        ld      h,e
0587 70        ld      (hl),b
0588 6c        ld      l,h
0589 2c        inc     l
058a 73        ld      (hl),e
058b 63        ld      h,e
058c 66        ld      h,(hl)
058d 2c        inc     l
058e 63        ld      h,e
058f 63        ld      h,e
0590 66        ld      h,(hl)
0591 3e2e      ld      a,2eh
0593 2e2e      ld      l,2eh
0595 2e2e      ld      l,2eh
0597 2e2e      ld      l,2eh
0599 2e2e      ld      l,2eh
059b 2e2e      ld      l,2eh
059d 2e2e      ld      l,2eh
059f 24        inc     h
05a0 d7        rst     10h
05a1 3c        inc     a
05a2 00        nop     
05a3 00        nop     
05a4 00        nop     
05a5 df        rst     18h
05a6 4a        ld      c,d
05a7 d8        ret     c

05a8 d5        push    de
05a9 98        sbc     a,b
05aa e5        push    hl
05ab 2b        dec     hl
05ac 8a        adc     a,d
05ad b0        or      b
05ae a7        and     a
05af 1b        dec     de
05b0 43        ld      b,e
05b1 44        ld      b,h
05b2 5a        ld      e,d
05b3 30d0      jr      nc,0585h
05b5 010000    ld      bc,0000h
05b8 00        nop     
05b9 00        nop     
05ba 00        nop     
05bb 00        nop     
05bc 00        nop     
05bd 00        nop     
05be 00        nop     
05bf 00        nop     
05c0 00        nop     
05c1 00        nop     
05c2 00        nop     
05c3 00        nop     
05c4 00        nop     
05c5 00        nop     
05c6 ff        rst     38h
05c7 00        nop     
05c8 00        nop     
05c9 00        nop     
05ca 00        nop     
05cb 00        nop     
05cc 00        nop     
05cd 00        nop     
05ce 00        nop     
05cf 00        nop     
05d0 00        nop     
05d1 00        nop     
05d2 00        nop     
05d3 00        nop     
05d4 00        nop     
05d5 00        nop     
05d6 00        nop     
05d7 00        nop     
05d8 00        nop     
05d9 d7        rst     10h
05da 00        nop     
05db 00        nop     
05dc 00        nop     
05dd d1        pop     de
05de 88        adc     a,b
05df 15        dec     d
05e0 a4        and     h
05e1 3c        inc     a
05e2 69        ld      l,c
05e3 6e        ld      l,(hl)
05e4 63        ld      h,e
05e5 2c        inc     l
05e6 64        ld      h,h
05e7 65        ld      h,l
05e8 63        ld      h,e
05e9 3e20      ld      a,20h
05eb 61        ld      h,c
05ec 2e2e      ld      l,2eh
05ee 2e2e      ld      l,2eh
05f0 2e2e      ld      l,2eh
05f2 2e2e      ld      l,2eh
05f4 2e2e      ld      l,2eh
05f6 2e2e      ld      l,2eh
05f8 2e2e      ld      l,2eh
05fa 2e2e      ld      l,2eh
05fc 2e2e      ld      l,2eh
05fe 2e24      ld      l,24h
0600 d7        rst     10h
0601 04        inc     b
0602 00        nop     
0603 00        nop     
0604 00        nop     
0605 23        inc     hl
0606 d62d      sub     2dh
0608 43        ld      b,e
0609 61        ld      h,c
060a 7a        ld      a,d
060b 80        add     a,b
060c 81        add     a,c
060d 86        add     a,(hl)
060e 5a        ld      e,d
060f 85        add     a,l
0610 1e86      ld      e,86h
0612 58        ld      e,b
0613 bb        cp      e
0614 9b        sbc     a,e
0615 010000    ld      bc,0000h
0618 00        nop     
0619 00        nop     
061a 00        nop     
061b 00        nop     
061c 00        nop     
061d 00        nop     
061e 00        nop     
061f 00        nop     
0620 00        nop     
0621 00        nop     
0622 00        nop     
0623 00        nop     
0624 ff        rst     38h
0625 00        nop     
0626 00        nop     
0627 00        nop     
0628 00        nop     
0629 00        nop     
062a 00        nop     
062b 00        nop     
062c 00        nop     
062d 00        nop     
062e 00        nop     
062f 00        nop     
0630 00        nop     
0631 00        nop     
0632 00        nop     
0633 00        nop     
0634 00        nop     
0635 00        nop     
0636 00        nop     
0637 00        nop     
0638 00        nop     
0639 d7        rst     10h
063a 00        nop     
063b 00        nop     
063c 00        nop     
063d 5f        ld      e,a
063e 68        ld      l,b
063f 22643c    ld      (3c64h),hl
0642 69        ld      l,c
0643 6e        ld      l,(hl)
0644 63        ld      h,e
0645 2c        inc     l
0646 64        ld      h,h
0647 65        ld      h,l
0648 63        ld      h,e
0649 3e20      ld      a,20h
064b 62        ld      h,d
064c 2e2e      ld      l,2eh
064e 2e2e      ld      l,2eh
0650 2e2e      ld      l,2eh
0652 2e2e      ld      l,2eh
0654 2e2e      ld      l,2eh
0656 2e2e      ld      l,2eh
0658 2e2e      ld      l,2eh
065a 2e2e      ld      l,2eh
065c 2e2e      ld      l,2eh
065e 2e24      ld      l,24h
0660 d7        rst     10h
0661 03        inc     bc
0662 00        nop     
0663 00        nop     
0664 00        nop     
0665 97        sub     a
0666 cdab44    call    44abh
0669 c9        ret     

066a 8d        adc     a,l
066b e3        ex      (sp),hl
066c e3        ex      (sp),hl
066d cc11a4    call    z,0a411h
0670 e8        ret     pe

0671 02        ld      (bc),a
0672 49        ld      c,c
0673 4d        ld      c,l
0674 2a0800    ld      hl,(0008h)
0677 00        nop     
0678 00        nop     
0679 00        nop     
067a 00        nop     
067b 00        nop     
067c 00        nop     
067d 00        nop     
067e 00        nop     
067f 00        nop     
0680 00        nop     
0681 00        nop     
0682 00        nop     
0683 21f800    ld      hl,00f8h
0686 00        nop     
0687 00        nop     
0688 00        nop     
0689 00        nop     
068a 00        nop     
068b 00        nop     
068c 00        nop     
068d 00        nop     
068e 00        nop     
068f 00        nop     
0690 00        nop     
0691 00        nop     
0692 00        nop     
0693 00        nop     
0694 00        nop     
0695 00        nop     
0696 00        nop     
0697 00        nop     
0698 00        nop     
0699 d7        rst     10h
069a 00        nop     
069b 00        nop     
069c 00        nop     
069d d2ae3b    jp      nc,3baeh
06a0 ec3c69    call    pe,693ch
06a3 6e        ld      l,(hl)
06a4 63        ld      h,e
06a5 2c        inc     l
06a6 64        ld      h,h
06a7 65        ld      h,l
06a8 63        ld      h,e
06a9 3e20      ld      a,20h
06ab 62        ld      h,d
06ac 63        ld      h,e
06ad 2e2e      ld      l,2eh
06af 2e2e      ld      l,2eh
06b1 2e2e      ld      l,2eh
06b3 2e2e      ld      l,2eh
06b5 2e2e      ld      l,2eh
06b7 2e2e      ld      l,2eh
06b9 2e2e      ld      l,2eh
06bb 2e2e      ld      l,2eh
06bd 2e2e      ld      l,2eh
06bf 24        inc     h
06c0 d7        rst     10h
06c1 0c        inc     c
06c2 00        nop     
06c3 00        nop     
06c4 00        nop     
06c5 89        adc     a,c
06c6 d7        rst     10h
06c7 35        dec     (hl)
06c8 09        add     hl,bc
06c9 5b        ld      e,e
06ca 05        dec     b
06cb 85        add     a,l
06cc 9f        sbc     a,a
06cd 27        daa     
06ce 8b        adc     a,e
06cf 08        ex      af,af'
06d0 d29505    jp      nc,0595h
06d3 60        ld      h,b
06d4 0601      ld      b,01h
06d6 00        nop     
06d7 00        nop     
06d8 00        nop     
06d9 00        nop     
06da 00        nop     
06db 00        nop     
06dc 00        nop     
06dd 00        nop     
06de 00        nop     
06df 00        nop     
06e0 00        nop     
06e1 00        nop     
06e2 00        nop     
06e3 ff        rst     38h
06e4 00        nop     
06e5 00        nop     
06e6 00        nop     
06e7 00        nop     
06e8 00        nop     
06e9 00        nop     
06ea 00        nop     
06eb 00        nop     
06ec 00        nop     
06ed 00        nop     
06ee 00        nop     
06ef 00        nop     
06f0 00        nop     
06f1 00        nop     
06f2 00        nop     
06f3 00        nop     
06f4 00        nop     
06f5 00        nop     
06f6 00        nop     
06f7 00        nop     
06f8 00        nop     
06f9 d7        rst     10h
06fa 00        nop     
06fb 00        nop     
06fc 00        nop     
06fd c28455    jp      nz,5584h
0700 4c        ld      c,h
0701 3c        inc     a
0702 69        ld      l,c
0703 6e        ld      l,(hl)
0704 63        ld      h,e
0705 2c        inc     l
0706 64        ld      h,h
0707 65        ld      h,l
0708 63        ld      h,e
0709 3e20      ld      a,20h
070b 63        ld      h,e
070c 2e2e      ld      l,2eh
070e 2e2e      ld      l,2eh
0710 2e2e      ld      l,2eh
0712 2e2e      ld      l,2eh
0714 2e2e      ld      l,2eh
0716 2e2e      ld      l,2eh
0718 2e2e      ld      l,2eh
071a 2e2e      ld      l,2eh
071c 2e2e      ld      l,2eh
071e 2e24      ld      l,24h
0720 d7        rst     10h
0721 14        inc     d
0722 00        nop     
0723 00        nop     
0724 00        nop     
0725 eaa0ba    jp      pe,0baa0h
0728 5f        ld      e,a
0729 fb        ei      
072a 65        ld      h,l
072b 1c        inc     e
072c 98        sbc     a,b
072d cc38bc    call    z,0bc38h
0730 de43      sbc     a,43h
0732 5c        ld      e,h
0733 bd        cp      l
0734 03        inc     bc
0735 010000    ld      bc,0000h
0738 00        nop     
0739 00        nop     
073a 00        nop     
073b 00        nop     
073c 00        nop     
073d 00        nop     
073e 00        nop     
073f 00        nop     
0740 00        nop     
0741 00        nop     
0742 ff        rst     38h
0743 00        nop     
0744 00        nop     
0745 00        nop     
0746 00        nop     
0747 00        nop     
0748 00        nop     
0749 00        nop     
074a 00        nop     
074b 00        nop     
074c 00        nop     
074d 00        nop     
074e 00        nop     
074f 00        nop     
0750 00        nop     
0751 00        nop     
0752 00        nop     
0753 00        nop     
0754 00        nop     
0755 00        nop     
0756 00        nop     
0757 00        nop     
0758 00        nop     
0759 d7        rst     10h
075a 00        nop     
075b 00        nop     
075c 00        nop     
075d 45        ld      b,l
075e 23        inc     hl
075f de10      sbc     a,10h
0761 3c        inc     a
0762 69        ld      l,c
0763 6e        ld      l,(hl)
0764 63        ld      h,e
0765 2c        inc     l
0766 64        ld      h,h
0767 65        ld      h,l
0768 63        ld      h,e
0769 3e20      ld      a,20h
076b 64        ld      h,h
076c 2e2e      ld      l,2eh
076e 2e2e      ld      l,2eh
0770 2e2e      ld      l,2eh
0772 2e2e      ld      l,2eh
0774 2e2e      ld      l,2eh
0776 2e2e      ld      l,2eh
0778 2e2e      ld      l,2eh
077a 2e2e      ld      l,2eh
077c 2e2e      ld      l,2eh
077e 2e24      ld      l,24h
0780 d7        rst     10h
0781 13        inc     de
0782 00        nop     
0783 00        nop     
0784 00        nop     
0785 2e34      ld      l,34h
0787 1d        dec     e
0788 13        inc     de
0789 c9        ret     

078a 28ca      jr      z,0756h
078c 0a        ld      a,(bc)
078d 67        ld      h,a
078e 99        sbc     a,c
078f 2e3a      ld      l,3ah
0791 92        sub     d
0792 f654      or      54h
0794 9d        sbc     a,l
0795 08        ex      af,af'
0796 00        nop     
0797 00        nop     
0798 00        nop     
0799 00        nop     
079a 00        nop     
079b 00        nop     
079c 00        nop     
079d 00        nop     
079e 00        nop     
079f 00        nop     
07a0 00        nop     
07a1 21f800    ld      hl,00f8h
07a4 00        nop     
07a5 00        nop     
07a6 00        nop     
07a7 00        nop     
07a8 00        nop     
07a9 00        nop     
07aa 00        nop     
07ab 00        nop     
07ac 00        nop     
07ad 00        nop     
07ae 00        nop     
07af 00        nop     
07b0 00        nop     
07b1 00        nop     
07b2 00        nop     
07b3 00        nop     
07b4 00        nop     
07b5 00        nop     
07b6 00        nop     
07b7 00        nop     
07b8 00        nop     
07b9 d7        rst     10h
07ba 00        nop     
07bb 00        nop     
07bc 00        nop     
07bd ae        xor     (hl)
07be c6d4      add     a,0d4h
07c0 2c        inc     l
07c1 3c        inc     a
07c2 69        ld      l,c
07c3 6e        ld      l,(hl)
07c4 63        ld      h,e
07c5 2c        inc     l
07c6 64        ld      h,h
07c7 65        ld      h,l
07c8 63        ld      h,e
07c9 3e20      ld      a,20h
07cb 64        ld      h,h
07cc 65        ld      h,l
07cd 2e2e      ld      l,2eh
07cf 2e2e      ld      l,2eh
07d1 2e2e      ld      l,2eh
07d3 2e2e      ld      l,2eh
07d5 2e2e      ld      l,2eh
07d7 2e2e      ld      l,2eh
07d9 2e2e      ld      l,2eh
07db 2e2e      ld      l,2eh
07dd 2e2e      ld      l,2eh
07df 24        inc     h
07e0 d7        rst     10h
07e1 1c        inc     e
07e2 00        nop     
07e3 00        nop     
07e4 00        nop     
07e5 2f        cpl     
07e6 60        ld      h,b
07e7 0d        dec     c
07e8 4c        ld      c,h
07e9 02        ld      (bc),a
07ea 24        inc     h
07eb f5        push    af
07ec e2f4a0    jp      po,0a0f4h
07ef 0a        ld      a,(bc)
07f0 a1        and     c
07f1 13        inc     de
07f2 322559    ld      (5925h),a
07f5 010000    ld      bc,0000h
07f8 00        nop     
07f9 00        nop     
07fa 00        nop     
07fb 00        nop     
07fc 00        nop     
07fd 00        nop     
07fe 00        nop     
07ff 00        nop     
0800 00        nop     
0801 ff        rst     38h
0802 00        nop     
0803 00        nop     
0804 00        nop     
0805 00        nop     
0806 00        nop     
0807 00        nop     
0808 00        nop     
0809 00        nop     
080a 00        nop     
080b 00        nop     
080c 00        nop     
080d 00        nop     
080e 00        nop     
080f 00        nop     
0810 00        nop     
0811 00        nop     
0812 00        nop     
0813 00        nop     
0814 00        nop     
0815 00        nop     
0816 00        nop     
0817 00        nop     
0818 00        nop     
0819 d7        rst     10h
081a 00        nop     
081b 00        nop     
081c 00        nop     
081d e1        pop     hl
081e 75        ld      (hl),l
081f af        xor     a
0820 cc3c69    call    z,693ch
0823 6e        ld      l,(hl)
0824 63        ld      h,e
0825 2c        inc     l
0826 64        ld      h,h
0827 65        ld      h,l
0828 63        ld      h,e
0829 3e20      ld      a,20h
082b 65        ld      h,l
082c 2e2e      ld      l,2eh
082e 2e2e      ld      l,2eh
0830 2e2e      ld      l,2eh
0832 2e2e      ld      l,2eh
0834 2e2e      ld      l,2eh
0836 2e2e      ld      l,2eh
0838 2e2e      ld      l,2eh
083a 2e2e      ld      l,2eh
083c 2e2e      ld      l,2eh
083e 2e24      ld      l,24h
0840 d7        rst     10h
0841 24        inc     h
0842 00        nop     
0843 00        nop     
0844 00        nop     
0845 0615      ld      b,15h
0847 eb        ex      de,hl
0848 f2dde8    jp      p,0e8ddh
084b 2b        dec     hl
084c 26a6      ld      h,0a6h
084e 111abc    ld      de,0bc1ah
0851 17        rla     
0852 0618      ld      b,18h
0854 2801      jr      z,0857h
0856 00        nop     
0857 00        nop     
0858 00        nop     
0859 00        nop     
085a 00        nop     
085b 00        nop     
085c 00        nop     
085d 00        nop     
085e 00        nop     
085f 00        nop     
0860 ff        rst     38h
0861 00        nop     
0862 00        nop     
0863 00        nop     
0864 00        nop     
0865 00        nop     
0866 00        nop     
0867 00        nop     
0868 00        nop     
0869 00        nop     
086a 00        nop     
086b 00        nop     
086c 00        nop     
086d 00        nop     
086e 00        nop     
086f 00        nop     
0870 00        nop     
0871 00        nop     
0872 00        nop     
0873 00        nop     
0874 00        nop     
0875 00        nop     
0876 00        nop     
0877 00        nop     
0878 00        nop     
0879 d7        rst     10h
087a 00        nop     
087b 00        nop     
087c 00        nop     
087d 1c        inc     e
087e ed84      db      0edh, 84h        ; Undocumented 8 T-State NOP
0880 7d        ld      a,l
0881 3c        inc     a
0882 69        ld      l,c
0883 6e        ld      l,(hl)
0884 63        ld      h,e
0885 2c        inc     l
0886 64        ld      h,h
0887 65        ld      h,l
0888 63        ld      h,e
0889 3e20      ld      a,20h
088b 68        ld      l,b
088c 2e2e      ld      l,2eh
088e 2e2e      ld      l,2eh
0890 2e2e      ld      l,2eh
0892 2e2e      ld      l,2eh
0894 2e2e      ld      l,2eh
0896 2e2e      ld      l,2eh
0898 2e2e      ld      l,2eh
089a 2e2e      ld      l,2eh
089c 2e2e      ld      l,2eh
089e 2e24      ld      l,24h
08a0 d7        rst     10h
08a1 23        inc     hl
08a2 00        nop     
08a3 00        nop     
08a4 00        nop     
08a5 f4c3a5    call    p,0a5c3h
08a8 07        rlca    
08a9 6d        ld      l,l
08aa 1b        dec     de
08ab 04        inc     b
08ac 4f        ld      c,a
08ad c2e22a    jp      nz,2ae2h
08b0 82        add     a,d
08b1 57        ld      d,a
08b2 e0        ret     po

08b3 e1        pop     hl
08b4 c30800    jp      0008h
08b7 00        nop     
08b8 00        nop     
08b9 00        nop     
08ba 00        nop     
08bb 00        nop     
08bc 00        nop     
08bd 00        nop     
08be 00        nop     
08bf 21f800    ld      hl,00f8h
08c2 00        nop     
08c3 00        nop     
08c4 00        nop     
08c5 00        nop     
08c6 00        nop     
08c7 00        nop     
08c8 00        nop     
08c9 00        nop     
08ca 00        nop     
08cb 00        nop     
08cc 00        nop     
08cd 00        nop     
08ce 00        nop     
08cf 00        nop     
08d0 00        nop     
08d1 00        nop     
08d2 00        nop     
08d3 00        nop     
08d4 00        nop     
08d5 00        nop     
08d6 00        nop     
08d7 00        nop     
08d8 00        nop     
08d9 d7        rst     10h
08da 00        nop     
08db 00        nop     
08dc 00        nop     
08dd fc0d6d    call    m,6d0dh
08e0 4a        ld      c,d
08e1 3c        inc     a
08e2 69        ld      l,c
08e3 6e        ld      l,(hl)
08e4 63        ld      h,e
08e5 2c        inc     l
08e6 64        ld      h,h
08e7 65        ld      h,l
08e8 63        ld      h,e
08e9 3e20      ld      a,20h
08eb 68        ld      l,b
08ec 6c        ld      l,h
08ed 2e2e      ld      l,2eh
08ef 2e2e      ld      l,2eh
08f1 2e2e      ld      l,2eh
08f3 2e2e      ld      l,2eh
08f5 2e2e      ld      l,2eh
08f7 2e2e      ld      l,2eh
08f9 2e2e      ld      l,2eh
08fb 2e2e      ld      l,2eh
08fd 2e2e      ld      l,2eh
08ff 24        inc     h
0900 d7        rst     10h
0901 dd23      inc     ix
0903 00        nop     
0904 00        nop     
0905 3c        inc     a
0906 bc        cp      h
0907 9b        sbc     a,e
0908 0d        dec     c
0909 81        add     a,c
090a e0        ret     po

090b fdad      xor     iyl
090d 7f        ld      a,a
090e 9a        sbc     a,d
090f e5        push    hl
0910 96        sub     (hl)
0911 13        inc     de
0912 85        add     a,l
0913 e20b00    jp      po,000bh
0916 08        ex      af,af'
0917 00        nop     
0918 00        nop     
0919 00        nop     
091a 00        nop     
091b 00        nop     
091c 00        nop     
091d 21f800    ld      hl,00f8h
0920 00        nop     
0921 00        nop     
0922 00        nop     
0923 00        nop     
0924 00        nop     
0925 00        nop     
0926 00        nop     
0927 00        nop     
0928 00        nop     
0929 00        nop     
092a 00        nop     
092b 00        nop     
092c 00        nop     
092d 00        nop     
092e 00        nop     
092f 00        nop     
0930 00        nop     
0931 00        nop     
0932 00        nop     
0933 00        nop     
0934 00        nop     
0935 00        nop     
0936 00        nop     
0937 00        nop     
0938 00        nop     
0939 d7        rst     10h
093a 00        nop     
093b 00        nop     
093c 00        nop     
093d a5        and     l
093e 4d        ld      c,l
093f be        cp      (hl)
0940 313c69    ld      sp,693ch
0943 6e        ld      l,(hl)
0944 63        ld      h,e
0945 2c        inc     l
0946 64        ld      h,h
0947 65        ld      h,l
0948 63        ld      h,e
0949 3e20      ld      a,20h
094b 69        ld      l,c
094c 78        ld      a,b
094d 2e2e      ld      l,2eh
094f 2e2e      ld      l,2eh
0951 2e2e      ld      l,2eh
0953 2e2e      ld      l,2eh
0955 2e2e      ld      l,2eh
0957 2e2e      ld      l,2eh
0959 2e2e      ld      l,2eh
095b 2e2e      ld      l,2eh
095d 2e2e      ld      l,2eh
095f 24        inc     h
0960 d7        rst     10h
0961 fd23      inc     iy
0963 00        nop     
0964 00        nop     
0965 02        ld      (bc),a
0966 94        sub     h
0967 7a        ld      a,d
0968 63        ld      h,e
0969 82        add     a,d
096a 315ac6    ld      sp,0c65ah
096d e9        jp      (hl)
096e b2        or      d
096f b4        or      h
0970 ab        xor     e
0971 16f2      ld      d,0f2h
0973 05        dec     b
0974 6d        ld      l,l
0975 00        nop     
0976 08        ex      af,af'
0977 00        nop     
0978 00        nop     
0979 00        nop     
097a 00        nop     
097b 21f800    ld      hl,00f8h
097e 00        nop     
097f 00        nop     
0980 00        nop     
0981 00        nop     
0982 00        nop     
0983 00        nop     
0984 00        nop     
0985 00        nop     
0986 00        nop     
0987 00        nop     
0988 00        nop     
0989 00        nop     
098a 00        nop     
098b 00        nop     
098c 00        nop     
098d 00        nop     
098e 00        nop     
098f 00        nop     
0990 00        nop     
0991 00        nop     
0992 00        nop     
0993 00        nop     
0994 00        nop     
0995 00        nop     
0996 00        nop     
0997 00        nop     
0998 00        nop     
0999 d7        rst     10h
099a 00        nop     
099b 00        nop     
099c 00        nop     
099d 50        ld      d,b
099e 5d        ld      e,l
099f 51        ld      d,c
09a0 a3        and     e
09a1 3c        inc     a
09a2 69        ld      l,c
09a3 6e        ld      l,(hl)
09a4 63        ld      h,e
09a5 2c        inc     l
09a6 64        ld      h,h
09a7 65        ld      h,l
09a8 63        ld      h,e
09a9 3e20      ld      a,20h
09ab 69        ld      l,c
09ac 79        ld      a,c
09ad 2e2e      ld      l,2eh
09af 2e2e      ld      l,2eh
09b1 2e2e      ld      l,2eh
09b3 2e2e      ld      l,2eh
09b5 2e2e      ld      l,2eh
09b7 2e2e      ld      l,2eh
09b9 2e2e      ld      l,2eh
09bb 2e2e      ld      l,2eh
09bd 2e2e      ld      l,2eh
09bf 24        inc     h
09c0 d7        rst     10h
09c1 2c        inc     l
09c2 00        nop     
09c3 00        nop     
09c4 00        nop     
09c5 318020    ld      sp,2080h
09c8 a5        and     l
09c9 56        ld      d,(hl)
09ca 43        ld      b,e
09cb 09        add     hl,bc
09cc b4        or      h
09cd c1        pop     bc
09ce f4a2df    call    p,0dfa2h
09d1 d1        pop     de
09d2 3c        inc     a
09d3 a2        and     d
09d4 3e01      ld      a,01h
09d6 00        nop     
09d7 00        nop     
09d8 00        nop     
09d9 00        nop     
09da 00        nop     
09db 00        nop     
09dc 00        nop     
09dd 00        nop     
09de 00        nop     
09df ff        rst     38h
09e0 00        nop     
09e1 00        nop     
09e2 00        nop     
09e3 00        nop     
09e4 00        nop     
09e5 00        nop     
09e6 00        nop     
09e7 00        nop     
09e8 00        nop     
09e9 00        nop     
09ea 00        nop     
09eb 00        nop     
09ec 00        nop     
09ed 00        nop     
09ee 00        nop     
09ef 00        nop     
09f0 00        nop     
09f1 00        nop     
09f2 00        nop     
09f3 00        nop     
09f4 00        nop     
09f5 00        nop     
09f6 00        nop     
09f7 00        nop     
09f8 00        nop     
09f9 d7        rst     10h
09fa 00        nop     
09fb 00        nop     
09fc 00        nop     
09fd 56        ld      d,(hl)
09fe cd06f3    call    0f306h
0a01 3c        inc     a
0a02 69        ld      l,c
0a03 6e        ld      l,(hl)
0a04 63        ld      h,e
0a05 2c        inc     l
0a06 64        ld      h,h
0a07 65        ld      h,l
0a08 63        ld      h,e
0a09 3e20      ld      a,20h
0a0b 6c        ld      l,h
0a0c 2e2e      ld      l,2eh
0a0e 2e2e      ld      l,2eh
0a10 2e2e      ld      l,2eh
0a12 2e2e      ld      l,2eh
0a14 2e2e      ld      l,2eh
0a16 2e2e      ld      l,2eh
0a18 2e2e      ld      l,2eh
0a1a 2e2e      ld      l,2eh
0a1c 2e2e      ld      l,2eh
0a1e 2e24      ld      l,24h
0a20 d7        rst     10h
0a21 34        inc     (hl)
0a22 00        nop     
0a23 00        nop     
0a24 00        nop     
0a25 56        ld      d,(hl)
0a26 b8        cp      b
0a27 7c        ld      a,h
0a28 0c        inc     c
0a29 3ee5      ld      a,0e5h
0a2b 03        inc     bc
0a2c 017e87    ld      bc,877eh
0a2f 58        ld      e,b
0a30 da155c    jp      c,5c15h
0a33 37        scf     
0a34 1f        rra     
0a35 010000    ld      bc,0000h
0a38 00        nop     
0a39 ff        rst     38h
0a3a 00        nop     
0a3b 00        nop     
0a3c 00        nop     
0a3d 00        nop     
0a3e 00        nop     
0a3f 00        nop     
0a40 00        nop     
0a41 00        nop     
0a42 00        nop     
0a43 00        nop     
0a44 00        nop     
0a45 00        nop     
0a46 00        nop     
0a47 00        nop     
0a48 00        nop     
0a49 00        nop     
0a4a 00        nop     
0a4b 00        nop     
0a4c 00        nop     
0a4d 00        nop     
0a4e 00        nop     
0a4f 00        nop     
0a50 00        nop     
0a51 00        nop     
0a52 00        nop     
0a53 00        nop     
0a54 00        nop     
0a55 00        nop     
0a56 00        nop     
0a57 00        nop     
0a58 00        nop     
0a59 d7        rst     10h
0a5a 00        nop     
0a5b 00        nop     
0a5c 00        nop     
0a5d b8        cp      b
0a5e 3adcef    ld      a,(0efdch)
0a61 3c        inc     a
0a62 69        ld      l,c
0a63 6e        ld      l,(hl)
0a64 63        ld      h,e
0a65 2c        inc     l
0a66 64        ld      h,h
0a67 65        ld      h,l
0a68 63        ld      h,e
0a69 3e20      ld      a,20h
0a6b 2868      jr      z,0ad5h
0a6d 6c        ld      l,h
0a6e 29        add     hl,hl
0a6f 2e2e      ld      l,2eh
0a71 2e2e      ld      l,2eh
0a73 2e2e      ld      l,2eh
0a75 2e2e      ld      l,2eh
0a77 2e2e      ld      l,2eh
0a79 2e2e      ld      l,2eh
0a7b 2e2e      ld      l,2eh
0a7d 2e2e      ld      l,2eh
0a7f 24        inc     h
0a80 d7        rst     10h
0a81 33        inc     sp
0a82 00        nop     
0a83 00        nop     
0a84 00        nop     
0a85 6f        ld      l,a
0a86 34        inc     (hl)
0a87 82        add     a,d
0a88 d469d1    call    nc,0d169h
0a8b b6        or      (hl)
0a8c de94      sbc     a,94h
0a8e a4        and     h
0a8f 76        halt    
0a90 f45302    call    p,0253h
0a93 5b        ld      e,e
0a94 85        add     a,l
0a95 08        ex      af,af'
0a96 00        nop     
0a97 00        nop     
0a98 00        nop     
0a99 00        nop     
0a9a 00        nop     
0a9b 00        nop     
0a9c 00        nop     
0a9d 00        nop     
0a9e 00        nop     
0a9f 00        nop     
0aa0 00        nop     
0aa1 00        nop     
0aa2 00        nop     
0aa3 00        nop     
0aa4 00        nop     
0aa5 00        nop     
0aa6 00        nop     
0aa7 21f800    ld      hl,00f8h
0aaa 00        nop     
0aab 00        nop     
0aac 00        nop     
0aad 00        nop     
0aae 00        nop     
0aaf 00        nop     
0ab0 00        nop     
0ab1 00        nop     
0ab2 00        nop     
0ab3 00        nop     
0ab4 00        nop     
0ab5 00        nop     
0ab6 00        nop     
0ab7 00        nop     
0ab8 00        nop     
0ab9 d7        rst     10h
0aba 00        nop     
0abb 00        nop     
0abc 00        nop     
0abd 5d        ld      e,l
0abe ac        xor     h
0abf d5        push    de
0ac0 27        daa     
0ac1 3c        inc     a
0ac2 69        ld      l,c
0ac3 6e        ld      l,(hl)
0ac4 63        ld      h,e
0ac5 2c        inc     l
0ac6 64        ld      h,h
0ac7 65        ld      h,l
0ac8 63        ld      h,e
0ac9 3e20      ld      a,20h
0acb 73        ld      (hl),e
0acc 70        ld      (hl),b
0acd 2e2e      ld      l,2eh
0acf 2e2e      ld      l,2eh
0ad1 2e2e      ld      l,2eh
0ad3 2e2e      ld      l,2eh
0ad5 2e2e      ld      l,2eh
0ad7 2e2e      ld      l,2eh
0ad9 2e2e      ld      l,2eh
0adb 2e2e      ld      l,2eh
0add 2e2e      ld      l,2eh
0adf 24        inc     h
0ae0 d7        rst     10h
0ae1 dd3401    inc     (ix+01h)
0ae4 00        nop     
0ae5 6e        ld      l,(hl)
0ae6 fa0201    jp      m,0102h
0ae9 02        ld      (bc),a
0aea 01282c    ld      bc,2c28h
0aed 94        sub     h
0aee 88        adc     a,b
0aef 57        ld      d,a
0af0 50        ld      d,b
0af1 1633      ld      d,33h
0af3 6f        ld      l,a
0af4 2820      jr      z,0b16h
0af6 010000    ld      bc,0000h
0af9 ff        rst     38h
0afa 00        nop     
0afb 00        nop     
0afc 00        nop     
0afd 00        nop     
0afe 00        nop     
0aff 00        nop     
0b00 00        nop     
0b01 00        nop     
0b02 00        nop     
0b03 00        nop     
0b04 00        nop     
0b05 00        nop     
0b06 00        nop     
0b07 00        nop     
0b08 00        nop     
0b09 00        nop     
0b0a 00        nop     
0b0b 00        nop     
0b0c 00        nop     
0b0d 00        nop     
0b0e 00        nop     
0b0f 00        nop     
0b10 00        nop     
0b11 00        nop     
0b12 00        nop     
0b13 00        nop     
0b14 00        nop     
0b15 00        nop     
0b16 00        nop     
0b17 00        nop     
0b18 00        nop     
0b19 d7        rst     10h
0b1a 00        nop     
0b1b 00        nop     
0b1c 00        nop     
0b1d 2058      jr      nz,0b77h
0b1f 14        inc     d
0b20 70        ld      (hl),b
0b21 3c        inc     a
0b22 69        ld      l,c
0b23 6e        ld      l,(hl)
0b24 63        ld      h,e
0b25 2c        inc     l
0b26 64        ld      h,h
0b27 65        ld      h,l
0b28 63        ld      h,e
0b29 3e20      ld      a,20h
0b2b 283c      jr      z,0b69h
0b2d 69        ld      l,c
0b2e 78        ld      a,b
0b2f 2c        inc     l
0b30 69        ld      l,c
0b31 79        ld      a,c
0b32 3e2b      ld      a,2bh
0b34 31292e    ld      sp,2e29h
0b37 2e2e      ld      l,2eh
0b39 2e2e      ld      l,2eh
0b3b 2e2e      ld      l,2eh
0b3d 2e2e      ld      l,2eh
0b3f 24        inc     h
0b40 d7        rst     10h
0b41 dd24      inc     ixh
0b43 00        nop     
0b44 00        nop     
0b45 38b8      jr      c,0affh
0b47 6c        ld      l,h
0b48 31d4c6    ld      sp,0c6d4h
0b4b 013e58    ld      bc,583eh
0b4e 83        add     a,e
0b4f b4        or      h
0b50 15        dec     d
0b51 81        add     a,c
0b52 de59      sbc     a,59h
0b54 42        ld      b,d
0b55 00        nop     
0b56 010000    ld      bc,0000h
0b59 00        nop     
0b5a 00        nop     
0b5b 00        nop     
0b5c ff        rst     38h
0b5d 00        nop     
0b5e 00        nop     
0b5f 00        nop     
0b60 00        nop     
0b61 00        nop     
0b62 00        nop     
0b63 00        nop     
0b64 00        nop     
0b65 00        nop     
0b66 00        nop     
0b67 00        nop     
0b68 00        nop     
0b69 00        nop     
0b6a 00        nop     
0b6b 00        nop     
0b6c 00        nop     
0b6d 00        nop     
0b6e 00        nop     
0b6f 00        nop     
0b70 00        nop     
0b71 00        nop     
0b72 00        nop     
0b73 00        nop     
0b74 00        nop     
0b75 00        nop     
0b76 00        nop     
0b77 00        nop     
0b78 00        nop     
0b79 d7        rst     10h
0b7a 00        nop     
0b7b 00        nop     
0b7c 00        nop     
0b7d 6f        ld      l,a
0b7e 46        ld      b,(hl)
0b7f 3662      ld      (hl),62h
0b81 3c        inc     a
0b82 69        ld      l,c
0b83 6e        ld      l,(hl)
0b84 63        ld      h,e
0b85 2c        inc     l
0b86 64        ld      h,h
0b87 65        ld      h,l
0b88 63        ld      h,e
0b89 3e20      ld      a,20h
0b8b 69        ld      l,c
0b8c 78        ld      a,b
0b8d 68        ld      l,b
0b8e 2e2e      ld      l,2eh
0b90 2e2e      ld      l,2eh
0b92 2e2e      ld      l,2eh
0b94 2e2e      ld      l,2eh
0b96 2e2e      ld      l,2eh
0b98 2e2e      ld      l,2eh
0b9a 2e2e      ld      l,2eh
0b9c 2e2e      ld      l,2eh
0b9e 2e24      ld      l,24h
0ba0 d7        rst     10h
0ba1 dd2c      inc     ixl
0ba3 00        nop     
0ba4 00        nop     
0ba5 14        inc     d
0ba6 4d        ld      c,l
0ba7 60        ld      h,b
0ba8 74        ld      (hl),h
0ba9 d476e7    call    nc,0e776h
0bac 06a2      ld      b,0a2h
0bae 323c21    ld      (213ch),a
0bb1 d6d7      sub     0d7h
0bb3 a5        and     l
0bb4 99        sbc     a,c
0bb5 00        nop     
0bb6 010000    ld      bc,0000h
0bb9 00        nop     
0bba 00        nop     
0bbb ff        rst     38h
0bbc 00        nop     
0bbd 00        nop     
0bbe 00        nop     
0bbf 00        nop     
0bc0 00        nop     
0bc1 00        nop     
0bc2 00        nop     
0bc3 00        nop     
0bc4 00        nop     
0bc5 00        nop     
0bc6 00        nop     
0bc7 00        nop     
0bc8 00        nop     
0bc9 00        nop     
0bca 00        nop     
0bcb 00        nop     
0bcc 00        nop     
0bcd 00        nop     
0bce 00        nop     
0bcf 00        nop     
0bd0 00        nop     
0bd1 00        nop     
0bd2 00        nop     
0bd3 00        nop     
0bd4 00        nop     
0bd5 00        nop     
0bd6 00        nop     
0bd7 00        nop     
0bd8 00        nop     
0bd9 d7        rst     10h
0bda 00        nop     
0bdb 00        nop     
0bdc 00        nop     
0bdd 02        ld      (bc),a
0bde 7b        ld      a,e
0bdf ef        rst     28h
0be0 2c        inc     l
0be1 3c        inc     a
0be2 69        ld      l,c
0be3 6e        ld      l,(hl)
0be4 63        ld      h,e
0be5 2c        inc     l
0be6 64        ld      h,h
0be7 65        ld      h,l
0be8 63        ld      h,e
0be9 3e20      ld      a,20h
0beb 69        ld      l,c
0bec 78        ld      a,b
0bed 6c        ld      l,h
0bee 2e2e      ld      l,2eh
0bf0 2e2e      ld      l,2eh
0bf2 2e2e      ld      l,2eh
0bf4 2e2e      ld      l,2eh
0bf6 2e2e      ld      l,2eh
0bf8 2e2e      ld      l,2eh
0bfa 2e2e      ld      l,2eh
0bfc 2e2e      ld      l,2eh
0bfe 2e24      ld      l,24h
0c00 d7        rst     10h
0c01 dd24      inc     ixh
0c03 00        nop     
0c04 00        nop     
0c05 3628      ld      (hl),28h
0c07 6f        ld      l,a
0c08 9f        sbc     a,a
0c09 1691      ld      d,91h
0c0b b9        cp      c
0c0c 61        ld      h,c
0c0d cb82      res     0,d
0c0f 19        add     hl,de
0c10 e29273    jp      po,7392h
0c13 8c        adc     a,h
0c14 a9        xor     c
0c15 00        nop     
0c16 010000    ld      bc,0000h
0c19 00        nop     
0c1a ff        rst     38h
0c1b 00        nop     
0c1c 00        nop     
0c1d 00        nop     
0c1e 00        nop     
0c1f 00        nop     
0c20 00        nop     
0c21 00        nop     
0c22 00        nop     
0c23 00        nop     
0c24 00        nop     
0c25 00        nop     
0c26 00        nop     
0c27 00        nop     
0c28 00        nop     
0c29 00        nop     
0c2a 00        nop     
0c2b 00        nop     
0c2c 00        nop     
0c2d 00        nop     
0c2e 00        nop     
0c2f 00        nop     
0c30 00        nop     
0c31 00        nop     
0c32 00        nop     
0c33 00        nop     
0c34 00        nop     
0c35 00        nop     
0c36 00        nop     
0c37 00        nop     
0c38 00        nop     
0c39 d7        rst     10h
0c3a 00        nop     
0c3b 00        nop     
0c3c 00        nop     
0c3d 2d        dec     l
0c3e 96        sub     (hl)
0c3f 6c        ld      l,h
0c40 f3        di      
0c41 3c        inc     a
0c42 69        ld      l,c
0c43 6e        ld      l,(hl)
0c44 63        ld      h,e
0c45 2c        inc     l
0c46 64        ld      h,h
0c47 65        ld      h,l
0c48 63        ld      h,e
0c49 3e20      ld      a,20h
0c4b 69        ld      l,c
0c4c 79        ld      a,c
0c4d 68        ld      l,b
0c4e 2e2e      ld      l,2eh
0c50 2e2e      ld      l,2eh
0c52 2e2e      ld      l,2eh
0c54 2e2e      ld      l,2eh
0c56 2e2e      ld      l,2eh
0c58 2e2e      ld      l,2eh
0c5a 2e2e      ld      l,2eh
0c5c 2e2e      ld      l,2eh
0c5e 2e24      ld      l,24h
0c60 d7        rst     10h
0c61 dd2c      inc     ixl
0c63 00        nop     
0c64 00        nop     
0c65 c6d7      add     a,0d7h
0c67 d5        push    de
0c68 62        ld      h,d
0c69 9e        sbc     a,(hl)
0c6a a0        and     b
0c6b 39        add     hl,sp
0c6c 70        ld      (hl),b
0c6d 7e        ld      a,(hl)
0c6e 3e12      ld      a,12h
0c70 9f        sbc     a,a
0c71 90        sub     b
0c72 d9        exx     
0c73 0f        rrca    
0c74 220001    ld      (0100h),hl
0c77 00        nop     
0c78 00        nop     
0c79 ff        rst     38h
0c7a 00        nop     
0c7b 00        nop     
0c7c 00        nop     
0c7d 00        nop     
0c7e 00        nop     
0c7f 00        nop     
0c80 00        nop     
0c81 00        nop     
0c82 00        nop     
0c83 00        nop     
0c84 00        nop     
0c85 00        nop     
0c86 00        nop     
0c87 00        nop     
0c88 00        nop     
0c89 00        nop     
0c8a 00        nop     
0c8b 00        nop     
0c8c 00        nop     
0c8d 00        nop     
0c8e 00        nop     
0c8f 00        nop     
0c90 00        nop     
0c91 00        nop     
0c92 00        nop     
0c93 00        nop     
0c94 00        nop     
0c95 00        nop     
0c96 00        nop     
0c97 00        nop     
0c98 00        nop     
0c99 d7        rst     10h
0c9a 00        nop     
0c9b 00        nop     
0c9c 00        nop     
0c9d fb        ei      
0c9e cbba      res     7,d
0ca0 95        sub     l
0ca1 3c        inc     a
0ca2 69        ld      l,c
0ca3 6e        ld      l,(hl)
0ca4 63        ld      h,e
0ca5 2c        inc     l
0ca6 64        ld      h,h
0ca7 65        ld      h,l
0ca8 63        ld      h,e
0ca9 3e20      ld      a,20h
0cab 69        ld      l,c
0cac 79        ld      a,c
0cad 6c        ld      l,h
0cae 2e2e      ld      l,2eh
0cb0 2e2e      ld      l,2eh
0cb2 2e2e      ld      l,2eh
0cb4 2e2e      ld      l,2eh
0cb6 2e2e      ld      l,2eh
0cb8 2e2e      ld      l,2eh
0cba 2e2e      ld      l,2eh
0cbc 2e2e      ld      l,2eh
0cbe 2e24      ld      l,24h
0cc0 d7        rst     10h
0cc1 ed4b0301  ld      bc,(0103h)
0cc5 a8        xor     b
0cc6 f9        ld      sp,hl
0cc7 59        ld      e,c
0cc8 f5        push    af
0cc9 a4        and     h
0cca 93        sub     e
0ccb edf5      db      0edh, 0f5h       ; Undocumented 8 T-State NOP
0ccd 96        sub     (hl)
0cce 6f        ld      l,a
0ccf 68        ld      l,b
0cd0 d9        exx     
0cd1 86        add     a,(hl)
0cd2 e6d8      and     0d8h
0cd4 4b        ld      c,e
0cd5 00        nop     
0cd6 1000      djnz    0cd8h
0cd8 00        nop     
0cd9 00        nop     
0cda 00        nop     
0cdb 00        nop     
0cdc 00        nop     
0cdd 00        nop     
0cde 00        nop     
0cdf 00        nop     
0ce0 00        nop     
0ce1 00        nop     
0ce2 00        nop     
0ce3 00        nop     
0ce4 00        nop     
0ce5 00        nop     
0ce6 00        nop     
0ce7 00        nop     
0ce8 00        nop     
0ce9 00        nop     
0cea 00        nop     
0ceb 00        nop     
0cec 00        nop     
0ced ff        rst     38h
0cee ff        rst     38h
0cef 00        nop     
0cf0 00        nop     
0cf1 00        nop     
0cf2 00        nop     
0cf3 00        nop     
0cf4 00        nop     
0cf5 00        nop     
0cf6 00        nop     
0cf7 00        nop     
0cf8 00        nop     
0cf9 00        nop     
0cfa 00        nop     
0cfb 00        nop     
0cfc 00        nop     
0cfd 4d        ld      c,l
0cfe 45        ld      b,l
0cff a9        xor     c
0d00 ac        xor     h
0d01 6c        ld      l,h
0d02 64        ld      h,h
0d03 203c      jr      nz,0d41h
0d05 62        ld      h,d
0d06 63        ld      h,e
0d07 2c        inc     l
0d08 64        ld      h,h
0d09 65        ld      h,l
0d0a 3e2c      ld      a,2ch
0d0c 286e      jr      z,0d7ch
0d0e 6e        ld      l,(hl)
0d0f 6e        ld      l,(hl)
0d10 6e        ld      l,(hl)
0d11 29        add     hl,hl
0d12 2e2e      ld      l,2eh
0d14 2e2e      ld      l,2eh
0d16 2e2e      ld      l,2eh
0d18 2e2e      ld      l,2eh
0d1a 2e2e      ld      l,2eh
0d1c 2e2e      ld      l,2eh
0d1e 2e24      ld      l,24h
0d20 d7        rst     10h
0d21 2a0301    ld      hl,(0103h)
0d24 00        nop     
0d25 63        ld      h,e
0d26 98        sbc     a,b
0d27 3078      jr      nc,0da1h
0d29 77        ld      (hl),a
0d2a 20fe      jr      nz,0d2ah
0d2c b1        or      c
0d2d fab9b8    jp      m,0b8b9h
0d30 ab        xor     e
0d31 04        inc     b
0d32 0615      ld      b,15h
0d34 60        ld      h,b
0d35 00        nop     
0d36 00        nop     
0d37 00        nop     
0d38 00        nop     
0d39 00        nop     
0d3a 00        nop     
0d3b 00        nop     
0d3c 00        nop     
0d3d 00        nop     
0d3e 00        nop     
0d3f 00        nop     
0d40 00        nop     
0d41 00        nop     
0d42 00        nop     
0d43 00        nop     
0d44 00        nop     
0d45 00        nop     
0d46 00        nop     
0d47 00        nop     
0d48 00        nop     
0d49 00        nop     
0d4a 00        nop     
0d4b 00        nop     
0d4c 00        nop     
0d4d ff        rst     38h
0d4e ff        rst     38h
0d4f 00        nop     
0d50 00        nop     
0d51 00        nop     
0d52 00        nop     
0d53 00        nop     
0d54 00        nop     
0d55 00        nop     
0d56 00        nop     
0d57 00        nop     
0d58 00        nop     
0d59 00        nop     
0d5a 00        nop     
0d5b 00        nop     
0d5c 00        nop     
0d5d 5f        ld      e,a
0d5e 97        sub     a
0d5f 24        inc     h
0d60 87        add     a,a
0d61 6c        ld      l,h
0d62 64        ld      h,h
0d63 2068      jr      nz,0dcdh
0d65 6c        ld      l,h
0d66 2c        inc     l
0d67 286e      jr      z,0dd7h
0d69 6e        ld      l,(hl)
0d6a 6e        ld      l,(hl)
0d6b 6e        ld      l,(hl)
0d6c 29        add     hl,hl
0d6d 2e2e      ld      l,2eh
0d6f 2e2e      ld      l,2eh
0d71 2e2e      ld      l,2eh
0d73 2e2e      ld      l,2eh
0d75 2e2e      ld      l,2eh
0d77 2e2e      ld      l,2eh
0d79 2e2e      ld      l,2eh
0d7b 2e2e      ld      l,2eh
0d7d 2e2e      ld      l,2eh
0d7f 24        inc     h
0d80 d7        rst     10h
0d81 ed7b0301  ld      sp,(0103h)
0d85 fc8dd7    call    m,0d78dh
0d88 57        ld      d,a
0d89 61        ld      h,c
0d8a 2118ca    ld      hl,0ca18h
0d8d 85        add     a,l
0d8e c1        pop     bc
0d8f da2783    jp      c,8327h
0d92 1e60      ld      e,60h
0d94 f40000    call    p,0000h
0d97 00        nop     
0d98 00        nop     
0d99 00        nop     
0d9a 00        nop     
0d9b 00        nop     
0d9c 00        nop     
0d9d 00        nop     
0d9e 00        nop     
0d9f 00        nop     
0da0 00        nop     
0da1 00        nop     
0da2 00        nop     
0da3 00        nop     
0da4 00        nop     
0da5 00        nop     
0da6 00        nop     
0da7 00        nop     
0da8 00        nop     
0da9 00        nop     
0daa 00        nop     
0dab 00        nop     
0dac 00        nop     
0dad ff        rst     38h
0dae ff        rst     38h
0daf 00        nop     
0db0 00        nop     
0db1 00        nop     
0db2 00        nop     
0db3 00        nop     
0db4 00        nop     
0db5 00        nop     
0db6 00        nop     
0db7 00        nop     
0db8 00        nop     
0db9 00        nop     
0dba 00        nop     
0dbb 00        nop     
0dbc 00        nop     
0dbd 7a        ld      a,d
0dbe cea1      adc     a,0a1h
0dc0 1b        dec     de
0dc1 6c        ld      l,h
0dc2 64        ld      h,h
0dc3 2073      jr      nz,0e38h
0dc5 70        ld      (hl),b
0dc6 2c        inc     l
0dc7 286e      jr      z,0e37h
0dc9 6e        ld      l,(hl)
0dca 6e        ld      l,(hl)
0dcb 6e        ld      l,(hl)
0dcc 29        add     hl,hl
0dcd 2e2e      ld      l,2eh
0dcf 2e2e      ld      l,2eh
0dd1 2e2e      ld      l,2eh
0dd3 2e2e      ld      l,2eh
0dd5 2e2e      ld      l,2eh
0dd7 2e2e      ld      l,2eh
0dd9 2e2e      ld      l,2eh
0ddb 2e2e      ld      l,2eh
0ddd 2e2e      ld      l,2eh
0ddf 24        inc     h
0de0 d7        rst     10h
0de1 dd2a0301  ld      ix,(0103h)
0de5 d7        rst     10h
0de6 defa      sbc     a,0fah
0de8 a6        and     (hl)
0de9 80        add     a,b
0dea f7        rst     30h
0deb 4c        ld      c,h
0dec 24        inc     h
0ded de87      sbc     a,87h
0def c2bc16    jp      nz,16bch
0df2 63        ld      h,e
0df3 96        sub     (hl)
0df4 4c        ld      c,h
0df5 2000      jr      nz,0df7h
0df7 00        nop     
0df8 00        nop     
0df9 00        nop     
0dfa 00        nop     
0dfb 00        nop     
0dfc 00        nop     
0dfd 00        nop     
0dfe 00        nop     
0dff 00        nop     
0e00 00        nop     
0e01 00        nop     
0e02 00        nop     
0e03 00        nop     
0e04 00        nop     
0e05 00        nop     
0e06 00        nop     
0e07 00        nop     
0e08 00        nop     
0e09 00        nop     
0e0a 00        nop     
0e0b 00        nop     
0e0c 00        nop     
0e0d ff        rst     38h
0e0e ff        rst     38h
0e0f 00        nop     
0e10 00        nop     
0e11 00        nop     
0e12 00        nop     
0e13 00        nop     
0e14 00        nop     
0e15 00        nop     
0e16 00        nop     
0e17 00        nop     
0e18 00        nop     
0e19 00        nop     
0e1a 00        nop     
0e1b 00        nop     
0e1c 00        nop     
0e1d 85        add     a,l
0e1e 8b        adc     a,e
0e1f f1        pop     af
0e20 6d        ld      l,l
0e21 6c        ld      l,h
0e22 64        ld      h,h
0e23 203c      jr      nz,0e61h
0e25 69        ld      l,c
0e26 78        ld      a,b
0e27 2c        inc     l
0e28 69        ld      l,c
0e29 79        ld      a,c
0e2a 3e2c      ld      a,2ch
0e2c 286e      jr      z,0e9ch
0e2e 6e        ld      l,(hl)
0e2f 6e        ld      l,(hl)
0e30 6e        ld      l,(hl)
0e31 29        add     hl,hl
0e32 2e2e      ld      l,2eh
0e34 2e2e      ld      l,2eh
0e36 2e2e      ld      l,2eh
0e38 2e2e      ld      l,2eh
0e3a 2e2e      ld      l,2eh
0e3c 2e2e      ld      l,2eh
0e3e 2e24      ld      l,24h
0e40 d7        rst     10h
0e41 ed430301  ld      (0103h),bc
0e45 98        sbc     a,b
0e46 1f        rra     
0e47 4d        ld      c,l
0e48 84        add     a,h
0e49 ac        xor     h
0e4a e8        ret     pe

0e4b edc9      db      0edh, 0c9h       ; Undocumented 8 T-State NOP
0e4d 5d        ld      e,l
0e4e c9        ret     

0e4f 61        ld      h,c
0e50 8f        adc     a,a
0e51 80        add     a,b
0e52 3f        ccf     
0e53 bf        cp      a
0e54 c7        rst     00h
0e55 00        nop     
0e56 1000      djnz    0e58h
0e58 00        nop     
0e59 00        nop     
0e5a 00        nop     
0e5b 00        nop     
0e5c 00        nop     
0e5d 00        nop     
0e5e 00        nop     
0e5f 00        nop     
0e60 00        nop     
0e61 00        nop     
0e62 00        nop     
0e63 00        nop     
0e64 00        nop     
0e65 00        nop     
0e66 00        nop     
0e67 00        nop     
0e68 00        nop     
0e69 00        nop     
0e6a 00        nop     
0e6b 00        nop     
0e6c 00        nop     
0e6d 00        nop     
0e6e 00        nop     
0e6f 00        nop     
0e70 00        nop     
0e71 00        nop     
0e72 00        nop     
0e73 00        nop     
0e74 00        nop     
0e75 ff        rst     38h
0e76 ff        rst     38h
0e77 ff        rst     38h
0e78 ff        rst     38h
0e79 00        nop     
0e7a 00        nop     
0e7b 00        nop     
0e7c 00        nop     
0e7d 64        ld      h,h
0e7e 1e87      ld      e,87h
0e80 15        dec     d
0e81 6c        ld      l,h
0e82 64        ld      h,h
0e83 2028      jr      nz,0eadh
0e85 6e        ld      l,(hl)
0e86 6e        ld      l,(hl)
0e87 6e        ld      l,(hl)
0e88 6e        ld      l,(hl)
0e89 29        add     hl,hl
0e8a 2c        inc     l
0e8b 3c        inc     a
0e8c 62        ld      h,d
0e8d 63        ld      h,e
0e8e 2c        inc     l
0e8f 64        ld      h,h
0e90 65        ld      h,l
0e91 3e2e      ld      a,2eh
0e93 2e2e      ld      l,2eh
0e95 2e2e      ld      l,2eh
0e97 2e2e      ld      l,2eh
0e99 2e2e      ld      l,2eh
0e9b 2e2e      ld      l,2eh
0e9d 2e2e      ld      l,2eh
0e9f 24        inc     h
0ea0 d7        rst     10h
0ea1 220301    ld      (0103h),hl
0ea4 00        nop     
0ea5 03        inc     bc
0ea6 d0        ret     nc

0ea7 72        ld      (hl),d
0ea8 77        ld      (hl),a
0ea9 53        ld      d,e
0eaa 7f        ld      a,a
0eab 72        ld      (hl),d
0eac 3f        ccf     
0ead ea6480    jp      pe,8064h
0eb0 e1        pop     hl
0eb1 102d      djnz    0ee0h
0eb3 e9        jp      (hl)
0eb4 35        dec     (hl)
0eb5 00        nop     
0eb6 00        nop     
0eb7 00        nop     
0eb8 00        nop     
0eb9 00        nop     
0eba 00        nop     
0ebb 00        nop     
0ebc 00        nop     
0ebd 00        nop     
0ebe 00        nop     
0ebf 00        nop     
0ec0 00        nop     
0ec1 00        nop     
0ec2 00        nop     
0ec3 00        nop     
0ec4 00        nop     
0ec5 00        nop     
0ec6 00        nop     
0ec7 00        nop     
0ec8 00        nop     
0ec9 00        nop     
0eca 00        nop     
0ecb 00        nop     
0ecc 00        nop     
0ecd 00        nop     
0ece 00        nop     
0ecf 00        nop     
0ed0 00        nop     
0ed1 00        nop     
0ed2 00        nop     
0ed3 ff        rst     38h
0ed4 ff        rst     38h
0ed5 00        nop     
0ed6 00        nop     
0ed7 00        nop     
0ed8 00        nop     
0ed9 00        nop     
0eda 00        nop     
0edb 00        nop     
0edc 00        nop     
0edd a3        and     e
0ede 60        ld      h,b
0edf 8b        adc     a,e
0ee0 47        ld      b,a
0ee1 6c        ld      l,h
0ee2 64        ld      h,h
0ee3 2028      jr      nz,0f0dh
0ee5 6e        ld      l,(hl)
0ee6 6e        ld      l,(hl)
0ee7 6e        ld      l,(hl)
0ee8 6e        ld      l,(hl)
0ee9 29        add     hl,hl
0eea 2c        inc     l
0eeb 68        ld      l,b
0eec 6c        ld      l,h
0eed 2e2e      ld      l,2eh
0eef 2e2e      ld      l,2eh
0ef1 2e2e      ld      l,2eh
0ef3 2e2e      ld      l,2eh
0ef5 2e2e      ld      l,2eh
0ef7 2e2e      ld      l,2eh
0ef9 2e2e      ld      l,2eh
0efb 2e2e      ld      l,2eh
0efd 2e2e      ld      l,2eh
0eff 24        inc     h
0f00 d7        rst     10h
0f01 ed730301  ld      (0103h),sp
0f05 dcc0d6    call    c,0d6c0h
0f08 d1        pop     de
0f09 5a        ld      e,d
0f0a ed56      im      1
0f0c f3        di      
0f0d daafa7    jp      c,0a7afh
0f10 6c        ld      l,h
0f11 44        ld      b,h
0f12 9f        sbc     a,a
0f13 0a        ld      a,(bc)
0f14 3f        ccf     
0f15 00        nop     
0f16 00        nop     
0f17 00        nop     
0f18 00        nop     
0f19 00        nop     
0f1a 00        nop     
0f1b 00        nop     
0f1c 00        nop     
0f1d 00        nop     
0f1e 00        nop     
0f1f 00        nop     
0f20 00        nop     
0f21 00        nop     
0f22 00        nop     
0f23 00        nop     
0f24 00        nop     
0f25 00        nop     
0f26 00        nop     
0f27 00        nop     
0f28 00        nop     
0f29 00        nop     
0f2a 00        nop     
0f2b 00        nop     
0f2c 00        nop     
0f2d 00        nop     
0f2e 00        nop     
0f2f 00        nop     
0f30 00        nop     
0f31 00        nop     
0f32 00        nop     
0f33 00        nop     
0f34 00        nop     
0f35 00        nop     
0f36 00        nop     
0f37 00        nop     
0f38 00        nop     
0f39 00        nop     
0f3a 00        nop     
0f3b ff        rst     38h
0f3c ff        rst     38h
0f3d 1658      ld      d,58h
0f3f 5f        ld      e,a
0f40 d7        rst     10h
0f41 6c        ld      l,h
0f42 64        ld      h,h
0f43 2028      jr      nz,0f6dh
0f45 6e        ld      l,(hl)
0f46 6e        ld      l,(hl)
0f47 6e        ld      l,(hl)
0f48 6e        ld      l,(hl)
0f49 29        add     hl,hl
0f4a 2c        inc     l
0f4b 73        ld      (hl),e
0f4c 70        ld      (hl),b
0f4d 2e2e      ld      l,2eh
0f4f 2e2e      ld      l,2eh
0f51 2e2e      ld      l,2eh
0f53 2e2e      ld      l,2eh
0f55 2e2e      ld      l,2eh
0f57 2e2e      ld      l,2eh
0f59 2e2e      ld      l,2eh
0f5b 2e2e      ld      l,2eh
0f5d 2e2e      ld      l,2eh
0f5f 24        inc     h
0f60 d7        rst     10h
0f61 dd220301  ld      (0103h),ix
0f65 c36c91    jp      916ch
0f68 0d        dec     c
0f69 00        nop     
0f6a 69        ld      l,c
0f6b f8        ret     m

0f6c 8e        adc     a,(hl)
0f6d d6e3      sub     0e3h
0f6f f7        rst     30h
0f70 c3c6d9    jp      0d9c6h
0f73 df        rst     18h
0f74 c22000    jp      nz,0020h
0f77 00        nop     
0f78 00        nop     
0f79 00        nop     
0f7a 00        nop     
0f7b 00        nop     
0f7c 00        nop     
0f7d 00        nop     
0f7e 00        nop     
0f7f 00        nop     
0f80 00        nop     
0f81 00        nop     
0f82 00        nop     
0f83 00        nop     
0f84 00        nop     
0f85 00        nop     
0f86 00        nop     
0f87 00        nop     
0f88 00        nop     
0f89 00        nop     
0f8a 00        nop     
0f8b 00        nop     
0f8c 00        nop     
0f8d 00        nop     
0f8e 00        nop     
0f8f ff        rst     38h
0f90 ff        rst     38h
0f91 ff        rst     38h
0f92 ff        rst     38h
0f93 00        nop     
0f94 00        nop     
0f95 00        nop     
0f96 00        nop     
0f97 00        nop     
0f98 00        nop     
0f99 00        nop     
0f9a 00        nop     
0f9b 00        nop     
0f9c 00        nop     
0f9d ba        cp      d
0f9e 102a      djnz    0fcah
0fa0 6b        ld      l,e
0fa1 6c        ld      l,h
0fa2 64        ld      h,h
0fa3 2028      jr      nz,0fcdh
0fa5 6e        ld      l,(hl)
0fa6 6e        ld      l,(hl)
0fa7 6e        ld      l,(hl)
0fa8 6e        ld      l,(hl)
0fa9 29        add     hl,hl
0faa 2c        inc     l
0fab 3c        inc     a
0fac 69        ld      l,c
0fad 78        ld      a,b
0fae 2c        inc     l
0faf 69        ld      l,c
0fb0 79        ld      a,c
0fb1 3e2e      ld      a,2eh
0fb3 2e2e      ld      l,2eh
0fb5 2e2e      ld      l,2eh
0fb7 2e2e      ld      l,2eh
0fb9 2e2e      ld      l,2eh
0fbb 2e2e      ld      l,2eh
0fbd 2e2e      ld      l,2eh
0fbf 24        inc     h
0fc0 d7        rst     10h
0fc1 010000    ld      bc,0000h
0fc4 00        nop     
0fc5 1c        inc     e
0fc6 5c        ld      e,h
0fc7 46        ld      b,(hl)
0fc8 2d        dec     l
0fc9 b9        cp      c
0fca 8e        adc     a,(hl)
0fcb 78        ld      a,b
0fcc 60        ld      h,b
0fcd b1        or      c
0fce 74        ld      (hl),h
0fcf 0eb3      ld      c,0b3h
0fd1 46        ld      b,(hl)
0fd2 d1        pop     de
0fd3 cc3030    call    z,3030h
0fd6 00        nop     
0fd7 00        nop     
0fd8 00        nop     
0fd9 00        nop     
0fda 00        nop     
0fdb 00        nop     
0fdc 00        nop     
0fdd 00        nop     
0fde 00        nop     
0fdf 00        nop     
0fe0 00        nop     
0fe1 00        nop     
0fe2 00        nop     
0fe3 00        nop     
0fe4 00        nop     
0fe5 00        nop     
0fe6 00        nop     
0fe7 00        nop     
0fe8 00        nop     
0fe9 00        nop     
0fea ff        rst     38h
0feb ff        rst     38h
0fec 00        nop     
0fed 00        nop     
0fee 00        nop     
0fef 00        nop     
0ff0 00        nop     
0ff1 00        nop     
0ff2 00        nop     
0ff3 00        nop     
0ff4 00        nop     
0ff5 00        nop     
0ff6 00        nop     
0ff7 00        nop     
0ff8 00        nop     
0ff9 00        nop     
0ffa 00        nop     
0ffb 00        nop     
0ffc 00        nop     
0ffd de39      sbc     a,39h
0fff 19        add     hl,de
1000 69        ld      l,c
1001 6c        ld      l,h
1002 64        ld      h,h
1003 203c      jr      nz,1041h
1005 62        ld      h,d
1006 63        ld      h,e
1007 2c        inc     l
1008 64        ld      h,h
1009 65        ld      h,l
100a 2c        inc     l
100b 68        ld      l,b
100c 6c        ld      l,h
100d 2c        inc     l
100e 73        ld      (hl),e
100f 70        ld      (hl),b
1010 3e2c      ld      a,2ch
1012 6e        ld      l,(hl)
1013 6e        ld      l,(hl)
1014 6e        ld      l,(hl)
1015 6e        ld      l,(hl)
1016 2e2e      ld      l,2eh
1018 2e2e      ld      l,2eh
101a 2e2e      ld      l,2eh
101c 2e2e      ld      l,2eh
101e 2e24      ld      l,24h
1020 d7        rst     10h
1021 dd210000  ld      ix,0000h
1025 e8        ret     pe

1026 87        add     a,a
1027 0620      ld      b,20h
1029 12        ld      (de),a
102a bd        cp      l
102b 9b        sbc     a,e
102c b6        or      (hl)
102d 53        ld      d,e
102e 72        ld      (hl),d
102f e5        push    hl
1030 a1        and     c
1031 51        ld      d,c
1032 13        inc     de
1033 bd        cp      l
1034 f1        pop     af
1035 2000      jr      nz,1037h
1037 00        nop     
1038 00        nop     
1039 00        nop     
103a 00        nop     
103b 00        nop     
103c 00        nop     
103d 00        nop     
103e 00        nop     
103f 00        nop     
1040 00        nop     
1041 00        nop     
1042 00        nop     
1043 00        nop     
1044 00        nop     
1045 00        nop     
1046 00        nop     
1047 00        nop     
1048 00        nop     
1049 00        nop     
104a 00        nop     
104b ff        rst     38h
104c ff        rst     38h
104d 00        nop     
104e 00        nop     
104f 00        nop     
1050 00        nop     
1051 00        nop     
1052 00        nop     
1053 00        nop     
1054 00        nop     
1055 00        nop     
1056 00        nop     
1057 00        nop     
1058 00        nop     
1059 00        nop     
105a 00        nop     
105b 00        nop     
105c 00        nop     
105d 227dd5    ld      (0d57dh),hl
1060 25        dec     h
1061 6c        ld      l,h
1062 64        ld      h,h
1063 203c      jr      nz,10a1h
1065 69        ld      l,c
1066 78        ld      a,b
1067 2c        inc     l
1068 69        ld      l,c
1069 79        ld      a,c
106a 3e2c      ld      a,2ch
106c 6e        ld      l,(hl)
106d 6e        ld      l,(hl)
106e 6e        ld      l,(hl)
106f 6e        ld      l,(hl)
1070 2e2e      ld      l,2eh
1072 2e2e      ld      l,2eh
1074 2e2e      ld      l,2eh
1076 2e2e      ld      l,2eh
1078 2e2e      ld      l,2eh
107a 2e2e      ld      l,2eh
107c 2e2e      ld      l,2eh
107e 2e24      ld      l,24h
1080 d7        rst     10h
1081 0a        ld      a,(bc)
1082 00        nop     
1083 00        nop     
1084 00        nop     
1085 a8        xor     b
1086 b3        or      e
1087 2a1d8e    ld      hl,(8e1dh)
108a 7f        ld      a,a
108b ac        xor     h
108c 42        ld      b,d
108d 03        inc     bc
108e 010301    ld      bc,0103h
1091 c6b1      add     a,0b1h
1093 8e        adc     a,(hl)
1094 ef        rst     28h
1095 1000      djnz    1097h
1097 00        nop     
1098 00        nop     
1099 00        nop     
109a 00        nop     
109b 00        nop     
109c 00        nop     
109d 00        nop     
109e 00        nop     
109f 00        nop     
10a0 00        nop     
10a1 00        nop     
10a2 00        nop     
10a3 00        nop     
10a4 00        nop     
10a5 00        nop     
10a6 00        nop     
10a7 00        nop     
10a8 00        nop     
10a9 00        nop     
10aa 00        nop     
10ab 00        nop     
10ac 00        nop     
10ad ff        rst     38h
10ae 00        nop     
10af 00        nop     
10b0 00        nop     
10b1 00        nop     
10b2 00        nop     
10b3 00        nop     
10b4 00        nop     
10b5 00        nop     
10b6 00        nop     
10b7 00        nop     
10b8 00        nop     
10b9 d7        rst     10h
10ba ff        rst     38h
10bb 00        nop     
10bc 00        nop     
10bd b0        or      b
10be 81        add     a,c
10bf 89        adc     a,c
10c0 35        dec     (hl)
10c1 6c        ld      l,h
10c2 64        ld      h,h
10c3 2061      jr      nz,1126h
10c5 2c        inc     l
10c6 3c        inc     a
10c7 2862      jr      z,112bh
10c9 63        ld      h,e
10ca 29        add     hl,hl
10cb 2c        inc     l
10cc 2864      jr      z,1132h
10ce 65        ld      h,l
10cf 29        add     hl,hl
10d0 3e2e      ld      a,2eh
10d2 2e2e      ld      l,2eh
10d4 2e2e      ld      l,2eh
10d6 2e2e      ld      l,2eh
10d8 2e2e      ld      l,2eh
10da 2e2e      ld      l,2eh
10dc 2e2e      ld      l,2eh
10de 2e24      ld      l,24h
10e0 d7        rst     10h
10e1 0600      ld      b,00h
10e3 00        nop     
10e4 00        nop     
10e5 07        rlca    
10e6 c49df4    call    nz,0f49dh
10e9 3d        dec     a
10ea d1        pop     de
10eb 39        add     hl,sp
10ec 03        inc     bc
10ed 89        adc     a,c
10ee de55      sbc     a,55h
10f0 74        ld      (hl),h
10f1 53        ld      d,e
10f2 c0        ret     nz

10f3 09        add     hl,bc
10f4 55        ld      d,l
10f5 3800      jr      c,10f7h
10f7 00        nop     
10f8 00        nop     
10f9 00        nop     
10fa 00        nop     
10fb 00        nop     
10fc 00        nop     
10fd 00        nop     
10fe 00        nop     
10ff 00        nop     
1100 00        nop     
1101 00        nop     
1102 00        nop     
1103 00        nop     
1104 00        nop     
1105 00        nop     
1106 00        nop     
1107 00        nop     
1108 00        nop     
1109 00        nop     
110a 00        nop     
110b 00        nop     
110c 00        nop     
110d 00        nop     
110e 00        nop     
110f 00        nop     
1110 00        nop     
1111 00        nop     
1112 00        nop     
1113 00        nop     
1114 00        nop     
1115 00        nop     
1116 00        nop     
1117 00        nop     
1118 00        nop     
1119 00        nop     
111a ff        rst     38h
111b 00        nop     
111c 00        nop     
111d f1        pop     af
111e dab556    jp      c,56b5h
1121 6c        ld      l,h
1122 64        ld      h,h
1123 203c      jr      nz,1161h
1125 62        ld      h,d
1126 2c        inc     l
1127 63        ld      h,e
1128 2c        inc     l
1129 64        ld      h,h
112a 2c        inc     l
112b 65        ld      h,l
112c 2c        inc     l
112d 68        ld      l,b
112e 2c        inc     l
112f 6c        ld      l,h
1130 2c        inc     l
1131 2868      jr      z,119bh
1133 6c        ld      l,h
1134 29        add     hl,hl
1135 2c        inc     l
1136 61        ld      h,c
1137 3e2c      ld      a,2ch
1139 6e        ld      l,(hl)
113a 6e        ld      l,(hl)
113b 2e2e      ld      l,2eh
113d 2e2e      ld      l,2eh
113f 24        inc     h
1140 d7        rst     10h
1141 dd360100  ld      (ix+01h),00h
1145 45        ld      b,l
1146 1b        dec     de
1147 02        ld      (bc),a
1148 010201    ld      bc,0102h
114b c1        pop     bc
114c d5        push    de
114d c7        rst     00h
114e 61        ld      h,c
114f c4bdc0    call    nz,0c0bdh
1152 85        add     a,l
1153 16cd      ld      d,0cdh
1155 2000      jr      nz,1157h
1157 00        nop     
1158 00        nop     
1159 00        nop     
115a 00        nop     
115b 00        nop     
115c 00        nop     
115d 00        nop     
115e 00        nop     
115f 00        nop     
1160 00        nop     
1161 00        nop     
1162 00        nop     
1163 00        nop     
1164 00        nop     
1165 00        nop     
1166 00        nop     
1167 00        nop     
1168 00        nop     
1169 00        nop     
116a 00        nop     
116b 00        nop     
116c ff        rst     38h
116d 00        nop     
116e 00        nop     
116f 00        nop     
1170 00        nop     
1171 00        nop     
1172 00        nop     
1173 00        nop     
1174 00        nop     
1175 00        nop     
1176 00        nop     
1177 00        nop     
1178 00        nop     
1179 00        nop     
117a ff        rst     38h
117b 00        nop     
117c 00        nop     
117d 26db      ld      h,0dbh
117f 47        ld      b,a
1180 7e        ld      a,(hl)
1181 6c        ld      l,h
1182 64        ld      h,h
1183 2028      jr      nz,11adh
1185 3c        inc     a
1186 69        ld      l,c
1187 78        ld      a,b
1188 2c        inc     l
1189 69        ld      l,c
118a 79        ld      a,c
118b 3e2b      ld      a,2bh
118d 31292c    ld      sp,2c29h
1190 6e        ld      l,(hl)
1191 6e        ld      l,(hl)
1192 2e2e      ld      l,2eh
1194 2e2e      ld      l,2eh
1196 2e2e      ld      l,2eh
1198 2e2e      ld      l,2eh
119a 2e2e      ld      l,2eh
119c 2e2e      ld      l,2eh
119e 2e24      ld      l,24h
11a0 d7        rst     10h
11a1 dd4601    ld      b,(ix+01h)
11a4 00        nop     
11a5 16d0      ld      d,0d0h
11a7 02        ld      (bc),a
11a8 010201    ld      bc,0102h
11ab 60        ld      h,b
11ac 42        ld      b,d
11ad 39        add     hl,sp
11ae 7f        ld      a,a
11af 04        inc     b
11b0 04        inc     b
11b1 97        sub     a
11b2 4a        ld      c,d
11b3 85        add     a,l
11b4 d0        ret     nc

11b5 2018      jr      nz,11cfh
11b7 00        nop     
11b8 00        nop     
11b9 00        nop     
11ba 00        nop     
11bb 010001    ld      bc,0100h
11be 00        nop     
11bf 00        nop     
11c0 00        nop     
11c1 00        nop     
11c2 00        nop     
11c3 00        nop     
11c4 00        nop     
11c5 00        nop     
11c6 00        nop     
11c7 00        nop     
11c8 00        nop     
11c9 00        nop     
11ca 00        nop     
11cb 00        nop     
11cc 00        nop     
11cd ff        rst     38h
11ce ff        rst     38h
11cf 00        nop     
11d0 00        nop     
11d1 00        nop     
11d2 00        nop     
11d3 00        nop     
11d4 00        nop     
11d5 00        nop     
11d6 00        nop     
11d7 00        nop     
11d8 00        nop     
11d9 00        nop     
11da 00        nop     
11db 00        nop     
11dc 00        nop     
11dd cc1106    call    z,0611h
11e0 a8        xor     b
11e1 6c        ld      l,h
11e2 64        ld      h,h
11e3 203c      jr      nz,1221h
11e5 62        ld      h,d
11e6 2c        inc     l
11e7 63        ld      h,e
11e8 2c        inc     l
11e9 64        ld      h,h
11ea 2c        inc     l
11eb 65        ld      h,l
11ec 3e2c      ld      a,2ch
11ee 283c      jr      z,122ch
11f0 69        ld      l,c
11f1 78        ld      a,b
11f2 2c        inc     l
11f3 69        ld      l,c
11f4 79        ld      a,c
11f5 3e2b      ld      a,2bh
11f7 31292e    ld      sp,2e29h
11fa 2e2e      ld      l,2eh
11fc 2e2e      ld      l,2eh
11fe 2e24      ld      l,24h
1200 d7        rst     10h
1201 dd6601    ld      h,(ix+01h)
1204 00        nop     
1205 e0        ret     po

1206 84        add     a,h
1207 02        ld      (bc),a
1208 010201    ld      bc,0102h
120b 52        ld      d,d
120c 9c        sbc     a,h
120d 99        sbc     a,c
120e a7        and     a
120f b6        or      (hl)
1210 49        ld      c,c
1211 93        sub     e
1212 00        nop     
1213 ad        xor     l
1214 ee20      xor     20h
1216 08        ex      af,af'
1217 00        nop     
1218 00        nop     
1219 00        nop     
121a 00        nop     
121b 010001    ld      bc,0100h
121e 00        nop     
121f 00        nop     
1220 00        nop     
1221 00        nop     
1222 00        nop     
1223 00        nop     
1224 00        nop     
1225 00        nop     
1226 00        nop     
1227 00        nop     
1228 00        nop     
1229 00        nop     
122a 00        nop     
122b 00        nop     
122c 00        nop     
122d ff        rst     38h
122e ff        rst     38h
122f 00        nop     
1230 00        nop     
1231 00        nop     
1232 00        nop     
1233 00        nop     
1234 00        nop     
1235 00        nop     
1236 00        nop     
1237 00        nop     
1238 00        nop     
1239 00        nop     
123a 00        nop     
123b 00        nop     
123c 00        nop     
123d fa2a4d    jp      m,4d2ah
1240 03        inc     bc
1241 6c        ld      l,h
1242 64        ld      h,h
1243 203c      jr      nz,1281h
1245 68        ld      l,b
1246 2c        inc     l
1247 6c        ld      l,h
1248 3e2c      ld      a,2ch
124a 283c      jr      z,1288h
124c 69        ld      l,c
124d 78        ld      a,b
124e 2c        inc     l
124f 69        ld      l,c
1250 79        ld      a,c
1251 3e2b      ld      a,2bh
1253 31292e    ld      sp,2e29h
1256 2e2e      ld      l,2eh
1258 2e2e      ld      l,2eh
125a 2e2e      ld      l,2eh
125c 2e2e      ld      l,2eh
125e 2e24      ld      l,24h
1260 d7        rst     10h
1261 dd7e01    ld      a,(ix+01h)
1264 00        nop     
1265 b6        or      (hl)
1266 d8        ret     c

1267 02        ld      (bc),a
1268 010201    ld      bc,0102h
126b 12        ld      (de),a
126c c607      add     a,07h
126e df        rst     18h
126f d0        ret     nc

1270 9c        sbc     a,h
1271 43        ld      b,e
1272 a6        and     (hl)
1273 e5        push    hl
1274 a0        and     b
1275 2000      jr      nz,1277h
1277 00        nop     
1278 00        nop     
1279 00        nop     
127a 00        nop     
127b 010001    ld      bc,0100h
127e 00        nop     
127f 00        nop     
1280 00        nop     
1281 00        nop     
1282 00        nop     
1283 00        nop     
1284 00        nop     
1285 00        nop     
1286 00        nop     
1287 00        nop     
1288 00        nop     
1289 00        nop     
128a 00        nop     
128b 00        nop     
128c 00        nop     
128d ff        rst     38h
128e ff        rst     38h
128f 00        nop     
1290 00        nop     
1291 00        nop     
1292 00        nop     
1293 00        nop     
1294 00        nop     
1295 00        nop     
1296 00        nop     
1297 00        nop     
1298 00        nop     
1299 00        nop     
129a 00        nop     
129b 00        nop     
129c 00        nop     
129d a5        and     l
129e e9        jp      (hl)
129f ac        xor     h
12a0 64        ld      h,h
12a1 6c        ld      l,h
12a2 64        ld      h,h
12a3 2061      jr      nz,1306h
12a5 2c        inc     l
12a6 283c      jr      z,12e4h
12a8 69        ld      l,c
12a9 78        ld      a,b
12aa 2c        inc     l
12ab 69        ld      l,c
12ac 79        ld      a,c
12ad 3e2b      ld      a,2bh
12af 31292e    ld      sp,2e29h
12b2 2e2e      ld      l,2eh
12b4 2e2e      ld      l,2eh
12b6 2e2e      ld      l,2eh
12b8 2e2e      ld      l,2eh
12ba 2e2e      ld      l,2eh
12bc 2e2e      ld      l,2eh
12be 2e24      ld      l,24h
12c0 d7        rst     10h
12c1 dd2600    ld      ixh,00h
12c4 00        nop     
12c5 53        ld      d,e
12c6 3c        inc     a
12c7 40        ld      b,b
12c8 46        ld      b,(hl)
12c9 79        ld      a,c
12ca e1        pop     hl
12cb 117707    ld      de,0777h
12ce c1        pop     bc
12cf fa1a81    jp      m,811ah
12d2 ad        xor     l
12d3 9b        sbc     a,e
12d4 5d        ld      e,l
12d5 2008      jr      nz,12dfh
12d7 00        nop     
12d8 00        nop     
12d9 00        nop     
12da 00        nop     
12db 00        nop     
12dc 00        nop     
12dd 00        nop     
12de 00        nop     
12df 00        nop     
12e0 00        nop     
12e1 00        nop     
12e2 00        nop     
12e3 00        nop     
12e4 00        nop     
12e5 00        nop     
12e6 00        nop     
12e7 00        nop     
12e8 00        nop     
12e9 00        nop     
12ea 00        nop     
12eb 00        nop     
12ec 00        nop     
12ed 00        nop     
12ee 00        nop     
12ef 00        nop     
12f0 00        nop     
12f1 00        nop     
12f2 00        nop     
12f3 00        nop     
12f4 00        nop     
12f5 00        nop     
12f6 00        nop     
12f7 00        nop     
12f8 00        nop     
12f9 00        nop     
12fa ff        rst     38h
12fb 00        nop     
12fc 00        nop     
12fd 24        inc     h
12fe e8        ret     pe

12ff 82        add     a,d
1300 8b        adc     a,e
1301 6c        ld      l,h
1302 64        ld      h,h
1303 203c      jr      nz,1341h
1305 69        ld      l,c
1306 78        ld      a,b
1307 68        ld      l,b
1308 2c        inc     l
1309 69        ld      l,c
130a 78        ld      a,b
130b 6c        ld      l,h
130c 2c        inc     l
130d 69        ld      l,c
130e 79        ld      a,c
130f 68        ld      l,b
1310 2c        inc     l
1311 69        ld      l,c
1312 79        ld      a,c
1313 6c        ld      l,h
1314 3e2c      ld      a,2ch
1316 6e        ld      l,(hl)
1317 6e        ld      l,(hl)
1318 2e2e      ld      l,2eh
131a 2e2e      ld      l,2eh
131c 2e2e      ld      l,2eh
131e 2e24      ld      l,24h
1320 d7        rst     10h
1321 40        ld      b,b
1322 00        nop     
1323 00        nop     
1324 00        nop     
1325 a4        and     h
1326 72        ld      (hl),d
1327 24        inc     h
1328 a0        and     b
1329 ac        xor     h
132a 61        ld      h,c
132b 03        inc     bc
132c 01c782    ld      bc,82c7h
132f 8f        adc     a,a
1330 71        ld      (hl),c
1331 97        sub     a
1332 8f        adc     a,a
1333 8e        adc     a,(hl)
1334 ef        rst     28h
1335 3f        ccf     
1336 00        nop     
1337 00        nop     
1338 00        nop     
1339 00        nop     
133a 00        nop     
133b 00        nop     
133c 00        nop     
133d 00        nop     
133e 00        nop     
133f 00        nop     
1340 00        nop     
1341 00        nop     
1342 00        nop     
1343 00        nop     
1344 00        nop     
1345 00        nop     
1346 00        nop     
1347 00        nop     
1348 00        nop     
1349 00        nop     
134a 00        nop     
134b 00        nop     
134c 00        nop     
134d ff        rst     38h
134e 00        nop     
134f 00        nop     
1350 00        nop     
1351 00        nop     
1352 00        nop     
1353 00        nop     
1354 00        nop     
1355 ff        rst     38h
1356 ff        rst     38h
1357 ff        rst     38h
1358 ff        rst     38h
1359 d7        rst     10h
135a ff        rst     38h
135b 00        nop     
135c 00        nop     
135d 74        ld      (hl),h
135e 4b        ld      c,e
135f 01186c    ld      bc,6c18h
1362 64        ld      h,h
1363 203c      jr      nz,13a1h
1365 62        ld      h,d
1366 63        ld      h,e
1367 64        ld      h,h
1368 65        ld      h,l
1369 68        ld      l,b
136a 6c        ld      l,h
136b 61        ld      h,c
136c 3e2c      ld      a,2ch
136e 3c        inc     a
136f 62        ld      h,d
1370 63        ld      h,e
1371 64        ld      h,h
1372 65        ld      h,l
1373 68        ld      l,b
1374 6c        ld      l,h
1375 61        ld      h,c
1376 3e2e      ld      a,2eh
1378 2e2e      ld      l,2eh
137a 2e2e      ld      l,2eh
137c 2e2e      ld      l,2eh
137e 2e24      ld      l,24h
1380 d7        rst     10h
1381 dd40      ld      b,b
1383 00        nop     
1384 00        nop     
1385 c5        push    bc
1386 bc        cp      h
1387 03        inc     bc
1388 010301    ld      bc,0103h
138b 03        inc     bc
138c 01c22f    ld      bc,2fc2h
138f c0        ret     nz

1390 98        sbc     a,b
1391 83        add     a,e
1392 1f        rra     
1393 cd3b20    call    203bh
1396 3f        ccf     
1397 00        nop     
1398 00        nop     
1399 00        nop     
139a 00        nop     
139b 00        nop     
139c 00        nop     
139d 00        nop     
139e 00        nop     
139f 00        nop     
13a0 00        nop     
13a1 00        nop     
13a2 00        nop     
13a3 00        nop     
13a4 00        nop     
13a5 00        nop     
13a6 00        nop     
13a7 00        nop     
13a8 00        nop     
13a9 00        nop     
13aa 00        nop     
13ab 00        nop     
13ac 00        nop     
13ad ff        rst     38h
13ae 00        nop     
13af 00        nop     
13b0 00        nop     
13b1 00        nop     
13b2 00        nop     
13b3 00        nop     
13b4 00        nop     
13b5 ff        rst     38h
13b6 ff        rst     38h
13b7 ff        rst     38h
13b8 ff        rst     38h
13b9 d7        rst     10h
13ba ff        rst     38h
13bb 00        nop     
13bc 00        nop     
13bd 47        ld      b,a
13be 8b        adc     a,e
13bf a3        and     e
13c0 6b        ld      l,e
13c1 6c        ld      l,h
13c2 64        ld      h,h
13c3 203c      jr      nz,1401h
13c5 62        ld      h,d
13c6 63        ld      h,e
13c7 64        ld      h,h
13c8 65        ld      h,l
13c9 78        ld      a,b
13ca 79        ld      a,c
13cb 61        ld      h,c
13cc 3e2c      ld      a,2ch
13ce 3c        inc     a
13cf 62        ld      h,d
13d0 63        ld      h,e
13d1 64        ld      h,h
13d2 65        ld      h,l
13d3 78        ld      a,b
13d4 79        ld      a,c
13d5 61        ld      h,c
13d6 3e2e      ld      a,2eh
13d8 2e2e      ld      l,2eh
13da 2e2e      ld      l,2eh
13dc 2e2e      ld      l,2eh
13de 2e24      ld      l,24h
13e0 d7        rst     10h
13e1 320301    ld      (0103h),a
13e4 00        nop     
13e5 68        ld      l,b
13e6 fdecf4a0  call    pe,0a0f4h
13ea 44        ld      b,h
13eb 43        ld      b,e
13ec b5        or      l
13ed 53        ld      d,e
13ee 06ba      ld      b,0bah
13f0 cdd24f    call    4fd2h
13f3 d8        ret     c

13f4 1f        rra     
13f5 08        ex      af,af'
13f6 00        nop     
13f7 00        nop     
13f8 00        nop     
13f9 00        nop     
13fa 00        nop     
13fb 00        nop     
13fc 00        nop     
13fd 00        nop     
13fe 00        nop     
13ff 00        nop     
1400 00        nop     
1401 00        nop     
1402 00        nop     
1403 00        nop     
1404 00        nop     
1405 00        nop     
1406 00        nop     
1407 00        nop     
1408 00        nop     
1409 00        nop     
140a 00        nop     
140b 00        nop     
140c 00        nop     
140d ff        rst     38h
140e 00        nop     
140f 00        nop     
1410 00        nop     
1411 00        nop     
1412 00        nop     
1413 00        nop     
1414 00        nop     
1415 00        nop     
1416 00        nop     
1417 00        nop     
1418 00        nop     
1419 d7        rst     10h
141a ff        rst     38h
141b 00        nop     
141c 00        nop     
141d c9        ret     

141e 262d      ld      h,2dh
1420 e5        push    hl
1421 6c        ld      l,h
1422 64        ld      h,h
1423 2061      jr      nz,1486h
1425 2c        inc     l
1426 286e      jr      z,1496h
1428 6e        ld      l,(hl)
1429 6e        ld      l,(hl)
142a 6e        ld      l,(hl)
142b 29        add     hl,hl
142c 202f      jr      nz,145dh
142e 206c      jr      nz,149ch
1430 64        ld      h,h
1431 2028      jr      nz,145bh
1433 6e        ld      l,(hl)
1434 6e        ld      l,(hl)
1435 6e        ld      l,(hl)
1436 6e        ld      l,(hl)
1437 29        add     hl,hl
1438 2c        inc     l
1439 61        ld      h,c
143a 2e2e      ld      l,2eh
143c 2e2e      ld      l,2eh
143e 2e24      ld      l,24h
1440 d7        rst     10h
1441 eda8      ldd     
1443 00        nop     
1444 00        nop     
1445 52        ld      d,d
1446 98        sbc     a,b
1447 fa68a1    jp      m,0a168h
144a 66        ld      h,(hl)
144b 0601      ld      b,01h
144d 04        inc     b
144e 010100    ld      bc,0001h
1451 c1        pop     bc
1452 68        ld      l,b
1453 b7        or      a
1454 2000      jr      nz,1456h
1456 1000      djnz    1458h
1458 00        nop     
1459 00        nop     
145a 00        nop     
145b 00        nop     
145c 00        nop     
145d 00        nop     
145e 00        nop     
145f 00        nop     
1460 00        nop     
1461 00        nop     
1462 00        nop     
1463 00        nop     
1464 00        nop     
1465 00        nop     
1466 00        nop     
1467 00        nop     
1468 00        nop     
1469 00        nop     
146a 00        nop     
146b 00        nop     
146c 00        nop     
146d ff        rst     38h
146e ff        rst     38h
146f 00        nop     
1470 00        nop     
1471 00        nop     
1472 00        nop     
1473 00        nop     
1474 00        nop     
1475 00        nop     
1476 00        nop     
1477 00        nop     
1478 00        nop     
1479 d7        rst     10h
147a 00        nop     
147b 00        nop     
147c 00        nop     
147d 94        sub     h
147e f42769    call    p,6927h
1481 6c        ld      l,h
1482 64        ld      h,h
1483 64        ld      h,h
1484 3c        inc     a
1485 72        ld      (hl),d
1486 3e20      ld      a,20h
1488 2831      jr      z,14bbh
148a 29        add     hl,hl
148b 2e2e      ld      l,2eh
148d 2e2e      ld      l,2eh
148f 2e2e      ld      l,2eh
1491 2e2e      ld      l,2eh
1493 2e2e      ld      l,2eh
1495 2e2e      ld      l,2eh
1497 2e2e      ld      l,2eh
1499 2e2e      ld      l,2eh
149b 2e2e      ld      l,2eh
149d 2e2e      ld      l,2eh
149f 24        inc     h
14a0 d7        rst     10h
14a1 eda8      ldd     
14a3 00        nop     
14a4 00        nop     
14a5 2ef1      ld      l,0f1h
14a7 2aebba    ld      hl,(0baebh)
14aa d5        push    de
14ab 0601      ld      b,01h
14ad 04        inc     b
14ae 010200    ld      bc,0002h
14b1 47        ld      b,a
14b2 ff        rst     38h
14b3 e4fb00    call    po,00fbh
14b6 1000      djnz    14b8h
14b8 00        nop     
14b9 00        nop     
14ba 00        nop     
14bb 00        nop     
14bc 00        nop     
14bd 00        nop     
14be 00        nop     
14bf 00        nop     
14c0 00        nop     
14c1 00        nop     
14c2 00        nop     
14c3 00        nop     
14c4 00        nop     
14c5 00        nop     
14c6 00        nop     
14c7 00        nop     
14c8 00        nop     
14c9 00        nop     
14ca 00        nop     
14cb 00        nop     
14cc 00        nop     
14cd ff        rst     38h
14ce ff        rst     38h
14cf 00        nop     
14d0 00        nop     
14d1 00        nop     
14d2 00        nop     
14d3 00        nop     
14d4 00        nop     
14d5 00        nop     
14d6 00        nop     
14d7 00        nop     
14d8 00        nop     
14d9 d7        rst     10h
14da 00        nop     
14db 00        nop     
14dc 00        nop     
14dd 5a        ld      e,d
14de 90        sub     b
14df 7e        ld      a,(hl)
14e0 d46c64    call    nc,646ch
14e3 64        ld      h,h
14e4 3c        inc     a
14e5 72        ld      (hl),d
14e6 3e20      ld      a,20h
14e8 2832      jr      z,151ch
14ea 29        add     hl,hl
14eb 2e2e      ld      l,2eh
14ed 2e2e      ld      l,2eh
14ef 2e2e      ld      l,2eh
14f1 2e2e      ld      l,2eh
14f3 2e2e      ld      l,2eh
14f5 2e2e      ld      l,2eh
14f7 2e2e      ld      l,2eh
14f9 2e2e      ld      l,2eh
14fb 2e2e      ld      l,2eh
14fd 2e2e      ld      l,2eh
14ff 24        inc     h
1500 d7        rst     10h
1501 eda0      ldi     
1503 00        nop     
1504 00        nop     
1505 30fe      jr      nc,1505h
1507 cd0358    call    5803h
150a 60        ld      h,b
150b 05        dec     b
150c 010301    ld      bc,0103h
150f 010004    ld      bc,0400h
1512 60        ld      h,b
1513 88        adc     a,b
1514 2600      ld      h,00h
1516 1000      djnz    1518h
1518 00        nop     
1519 00        nop     
151a 00        nop     
151b 00        nop     
151c 00        nop     
151d 00        nop     
151e 00        nop     
151f 00        nop     
1520 00        nop     
1521 00        nop     
1522 00        nop     
1523 00        nop     
1524 00        nop     
1525 00        nop     
1526 00        nop     
1527 00        nop     
1528 00        nop     
1529 00        nop     
152a 00        nop     
152b 00        nop     
152c 00        nop     
152d ff        rst     38h
152e ff        rst     38h
152f 00        nop     
1530 00        nop     
1531 00        nop     
1532 00        nop     
1533 00        nop     
1534 00        nop     
1535 00        nop     
1536 00        nop     
1537 00        nop     
1538 00        nop     
1539 d7        rst     10h
153a 00        nop     
153b 00        nop     
153c 00        nop     
153d 9a        sbc     a,d
153e bd        cp      l
153f f6b5      or      0b5h
1541 6c        ld      l,h
1542 64        ld      h,h
1543 69        ld      l,c
1544 3c        inc     a
1545 72        ld      (hl),d
1546 3e20      ld      a,20h
1548 2831      jr      z,157bh
154a 29        add     hl,hl
154b 2e2e      ld      l,2eh
154d 2e2e      ld      l,2eh
154f 2e2e      ld      l,2eh
1551 2e2e      ld      l,2eh
1553 2e2e      ld      l,2eh
1555 2e2e      ld      l,2eh
1557 2e2e      ld      l,2eh
1559 2e2e      ld      l,2eh
155b 2e2e      ld      l,2eh
155d 2e2e      ld      l,2eh
155f 24        inc     h
1560 d7        rst     10h
1561 eda0      ldi     
1563 00        nop     
1564 00        nop     
1565 ce4a      adc     a,4ah
1567 6e        ld      l,(hl)
1568 c288b1    jp      nz,0b188h
156b 05        dec     b
156c 010301    ld      bc,0103h
156f 02        ld      (bc),a
1570 00        nop     
1571 14        inc     d
1572 2d        dec     l
1573 9f        sbc     a,a
1574 a3        and     e
1575 00        nop     
1576 1000      djnz    1578h
1578 00        nop     
1579 00        nop     
157a 00        nop     
157b 00        nop     
157c 00        nop     
157d 00        nop     
157e 00        nop     
157f 00        nop     
1580 00        nop     
1581 00        nop     
1582 00        nop     
1583 00        nop     
1584 00        nop     
1585 00        nop     
1586 00        nop     
1587 00        nop     
1588 00        nop     
1589 00        nop     
158a 00        nop     
158b 00        nop     
158c 00        nop     
158d ff        rst     38h
158e ff        rst     38h
158f 00        nop     
1590 00        nop     
1591 00        nop     
1592 00        nop     
1593 00        nop     
1594 00        nop     
1595 00        nop     
1596 00        nop     
1597 00        nop     
1598 00        nop     
1599 d7        rst     10h
159a 00        nop     
159b 00        nop     
159c 00        nop     
159d eb        ex      de,hl
159e 59        ld      e,c
159f 89        adc     a,c
15a0 1b        dec     de
15a1 6c        ld      l,h
15a2 64        ld      h,h
15a3 69        ld      l,c
15a4 3c        inc     a
15a5 72        ld      (hl),d
15a6 3e20      ld      a,20h
15a8 2832      jr      z,15dch
15aa 29        add     hl,hl
15ab 2e2e      ld      l,2eh
15ad 2e2e      ld      l,2eh
15af 2e2e      ld      l,2eh
15b1 2e2e      ld      l,2eh
15b3 2e2e      ld      l,2eh
15b5 2e2e      ld      l,2eh
15b7 2e2e      ld      l,2eh
15b9 2e2e      ld      l,2eh
15bb 2e2e      ld      l,2eh
15bd 2e2e      ld      l,2eh
15bf 24        inc     h
15c0 d7        rst     10h
15c1 ed44      neg     
15c3 00        nop     
15c4 00        nop     
15c5 a2        and     d
15c6 386b      jr      c,1633h
15c8 5f        ld      e,a
15c9 34        inc     (hl)
15ca d9        exx     
15cb e457d6    call    po,0d657h
15ce d24246    jp      nc,4642h
15d1 43        ld      b,e
15d2 5a        ld      e,d
15d3 cc0900    call    z,0009h
15d6 00        nop     
15d7 00        nop     
15d8 00        nop     
15d9 00        nop     
15da 00        nop     
15db 00        nop     
15dc 00        nop     
15dd 00        nop     
15de 00        nop     
15df 00        nop     
15e0 00        nop     
15e1 00        nop     
15e2 00        nop     
15e3 00        nop     
15e4 00        nop     
15e5 d7        rst     10h
15e6 ff        rst     38h
15e7 00        nop     
15e8 00        nop     
15e9 00        nop     
15ea 00        nop     
15eb 00        nop     
15ec 00        nop     
15ed 00        nop     
15ee 00        nop     
15ef 00        nop     
15f0 00        nop     
15f1 00        nop     
15f2 00        nop     
15f3 00        nop     
15f4 00        nop     
15f5 00        nop     
15f6 00        nop     
15f7 00        nop     
15f8 00        nop     
15f9 00        nop     
15fa 00        nop     
15fb 00        nop     
15fc 00        nop     
15fd 6a        ld      l,d
15fe 3c        inc     a
15ff 3b        dec     sp
1600 bd        cp      l
1601 6e        ld      l,(hl)
1602 65        ld      h,l
1603 67        ld      h,a
1604 2e2e      ld      l,2eh
1606 2e2e      ld      l,2eh
1608 2e2e      ld      l,2eh
160a 2e2e      ld      l,2eh
160c 2e2e      ld      l,2eh
160e 2e2e      ld      l,2eh
1610 2e2e      ld      l,2eh
1612 2e2e      ld      l,2eh
1614 2e2e      ld      l,2eh
1616 2e2e      ld      l,2eh
1618 2e2e      ld      l,2eh
161a 2e2e      ld      l,2eh
161c 2e2e      ld      l,2eh
161e 2e24      ld      l,24h
1620 d7        rst     10h
1621 ed67      rrd     
1623 00        nop     
1624 00        nop     
1625 cb91      res     2,c
1627 8b        adc     a,e
1628 c462fa    call    nz,0fa62h
162b 03        inc     bc
162c 0120e7    ld      bc,0e720h
162f 79        ld      a,c
1630 b4        or      h
1631 40        ld      b,b
1632 06e2      ld      b,0e2h
1634 8a        adc     a,d
1635 00        nop     
1636 08        ex      af,af'
1637 00        nop     
1638 00        nop     
1639 ff        rst     38h
163a 00        nop     
163b 00        nop     
163c 00        nop     
163d 00        nop     
163e 00        nop     
163f 00        nop     
1640 00        nop     
1641 00        nop     
1642 00        nop     
1643 00        nop     
1644 00        nop     
1645 00        nop     
1646 00        nop     
1647 00        nop     
1648 00        nop     
1649 00        nop     
164a 00        nop     
164b 00        nop     
164c 00        nop     
164d 00        nop     
164e 00        nop     
164f 00        nop     
1650 00        nop     
1651 00        nop     
1652 00        nop     
1653 00        nop     
1654 00        nop     
1655 00        nop     
1656 00        nop     
1657 00        nop     
1658 00        nop     
1659 d7        rst     10h
165a ff        rst     38h
165b 00        nop     
165c 00        nop     
165d 95        sub     l
165e 5b        ld      e,e
165f a3        and     e
1660 263c      ld      h,3ch
1662 72        ld      (hl),d
1663 72        ld      (hl),d
1664 64        ld      h,h
1665 2c        inc     l
1666 72        ld      (hl),d
1667 6c        ld      l,h
1668 64        ld      h,h
1669 3e2e      ld      a,2eh
166b 2e2e      ld      l,2eh
166d 2e2e      ld      l,2eh
166f 2e2e      ld      l,2eh
1671 2e2e      ld      l,2eh
1673 2e2e      ld      l,2eh
1675 2e2e      ld      l,2eh
1677 2e2e      ld      l,2eh
1679 2e2e      ld      l,2eh
167b 2e2e      ld      l,2eh
167d 2e2e      ld      l,2eh
167f 24        inc     h
1680 d7        rst     10h
1681 07        rlca    
1682 00        nop     
1683 00        nop     
1684 00        nop     
1685 92        sub     d
1686 cb43      bit     0,e
1688 6d        ld      l,l
1689 90        sub     b
168a 0a        ld      a,(bc)
168b 84        add     a,h
168c c2530c    jp      nz,0c53h
168f 0ef5      ld      c,0f5h
1691 91        sub     c
1692 eb        ex      de,hl
1693 fc4018    call    m,1840h
1696 00        nop     
1697 00        nop     
1698 00        nop     
1699 00        nop     
169a 00        nop     
169b 00        nop     
169c 00        nop     
169d 00        nop     
169e 00        nop     
169f 00        nop     
16a0 00        nop     
16a1 00        nop     
16a2 00        nop     
16a3 00        nop     
16a4 00        nop     
16a5 00        nop     
16a6 ff        rst     38h
16a7 00        nop     
16a8 00        nop     
16a9 00        nop     
16aa 00        nop     
16ab 00        nop     
16ac 00        nop     
16ad 00        nop     
16ae 00        nop     
16af 00        nop     
16b0 00        nop     
16b1 00        nop     
16b2 00        nop     
16b3 00        nop     
16b4 00        nop     
16b5 00        nop     
16b6 00        nop     
16b7 00        nop     
16b8 00        nop     
16b9 d7        rst     10h
16ba 00        nop     
16bb 00        nop     
16bc 00        nop     
16bd 25        dec     h
16be 13        inc     de
16bf 30ae      jr      nc,166fh
16c1 3c        inc     a
16c2 72        ld      (hl),d
16c3 6c        ld      l,h
16c4 63        ld      h,e
16c5 61        ld      h,c
16c6 2c        inc     l
16c7 72        ld      (hl),d
16c8 72        ld      (hl),d
16c9 63        ld      h,e
16ca 61        ld      h,c
16cb 2c        inc     l
16cc 72        ld      (hl),d
16cd 6c        ld      l,h
16ce 61        ld      h,c
16cf 2c        inc     l
16d0 72        ld      (hl),d
16d1 72        ld      (hl),d
16d2 61        ld      h,c
16d3 3e2e      ld      a,2eh
16d5 2e2e      ld      l,2eh
16d7 2e2e      ld      l,2eh
16d9 2e2e      ld      l,2eh
16db 2e2e      ld      l,2eh
16dd 2e2e      ld      l,2eh
16df 24        inc     h
16e0 d7        rst     10h
16e1 ddcb0106  rlc     (ix+01h)
16e5 af        xor     a
16e6 dd02      ld      (bc),a
16e8 010201    ld      bc,0102h
16eb 3c        inc     a
16ec ff        rst     38h
16ed f6db      or      0dbh
16ef f49482    call    p,8294h
16f2 80        add     a,b
16f3 d9        exx     
16f4 61        ld      h,c
16f5 2000      jr      nz,16f7h
16f7 00        nop     
16f8 3800      jr      c,16fah
16fa 00        nop     
16fb 00        nop     
16fc 00        nop     
16fd 00        nop     
16fe 00        nop     
16ff 00        nop     
1700 00        nop     
1701 00        nop     
1702 00        nop     
1703 00        nop     
1704 00        nop     
1705 80        add     a,b
1706 00        nop     
1707 00        nop     
1708 00        nop     
1709 00        nop     
170a 00        nop     
170b 00        nop     
170c 00        nop     
170d ff        rst     38h
170e 00        nop     
170f 00        nop     
1710 00        nop     
1711 00        nop     
1712 00        nop     
1713 00        nop     
1714 00        nop     
1715 00        nop     
1716 00        nop     
1717 00        nop     
1718 00        nop     
1719 57        ld      d,a
171a 00        nop     
171b 00        nop     
171c 00        nop     
171d 71        ld      (hl),c
171e 3acd81    ld      a,(81cdh)
1721 73        ld      (hl),e
1722 68        ld      l,b
1723 66        ld      h,(hl)
1724 2f        cpl     
1725 72        ld      (hl),d
1726 6f        ld      l,a
1727 74        ld      (hl),h
1728 2028      jr      nz,1752h
172a 3c        inc     a
172b 69        ld      l,c
172c 78        ld      a,b
172d 2c        inc     l
172e 69        ld      l,c
172f 79        ld      a,c
1730 3e2b      ld      a,2bh
1732 31292e    ld      sp,2e29h
1735 2e2e      ld      l,2eh
1737 2e2e      ld      l,2eh
1739 2e2e      ld      l,2eh
173b 2e2e      ld      l,2eh
173d 2e2e      ld      l,2eh
173f 24        inc     h
1740 d7        rst     10h
1741 cb00      rlc     b
1743 00        nop     
1744 00        nop     
1745 eb        ex      de,hl
1746 cc4a5d    call    z,5d4ah
1749 07        rlca    
174a e0        ret     po

174b 03        inc     bc
174c 019513    ld      bc,1395h
174f ee30      xor     30h
1751 43        ld      b,e
1752 78        ld      a,b
1753 ad        xor     l
1754 3d        dec     a
1755 00        nop     
1756 3f        ccf     
1757 00        nop     
1758 00        nop     
1759 00        nop     
175a 00        nop     
175b 00        nop     
175c 00        nop     
175d 00        nop     
175e 00        nop     
175f 00        nop     
1760 00        nop     
1761 00        nop     
1762 00        nop     
1763 00        nop     
1764 00        nop     
1765 80        add     a,b
1766 00        nop     
1767 00        nop     
1768 00        nop     
1769 00        nop     
176a 00        nop     
176b 00        nop     
176c 00        nop     
176d ff        rst     38h
176e 00        nop     
176f 00        nop     
1770 00        nop     
1771 00        nop     
1772 00        nop     
1773 00        nop     
1774 00        nop     
1775 ff        rst     38h
1776 ff        rst     38h
1777 ff        rst     38h
1778 ff        rst     38h
1779 57        ld      d,a
177a ff        rst     38h
177b 00        nop     
177c 00        nop     
177d eb        ex      de,hl
177e 60        ld      h,b
177f 4d        ld      c,l
1780 58        ld      e,b
1781 73        ld      (hl),e
1782 68        ld      l,b
1783 66        ld      h,(hl)
1784 2f        cpl     
1785 72        ld      (hl),d
1786 6f        ld      l,a
1787 74        ld      (hl),h
1788 203c      jr      nz,17c6h
178a 62        ld      h,d
178b 2c        inc     l
178c 63        ld      h,e
178d 2c        inc     l
178e 64        ld      h,h
178f 2c        inc     l
1790 65        ld      h,l
1791 2c        inc     l
1792 68        ld      l,b
1793 2c        inc     l
1794 6c        ld      l,h
1795 2c        inc     l
1796 2868      jr      z,1800h
1798 6c        ld      l,h
1799 29        add     hl,hl
179a 2c        inc     l
179b 61        ld      h,c
179c 3e2e      ld      a,2eh
179e 2e24      ld      l,24h
17a0 d7        rst     10h
17a1 cb80      res     0,b
17a3 00        nop     
17a4 00        nop     
17a5 d5        push    de
17a6 2c        inc     l
17a7 ab        xor     e
17a8 97        sub     a
17a9 ff        rst     38h
17aa 39        add     hl,sp
17ab 03        inc     bc
17ac 014bd1    ld      bc,0d14bh
17af b2        or      d
17b0 6a        ld      l,d
17b1 53        ld      d,e
17b2 27        daa     
17b3 38b5      jr      c,176ah
17b5 00        nop     
17b6 7f        ld      a,a
17b7 00        nop     
17b8 00        nop     
17b9 00        nop     
17ba 00        nop     
17bb 00        nop     
17bc 00        nop     
17bd 00        nop     
17be 00        nop     
17bf 00        nop     
17c0 00        nop     
17c1 00        nop     
17c2 00        nop     
17c3 00        nop     
17c4 00        nop     
17c5 00        nop     
17c6 00        nop     
17c7 00        nop     
17c8 00        nop     
17c9 00        nop     
17ca 00        nop     
17cb 00        nop     
17cc 00        nop     
17cd ff        rst     38h
17ce 00        nop     
17cf 00        nop     
17d0 00        nop     
17d1 00        nop     
17d2 00        nop     
17d3 00        nop     
17d4 00        nop     
17d5 ff        rst     38h
17d6 ff        rst     38h
17d7 ff        rst     38h
17d8 ff        rst     38h
17d9 d7        rst     10h
17da ff        rst     38h
17db 00        nop     
17dc 00        nop     
17dd 8b        adc     a,e
17de 57        ld      d,a
17df f0        ret     p

17e0 08        ex      af,af'
17e1 3c        inc     a
17e2 73        ld      (hl),e
17e3 65        ld      h,l
17e4 74        ld      (hl),h
17e5 2c        inc     l
17e6 72        ld      (hl),d
17e7 65        ld      h,l
17e8 73        ld      (hl),e
17e9 3e20      ld      a,20h
17eb 6e        ld      l,(hl)
17ec 2c        inc     l
17ed 3c        inc     a
17ee 62        ld      h,d
17ef 63        ld      h,e
17f0 64        ld      h,h
17f1 65        ld      h,l
17f2 68        ld      l,b
17f3 6c        ld      l,h
17f4 2868      jr      z,185eh
17f6 6c        ld      l,h
17f7 29        add     hl,hl
17f8 61        ld      h,c
17f9 3e2e      ld      a,2eh
17fb 2e2e      ld      l,2eh
17fd 2e2e      ld      l,2eh
17ff 24        inc     h
1800 d7        rst     10h
1801 ddcb0186  res     0,(ix+01h)
1805 44        ld      b,h
1806 fb        ei      
1807 02        ld      (bc),a
1808 010201    ld      bc,0102h
180b 09        add     hl,bc
180c ba        cp      d
180d be        cp      (hl)
180e 68        ld      l,b
180f d8        ret     c

1810 32105e    ld      (5e10h),a
1813 67        ld      h,a
1814 a8        xor     b
1815 2000      jr      nz,1817h
1817 00        nop     
1818 78        ld      a,b
1819 00        nop     
181a 00        nop     
181b 00        nop     
181c 00        nop     
181d 00        nop     
181e 00        nop     
181f 00        nop     
1820 00        nop     
1821 00        nop     
1822 00        nop     
1823 00        nop     
1824 00        nop     
1825 00        nop     
1826 00        nop     
1827 00        nop     
1828 00        nop     
1829 00        nop     
182a 00        nop     
182b 00        nop     
182c 00        nop     
182d ff        rst     38h
182e 00        nop     
182f 00        nop     
1830 00        nop     
1831 00        nop     
1832 00        nop     
1833 00        nop     
1834 00        nop     
1835 00        nop     
1836 00        nop     
1837 00        nop     
1838 00        nop     
1839 d7        rst     10h
183a 00        nop     
183b 00        nop     
183c 00        nop     
183d cc63f9    call    z,0f963h
1840 8a        adc     a,d
1841 3c        inc     a
1842 73        ld      (hl),e
1843 65        ld      h,l
1844 74        ld      (hl),h
1845 2c        inc     l
1846 72        ld      (hl),d
1847 65        ld      h,l
1848 73        ld      (hl),e
1849 3e20      ld      a,20h
184b 6e        ld      l,(hl)
184c 2c        inc     l
184d 283c      jr      z,188bh
184f 69        ld      l,c
1850 78        ld      a,b
1851 2c        inc     l
1852 69        ld      l,c
1853 79        ld      a,c
1854 3e2b      ld      a,2bh
1856 31292e    ld      sp,2e29h
1859 2e2e      ld      l,2eh
185b 2e2e      ld      l,2eh
185d 2e2e      ld      l,2eh
185f 24        inc     h
1860 d7        rst     10h
1861 dd7001    ld      (ix+01h),b
1864 00        nop     
1865 0d        dec     c
1866 27        daa     
1867 02        ld      (bc),a
1868 010201    ld      bc,0102h
186b 3ab77b    ld      a,(7bb7h)
186e 88        adc     a,b
186f ee99      xor     99h
1871 86        add     a,(hl)
1872 70        ld      (hl),b
1873 07        rlca    
1874 ca2003    jp      z,0320h
1877 00        nop     
1878 00        nop     
1879 00        nop     
187a 00        nop     
187b 010001    ld      bc,0100h
187e 00        nop     
187f 00        nop     
1880 00        nop     
1881 00        nop     
1882 00        nop     
1883 00        nop     
1884 00        nop     
1885 00        nop     
1886 00        nop     
1887 00        nop     
1888 00        nop     
1889 00        nop     
188a 00        nop     
188b 00        nop     
188c 00        nop     
188d 00        nop     
188e 00        nop     
188f 00        nop     
1890 00        nop     
1891 00        nop     
1892 00        nop     
1893 00        nop     
1894 00        nop     
1895 ff        rst     38h
1896 ff        rst     38h
1897 ff        rst     38h
1898 ff        rst     38h
1899 00        nop     
189a 00        nop     
189b 00        nop     
189c 00        nop     
189d 04        inc     b
189e 62        ld      h,d
189f 6a        ld      l,d
18a0 bf        cp      a
18a1 6c        ld      l,h
18a2 64        ld      h,h
18a3 2028      jr      nz,18cdh
18a5 3c        inc     a
18a6 69        ld      l,c
18a7 78        ld      a,b
18a8 2c        inc     l
18a9 69        ld      l,c
18aa 79        ld      a,c
18ab 3e2b      ld      a,2bh
18ad 31292c    ld      sp,2c29h
18b0 3c        inc     a
18b1 62        ld      h,d
18b2 2c        inc     l
18b3 63        ld      h,e
18b4 2c        inc     l
18b5 64        ld      h,h
18b6 2c        inc     l
18b7 65        ld      h,l
18b8 3e2e      ld      a,2eh
18ba 2e2e      ld      l,2eh
18bc 2e2e      ld      l,2eh
18be 2e24      ld      l,24h
18c0 d7        rst     10h
18c1 dd7401    ld      (ix+01h),h
18c4 00        nop     
18c5 64        ld      h,h
18c6 b6        or      (hl)
18c7 02        ld      (bc),a
18c8 010201    ld      bc,0102h
18cb ac        xor     h
18cc e8        ret     pe

18cd f5        push    af
18ce b5        or      l
18cf feaa      cp      0aah
18d1 12        ld      (de),a
18d2 1066      djnz    193ah
18d4 95        sub     l
18d5 2001      jr      nz,18d8h
18d7 00        nop     
18d8 00        nop     
18d9 00        nop     
18da 00        nop     
18db 010001    ld      bc,0100h
18de 00        nop     
18df 00        nop     
18e0 00        nop     
18e1 00        nop     
18e2 00        nop     
18e3 00        nop     
18e4 00        nop     
18e5 00        nop     
18e6 00        nop     
18e7 00        nop     
18e8 00        nop     
18e9 00        nop     
18ea 00        nop     
18eb 00        nop     
18ec 00        nop     
18ed 00        nop     
18ee 00        nop     
18ef 00        nop     
18f0 00        nop     
18f1 00        nop     
18f2 00        nop     
18f3 ff        rst     38h
18f4 ff        rst     38h
18f5 00        nop     
18f6 00        nop     
18f7 00        nop     
18f8 00        nop     
18f9 00        nop     
18fa 00        nop     
18fb 00        nop     
18fc 00        nop     
18fd 6a        ld      l,d
18fe 1a        ld      a,(de)
18ff 88        adc     a,b
1900 316c64    ld      sp,646ch
1903 2028      jr      nz,192dh
1905 3c        inc     a
1906 69        ld      l,c
1907 78        ld      a,b
1908 2c        inc     l
1909 69        ld      l,c
190a 79        ld      a,c
190b 3e2b      ld      a,2bh
190d 31292c    ld      sp,2c29h
1910 3c        inc     a
1911 68        ld      l,b
1912 2c        inc     l
1913 6c        ld      l,h
1914 3e2e      ld      a,2eh
1916 2e2e      ld      l,2eh
1918 2e2e      ld      l,2eh
191a 2e2e      ld      l,2eh
191c 2e2e      ld      l,2eh
191e 2e24      ld      l,24h
1920 d7        rst     10h
1921 dd7701    ld      (ix+01h),a
1924 00        nop     
1925 af        xor     a
1926 67        ld      h,a
1927 02        ld      (bc),a
1928 010201    ld      bc,0102h
192b 13        inc     de
192c 4f        ld      c,a
192d 44        ld      b,h
192e 06d7      ld      b,0d7h
1930 bc        cp      h
1931 50        ld      d,b
1932 ac        xor     h
1933 af        xor     a
1934 5f        ld      e,a
1935 2000      jr      nz,1937h
1937 00        nop     
1938 00        nop     
1939 00        nop     
193a 00        nop     
193b 010001    ld      bc,0100h
193e 00        nop     
193f 00        nop     
1940 00        nop     
1941 00        nop     
1942 00        nop     
1943 00        nop     
1944 00        nop     
1945 00        nop     
1946 00        nop     
1947 00        nop     
1948 00        nop     
1949 00        nop     
194a 00        nop     
194b 00        nop     
194c 00        nop     
194d 00        nop     
194e 00        nop     
194f 00        nop     
1950 00        nop     
1951 00        nop     
1952 00        nop     
1953 00        nop     
1954 00        nop     
1955 00        nop     
1956 00        nop     
1957 00        nop     
1958 00        nop     
1959 00        nop     
195a ff        rst     38h
195b 00        nop     
195c 00        nop     
195d ccbe5a    call    z,5abeh
1960 96        sub     (hl)
1961 6c        ld      l,h
1962 64        ld      h,h
1963 2028      jr      nz,198dh
1965 3c        inc     a
1966 69        ld      l,c
1967 78        ld      a,b
1968 2c        inc     l
1969 69        ld      l,c
196a 79        ld      a,c
196b 3e2b      ld      a,2bh
196d 31292c    ld      sp,2c29h
1970 61        ld      h,c
1971 2e2e      ld      l,2eh
1973 2e2e      ld      l,2eh
1975 2e2e      ld      l,2eh
1977 2e2e      ld      l,2eh
1979 2e2e      ld      l,2eh
197b 2e2e      ld      l,2eh
197d 2e2e      ld      l,2eh
197f 24        inc     h
1980 d7        rst     10h
1981 02        ld      (bc),a
1982 00        nop     
1983 00        nop     
1984 00        nop     
1985 3b        dec     sp
1986 0c        inc     c
1987 92        sub     d
1988 b5        or      l
1989 ff        rst     38h
198a 6c        ld      l,h
198b 9e        sbc     a,(hl)
198c 95        sub     l
198d 03        inc     bc
198e 010401    ld      bc,0104h
1991 c1        pop     bc
1992 21e7bd    ld      hl,0bde7h
1995 1800      jr      1997h
1997 00        nop     
1998 00        nop     
1999 00        nop     
199a 00        nop     
199b 00        nop     
199c 00        nop     
199d 00        nop     
199e 00        nop     
199f 00        nop     
19a0 00        nop     
19a1 00        nop     
19a2 00        nop     
19a3 00        nop     
19a4 00        nop     
19a5 00        nop     
19a6 00        nop     
19a7 00        nop     
19a8 00        nop     
19a9 00        nop     
19aa 00        nop     
19ab 00        nop     
19ac 00        nop     
19ad ff        rst     38h
19ae ff        rst     38h
19af 00        nop     
19b0 00        nop     
19b1 00        nop     
19b2 00        nop     
19b3 00        nop     
19b4 00        nop     
19b5 00        nop     
19b6 00        nop     
19b7 00        nop     
19b8 00        nop     
19b9 00        nop     
19ba ff        rst     38h
19bb 00        nop     
19bc 00        nop     
19bd 7a        ld      a,d
19be 4c        ld      c,h
19bf 114f6c    ld      de,6c4fh
19c2 64        ld      h,h
19c3 2028      jr      nz,19edh
19c5 3c        inc     a
19c6 62        ld      h,d
19c7 63        ld      h,e
19c8 2c        inc     l
19c9 64        ld      h,h
19ca 65        ld      h,l
19cb 3e29      ld      a,29h
19cd 2c        inc     l
19ce 61        ld      h,c
19cf 2e2e      ld      l,2eh
19d1 2e2e      ld      l,2eh
19d3 2e2e      ld      l,2eh
19d5 2e2e      ld      l,2eh
19d7 2e2e      ld      l,2eh
19d9 2e2e      ld      l,2eh
19db 2e2e      ld      l,2eh
19dd 2e2e      ld      l,2eh
19df 24        inc     h
19e0 e5        push    hl
19e1 7e        ld      a,(hl)
19e2 23        inc     hl
19e3 66        ld      h,(hl)
19e4 6f        ld      l,a
19e5 220080    ld      (8000h),hl
19e8 7e        ld      a,(hl)
19e9 32691d    ld      (1d69h),a
19ec 23        inc     hl
19ed e5        push    hl
19ee 111400    ld      de,0014h
19f1 19        add     hl,de
19f2 11de1c    ld      de,1cdeh
19f5 cd4d1c    call    1c4dh
19f8 e1        pop     hl
19f9 e5        push    hl
19fa 112800    ld      de,0028h
19fd 19        add     hl,de
19fe 11061d    ld      de,1d06h
1a01 cd4d1c    call    1c4dh
1a04 21061d    ld      hl,1d06h
1a07 3601      ld      (hl),01h
1a09 e1        pop     hl
1a0a e5        push    hl
1a0b 11461d    ld      de,1d46h
1a0e 010400    ld      bc,0004h
1a11 edb0      ldir    
1a13 110301    ld      de,0103h
1a16 011000    ld      bc,0010h
1a19 edb0      ldir    
1a1b 114000    ld      de,0040h
1a1e 2a0080    ld      hl,(8000h)
1a21 19        add     hl,de
1a22 eb        ex      de,hl
1a23 0e09      ld      c,09h
1a25 cdbe1d    call    1dbeh
1a28 cd631e    call    1e63h
1a2b 3a461d    ld      a,(1d46h)
1a2e fe76      cp      76h
1a30 ca421b    jp      z,1b42h
1a33 e6df      and     0dfh
1a35 fedd      cp      0ddh
1a37 c23f1b    jp      nz,1b3fh
1a3a 3a471d    ld      a,(1d47h)
1a3d fe76      cp      76h
1a3f c42e1d    call    nz,1d2eh
1a42 cd8d1c    call    1c8dh
1a45 c4b11c    call    nz,1cb1h
1a48 e1        pop     hl
1a49 ca7e1b    jp      z,1b7eh
1a4c 113c00    ld      de,003ch
1a4f 19        add     hl,de
1a50 cd241e    call    1e24h
1a53 11fa1d    ld      de,1dfah
1a56 ca751b    jp      z,1b75h
1a59 11011e    ld      de,1e01h
1a5c 0e09      ld      c,09h
1a5e cdbe1d    call    1dbeh
1a61 cd9d1d    call    1d9dh
1a64 111c1e    ld      de,1e1ch
1a67 0e09      ld      c,09h
1a69 cdbe1d    call    1dbeh
1a6c 21771e    ld      hl,1e77h
1a6f cd9d1d    call    1d9dh
1a72 11f71d    ld      de,1df7h
1a75 0e09      ld      c,09h
1a77 cdbe1d    call    1dbeh
1a7a e1        pop     hl
1a7b 23        inc     hl
1a7c 23        inc     hl
1a7d c9        ret     

1a7e e5        push    hl
1a7f 3e01      ld      a,01h
1a81 32f41b    ld      (1bf4h),a
1a84 32181c    ld      (1c18h),a
1a87 21de1c    ld      hl,1cdeh
1a8a 22f51b    ld      (1bf5h),hl
1a8d 21061d    ld      hl,1d06h
1a90 22191c    ld      (1c19h),hl
1a93 0604      ld      b,04h
1a95 e1        pop     hl
1a96 e5        push    hl
1a97 11461d    ld      de,1d46h
1a9a cda81b    call    1ba8h
1a9d 0610      ld      b,10h
1a9f 110301    ld      de,0103h
1aa2 cda81b    call    1ba8h
1aa5 c32b1b    jp      1b2bh
1aa8 cdb11b    call    1bb1h
1aab 23        inc     hl
1aac 05        dec     b
1aad c2a81b    jp      nz,1ba8h
1ab0 c9        ret     

1ab1 c5        push    bc
1ab2 d5        push    de
1ab3 e5        push    hl
1ab4 4e        ld      c,(hl)
1ab5 111400    ld      de,0014h
1ab8 19        add     hl,de
1ab9 7e        ld      a,(hl)
1aba fe00      cp      00h
1abc cad21b    jp      z,1bd2h
1abf 0608      ld      b,08h
1ac1 0f        rrca    
1ac2 f5        push    af
1ac3 3e00      ld      a,00h
1ac5 dcf71b    call    c,1bf7h
1ac8 a9        xor     c
1ac9 0f        rrca    
1aca 4f        ld      c,a
1acb f1        pop     af
1acc 05        dec     b
1acd c2c11b    jp      nz,1bc1h
1ad0 0608      ld      b,08h
1ad2 111400    ld      de,0014h
1ad5 19        add     hl,de
1ad6 7e        ld      a,(hl)
1ad7 fe00      cp      00h
1ad9 caed1b    jp      z,1bedh
1adc 0608      ld      b,08h
1ade 0f        rrca    
1adf f5        push    af
1ae0 3e00      ld      a,00h
1ae2 dc1b1c    call    c,1c1bh
1ae5 a9        xor     c
1ae6 0f        rrca    
1ae7 4f        ld      c,a
1ae8 f1        pop     af
1ae9 05        dec     b
1aea c2de1b    jp      nz,1bdeh
1aed e1        pop     hl
1aee d1        pop     de
1aef 79        ld      a,c
1af0 12        ld      (de),a
1af1 13        inc     de
1af2 c1        pop     bc
1af3 c9        ret     

1af4 00        nop     
1af5 00        nop     
1af6 00        nop     
1af7 c5        push    bc
1af8 e5        push    hl
1af9 2af51b    ld      hl,(1bf5h)
1afc 46        ld      b,(hl)
1afd 21f41b    ld      hl,1bf4h
1b00 7e        ld      a,(hl)
1b01 4f        ld      c,a
1b02 07        rlca    
1b03 77        ld      (hl),a
1b04 fe01      cp      01h
1b06 c2101c    jp      nz,1c10h
1b09 2af51b    ld      hl,(1bf5h)
1b0c 23        inc     hl
1b0d 22f51b    ld      (1bf5h),hl
1b10 78        ld      a,b
1b11 a1        and     c
1b12 e1        pop     hl
1b13 c1        pop     bc
1b14 c8        ret     z

1b15 3e01      ld      a,01h
1b17 c9        ret     

1b18 00        nop     
1b19 00        nop     
1b1a 00        nop     
1b1b c5        push    bc
1b1c e5        push    hl
1b1d 2a191c    ld      hl,(1c19h)
1b20 46        ld      b,(hl)
1b21 21181c    ld      hl,1c18h
1b24 7e        ld      a,(hl)
1b25 4f        ld      c,a
1b26 07        rlca    
1b27 77        ld      (hl),a
1b28 fe01      cp      01h
1b2a c2341c    jp      nz,1c34h
1b2d 2a191c    ld      hl,(1c19h)
1b30 23        inc     hl
1b31 22191c    ld      (1c19h),hl
1b34 78        ld      a,b
1b35 a1        and     c
1b36 e1        pop     hl
1b37 c1        pop     bc
1b38 c8        ret     z

1b39 3e01      ld      a,01h
1b3b c9        ret     

1b3c f5        push    af
1b3d c5        push    bc
1b3e d5        push    de
1b3f e5        push    hl
1b40 3600      ld      (hl),00h
1b42 54        ld      d,h
1b43 5d        ld      e,l
1b44 13        inc     de
1b45 0b        dec     bc
1b46 edb0      ldir    
1b48 e1        pop     hl
1b49 d1        pop     de
1b4a c1        pop     bc
1b4b f1        pop     af
1b4c c9        ret     

1b4d d5        push    de
1b4e eb        ex      de,hl
1b4f 012800    ld      bc,0028h
1b52 cd3c1c    call    1c3ch
1b55 eb        ex      de,hl
1b56 0614      ld      b,14h
1b58 0e01      ld      c,01h
1b5a 1600      ld      d,00h
1b5c 5e        ld      e,(hl)
1b5d 7b        ld      a,e
1b5e a1        and     c
1b5f ca631c    jp      z,1c63h
1b62 14        inc     d
1b63 79        ld      a,c
1b64 07        rlca    
1b65 4f        ld      c,a
1b66 fe01      cp      01h
1b68 c25d1c    jp      nz,1c5dh
1b6b 23        inc     hl
1b6c 05        dec     b
1b6d c25c1c    jp      nz,1c5ch
1b70 7a        ld      a,d
1b71 e6f8      and     0f8h
1b73 0f        rrca    
1b74 0f        rrca    
1b75 0f        rrca    
1b76 6f        ld      l,a
1b77 2600      ld      h,00h
1b79 7a        ld      a,d
1b7a e607      and     07h
1b7c 3c        inc     a
1b7d 47        ld      b,a
1b7e 3e80      ld      a,80h
1b80 07        rlca    
1b81 05        dec     b
1b82 c2801c    jp      nz,1c80h
1b85 d1        pop     de
1b86 19        add     hl,de
1b87 111400    ld      de,0014h
1b8a 19        add     hl,de
1b8b 77        ld      (hl),a
1b8c c9        ret     

1b8d c5        push    bc
1b8e d5        push    de
1b8f e5        push    hl
1b90 21de1c    ld      hl,1cdeh
1b93 111400    ld      de,0014h
1b96 eb        ex      de,hl
1b97 19        add     hl,de
1b98 eb        ex      de,hl
1b99 34        inc     (hl)
1b9a 7e        ld      a,(hl)
1b9b fe00      cp      00h
1b9d caac1c    jp      z,1cach
1ba0 47        ld      b,a
1ba1 1a        ld      a,(de)
1ba2 a0        and     b
1ba3 caa81c    jp      z,1ca8h
1ba6 3600      ld      (hl),00h
1ba8 c1        pop     bc
1ba9 d1        pop     de
1baa e1        pop     hl
1bab c9        ret     

1bac 23        inc     hl
1bad 13        inc     de
1bae c3991c    jp      1c99h
1bb1 c5        push    bc
1bb2 d5        push    de
1bb3 e5        push    hl
1bb4 21061d    ld      hl,1d06h
1bb7 111400    ld      de,0014h
1bba eb        ex      de,hl
1bbb 19        add     hl,de
1bbc eb        ex      de,hl
1bbd 7e        ld      a,(hl)
1bbe b7        or      a
1bbf cad91c    jp      z,1cd9h
1bc2 47        ld      b,a
1bc3 1a        ld      a,(de)
1bc4 a0        and     b
1bc5 c2d51c    jp      nz,1cd5h
1bc8 78        ld      a,b
1bc9 07        rlca    
1bca fe01      cp      01h
1bcc c2d31c    jp      nz,1cd3h
1bcf 3600      ld      (hl),00h
1bd1 23        inc     hl
1bd2 13        inc     de
1bd3 77        ld      (hl),a
1bd4 af        xor     a
1bd5 e1        pop     hl
1bd6 d1        pop     de
1bd7 c1        pop     bc
1bd8 c9        ret     

1bd9 23        inc     hl
1bda 13        inc     de
1bdb c3bd1c    jp      1cbdh
1bde 00        nop     
1bdf 00        nop     
1be0 00        nop     
1be1 00        nop     
1be2 00        nop     
1be3 00        nop     
1be4 00        nop     
1be5 00        nop     
1be6 00        nop     
1be7 00        nop     
1be8 00        nop     
1be9 00        nop     
1bea 00        nop     
1beb 00        nop     
1bec 00        nop     
1bed 00        nop     
1bee 00        nop     
1bef 00        nop     
1bf0 00        nop     
1bf1 00        nop     
1bf2 00        nop     
1bf3 00        nop     
1bf4 00        nop     
1bf5 00        nop     
1bf6 00        nop     
1bf7 00        nop     
1bf8 00        nop     
1bf9 00        nop     
1bfa 00        nop     
1bfb 00        nop     
1bfc 00        nop     
1bfd 00        nop     
1bfe 00        nop     
1bff 00        nop     
1c00 00        nop     
1c01 00        nop     
1c02 00        nop     
1c03 00        nop     
1c04 00        nop     
1c05 00        nop     
1c06 00        nop     
1c07 00        nop     
1c08 00        nop     
1c09 00        nop     
1c0a 00        nop     
1c0b 00        nop     
1c0c 00        nop     
1c0d 00        nop     
1c0e 00        nop     
1c0f 00        nop     
1c10 00        nop     
1c11 00        nop     
1c12 00        nop     
1c13 00        nop     
1c14 00        nop     
1c15 00        nop     
1c16 00        nop     
1c17 00        nop     
1c18 00        nop     
1c19 00        nop     
1c1a 00        nop     
1c1b 00        nop     
1c1c 00        nop     
1c1d 00        nop     
1c1e 00        nop     
1c1f 00        nop     
1c20 00        nop     
1c21 00        nop     
1c22 00        nop     
1c23 00        nop     
1c24 00        nop     
1c25 00        nop     
1c26 00        nop     
1c27 00        nop     
1c28 00        nop     
1c29 00        nop     
1c2a 00        nop     
1c2b 00        nop     
1c2c 00        nop     
1c2d 00        nop     
1c2e f5        push    af
1c2f c5        push    bc
1c30 d5        push    de
1c31 e5        push    hl
1c32 f3        di      
1c33 ed73911d  ld      (1d91h),sp
1c37 310501    ld      sp,0105h
1c3a fde1      pop     iy
1c3c dde1      pop     ix
1c3e e1        pop     hl
1c3f d1        pop     de
1c40 c1        pop     bc
1c41 f1        pop     af
1c42 ed7b1101  ld      sp,(0111h)
1c46 00        nop     
1c47 00        nop     
1c48 00        nop     
1c49 00        nop     
1c4a ed738f1d  ld      (1d8fh),sp
1c4e 318f1d    ld      sp,1d8fh
1c51 f5        push    af
1c52 c5        push    bc
1c53 d5        push    de
1c54 e5        push    hl
1c55 dde5      push    ix
1c57 fde5      push    iy
1c59 ed7b911d  ld      sp,(1d91h)
1c5d fb        ei      
1c5e 2a0301    ld      hl,(0103h)
1c61 22811d    ld      (1d81h),hl
1c64 218d1d    ld      hl,1d8dh
1c67 7e        ld      a,(hl)
1c68 e6d7      and     0d7h
1c6a 77        ld      (hl),a
1c6b 0610      ld      b,10h
1c6d 11811d    ld      de,1d81h
1c70 21771e    ld      hl,1e77h
1c73 1a        ld      a,(de)
1c74 13        inc     de
1c75 cd3b1e    call    1e3bh
1c78 05        dec     b
1c79 c2731d    jp      nz,1d73h
1c7c e1        pop     hl
1c7d d1        pop     de
1c7e c1        pop     bc
1c7f f1        pop     af
1c80 c9        ret     

1c81 00        nop     
1c82 00        nop     
1c83 00        nop     
1c84 00        nop     
1c85 00        nop     
1c86 00        nop     
1c87 00        nop     
1c88 00        nop     
1c89 00        nop     
1c8a 00        nop     
1c8b 00        nop     
1c8c 00        nop     
1c8d 00        nop     
1c8e 00        nop     
1c8f 00        nop     
1c90 00        nop     
1c91 00        nop     
1c92 00        nop     
1c93 7e        ld      a,(hl)
1c94 cdaf1d    call    1dafh
1c97 23        inc     hl
1c98 05        dec     b
1c99 c2931d    jp      nz,1d93h
1c9c c9        ret     

1c9d f5        push    af
1c9e c5        push    bc
1c9f e5        push    hl
1ca0 0604      ld      b,04h
1ca2 7e        ld      a,(hl)
1ca3 cdaf1d    call    1dafh
1ca6 23        inc     hl
1ca7 05        dec     b
1ca8 c2a21d    jp      nz,1da2h
1cab e1        pop     hl
1cac c1        pop     bc
1cad f1        pop     af
1cae c9        ret     

1caf f5        push    af
1cb0 c5        push    bc
1cb1 d5        push    de
1cb2 e5        push    hl
1cb3 5f        ld      e,a
1cb4 0e03      ld      c,03h
1cb6 cdbe1d    call    1dbeh
1cb9 e1        pop     hl
1cba d1        pop     de
1cbb c1        pop     bc
1cbc f1        pop     af
1cbd c9        ret     

1cbe f5        push    af
1cbf c5        push    bc
1cc0 d5        push    de
1cc1 e5        push    hl
1cc2 cd0500    call    0005h
1cc5 e1        pop     hl
1cc6 d1        pop     de
1cc7 c1        pop     bc
1cc8 f1        pop     af
1cc9 c9        ret     

1cca 5a        ld      e,d
1ccb 3830      jr      c,1cfdh
1ccd 64        ld      h,h
1cce 6f        ld      l,a
1ccf 63        ld      h,e
1cd0 2069      jr      nz,1d3bh
1cd2 6e        ld      l,(hl)
1cd3 73        ld      (hl),e
1cd4 74        ld      (hl),h
1cd5 72        ld      (hl),d
1cd6 75        ld      (hl),l
1cd7 63        ld      h,e
1cd8 74        ld      (hl),h
1cd9 69        ld      l,c
1cda 6f        ld      l,a
1cdb 6e        ld      l,(hl)
1cdc 2065      jr      nz,1d43h
1cde 78        ld      a,b
1cdf 65        ld      h,l
1ce0 72        ld      (hl),d
1ce1 63        ld      h,e
1ce2 69        ld      l,c
1ce3 73        ld      (hl),e
1ce4 65        ld      h,l
1ce5 72        ld      (hl),d
1ce6 0a        ld      a,(bc)
1ce7 0d        dec     c
1ce8 24        inc     h
1ce9 54        ld      d,h
1cea 65        ld      h,l
1ceb 73        ld      (hl),e
1cec 74        ld      (hl),h
1ced 73        ld      (hl),e
1cee 2063      jr      nz,1d53h
1cf0 6f        ld      l,a
1cf1 6d        ld      l,l
1cf2 70        ld      (hl),b
1cf3 6c        ld      l,h
1cf4 65        ld      h,l
1cf5 74        ld      (hl),h
1cf6 65        ld      h,l
1cf7 0a        ld      a,(bc)
1cf8 0d        dec     c
1cf9 24        inc     h
1cfa 2020      jr      nz,1d1ch
1cfc 4f        ld      c,a
1cfd 4b        ld      c,e
1cfe 0a        ld      a,(bc)
1cff 0d        dec     c
1d00 24        inc     h
1d01 2020      jr      nz,1d23h
1d03 45        ld      b,l
1d04 52        ld      d,d
1d05 52        ld      d,d
1d06 4f        ld      c,a
1d07 52        ld      d,d
1d08 202a      jr      nz,1d34h
1d0a 2a2a2a    ld      hl,(2a2ah)
1d0d 2063      jr      nz,1d72h
1d0f 72        ld      (hl),d
1d10 63        ld      h,e
1d11 2065      jr      nz,1d78h
1d13 78        ld      a,b
1d14 70        ld      (hl),b
1d15 65        ld      h,l
1d16 63        ld      h,e
1d17 74        ld      (hl),h
1d18 65        ld      h,l
1d19 64        ld      h,h
1d1a 3a2420    ld      a,(2024h)
1d1d 66        ld      h,(hl)
1d1e 6f        ld      l,a
1d1f 75        ld      (hl),l
1d20 6e        ld      l,(hl)
1d21 64        ld      h,h
1d22 3a24c5    ld      a,(0c524h)
1d25 d5        push    de
1d26 e5        push    hl
1d27 11771e    ld      de,1e77h
1d2a 0604      ld      b,04h
1d2c 1a        ld      a,(de)
1d2d be        cp      (hl)
1d2e c2371e    jp      nz,1e37h
1d31 23        inc     hl
1d32 13        inc     de
1d33 05        dec     b
1d34 c22c1e    jp      nz,1e2ch
1d37 e1        pop     hl
1d38 d1        pop     de
1d39 c1        pop     bc
1d3a c9        ret     

1d3b f5        push    af
1d3c c5        push    bc
1d3d d5        push    de
1d3e e5        push    hl
1d3f e5        push    hl
1d40 110300    ld      de,0003h
1d43 19        add     hl,de
1d44 ae        xor     (hl)
1d45 6f        ld      l,a
1d46 2600      ld      h,00h
1d48 29        add     hl,hl
1d49 29        add     hl,hl
1d4a eb        ex      de,hl
1d4b 217b1e    ld      hl,1e7bh
1d4e 19        add     hl,de
1d4f eb        ex      de,hl
1d50 e1        pop     hl
1d51 010400    ld      bc,0004h
1d54 1a        ld      a,(de)
1d55 a8        xor     b
1d56 46        ld      b,(hl)
1d57 77        ld      (hl),a
1d58 13        inc     de
1d59 23        inc     hl
1d5a 0d        dec     c
1d5b c2541e    jp      nz,1e54h
1d5e e1        pop     hl
1d5f d1        pop     de
1d60 c1        pop     bc
1d61 f1        pop     af
1d62 c9        ret     

1d63 f5        push    af
1d64 c5        push    bc
1d65 e5        push    hl
1d66 21771e    ld      hl,1e77h
1d69 3eff      ld      a,0ffh
1d6b 0604      ld      b,04h
1d6d 77        ld      (hl),a
1d6e 23        inc     hl
1d6f 05        dec     b
1d70 c26d1e    jp      nz,1e6dh
1d73 e1        pop     hl
1d74 c1        pop     bc
1d75 f1        pop     af
1d76 c9        ret     

1d77 00        nop     
1d78 00        nop     
1d79 00        nop     
1d7a 00        nop     
1d7b 00        nop     
1d7c 00        nop     
1d7d 00        nop     
1d7e 00        nop     
1d7f 77        ld      (hl),a
1d80 07        rlca    
1d81 3096      jr      nc,1d19h
1d83 ee0e      xor     0eh
1d85 61        ld      h,c
1d86 2c        inc     l
1d87 99        sbc     a,c
1d88 09        add     hl,bc
1d89 51        ld      d,c
1d8a ba        cp      d
1d8b 07        rlca    
1d8c 6d        ld      l,l
1d8d c41970    call    nz,7019h
1d90 6a        ld      l,d
1d91 f48fe9    call    p,0e98fh
1d94 63        ld      h,e
1d95 a5        and     l
1d96 35        dec     (hl)
1d97 9e        sbc     a,(hl)
1d98 64        ld      h,h
1d99 95        sub     l
1d9a a3        and     e
1d9b 0edb      ld      c,0dbh
1d9d 88        adc     a,b
1d9e 3279dc    ld      (0dc79h),a
1da1 b8        cp      b
1da2 a4        and     h
1da3 e0        ret     po

1da4 d5        push    de
1da5 e9        jp      (hl)
1da6 1e97      ld      e,97h
1da8 d2d988    jp      nc,88d9h
1dab 09        add     hl,bc
1dac b6        or      (hl)
1dad 4c        ld      c,h
1dae 2b        dec     hl
1daf 7e        ld      a,(hl)
1db0 b1        or      c
1db1 7c        ld      a,h
1db2 bd        cp      l
1db3 e7        rst     20h
1db4 b8        cp      b
1db5 2d        dec     l
1db6 07        rlca    
1db7 90        sub     b
1db8 bf        cp      a
1db9 1d        dec     e
1dba 91        sub     c
1dbb 1d        dec     e
1dbc b7        or      a
1dbd 1064      djnz    1e23h
1dbf 6a        ld      l,d
1dc0 b0        or      b
1dc1 20f2      jr      nz,1db5h
1dc3 f3        di      
1dc4 b9        cp      c
1dc5 71        ld      (hl),c
1dc6 48        ld      c,b
1dc7 84        add     a,h
1dc8 be        cp      (hl)
1dc9 41        ld      b,c
1dca de1a      sbc     a,1ah
1dcc dad47d    jp      c,7dd4h
1dcf 6d        ld      l,l
1dd0 dde4ebf4  call    po,0f4ebh
1dd4 d4b551    call    nc,51b5h
1dd7 83        add     a,e
1dd8 d385      out     (85h),a
1dda c7        rst     00h
1ddb 13        inc     de
1ddc 6c        ld      l,h
1ddd 98        sbc     a,b
1dde 56        ld      d,(hl)
1ddf 64        ld      h,h
1de0 6b        ld      l,e
1de1 a8        xor     b
1de2 c0        ret     nz

1de3 fd62      ld      iyh,d
1de5 f9        ld      sp,hl
1de6 7a        ld      a,d
1de7 8a        adc     a,d
1de8 65        ld      h,l
1de9 c9        ret     

1dea ec1401    call    pe,0114h
1ded 5c        ld      e,h
1dee 4f        ld      c,a
1def 63        ld      h,e
1df0 066c      ld      b,6ch
1df2 d9        exx     
1df3 fa0f3d    jp      m,3d0fh
1df6 63        ld      h,e
1df7 8d        adc     a,l
1df8 08        ex      af,af'
1df9 0d        dec     c
1dfa f5        push    af
1dfb 3b        dec     sp
1dfc 6e        ld      l,(hl)
1dfd 20c8      jr      nz,1dc7h
1dff 4c        ld      c,h
1e00 69        ld      l,c
1e01 105e      djnz    1e61h
1e03 d5        push    de
1e04 60        ld      h,b
1e05 41        ld      b,c
1e06 e4a267    call    po,67a2h
1e09 71        ld      (hl),c
1e0a 72        ld      (hl),d
1e0b 3c        inc     a
1e0c 03        inc     bc
1e0d e4d14b    call    po,4bd1h
1e10 04        inc     b
1e11 d447d2    call    nc,0d247h
1e14 0d        dec     c
1e15 85        add     a,l
1e16 fda5      and     iyl
1e18 0a        ld      a,(bc)
1e19 b5        or      l
1e1a 6b        ld      l,e
1e1b 35        dec     (hl)
1e1c b5        or      l
1e1d a8        xor     b
1e1e fa42b2    jp      m,0b242h
1e21 98        sbc     a,b
1e22 6c        ld      l,h
1e23 dbbb      in      a,(0bbh)
1e25 c9        ret     

1e26 d6ac      sub     0ach
1e28 bc        cp      h
1e29 f9        ld      sp,hl
1e2a 40        ld      b,b
1e2b 32d86c    ld      (6cd8h),a
1e2e e3        ex      (sp),hl
1e2f 45        ld      b,l
1e30 df        rst     18h
1e31 5c        ld      e,h
1e32 75        ld      (hl),l
1e33 dcd60d    call    c,0dd6h
1e36 cf        rst     08h
1e37 ab        xor     e
1e38 d1        pop     de
1e39 3d        dec     a
1e3a 59        ld      e,c
1e3b 26d9      ld      h,0d9h
1e3d 30ac      jr      nc,1debh
1e3f 51        ld      d,c
1e40 de00      sbc     a,00h
1e42 3ac8d7    ld      a,(0d7c8h)
1e45 51        ld      d,c
1e46 80        add     a,b
1e47 bf        cp      a
1e48 d0        ret     nc

1e49 61        ld      h,c
1e4a 1621      ld      d,21h
1e4c b4        or      h
1e4d f4b556    call    p,56b5h
1e50 b3        or      e
1e51 c423cf    call    nz,0cf23h
1e54 ba        cp      d
1e55 95        sub     l
1e56 99        sbc     a,c
1e57 b8        cp      b
1e58 bd        cp      l
1e59 a5        and     l
1e5a 0f        rrca    
1e5b 2802      jr      z,1e5fh
1e5d b8        cp      b
1e5e 9e        sbc     a,(hl)
1e5f 5f        ld      e,a
1e60 05        dec     b
1e61 88        adc     a,b
1e62 08        ex      af,af'
1e63 c60c      add     a,0ch
1e65 d9        exx     
1e66 b2        or      d
1e67 b1        or      c
1e68 0b        dec     bc
1e69 e9        jp      (hl)
1e6a 24        inc     h
1e6b 2f        cpl     
1e6c 6f        ld      l,a
1e6d 7c        ld      a,h
1e6e 87        add     a,a
1e6f 58        ld      e,b
1e70 68        ld      l,b
1e71 4c        ld      c,h
1e72 11c161    ld      de,61c1h
1e75 1d        dec     e
1e76 ab        xor     e
1e77 b6        or      (hl)
1e78 66        ld      h,(hl)
1e79 2d        dec     l
1e7a 3d        dec     a
1e7b 76        halt    
1e7c dc4190    call    c,9041h
1e7f 01db71    ld      bc,71dbh
1e82 0698      ld      b,98h
1e84 d220bc    jp      nc,0bc20h
1e87 ef        rst     28h
1e88 d5        push    de
1e89 102a      djnz    1eb5h
1e8b 71        ld      (hl),c
1e8c b1        or      c
1e8d 85        add     a,l
1e8e 89        adc     a,c
1e8f 06b6      ld      b,0b6h
1e91 b5        or      l
1e92 1f        rra     
1e93 9f        sbc     a,a
1e94 bf        cp      a
1e95 e4a5e8    call    po,0e8a5h
1e98 b8        cp      b
1e99 d43378    call    nc,7833h
1e9c 07        rlca    
1e9d c9        ret     

1e9e a2        and     d
1e9f 0f        rrca    
1ea0 00        nop     
1ea1 f9        ld      sp,hl
1ea2 34        inc     (hl)
1ea3 96        sub     (hl)
1ea4 09        add     hl,bc
1ea5 a8        xor     b
1ea6 8e        adc     a,(hl)
1ea7 e1        pop     hl
1ea8 0e98      ld      c,98h
1eaa 187f      jr      1f2bh
1eac 6a        ld      l,d
1ead 0d        dec     c
1eae bb        cp      e
1eaf 08        ex      af,af'
1eb0 6d        ld      l,l
1eb1 3d        dec     a
1eb2 2d        dec     l
1eb3 91        sub     c
1eb4 64        ld      h,h
1eb5 6c        ld      l,h
1eb6 97        sub     a
1eb7 e663      and     63h
1eb9 5c        ld      e,h
1eba 016b6b    ld      bc,6b6bh
1ebd 51        ld      d,c
1ebe f41c6c    call    p,6c1ch
1ec1 61        ld      h,c
1ec2 62        ld      h,d
1ec3 85        add     a,l
1ec4 65        ld      h,l
1ec5 30d8      jr      nc,1e9fh
1ec7 f26200    jp      p,0062h
1eca 4e        ld      c,(hl)
1ecb 6c        ld      l,h
1ecc 0695      ld      b,95h
1ece ed1b      db      0edh, 1bh        ; Undocumented 8 T-State NOP
1ed0 01a57b    ld      bc,7ba5h
1ed3 82        add     a,d
1ed4 08        ex      af,af'
1ed5 f4c1f5    call    p,0f5c1h
1ed8 0f        rrca    
1ed9 c45765    call    nz,6557h
1edc b0        or      b
1edd d9        exx     
1ede c612      add     a,12h
1ee0 b7        or      a
1ee1 e9        jp      (hl)
1ee2 50        ld      d,b
1ee3 8b        adc     a,e
1ee4 be        cp      (hl)
1ee5 b8        cp      b
1ee6 eafcb9    jp      pe,0b9fch
1ee9 88        adc     a,b
1eea 7c        ld      a,h
1eeb 62        ld      h,d
1eec dd1d      dec     e
1eee df        rst     18h
1eef 15        dec     d
1ef0 da2d49    jp      c,492dh
1ef3 8c        adc     a,h
1ef4 d37c      out     (7ch),a
1ef6 f3        di      
1ef7 fb        ei      
1ef8 d44c65    call    nc,654ch
1efb 4d        ld      c,l
1efc b2        or      d
1efd 61        ld      h,c
1efe 58        ld      e,b
1eff 3ab551    ld      a,(51b5h)
1f02 cea3      adc     a,0a3h
1f04 bc        cp      h
1f05 00        nop     
1f06 74        ld      (hl),h
1f07 d4bb30    call    nc,30bbh
1f0a e24adf    jp      po,0df4ah
1f0d a5        and     l
1f0e 41        ld      b,c
1f0f 3d        dec     a
1f10 d8        ret     c

1f11 95        sub     l
1f12 d7        rst     10h
1f13 a4        and     h
1f14 d1        pop     de
1f15 c46dd3    call    nz,0d36dh
1f18 d6f4      sub     0f4h
1f1a fb        ei      
1f1b 43        ld      b,e
1f1c 69        ld      l,c
1f1d e9        jp      (hl)
1f1e 6a        ld      l,d
1f1f 34        inc     (hl)
1f20 6e        ld      l,(hl)
1f21 d9        exx     
1f22 fcad67    call    m,67adh
1f25 88        adc     a,b
1f26 46        ld      b,(hl)
1f27 da60b8    jp      c,0b860h
1f2a d0        ret     nc

1f2b 44        ld      b,h
1f2c 04        inc     b
1f2d 2d        dec     l
1f2e 73        ld      (hl),e
1f2f 33        inc     sp
1f30 03        inc     bc
1f31 1d        dec     e
1f32 e5        push    hl
1f33 aa        xor     d
1f34 0a        ld      a,(bc)
1f35 4c        ld      c,h
1f36 5f        ld      e,a
1f37 dd0d      dec     c
1f39 7c        ld      a,h
1f3a c9        ret     

1f3b 50        ld      d,b
1f3c 05        dec     b
1f3d 71        ld      (hl),c
1f3e 3c        inc     a
1f3f 27        daa     
1f40 02        ld      (bc),a
1f41 41        ld      b,c
1f42 aa        xor     d
1f43 be        cp      (hl)
1f44 0b        dec     bc
1f45 1010      djnz    1f57h
1f47 c9        ret     

1f48 0c        inc     c
1f49 2086      jr      nz,1ed1h
1f4b 57        ld      d,a
1f4c 68        ld      l,b
1f4d b5        or      l
1f4e 25        dec     h
1f4f 206f      jr      nz,1fc0h
1f51 85        add     a,l
1f52 b3        or      e
1f53 b9        cp      c
1f54 66        ld      h,(hl)
1f55 d409ce    call    nc,0ce09h
1f58 61        ld      h,c
1f59 e49f5e    call    po,5e9fh
1f5c def9      sbc     a,0f9h
1f5e 0e29      ld      c,29h
1f60 d9        exx     
1f61 c9        ret     

1f62 98        sbc     a,b
1f63 b0        or      b
1f64 d0        ret     nc

1f65 98        sbc     a,b
1f66 22c7d7    ld      (0d7c7h),hl
1f69 a8        xor     b
1f6a b4        or      h
1f6b 59        ld      e,c
1f6c b3        or      e
1f6d 3d        dec     a
1f6e 17        rla     
1f6f 2eb4      ld      l,0b4h
1f71 0d        dec     c
1f72 81        add     a,c
1f73 b7        or      a
1f74 bd        cp      l
1f75 5c        ld      e,h
1f76 3b        dec     sp
1f77 c0        ret     nz

1f78 ba        cp      d
1f79 6c        ld      l,h
1f7a ad        xor     l
1f7b edb8      lddr    
1f7d 83        add     a,e
1f7e 209a      jr      nz,1f1ah
1f80 bf        cp      a
1f81 b3        or      e
1f82 b6        or      (hl)
1f83 03        inc     bc
1f84 b6        or      (hl)
1f85 e20c74    jp      po,740ch
1f88 b1        or      c
1f89 d29aea    jp      nc,0ea9ah
1f8c d5        push    de
1f8d 47        ld      b,a
1f8e 39        add     hl,sp
1f8f 9d        sbc     a,l
1f90 d277af    jp      nc,0af77h
1f93 04        inc     b
1f94 db26      in      a,(26h)
1f96 15        dec     d
1f97 73        ld      (hl),e
1f98 dc1683    call    c,8316h
1f9b e3        ex      (sp),hl
1f9c 63        ld      h,e
1f9d 0b        dec     bc
1f9e 12        ld      (de),a
1f9f 94        sub     h
1fa0 64        ld      h,h
1fa1 3b        dec     sp
1fa2 84        add     a,h
1fa3 0d        dec     c
1fa4 6d        ld      l,l
1fa5 6a        ld      l,d
1fa6 3e7a      ld      a,7ah
1fa8 6a        ld      l,d
1fa9 5a        ld      e,d
1faa a8        xor     b
1fab e40ecf    call    po,0cf0eh
1fae 0b        dec     bc
1faf 93        sub     e
1fb0 09        add     hl,bc
1fb1 ff        rst     38h
1fb2 9d        sbc     a,l
1fb3 0a        ld      a,(bc)
1fb4 00        nop     
1fb5 ae        xor     (hl)
1fb6 27        daa     
1fb7 7d        ld      a,l
1fb8 07        rlca    
1fb9 9e        sbc     a,(hl)
1fba b1        or      c
1fbb f0        ret     p

1fbc 0f        rrca    
1fbd 93        sub     e
1fbe 44        ld      b,h
1fbf 87        add     a,a
1fc0 08        ex      af,af'
1fc1 a3        and     e
1fc2 d21e01    jp      nc,011eh
1fc5 f26869    jp      p,6968h
1fc8 06c2      ld      b,0c2h
1fca fef7      cp      0f7h
1fcc 62        ld      h,d
1fcd 57        ld      d,a
1fce 5d        ld      e,l
1fcf 80        add     a,b
1fd0 65        ld      h,l
1fd1 67        ld      h,a
1fd2 cb19      rr      c
1fd4 6c        ld      l,h
1fd5 3671      ld      (hl),71h
1fd7 6e        ld      l,(hl)
1fd8 6b        ld      l,e
1fd9 06e7      ld      b,0e7h
1fdb fed4      cp      0d4h
1fdd 1b        dec     de
1fde 76        halt    
1fdf 89        adc     a,c
1fe0 d32b      out     (2bh),a
1fe2 e0        ret     po

1fe3 10da      djnz    1fbfh
1fe5 7a        ld      a,d
1fe6 5a        ld      e,d
1fe7 67        ld      h,a
1fe8 dd4a      ld      c,d
1fea ccf9b9    call    z,0b9f9h
1fed df        rst     18h
1fee 6f        ld      l,a
1fef 8e        adc     a,(hl)
1ff0 be        cp      (hl)
1ff1 ef        rst     28h
1ff2 f9        ld      sp,hl
1ff3 17        rla     
1ff4 b7        or      a
1ff5 be        cp      (hl)
1ff6 43        ld      b,e
1ff7 60        ld      h,b
1ff8 b0        or      b
1ff9 8e        adc     a,(hl)
1ffa d5        push    de
1ffb d6d6      sub     0d6h
1ffd a3        and     e
1ffe e8        ret     pe

1fff a1        and     c
2000 d1        pop     de
2001 93        sub     e
2002 7e        ld      a,(hl)
2003 38d8      jr      c,1fddh
2005 c2c44f    jp      nz,4fc4h
2008 df        rst     18h
2009 f252d1    jp      p,0d152h
200c bb        cp      e
200d 67        ld      h,a
200e f1        pop     af
200f a6        and     (hl)
2010 bc        cp      h
2011 57        ld      d,a
2012 67        ld      h,a
2013 3f        ccf     
2014 b5        or      l
2015 06dd      ld      b,0ddh
2017 48        ld      c,b
2018 b2        or      d
2019 364b      ld      (hl),4bh
201b d8        ret     c

201c 0d        dec     c
201d 2b        dec     hl
201e daaf0a    jp      c,0aafh
2021 1b        dec     de
2022 4c        ld      c,h
2023 3603      ld      (hl),03h
2025 4a        ld      c,d
2026 f641      or      41h
2028 04        inc     b
2029 7a        ld      a,d
202a 60        ld      h,b
202b df        rst     18h
202c 60        ld      h,b
202d ef        rst     28h
202e c3a867    jp      67a8h
2031 df        rst     18h
2032 55        ld      d,l
2033 316e8e    ld      sp,8e6eh
2036 ef        rst     28h
2037 46        ld      b,(hl)
2038 69        ld      l,c
2039 be        cp      (hl)
203a 79        ld      a,c
203b cb61      bit     4,c
203d b3        or      e
203e 8c        adc     a,h
203f bc        cp      h
2040 66        ld      h,(hl)
2041 83        add     a,e
2042 1a        ld      a,(de)
2043 25        dec     h
2044 6f        ld      l,a
2045 d2a052    jp      nc,52a0h
2048 68        ld      l,b
2049 e236cc    jp      po,0cc36h
204c 0c        inc     c
204d 77        ld      (hl),a
204e 95        sub     l
204f bb        cp      e
2050 0b        dec     bc
2051 47        ld      b,a
2052 03        inc     bc
2053 220216    ld      (1602h),hl
2056 b9        cp      c
2057 55        ld      d,l
2058 05        dec     b
2059 262f      ld      h,2fh
205b c5        push    bc
205c ba        cp      d
205d 3b        dec     sp
205e be        cp      (hl)
205f b2        or      d
2060 bd        cp      l
2061 0b        dec     bc
2062 282b      jr      z,208fh
2064 b4        or      h
2065 5a        ld      e,d
2066 92        sub     d
2067 5c        ld      e,h
2068 b3        or      e
2069 6a        ld      l,d
206a 04        inc     b
206b c2d7ff    jp      nz,0ffd7h
206e a7        and     a
206f b5        or      l
2070 d0        ret     nc

2071 cf        rst     08h
2072 312cd9    ld      sp,0d92ch
2075 9e        sbc     a,(hl)
2076 8b        adc     a,e
2077 5b        ld      e,e
2078 deae      sbc     a,0aeh
207a 1d        dec     e
207b 9b        sbc     a,e
207c 64        ld      h,h
207d c2b0ec    jp      nz,0ecb0h
2080 63        ld      h,e
2081 f22675    jp      p,7526h
2084 6a        ld      l,d
2085 a3        and     e
2086 9c        sbc     a,h
2087 02        ld      (bc),a
2088 6d        ld      l,l
2089 93        sub     e
208a 0a        ld      a,(bc)
208b 9c        sbc     a,h
208c 09        add     hl,bc
208d 06a9      ld      b,0a9h
208f eb        ex      de,hl
2090 0e36      ld      c,36h
2092 3f        ccf     
2093 72        ld      (hl),d
2094 07        rlca    
2095 67        ld      h,a
2096 85        add     a,l
2097 05        dec     b
2098 00        nop     
2099 57        ld      d,a
209a 13        inc     de
209b 95        sub     l
209c bf        cp      a
209d 4a        ld      c,d
209e 82        add     a,d
209f e2b87a    jp      po,7ab8h
20a2 14        inc     d
20a3 7b        ld      a,e
20a4 b1        or      c
20a5 2b        dec     hl
20a6 ae        xor     (hl)
20a7 0c        inc     c
20a8 b6        or      (hl)
20a9 1b        dec     de
20aa 3892      jr      c,203eh
20ac d28e9b    jp      nc,9b8eh
20af e5        push    hl
20b0 d5        push    de
20b1 be        cp      (hl)
20b2 0d        dec     c
20b3 7c        ld      a,h
20b4 dcefb7    call    c,0b7efh
20b7 0b        dec     bc
20b8 dbdf      in      a,(0dfh)
20ba 2186d3    ld      hl,0d386h
20bd d2d4f1    jp      nc,0f1d4h
20c0 d4e242    call    nc,42e2h
20c3 68        ld      l,b
20c4 ddb3      or      e
20c6 f8        ret     m

20c7 1f        rra     
20c8 da836e    jp      c,6e83h
20cb 81        add     a,c
20cc be        cp      (hl)
20cd 16cd      ld      d,0cdh
20cf f6b9      or      0b9h
20d1 265b      ld      h,5bh
20d3 6f        ld      l,a
20d4 b0        or      b
20d5 77        ld      (hl),a
20d6 e1        pop     hl
20d7 18b7      jr      2090h
20d9 47        ld      b,a
20da 77        ld      (hl),a
20db 88        adc     a,b
20dc 08        ex      af,af'
20dd 5a        ld      e,d
20de e6ff      and     0ffh
20e0 0f        rrca    
20e1 6a        ld      l,d
20e2 70        ld      (hl),b
20e3 66        ld      h,(hl)
20e4 063b      ld      b,3bh
20e6 ca1101    jp      z,0111h
20e9 0b        dec     bc
20ea 5c        ld      e,h
20eb 8f        adc     a,a
20ec 65        ld      h,l
20ed 9e        sbc     a,(hl)
20ee ff        rst     38h
20ef f8        ret     m

20f0 62        ld      h,d
20f1 ae        xor     (hl)
20f2 69        ld      l,c
20f3 61        ld      h,c
20f4 6b        ld      l,e
20f5 ff        rst     38h
20f6 d316      out     (16h),a
20f8 6c        ld      l,h
20f9 cf        rst     08h
20fa 45        ld      b,l
20fb a0        and     b
20fc 0a        ld      a,(bc)
20fd e278d7    jp      po,0d778h
2100 0d        dec     c
2101 d2ee4e    jp      nc,4eeeh
2104 04        inc     b
2105 83        add     a,e
2106 54        ld      d,h
2107 39        add     hl,sp
2108 03        inc     bc
2109 b3        or      e
210a c2a767    jp      nz,67a7h
210d 2661      ld      h,61h
210f d0        ret     nc

2110 60        ld      h,b
2111 16f7      ld      d,0f7h
2113 49        ld      c,c
2114 69        ld      l,c
2115 47        ld      b,a
2116 4d        ld      c,l
2117 3e6e      ld      a,6eh
2119 77        ld      (hl),a
211a dbae      in      a,(0aeh)
211c d1        pop     de
211d 6a        ld      l,d
211e 4a        ld      c,d
211f d9        exx     
2120 d65a      sub     5ah
2122 dc40df    call    c,0df40h
2125 0b        dec     bc
2126 66        ld      h,(hl)
2127 37        scf     
2128 d8        ret     c

2129 3b        dec     sp
212a f0        ret     p

212b a9        xor     c
212c bc        cp      h
212d ae        xor     (hl)
212e 53        ld      d,e
212f debb      sbc     a,0bbh
2131 9e        sbc     a,(hl)
2132 c5        push    bc
2133 47        ld      b,a
2134 b2        or      d
2135 cf        rst     08h
2136 7f        ld      a,a
2137 30b5      jr      nc,20eeh
2139 ff        rst     38h
213a e9        jp      (hl)
213b bd        cp      l
213c bd        cp      l
213d f21cca    jp      p,0ca1ch
2140 ba        cp      d
2141 c28a53    jp      nz,538ah
2144 b3        or      e
2145 93        sub     e
2146 3024      jr      nc,216ch
2148 b4        or      h
2149 a3        and     e
214a a6        and     (hl)
214b ba        cp      d
214c d0        ret     nc

214d 3605      ld      (hl),05h
214f cdd706    call    06d7h
2152 93        sub     e
2153 54        ld      d,h
2154 de57      sbc     a,57h
2156 29        add     hl,hl
2157 23        inc     hl
2158 d9        exx     
2159 67        ld      h,a
215a bf        cp      a
215b b3        or      e
215c 66        ld      h,(hl)
215d 7a        ld      a,d
215e 2ec4      ld      l,0c4h
2160 61        ld      h,c
2161 4a        ld      c,d
2162 b8        cp      b
2163 5d        ld      e,l
2164 68        ld      l,b
2165 1b        dec     de
2166 02        ld      (bc),a
2167 2a6f2b    ld      hl,(2b6fh)
216a 94        sub     h
216b b4        or      h
216c 0b        dec     bc
216d be        cp      (hl)
216e 37        scf     
216f c30c8e    jp      8e0ch
2172 a1        and     c
2173 5a        ld      e,d
2174 05        dec     b
2175 df        rst     18h
2176 1b        dec     de
2177 2d        dec     l
2178 02        ld      (bc),a
2179 ef        rst     28h
217a 8d        adc     a,l

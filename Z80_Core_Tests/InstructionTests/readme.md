## Notes

This folder contains the complete set of instruction tests for the Z80 instruction set. Note that a convention has been used to name the tests, as follows:

* r: a register
* n: an immediate byte
* rr: a register pair
* nn: an immediate word

IndexOffset: either IX or IY as an index address plus a signed byte offset

*'x' before any value indicates the value should be treated as an address. So, for example:
*LD_r_n - load register r with immediate byte n
*LD_rr_nn - load register pair rr with immediate word nn
*LD_xHL_r - load value in register r to memory address pointed to by HL
*LD_rr_xIndexOffset - load register pair rr with word stored at address pointed to by IX or IY + the offset

and so on. The purpose of any test should therefore be obvious by inspection. For more details refer to the Z80 instruction table and reference material in /docs.

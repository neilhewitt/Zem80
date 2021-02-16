# Instruction Tests

The set of instruction tests is designed to exercise all the instructions of the Z80 and check that they produce the results defined by the
Z80 manual (and other sources that define undocumented behaviour).

These tests do **not** perform machine-cycle-level verification of internal microcode steps or machine cycles.

The tests are divided up into two sections: Microcode tests and Zexall tests

## Microcode tests

These are a set of conventional NUnit tests which run various instructions, checking the affected state, output values, and flags. They verify that the 
instruction execution leaves the state (memory, flags, registers etc) as defined in the Z80 manual and also that the undocumented X & Y flags are set correctly.
They do not currently test the value of the internal WZ register, although Zem80 does implement the WZ register.

These tests only cover instructions which are not exercised by the Zexall tests. 

## Zexall tests

This runs a set of state tests derived from Frank Cringle's Zexall test suite. Zexall is an instruction exerciser written as a Z80 assembly program. 
It uses test state vectors to run a series of instructions and calculates a checksum from the outcome state of each execution, then checks this checksum 
against a known-good value from an actual Z80 CPU. As such, it's good for verifying behaviour but you can't easily diagnose a particular instruction fault with it.
The original program was built to run under CP/M and contains calls to CPM's BDOS API. Versions for other platforms (notably the ZX Spectrum) were made. 

These tests were created using the CPM version of the program with amends by J.G. Harston to allow it to be assembled with the ZMAC assembler.

The Zexall test set tests most of the Z80 instructions, but not all. Those not tested by Zexall are tested separately (see Microcode tests above). It also tests undocumented
instructions (though not undocumented overloads) and the undocumented X & Y flags.

I created a framework called ZexNext to run state-based tests directly from C# and hooked this into Zem80. ZexNext sets up each instruction run separately (rather than letting the state change over time like Zexall) by setting the full processor state (as well as the contents of a memory buffer) from its test data set before executing the instruction, then collecting the state after execution and comparing it to the known-good expected state for a pass/fail per instruction. 

I then extracted the complete state space from the Zexall tests using a known-good emulator (SpectNetIDE - https://github.com/Dotneteer/spectnetide) so that it would
be correct to the Z80 hardware, and turned this into the input for ZexNext.

The Zexall tests in this project consist of a single test with a test parameter set consisting of the names of each Zexall test set (there are 60+) and for each of these
it uses ZexNext to run the test set against the Zem80 emulator and compares the results with its known-good data. If every test in the set passes, the NUnit test passes.

Because there are a very large number of instructions to execute, running the NUnit tests in this folder takes up to 30 seconds. So they should not be run on every build or
using NCrunch etc. 

Bear in mind that each NUnit test covers a large number of test executions across potentially several instructions. However, if you need to find the specific failing test/s,
you can set a breakpoint in the NUnit test and dig into the Zexnext results which contain a pass/fail for each individual instruction execution including the input state,
expected output state, contents of the memory buffer (the Zexall tests use a small area of memory to write persistent data).

ZexNext is included here as a binary dependency (a DLL), but the ZexNext project will be available on Github later in 2021 and will be able to be interfaced with other emulators written in .NET (and potentially other languages via COM interop on Windows). 

The ZexNext test data is included as a Zip file which is unzipped on execution of the tests into a file called 'Zexall.zxl' (which I cannot check directly into Github as it's too big). The format of this text file will be documented as part of the Zexnext Github project. **Please note that if you edit this file you could very easily cause false negatives.**

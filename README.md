# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.0 or above. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

The first module of the project will be the Z80 core emulator itself. After this I aim to write a very simple virtual machine to host the Z80 and run a test suite. Once this is done, the Z80 emulation should be usable for building logical emulations of real Z80-based machines. 

Beyond this, I *may* attempt to write a ZX Spectrum VM hosting the Z80.

## Project status
07/10/20 - *Renamed the project from 'Z80 Emulator' to 'Zem80'. There are basically no unique names left for Z80 emulators on Github :-) I renamed all the projects and namespaces and restructured the project files.*

At this point, the emulator is ready to be used in projects, with some caveats. 

* I'm looking at writing some documentation and some how-to pages. For now, the code is all there is.
* The test suite has been gutted and is being re-built to do instruction tests via ZexNext (this will be a project in its own right soon), and new tests will be added (sorry, this isn't a TDD project!).
* All of the projects in the solution are built for .NET Core 3.1 and all the libraries (Zem80.Core etc) are built to .NET Standard 2.1

21/09/20 - The Z80 emulation is complete and passes all Zexlax tests including for undocumented instructions and behaviours. 

I've added a new Zexlax test frame which will become a separate project. I also added a simple ZX Spectrum emulator that boots the Spectrum 48K ROM (no keyboard handling as yet, so you can't actually *use* it!'), which I will expand and refactor until it's a basic working sample. The goal of the project is not to write Yet Another Spectrum Emulator - there are plenty already. But the ZX Spectrum emulation will provide an example of how to integrate the Z80 Emulator library into a project. 

The unit tests for the project are *broken*. The tests themselves are fine, but the test cases are wrong because the CPU behaviour has changed (to be correct). I am deciding whether to re-generate the test cases and update the test suite, or remove all the instruction unit tests and replace them with ZexNext (the new Zexlax test environment), possibly integrated into Visual Studio. For now, if you use this code, remove the unit tests.

Tests for non-instruction code were never really done beyond some very basic stuff, so this is an area I'm looking to work on now. 

Looking for a new name for the project - 'Z80 Emulator' is descriptive but boring, and there are many other projects out there with the same name!

10/07/20 - Huge changes since the last update. I've implemented timing and machine cycles, and heavily refactored most areas of the emulation. About 75% of the Zexdoc test suite now passes, but the unit test suite needs a refresh of test cases. My focus now is on getting Zexdoc (and then Zexall) to pass 100%. Once is is done and the test suite is fully updated and passing, I will resume work on the ZX Spectrum VM that I began work on a while back - this has support for the basic 48K Spectrum architecture including the display system, but not yet other forms of IO, keyboard, beeper etc. I paused that because the Spectrum ROM simply didn't work due to implementation problems with the emulator itself. Next merge to main (renamed from master) is some way away.

18/04/20 - Instruction test suite passes and all instructions are verified per the Z80 manual, however Zexdoc / Zexall do not pass at all and there are clearly still problems. However, I've merged dev to master as a snapshot of a working emulator without timings. Next step is to implement timing and then attempt to create a VM simulating real hardware (currently considering a simple CPM machine, or else the ZX80/ZX81, before the more complex ZX Spectrum). This will do a better job of revealing any remaining issues with instruction implementation or flag handling. Undocumented flags are not yet implemented although all undocumented instructions for the Zilog / clone Z80 are available. 

01/03/20 - Instruction test suite is done. Added Zexdoc / Zexall to the project - CP/M version - plus an asm stub to connect CP/M bdos calls needed by Zexdoc for screen output to the SimpleVM. Console test program now runs Zexdoc (currently it just loops endlessly, which tells me I have a microcode / flag problem). The flags system was re-done (and all microcode and tests reviewed and refactored) to ensure we're setting the right flags and leaving the ones marked 'not affected' alone rather than reset (documented only - doing the undocumented flags has to wait until I have a working VM). Merged back to Master as this version does build and run Z80 code. However, the test suite still needs completing and I'm likely going to further refactor the flag system. Then I'm going to work on timing which currently just isn't considered in the code, and to go much further than that I'll need to create a VM that emulates at least one actual system for which there is software available - possibly the ZX81, possibly something else. Then optimisation will be the name of the game - right now, the emulation runs too slowly, such that some instructions do not complete in the time they would do on an actual Z80 running at 4MHz! Oh, and Zexdoc just goes haywire after the third test (having failed tests 1-3). Not ready for prime-time yet!

27/02/20 - *still* working on the test suite, and I just refactored all the tests I had already done to make them more readable, which took a couple of weeks. Plus I've done some major refactoring on the emulator itself, removed a lot of redundant code, and just generally re-engineered a lot of stuff. I've merged the latest dev state to master but beware: tests are incomplete, and functionally it does nothing more than it did at the last merge. Lots of work yet to do to get a working emulator, particularly timing, and then optimisation for speed. 

12/01/20 - still working on the test suite (it's going to be bigger than the microcode by a distance) and it will take a while as I've slowed down a lot due to starting a new job. But I will probably merge a version to master soon that passes basic tests.

03/11/19 - basic framework of the emulator is in place. The emulator runs, executes instructions, and can successfully run some very basic test code. However: it's still a long way from being usable. There is currently no proper timing, I/O is something of a bodge, performance is actually slower than a Z80 at 4MHz in some cases (!), there are almost no unit tests, and most of the instructions have not been completely tested. Right now I am working on a unit test suite for the base emulator before I proceed with further development. 

25/09/19 - the project is in its infancy and I'm a long way from committing any usable code. Don't hold your breath :-)









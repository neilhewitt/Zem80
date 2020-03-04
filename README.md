# Z80_Emulator

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.0 or above. 

This emulator is strictly an *emulation* of the logical design and execution of the Z80 and not a physics-based *simulation*. If you're interested in a simulation that models the actual electronic and physical behaviour of the Z80 down to the signal level for a hardware project, I would recommend Symmetry (formerly Zim80) by obiwanjacobi:

https://github.com/obiwanjacobi/Zim80

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

The first module of the project will be the Z80 core emulator itself. After this I aim to write a very simple virtual machine to host the Z80 and run a test suite. Once this is done, the Z80 emulation should be usable for building logical emulations of real Z80-based machines. 

Beyond this, I *may* attempt to write a ZX Spectrum VM hosting the Z80.

## Project status

25/09/19 - the project is in its infancy and I'm a long way from committing any usable code. Don't hold your breath :-)

03/11/19 - basic framework of the emulator is in place. The emulator runs, executes instructions, and can successfully run some very basic test code. However: it's still a long way from being usable. There is currently no proper timing, I/O is something of a bodge, performance is actually slower than a Z80 at 4MHz in some cases (!), there are almost no unit tests, and most of the instructions have not been completely tested. Right now I am working on a unit test suite for the base emulator before I proceed with further development. 

12/01/20 - still working on the test suite (it's going to be bigger than the microcode by a distance) and it will take a while as I've slowed down a lot due to starting a new job. But I will probably merge a version to master soon that passes basic tests.

27/02/20 - *still* working on the test suite, and I just refactored all the tests I had already done to make them more readable, which took a couple of weeks. Plus I've done some major refactoring on the emulator itself, removed a lot of redundant code, and just generally re-engineered a lot of stuff. I've merged the latest dev state to master but beware: tests are incomplete, and functionally it does nothing more than it did at the last merge. Lots of work yet to do to get a working emulator, particularly timing, and then optimisation for speed. 

01/03/20 - Instruction test suite is done. Added Zexdoc / Zexall to the project - CP/M version - plus an asm stub to connect CP/M bdos calls needed by Zexdoc for screen output to the SimpleVM. Console test program now runs Zexdoc (currently it just loops endlessly, which tells me I have a microcode / flag problem). The flags system was re-done (and all microcode and tests reviewed and refactored) to ensure we're setting the right flags and leaving the ones marked 'not affected' alone rather than reset (documented only - doing the undocumented flags has to wait until I have a working VM). Merged back to Master as this version does build and run Z80 code. However, the test suite still needs completing and I'm likely going to further refactor the flag system. Then I'm going to work on timing which currently just isn't considered in the code, and to go much further than that I'll need to create a VM that emulates at least one actual system for which there is software available - possibly the ZX81, possibly something else. Then optimisation will be the name of the game - right now, the emulation runs too slowly, such that some instructions do not complete in the time they would do on an actual Z80 running at 4MHz! Oh, and Zexdoc just goes haywire after the third test (having failed tests 1-3). Not ready for prime-time yet!

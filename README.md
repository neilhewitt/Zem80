# Z80_Emulator

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.0 or above. 

This emulator is strictly an *emulation* of the logical design and execution of the Z80 and not a physics-based *simulation*. If you're interested in a simulation that models the actual electronic and physical behaviour of the Z80 down to the signal level for a hardware project, I would recommend Symmetry (formerly Zim80) by obiwanjacobi:

https://github.com/obiwanjacobi/Zim80

## Project goals

The first module of the project will be the Z80 core emulator itself. After this I aim to write a very simple virtual machine to host the Z80 and run a test suite. Once this is done, the Z80 emulation should be usable for building logical emulations of real Z80-based machines. 

Beyond this, I *may* attempt to write a ZX Spectrum VM hosting the Z80.

## Project status

25/09/19 - the project is in its infancy and I'm a long way from committing any usable code. Don't hold your breath :-)

03/11/19 - basic framework of the emulator is in place. The emulator runs, executes instructions, and can successfully run some very basic test code. However: it's still a long way from being usable. There is currently no proper timing, I/O is something of a bodge, performance is actually slower than a Z80 at 4MHz in some cases (!), there are almost no unit tests, and most of the instructions have not been completely tested. Right now I am working on a unit test suite for the base emulator before I proceed with further development. 

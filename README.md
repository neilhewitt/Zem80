# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.1 or above. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

I have now written a complete Z80 emulation including as much of the undocumented instructions and behaviour as I could find.

I have also added a basic ZX Spectrum emulation, but this is a sample and not intended for actual use as an emulator. 

## Project status
15/01/21 - **1.0 Release**. As far as I can tell, the Z80 emulation is complete and fully working. That said, I cannot warrant that there are no bugs! The ZX Spectrum demo VM works (in terms of the BASIC editor), and some games run well, while others do not (this is more likely to be due to defects in the Spectrum emulation rather than the Z80 core itself). I will not be extending the Spectrum VM any further, as it now serves its purpose as a sample. I will consider adding some further demo VMs, but I'm not sure which machine I want to tackle next!

13/01/21 - I finally found the problem that was preventing the Spectrum editor from working (an implementation bug in DJNZ), plus a number of other bugs in the emulation which were probably not breaking anything but which still needed fixing before games are likely to run. 

19/12/20 - I found a pretty major bug (the EX (SP),rr instruction was just not working) which was what was stopping the BASIC editor from working in the Spectrum VM. I also found a number of other small issues and have corrected them. The editor still does not work correctly but BASIC commands can be executed in immediate mode, so we're close... I may try to run some actual Spectrum games to see how bad things are. 

I also added further support for the undocumented X/Y aka 3/5 flags. This includes adding the WZ aka MEMPTR internal register, and all the cases that set this register, so that the X/Y flags for the BIT instruction work as per the Zilog chip implementation.

**As before, be careful about using the Z80 emulation in other projects. It is not guaranteed to be working yet.**

18/11/20 - It looks like there are still some bugs within the emulation, despite it passing the Zexall tests, as my attempts to emulate the basic ZX Spectrum have failed. I can get the VM to boot and the ROM runs to the point where the Sinclair copyright message is displayed, but I can't get the line editor to work and this suggests that somewhere either a flag is not being set correctly or an instruction is not working properly. I am currently integrating some new tests ('Zexall2') which should give the whole instruction set a bigger workout and hopefully I'll find where things are going wrong. 

* I'm looking at writing some documentation and some how-to pages. For now, the code is all there is.
* The test suite has been gutted and is being re-built to do instruction tests via ZexNext (this will be a project in its own right soon), and new tests will be added (sorry, this isn't a TDD project!).
* All of the projects in the solution are built for .NET Core 3.1 and all the libraries (Zem80.Core etc) are built to .NET Standard 2.1

## LICENSE ##

MIT License

Copyright (c) 2020 Neil Hewitt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.1 or above. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

I have now written a complete Z80 emulation including as much of the undocumented instructions and behaviour as I could find.

I have also added a basic ZX Spectrum emulation, but this is a sample and not intended for actual use as an emulator. 

## Project status
15/01/21 - **1.0 Release**. As far as I can tell, the Z80 emulation is complete and fully working. That said, I cannot warrant that there are no bugs! The ZX Spectrum demo VM works (in terms of the BASIC editor), and some games run well, while others do not (this is more likely to be due to defects in the Spectrum emulation rather than the Z80 core itself). I will not be extending the Spectrum VM any further, as it now serves its purpose as a sample. I will consider adding some further demo VMs, but I'm not sure which machine I want to tackle next!

### Known issues ###
There is a bug in the timing code for this release, so that the emulator always runs in non-realtime mode, and thus the emulation
may run faster than expected. If real-time performance per the assigned clock speed is a goal for your project, you should update to v1.0.1
which will contain this and other bug fixes and will be available soon.

* I'm looking at writing some documentation and some how-to pages. For now, the code is all there is.
* The test suite has been gutted and is being re-built, and new tests will be added (sorry, this isn't a TDD project!).
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

# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET Core 3.0 or above. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

The first module of the project will be the Z80 core emulator itself. After this I aim to write a very simple virtual machine to host the Z80 and run a test suite. Once this is done, the Z80 emulation should be usable for building logical emulations of real Z80-based machines. 

Beyond this, I *may* attempt to write a ZX Spectrum VM hosting the Z80.

## Project status
23/10/20 - Version 1.0 official. Made a few minor refactorings and added the ZexNext test framework (currently as a simple binary dependency - NuGet packaging will come) into the test suite. Note that these tests do take several seconds to run, so you should probably not run them for every single build. A full test suite and some documentation is my next project. 

15/10/20 - Version 1.0 candidate (not yet declared)

07/10/20 - *Renamed the project from 'Z80 Emulator' to 'Zem80'. There are basically no unique names left for Z80 emulators on Github :-) I renamed all the projects and namespaces and restructured the project files.*

Ancillary projects (the ZexNext Z80 test framework, the in-progress ZX Spectrum emulation) have been moved to separate repositories.

At this point, the emulator is ready to be used in projects, with some caveats. 

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

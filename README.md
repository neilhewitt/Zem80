# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET 8.0

## Version 2.1 now available with .NET 10 and .NET Standard support

I have moved all of the projects in the solution to target .NET 10. I am also targeting .NET Standard 2.1 for Zem80_Core. 

This means that you will need to have .NET 10 installed to build and run the code, but you can consume the Zem80_Core library from any .NET version that supports .NET Standard 2.1 (ie .NET Core 3.0 and later). Make sure to use the DLLs from the _netstandard2.1_ output folder. The Nuget package generated on build only contains the .NET 10 versions.

In order to make this work I had to remove the dependency on MultimediaTimer, which doesn't support .NET Standard. I have replaced this with a similar timer implementation in the core code that should work on all platforms. One job for the immediate future is to set up test hosts for Linux and MacOS to make sure this works properly.

**CRITICAL BUG FIX:** In 2.0 I introduced a bug in the handling of the HALT instruction, where the program counter would continue to increment while halted. This would have broken lots of programs and in particular it broke the ZX Spectrum ROM. This has now been fixed in 2.1. It is critical that you upgrade to 2.1 if you have been using 2.0. The 2.0 release package has been removed.

### ZexNext source

I've included the source for the ZexNext test framework in the projects ZexNext_Core and ZexNext_Runner, as having this as a separate repo makes little sense. These libraries are used by Zem80_Core_Tests to run the ZEXALL tests within Visual Studio rather than in a console app.

### Spectrum sample changes

I moved the ZX Spectrum VM sample to MAUI from WPF. However, this sample still only runs on Windows desktop. The move was to allow better virtual screen handling. I also added a basic debugger to the Spectrum sample, because I needed to step through code in order to identify and fix the HALT bug. I've left this code in just for curiosity's sake. If you want to break a Spectrum program and step through it, you can. However, since interrupts are disabled during debugging, things like keyboard handling won't work. I may just remove this debug window in the next release, so enjoy it for now if you care to!


### Version 2.0

Zem80 v2.0 included a completely redesigned emulator core with improved performance. There are some minor breaking API and event changes, but from the point of view of consuming code, little has changed. Most of the work here was to improve the architecture of the system and tidy up the timing mechanisms. 


## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

I have now written a complete Z80 emulation including as much of the undocumented instructions and behaviour as I could find.

I have also added a basic ZX Spectrum emulation, but this is a sample and not intended for actual use as an emulator.


## Performance - some thoughts
The amount of CPU required to run the Z80 emulation is significant using the RealTimeClock. This is because the emulation thread has to spin continuously while waiting for clock ticks in order to generate the right events at exactly the right time. This is likely to only be necessary for emulating hardware that is timing-critical, and even then it's probably possible to do it a different way.

To improve this I've added a TimeSlicedClock as an alternative where the emulator will run as fast as possible but after a specified number of Z80 ticks (the 'time slice', which you will need to work out against your timing and your Z80 emulated speed) it will suspend the CPU and wait for a timer (not on the main thread) to elapse and then resumes the CPU. This will resynchronise execution with real time, while avoiding executing any code on the main thread while the timer runs. This allows for instructions to be 'fast-forwarded' as quickly as possible, while still making sure that code like screen updates can run at the proper time. The ZX Spectrum VM sample now uses this technique and this drastically reduces the PC CPU required to run it on my machine (Release build).  


## The future for this project
Where I think this project can be useful is in perhaps explaining how to do CPU emulation in principle. It's a pretty complex subject that requires a lot of learning to approach and do, and learning from the existing code is difficult because it's often quite... opaque. This emulator is actually quite straightforward - not that you would understand it at first glance, or without knowledge of how the Z80 itself works in quite a lot of detail - and hopefully might give people a useful starting point to build their own emulators. If it does, then my work is done!

I built this thing just to prove to myself that I could. I have a fully-functioning emulator built on a platform that's not generally thought of as one you could build something so low-level on. Take that, C++.

### Known issues ###

* Interrupt Mode 0 remains essentially untested and may not work properly.
* Interrupt Mode 2 appears to be slightly bugged but I'm not 100% sure how or why. I am looking for Spectrum games which are known to use IM2 to test. 

The other main component is the ZX Spectrum VM, which has several known issues:

* Game compatibility is very patchy, many games either don't run or don't run properly
* Audio in is not supported, so you cannot use the LOAD command. The SAVE command will operate but not successfully (data will be corrupt even if recorded)
* It's frankly just not very good and is certainly not usable as a real emulator for playing games etc. Download FUSE for that!

## Acknowledgements ##

This project contains code taken from **MultimediaTimer** which is copyright **Mike James** and is distributed under the terms of that project's license. See /src/Zem80_Core/CPU/Clock/Timer/LICENSE.txt.

This project contains materials derived from **ZEXALL** which is copyright **Frank Cringle**, with amendments copyright **J.G. Harston**, and which are distributed under the terms of the ZEXALL license. 

This project contains the binary code of the **ZX Spectrum 48K ROM** which is copyright **Amstrad plc** and distributed under general permission given to the community to redistribute the code for emulation purposes provided that the code is unchanged. 

Portions of this project were created using an amended version of **SpectNetIDE** (https://github.com/Dotneteer/spectnetide) which is copyright **Istvan Novak** and other contributors, but this project does not reproduce any code or assets from SpectNetIDE.

Thanks to the wide community of enthusiasts who have documented much of the process of emulating a Z80 and other CPUs in various languages. Without your hard work and contributions, this project would not have been possible. 

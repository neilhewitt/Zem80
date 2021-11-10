# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET 6. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

I have now written a complete Z80 emulation including as much of the undocumented instructions and behaviour as I could find.

I have also added a basic ZX Spectrum emulation, but this is a sample and not intended for actual use as an emulator. 

## Project status
07/02/21 - **1.0.1 Release**. Fixes the timing bug. I've also made some general improvements and changes which *might* require you to make some small changes to your integration code if you integrated 1.0 (though AFAIK, no-one did!). I added basic beeper support to the Spectrum VM, this is still buggy and the audio is a little choppy because the timing is off somewhere, but at this point I'm done with the Spectrum work, it's just there to show you how to integrate Zem80 into an actual project. 

### Known issues ###

At this point there are no *known* issues with Zem80 itself. I'm quite certain there are still bugs and omissions, but I'm not aware of them.

The other main component is the ZX Spectrum VM, which has several known issues:

* ZX Spectrum audio is choppy
* Spectrum VM occasionally crashes on keypress 
* Game compatibility is very patchy, many games either don't run or don't run properly
* Audio in is not supported, so you cannot use the LOAD command. The SAVE command will operate but not successfully (data will be corrupt even if recorded)
* It's frankly just not very good and is certainly not usable as a real emulator for playing games etc. Download FUSE for that!

### Next steps ###

* Add new tests to cover the public API other than those Z80 instructions tested by Zexall
* Add some documentation / HOWTO etc
* Consider adding XML comments (but probably not)

At the present time, I consider the main emulator development complete and will not be making changes other than bug fixes, at least until any version 2.0.

## Acknowledgements ##

This project contains materials derived from **ZEXALL** which is copyright **Frank Cringle**, with amendments copyright **J.G. Harston**, and which are distributed under the terms of the ZEXALL license. 

This project contains the binary code of the **ZX Spectrum 48K ROM** which is copyright **Amstrad plc** and distributed under general permission given to the community to redistribute the code for emulation purposes provided that the code is unchanged. 

Portions of this project were created using an amended version of **SpectNetIDE** (https://github.com/Dotneteer/spectnetide) which is copyright **Istvan Novak** and other contributors, but this project does not reproduce any code or assets from SpectNetIDE.

Thanks to the wide community of enthusiasts who have documented much of the process of emulating a Z80 and other CPUs in various languages. Without your hard work and contributions, this project would not have been possible. 

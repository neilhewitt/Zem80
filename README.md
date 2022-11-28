# Zem80

A simple emulation of the Z80 CPU written in C# and running on .NET 7. 

## Project goals

Most emulators written in most languages are not very representative of those languages. Since emulation is all about twiddling bits and doing things as fast as possible, successful emulators tend to look quite unlike everyday software written on a given platform, and are often pretty incomprehensible. 

I'm trying, as far as is possible given the domain, to write idiomatic C# code, with fairly standard OO design principles. I'm aware that I will probably not get the best performance this way. But that's a problem I'll deal with if - and only if - it proves impossible to build an emulation core that can run Z80 software (CP/M, Spectrum, whatever) at a speed comparable to the original hardware.  

I have now written a complete Z80 emulation including as much of the undocumented instructions and behaviour as I could find.

I have also added a basic ZX Spectrum emulation, but this is a sample and not intended for actual use as an emulator.

## Performance - some thoughts
The amount of CPU required to run the Z80 emulation is significant in PseudoRealTime mode. This is because the emulation thread has to spin continuously while waiting for clock ticks in order to generate the right events at exactly the right time. This is likely to only be necessary for emulating hardware that is timing-critical, and even then it's probably possible to do it a different way.

To improve this I've added a TimeSliced timing mode where the emulator will run as fast as possible but after a specified number of Z80 ticks (the 'time slice', which you will need to work out against your timing and your Z80 emulated speed) it will fire an event. If you subscribe to this event then you can, for example, choose to suspend the Z80 at this point, wait for a timing event in your own code, and then resume the Z80; this allows for instructions to be 'fast-forwarded' as quickly as possible, while still making sure that code like screen updates can run at the proper time. The ZX Spectrum VM sample now uses this technique and this reduces the PC CPU required to run it to 2% on my machine (Release build).  

## The future for this project
Where I think this project can be useful is in perhaps explaining how to do CPU emulation in principle. It's a pretty complex subject that requires a lot of learning to approach and do, and learning from the existing code is difficult because it's often quite... opaque. This emulator is actually quite straightforward - not that you would understand it at first glance, or without knowledge of how the Z80 itself works in quite a lot of detail - and hopefully might give people a useful starting point to build their own emulators. If it does, then my work is done!

I built this thing just to prove to myself that I could. I have a fully-functioning emulator built in a platform that's not generally thought of as one you could build something so low-level in, and I'm very glad I did it.

## Project status
28/11/2022 - 1.2.1 release. I did a bunch of refactoring and I fixed a couple of fairly obscure bugs. I also improved the timing mechanism so that pseudo-real-time mode gets as close as possible to real time, and added a time-slice timing mode (described above). I also replaced the Spectrum audio implementation and this plus the timing work finally fixed the audio.

I moved the whole project to .NET 7, so this is the lowest version you can run the Zem80 library on with this build. I am looking at multi-targeting for the next release since there's no reason in principle why I can't ship a .NET Standard 2.0 version alongside a .NET 7 build. 

### Known issues ###

*Interrupt Mode 0 remains essentially untested and may not work properly. I plan to create a virtual machine to test this out. 
*Interrupt Mode 2 appears to be slightly bugged but I'm not 100$% sure how or why. I am looking for Spectrum games which are known to use IM2 to test. 

The other main component is the ZX Spectrum VM, which has several known issues:

* Game compatibility is very patchy, many games either don't run or don't run properly
* Audio in is not supported, so you cannot use the LOAD command. The SAVE command will operate but not successfully (data will be corrupt even if recorded)
* It's frankly just not very good and is certainly not usable as a real emulator for playing games etc. Download FUSE for that!

### Next steps ###

* Add new tests to cover the public API other than those Z80 instructions tested by Zexall
* Add some documentation / HOWTO etc
* Consider adding XML comments (but probably not)

## Acknowledgements ##

This project contains materials derived from **ZEXALL** which is copyright **Frank Cringle**, with amendments copyright **J.G. Harston**, and which are distributed under the terms of the ZEXALL license. 

This project contains the binary code of the **ZX Spectrum 48K ROM** which is copyright **Amstrad plc** and distributed under general permission given to the community to redistribute the code for emulation purposes provided that the code is unchanged. 

Portions of this project were created using an amended version of **SpectNetIDE** (https://github.com/Dotneteer/spectnetide) which is copyright **Istvan Novak** and other contributors, but this project does not reproduce any code or assets from SpectNetIDE.

Thanks to the wide community of enthusiasts who have documented much of the process of emulating a Z80 and other CPUs in various languages. Without your hard work and contributions, this project would not have been possible. 

# Sample Virtual Machines

This folder contains sample VMs written to use the Zem80 emulation library.

## Samples

### ZX Spectrum

A very simple emulation of the ZX Spectrum 48K. The VM itself is a .NET Standard 2.1 project but the UI in which this sample runs
is a Windows Forms application and can only run on Windows. 

This VM does not have an implementation of sound output or input, so loading programs from the Spectrum command line will not work,
and sound (beep etc) will not play.

There is a LoadSnapshot method to allow you to load a .SNA snapshot which will allow you to run Spectrum games, but many games
do not run properly or at all (most likely due to defects in the Spectrum emulation rather than in the Z80 emulation) and there is no hardware support
for peripherals etc.

If you want an actual Spectrum emulation to play games on, this is not the one to use!

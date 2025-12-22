# Sample Virtual Machines

This folder contains sample VMs written to use the Zem80 emulation library.

## Samples

### ZX Spectrum

A very simple emulation of the ZX Spectrum 48K. The UI in which this sample runs is a .NET MAUI application but can only run on Windows (for now). 

This VM does not have an implementation of sound input, so loading programs from the Spectrum command line will not work. It does have
a very simple beeper implementation, so sound will play, but this is buggy and the audio is choppy due to timing issues, and some effects
relying on precise timing will not work as expected. 

There is a LoadSnapshot method to allow you to load a .SNA or .Z80 snapshot which will allow you to run Spectrum games, but many games
do not run properly or at all (most likely due to defects in the Spectrum emulation rather than in the Z80 emulation) and there is no hardware support
for peripherals etc.

If you want an actual Spectrum emulation to play games on, this is not the one to use! I recommend FUSE.

@echo off
zmac zexdoc.z80
copy zout\zexdoc.cim zexdoc.bin /y
zmac zexdoc-partial.z80
copy zout\zexdoc-partial.cim zexdoc-partial.bin /y
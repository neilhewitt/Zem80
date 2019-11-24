using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_HALT : InstructionTestBase
    {
        [Test, Timeout(500)] // if the CPU goes into an infinite loop for whatever reason, this terminates the test after 500ms
        public void HALT()
        {
            bool halted = false;

            // we must actually run Z80 code from RAM so the CPU is in a running state and can be halted
            byte[] program = new byte[2] { 0x00, 0x76 }; // NOP then HALT
            _cpu.Memory.WriteBytesAt(0, program);
            _cpu.OnHalt += _cpu_OnHalt;
            
            _cpu.Start(true); // will start the CPU synchronously, this returns only when the CPU stops (not halts)

            void _cpu_OnHalt(object sender, EventArgs e) // ever seen a local function as an event handler? 
            {
                halted = true;
                _cpu.Stop(); // we're done, so stop the CPU and dump out to the assert
            }

            Assert.That(halted == true);
        }
    }
}
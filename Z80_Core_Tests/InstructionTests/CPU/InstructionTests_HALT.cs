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
            const byte NOP = 0x00;
            const byte HALT = 0x76;

            bool halted = false;

            // we must actually run Z80 code from RAM so the CPU is in a running state and can be halted
            byte[] program = new byte[2] 
            { 
                NOP,
                HALT 
            }; 

            CPU.Memory.WriteBytesAt(0, program, true);
            CPU.Debug.OnHalt += cpu_OnHalt;
            
            CPU.Start(synchronous: true, enableDebug: true); // will start the CPU synchronously, this returns only when the CPU stops (not halts)

            void cpu_OnHalt(object sender, HaltReason reason) // ever seen a local function as an event handler? 
            {
                halted = true;
                CPU.Stop(); // we're done, so stop the CPU and dump out to the assert
            }

            Assert.That(halted, Is.True);
        }
    }
}
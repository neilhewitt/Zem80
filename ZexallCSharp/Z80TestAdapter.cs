using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace ZexallCSharp
{
    public class Z80TestAdapter
    {
        private IDebugProcessor _cpu;
        private InstructionDecoder _decoder;

        public TestState ExecuteTest(TestVector test)
        {
            // load registers with test state
            // execute instruction
            // calculate new crc
            // store crc
            // return state

            SetState(test);
            ExecutionPackage package = _decoder.Decode(test.Instruction.AsByteArray());
            if (package != null)
            {
                ExecutionResult result = _cpu.Execute(package);
                return GetState();
            }
            else
            {
                return null;
            }
        }

        private void SetState(TestVector test)
        {
            _cpu.Registers.IY = test.IY;
            _cpu.Registers.IX = test.IX;
            _cpu.Registers.HL = test.HL;
            _cpu.Registers.DE = test.DE;
            _cpu.Registers.BC = test.BC;
            _cpu.Registers.SP = test.SP;

            _cpu.Registers[WordRegister.AF] = (ushort)(((test.A) * 256) + test.F);
        }

        private TestState GetState()
        {
            TestState state = new TestState(
                _cpu.Registers.IY,
                _cpu.Registers.IX,
                _cpu.Registers.HL,
                _cpu.Registers.DE,
                _cpu.Registers.BC,
                _cpu.Registers.F,
                _cpu.Registers.A,
                _cpu.Registers.SP
                );

            return state;
        }

        public Z80TestAdapter()
        {
            _cpu = Z80.Core.Bootstrapper.BuildCPU().Debuggable;
            _decoder = new InstructionDecoder();
        }
    }
}

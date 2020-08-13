using System;

namespace ZexNext.Core
{
    public class TestCycle
    {
        private Test _parent;

        public string Mnemonic => BeforeState.Mnemonic;
        public byte[] Opcode => BeforeState.Opcode;
        public string OpcodeString => BeforeState.OpcodeString;

        public TestState BeforeState { get; private set; }
        public TestState AfterState { get; private set; }

        public TestCycle(Test parent, TestState before, TestState after)
        {
            _parent = parent;
            BeforeState = before;
            AfterState = after;
        }
    }
}

using System;

namespace ZexNext.Core
{
    public class TestCycle
    {
        public Test ParentTest { get; private set; }
        public string Mnemonic => BeforeState.Mnemonic;
        public byte[] Opcode => BeforeState.Opcode;

        public TestState BeforeState { get; private set; }
        public TestState AfterState { get; private set; }

        public TestCycle(Test parent, TestState before, TestState after)
        {
            ParentTest = parent;
            BeforeState = before;
            AfterState = after;
        }
    }
}

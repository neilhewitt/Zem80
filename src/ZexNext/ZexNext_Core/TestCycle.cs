using System;

namespace ZexNext.Core
{
    public class TestCycle
    {
        public Test ParentTest { get; private set; }
        public string Mnemonic => BeforeState.Mnemonic;
        public byte[] Opcode => BeforeState.Opcode;
        public string TestInputString { get; private set; }

        public TestState BeforeState { get; private set; }
        public TestState AfterState { get; private set; }

        public TestCycle(Test parent, TestState before, TestState after, string testInputString)
        {
            ParentTest = parent;
            BeforeState = before;
            AfterState = after;
            TestInputString = testInputString;
        }
    }
}

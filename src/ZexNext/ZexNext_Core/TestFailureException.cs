using System;

namespace ZexNext.Core
{
    public class TestFailureException : Exception
    {
        public TestResult TestResult { get; private set; }

        public TestFailureException(string message, TestResult testResult) : base(message)
        {
            TestResult = testResult;
        }
    }
}
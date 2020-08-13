namespace ZexNext.Core
{
    public class TestResult
    {
        public string TestName { get; private set; }
        public string Mnemonic { get; private set; }
        public bool Passed { get; private set; }
        public TestState InitialState { get; private set; }
        public TestState ExpectedState { get; private set; }
        public TestState ActualState { get; private set; }

        public override string ToString()
        {
            return (Passed ? "PASSED " : "FAILED") + "\nInitial state: " + InitialState.ToString() + "\nExpected state: " + ExpectedState.ToString() + "\nActual state: " + ActualState.ToString();
        }

        public TestResult(string testName, string mnemonic, bool passed, TestState initial, TestState expected, TestState actual)
        {
            TestName = testName;
            Mnemonic = mnemonic;
            Passed = passed;
            InitialState = initial;
            ExpectedState = expected;
            ActualState = actual;
        }
    }
}

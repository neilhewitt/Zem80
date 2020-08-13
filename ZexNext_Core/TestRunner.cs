using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZexNext.Core
{
    public class TestRunner
    {
        public TestSet TestSet { get; private set; }

        public IEnumerable<TestResult> RunAll(Func<TestState, TestState> testExecutor, Func<TestResult, bool> afterCycle = null)
        {
            List<TestResult> results = new List<TestResult>();
            
            foreach(Test test in TestSet.Tests)
            {
                foreach(TestCycle cycle in test.Cycles)
                {
                    results.Add(ExecuteTest(test, cycle, testExecutor, afterCycle));
                }
            }

            return results;
        }

        public IEnumerable<TestResult> Run(string testName, Func<TestState, TestState> testExecutor, Func<TestResult, bool> afterCycle = null)
        {
            Test test = TestSet.Tests.SingleOrDefault(x => x.Name.ToLower() == testName.ToLower());
            if (test != null)
            {
                return Run(test, testExecutor, afterCycle);
            }
            else
            {
                throw new ArgumentException("Specified test name does not exist in this test set.");
            }
        }

        public IEnumerable<TestResult> Run(Test test, Func<TestState, TestState> testExecutor, Func<TestResult, bool> afterCycle = null)
        {
            List<TestResult> results = new List<TestResult>();

            foreach (TestCycle cycle in test.Cycles)
            {
                results.Add(ExecuteTest(test, cycle, testExecutor, afterCycle));
            }

            return results;
        }

        private TestResult ExecuteTest(Test test, TestCycle cycle, Func<TestState, TestState> testExecutor, Func<TestResult, bool> afterCycle = null)
        {
            TestState input = new TestState(cycle.BeforeState);
            TestState expected = new TestState(cycle.AfterState);
            input.MaskFlags(test.Mask);
            expected.MaskFlags(test.Mask);

            TestState actual = testExecutor(input);
            actual.MaskFlags(test.Mask);
            bool passed = actual.Equals(expected);

            TestResult result = new TestResult(test.Name, cycle.Mnemonic, passed, input, expected, actual);
            if (afterCycle?.Invoke(result) == false)
            {
                // nothing here ATM - debug point if we need it
            }

            return result;
        }

        public TestRunner(string testSetPath)
        {
            TestSet = new TestSet(testSetPath);
        }

        public TestRunner(TestSet tests)
        {
            TestSet = tests;
        }
    }
}
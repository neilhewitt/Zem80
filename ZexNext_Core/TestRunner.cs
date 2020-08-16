using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZexNext.Core
{
    public class TestRunner
    {
        public IEnumerable<TestSet> TestSets { get; private set; }

        public IEnumerable<TestResult> RunAll(Func<TestState, TestState> testExecutor, bool useFlagMasks, Func<TestResult, bool> afterCycle = null)
        {
            List<TestResult> results = new List<TestResult>();
            
            foreach(Test test in TestSets.SelectMany(x => x.Tests))
            {
                foreach(TestCycle cycle in test.Cycles)
                {
                    results.Add(ExecuteTest(test, cycle, testExecutor, useFlagMasks, afterCycle));
                }
            }

            return results;
        }

        public IEnumerable<TestResult> Run(string testName, Func<TestState, TestState> testExecutor, bool useFlagMasks, Func<TestResult, bool> afterCycle = null)
        {
            Test test = TestSets.SelectMany(x => x.Tests).SingleOrDefault(x => x.Name.ToLower() == testName.ToLower());
            if (test != null)
            {
                return Run(test, testExecutor, useFlagMasks, afterCycle);
            }
            else
            {
                throw new ArgumentException("Specified test name does not exist in this test set.");
            }
        }

        public IEnumerable<TestResult> Run(Test test, Func<TestState, TestState> testExecutor, bool useFlagMasks, Func<TestResult, bool> afterCycle = null)
        {
            List<TestResult> results = new List<TestResult>();

            foreach (TestCycle cycle in test.Cycles)
            {
                results.Add(ExecuteTest(test, cycle, testExecutor, useFlagMasks, afterCycle));
            }

            return results;
        }

        private TestResult ExecuteTest(Test test, TestCycle cycle, Func<TestState, TestState> testExecutor, bool useFlagMasks, Func<TestResult, bool> afterCycle = null)
        {
            TestState input = new TestState(cycle.BeforeState);
            TestState expected = new TestState(cycle.AfterState);
            if (useFlagMasks)
            {
                input.MaskFlags(test.Mask);
                expected.MaskFlags(test.Mask);
            }

            TestState actual = testExecutor(input);
            if (useFlagMasks)
            {
                actual.MaskFlags(test.Mask);
            }
            bool passed = actual.Equals(expected);

            TestResult result = new TestResult(test.Name, cycle.Mnemonic, passed, input, expected, actual);
            if (afterCycle?.Invoke(result) == false)
            {
                // nothing here ATM - debug point if we need it
            }

            return result;
        }

        public TestRunner(Action<ushort, byte[]> memoryPatcher, params string[] testSetPaths)
        {
            List<TestSet> testSets = new List<TestSet>();
            foreach(string testSetPath in testSetPaths)
            {
                TestSet testSet = new TestSet(Path.GetFileName(testSetPath), testSetPath);
                if (testSet.ContainsMemoryPatch)
                {
                    testSet.PatchMemory(memoryPatcher);
                }
                testSets.Add(testSet);
            }

            TestSets = testSets;
        }
    }
}
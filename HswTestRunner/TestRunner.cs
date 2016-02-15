using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HswTestRunner
{
    public class TestRunner
    {
        public static void Main(string[] args)
        {
            var testFinder = new TestFinder(args);
            var testExecutor = new TestExecutor();
            var testReporter = new TestReporter();

            var allTests = testFinder.FindTests();
            foreach (var test in allTests)
            {
                TestResult result = testExecutor.ExecuteSafely(test);
                testReporter.Report(result);
            }

            Console.ReadLine();
        }
    }

    public class TestReporter
    {
        public void Report(TestResult result)
        {
            Console.Write(result.Exception == null ? "." : "x");
        }
    }

    public class TestExecutor
    {
        public TestResult ExecuteSafely(MethodInfo test)
        {
            try
            {
                var instance = Activator.CreateInstance(test.DeclaringType);
                test.Invoke(instance, null);
                return TestResult.Pass(test);
            }
            catch (Exception ex)
            {
                return TestResult.Fail(test, ex);
            }
        }
    }

    public class TestResult
    {
        public MethodInfo MethodInfo { get; private set; }
        public Exception Exception { get; private set; }
        
        public static TestResult Pass(MethodInfo test)
        {
            return new TestResult
            {
                MethodInfo = test
            };
        }

        public static TestResult Fail(MethodInfo test, Exception ex)
        {
            return new TestResult
            {
                MethodInfo = test,
                Exception = ex
            };
        }
    }

    public class TestFinder
    {
        private readonly Assembly _testDll;

        public TestFinder(string[] args)
        {
            var assemblyName = AssemblyName.GetAssemblyName(args[0]);
            _testDll = AppDomain.CurrentDomain.Load(assemblyName);
        }

        public List<MethodInfo> FindTests()
        {
            var fixtures = _testDll.GetTypes()
                .Where(x => x.GetCustomAttributes()
                    .Any(c => c.GetType()
                        .Name.StartsWith("TestFixture"))).ToList();

            var allMethods = fixtures.SelectMany(f =>
                f.GetMethods(BindingFlags.Public | BindingFlags.Instance));

            return allMethods.Where(x => x.GetCustomAttributes()
                .Any(m => m.GetType().Name.StartsWith("Test")))
                .ToList();
        }
    }
}

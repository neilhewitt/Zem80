using System;
using System.Linq;
using System.Threading;

namespace ZexallCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Z80TestAdapter adapter = new Z80TestAdapter();
            ZexTest test = new ZexTest(new ZexdocTests(), adapter.ExecuteTest);
            
            foreach (TestDescriptor d in test.Tests)
            {
                test.RunTest(d);
                //Console.Write("Preparing next test");
                //for (int i = 0; i < 5; i++)
                //{
                //    Console.Write(".");
                //    Thread.Sleep(1000);
                //}
                ////Console.WriteLine("Press any key for next test...");
                ////Console.ReadKey();
                //Console.Clear();
            }

            //var inc = d.Base & d.Increment;
            //Console.WriteLine(d.Base.ToString(true, false));
            //Console.WriteLine(d.Increment.ToString(true, false));
            //Console.WriteLine(inc.ToString(true, false));


            //Console.WriteLine("Starting test...");
            //var results = test.RunAll();
            //Console.WriteLine("\nTEST RESULTS:\n-------------\n");
            //int passed = 0, failed = 0;
            //foreach(var result in results)
            //{
            //    Console.Write(result.Key);
            //    Console.SetCursorPosition(40, Console.CursorTop);
            //    if (result.Value)
            //    {
            //        passed++;
            //        Console.ForegroundColor = ConsoleColor.Green;
            //        Console.Write("Passed\n");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    }
            //    else
            //    {
            //        passed++;
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.Write("Failed\n");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    }
            //}
            //Console.WriteLine($"\n{ passed } tests passed, { failed } tests failed.");
            //Console.WriteLine("\n\nPress any key to exit.");
            //Console.Read();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ListWithinList
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test()
            {
                Name = "Testing",
                Tests = new List<TestList>()
                {
                    new TestList()
                    {
                        Name = "FirstList",
                        TestsWithin = new List<TestList>()
                        {
                            new TestList()
                            {
                                Name = "List Within A List"
                            }
                        }
                    }
                }
            };

            ParameterExpression param = Expression.Parameter(typeof(Test), "test");


            var thisWorks = "test.Tests.Any(Tests=>Tests.TestsWithin.Any(x=>x.Name==\"List Within A List\"))";
            var thisDoesNotWork = "test.Tests.Any(Tests=>Tests.TestsWithin.Any(TestsWithin=>TestsWithin.Name==\"List Within A List\"))";

            var lambdaResult = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(
                new ParameterExpression[]{ param },
                null,
                thisDoesNotWork, test);

            var lambdaDelegate = lambdaResult.Compile();
            var finalResult = lambdaDelegate.DynamicInvoke(test);
            Console.WriteLine(finalResult);
            Console.ReadKey();
        }
        
        public class Test
        {
            public string Name { get; set; }

            public List<TestList> Tests { get; set; }
        }

        public class TestList
        {
            public string Name { get; set; }

            public List<TestList> TestsWithin { get; set; }
        }
    }
}

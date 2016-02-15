using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace HowStuffWorksSamples.Test.Unit
{
    [TestFixture]
    public class OneOhOneTests
    {
        [Test]
        public void TestThatFails()
        {
            Assert.Fail();
        }

        //[Test]
        public void FirstExample()
        {
            Assembly[] assem = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = assem.SelectMany(x => x.GetTypes());

            types.ToList().ForEach(t =>
            {
                //Console.WriteLine(t.Name)
            });
        }
    }

    [TestFixture]
    public class ExampleTest
    {
        [Test]
        public void TestThatPasses()
        {
            Assert.Pass();
        }

        [Test]
        public void TestThatFails()
        {
            Assert.Fail();
        }
    }
}

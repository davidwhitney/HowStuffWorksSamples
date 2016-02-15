using System;
using System.Linq;
using HowStuffWorksSamples.OneOhOneExamples;
using NUnit.Framework;

namespace HowStuffWorksSamples.Test.Unit.OneOhOneExamples
{
    [TestFixture]
    public class BasicReflectionTests
    {
        private readonly BasicReflector _basicReflector = new BasicReflector();

        [Test]
        public void ListAllTypesInOurExampleAssembly()
        {
            var types = _basicReflector.ListAllTypesFromSamples().ToList();

            types.ForEach(t => /*Console.WriteLine(t.Name)*/
            {
                
            });
        }

        [Test]
        public void InspectAllMethodsOnAType()
        {
            var methods = _basicReflector.ListMethodsOn<BasicReflector>().ToList();

            methods.ForEach(t =>
            {
                var listOfParameters = t.GetParameters().ToList();

                var parameters = string.Join(", ", listOfParameters.Select(p => $"{p.ParameterType} {p.Name}"));
                var methodSignature = $"{t.ReturnParameter.ParameterType} {t.Name}({parameters})";

                //Console.WriteLine(methodSignature);
            });
        }
    }
}

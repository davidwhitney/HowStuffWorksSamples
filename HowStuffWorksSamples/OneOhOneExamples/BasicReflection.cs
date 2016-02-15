using System;
using System.Reflection;

namespace HowStuffWorksSamples.OneOhOneExamples
{
    public class BasicReflector
    {
        public Type[] ListAllTypesFromSamples()
        {
            return GetType().Assembly.GetTypes();
        }

        public MethodInfo[] ListMethodsOn<T>()
        {
            return typeof (T).GetMethods();
        }
    }
}

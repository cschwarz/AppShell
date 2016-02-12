using System;

namespace AppShell
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodAttribute : Attribute
    {
        public string MethodName { get; private set; }

        public ServiceMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}

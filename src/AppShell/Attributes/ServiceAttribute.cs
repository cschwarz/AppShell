using System;

namespace AppShell
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ServiceAttribute : Attribute
    {
        public string ServiceName { get; private set; }

        public ServiceAttribute(string serviceName)
        {
            ServiceName = serviceName;
        }
    }
}

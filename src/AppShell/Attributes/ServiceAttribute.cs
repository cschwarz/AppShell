using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

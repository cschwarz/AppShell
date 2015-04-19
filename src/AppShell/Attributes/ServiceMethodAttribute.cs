using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

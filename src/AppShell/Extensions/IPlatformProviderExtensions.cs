using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public static class IPlatformProviderExtensions
    {
        public static IEnumerable<Assembly> GetAssemblies<T>(this IPlatformProvider platformProvider)
        {
            return platformProvider.GetAssemblies().Where(a => a.IsDefined(typeof(T))).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public class ViewModelResolution : IViewModelResolution
    {
        public IDictionary<Type, Type> GetViewModelMapping(IEnumerable<Type> types)
        {
            return types.Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IViewModel))).ToDictionary(t => t, t => t);
        }
    }
}

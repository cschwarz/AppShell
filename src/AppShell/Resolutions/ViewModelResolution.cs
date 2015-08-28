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
            IDictionary<Type, Type> viewModelMapping = new Dictionary<Type, Type>();

            foreach (Type type in types)
            {
                if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IViewModel)))
                    continue;

                ViewModelAttribute viewModelAttribute = type.GetTypeInfo().GetCustomAttribute<ViewModelAttribute>();

                if (viewModelAttribute != null)
                {
                    if (viewModelAttribute.Substitute != null)
                        viewModelMapping[viewModelAttribute.Substitute] = type;
                }
                else
                {
                    if (!viewModelMapping.ContainsKey(type))
                        viewModelMapping.Add(type, type);
                }
            }

            return viewModelMapping;
        }
    }
}

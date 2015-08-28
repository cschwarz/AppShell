using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public class ViewResolution : IViewResolution
    {
        public IDictionary<Type, Type> GetViewMapping(IEnumerable<Type> types)
        {
            IDictionary<Type, Type> views = new Dictionary<Type, Type>();

            foreach (Type type in types)
            {
                ViewAttribute viewAttribute = type.GetTypeInfo().GetCustomAttribute<ViewAttribute>();

                if (viewAttribute == null)
                    continue;

                views[viewAttribute.ViewModelType] = type;

                foreach (Type subViewModelType in types.Where(t => t.GetTypeInfo().IsSubclassOf(viewAttribute.ViewModelType)))
                {
                    if (!views.ContainsKey(subViewModelType))
                        views.Add(subViewModelType, type);
                }
            }

            return views;
        }
    }
}

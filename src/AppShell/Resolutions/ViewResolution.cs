﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public class ViewResolution : IViewResolution
    {
        public IDictionary<Type, Type> GetViewMapping(IEnumerable<Type> types)
        {
            IDictionary<Type, Type> viewMapping = new Dictionary<Type, Type>();

            foreach (Type type in types)
            {
                IEnumerable<ViewAttribute> viewAttributes = type.GetTypeInfo().GetCustomAttributes<ViewAttribute>();

                foreach (ViewAttribute viewAttribute in viewAttributes)
                {
                    viewMapping[viewAttribute.ViewModelType] = type;

                    foreach (Type subViewModelType in types.Where(t => t.GetTypeInfo().IsSubclassOf(viewAttribute.ViewModelType)))
                    {
                        if (!viewMapping.ContainsKey(subViewModelType))
                            viewMapping.Add(subViewModelType, type);
                    }
                }
            }

            return viewMapping;
        }
    }
}
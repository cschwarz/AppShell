using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppShell
{
    public abstract class ViewFactory : IViewFactory
    {
        protected IPlatformProvider platformProvider;

        protected Dictionary<Type, Type> views;

        public ViewFactory(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;

            views = new Dictionary<Type, Type>();                        
        }

        public virtual void Initialize()
        {
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in platformProvider.GetAssemblies<ShellResourceAttribute>())
                types.AddRange(assembly.ExportedTypes);

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
        }
        
        public void Register<TViewModel, TView>()
        {
            views[typeof(TViewModel)] = typeof(TView);
        }

        public abstract object GetView(Type viewModelType);
        public abstract object GetView(IViewModel viewModel);

        public virtual Type GetViewType(Type viewModelType)
        {
            return views[viewModelType];
        }
    }
}

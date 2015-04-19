using System;
using System.Collections.Generic;
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
            foreach (Assembly assembly in platformProvider.GetAssemblies<AppShellResourceAttribute>())
            {
                foreach (Type type in assembly.ExportedTypes)
                {
                    ViewAttribute viewAttribute = type.GetTypeInfo().GetCustomAttribute<ViewAttribute>();

                    if (viewAttribute == null)
                        continue;

                    views.Add(viewAttribute.ViewModelType, type);
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

using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public abstract class ViewFactory : IViewFactory
    {
        protected IPlatformProvider platformProvider;
        protected IViewResolution viewResolution;

        protected IDictionary<Type, Type> viewMapping;

        public ViewFactory(IPlatformProvider platformProvider, IViewResolution viewResolution)
        {
            this.platformProvider = platformProvider;
            this.viewResolution = viewResolution;
        }

        public virtual void Initialize()
        {
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in platformProvider.GetAssemblies<ShellResourceAttribute>())
                types.AddRange(assembly.ExportedTypes);

            viewMapping = viewResolution.GetViewMapping(types);
        }

        public void Register<TViewModel, TView>()
        {
            viewMapping[typeof(TViewModel)] = typeof(TView);
        }

        public abstract object GetView(Type viewModelType);
        public abstract object GetView(IViewModel viewModel);

        public virtual Type GetViewType(Type viewModelType)
        {
            return viewMapping[viewModelType];
        }
    }
}

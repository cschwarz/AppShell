using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public class ViewModelFactory : IViewModelFactory
    {
        protected IPlatformProvider platformProvider;
        protected IViewModelResolution viewModelResolution;

        protected IDictionary<Type, Type> viewModelMapping;

        public ViewModelFactory(IPlatformProvider platformProvider, IViewModelResolution viewModelResolution)
        {
            this.platformProvider = platformProvider;
            this.viewModelResolution = viewModelResolution;               
        }

        public virtual void Initialize()
        {
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in platformProvider.GetAssemblies<ShellResourceAttribute>())
                types.AddRange(assembly.ExportedTypes);

            viewModelMapping = viewModelResolution.GetViewModelMapping(types);
        }

        public IViewModel GetViewModel(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = ShellCore.Container.GetInstance(viewModelMapping[viewModelType]) as IViewModel;

            if (data != null)
            {
                foreach (KeyValuePair<string, object> pair in data)
                {
                    PropertyInfo propertyInfo = viewModelType.GetRuntimeProperty(pair.Key);

                    if (propertyInfo != null)
                        propertyInfo.SetValue(viewModel, pair.Value);
                }
            }

            return viewModel;
        }
    }
}

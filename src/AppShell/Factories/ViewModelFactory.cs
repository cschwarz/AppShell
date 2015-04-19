using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppShell
{
    public class ViewModelFactory : IViewModelFactory
    {
        public IViewModel GetViewModel(Type viewModelType, Dictionary<string, object> data = null)
        {
            IViewModel viewModel = AppShellCore.Container.GetInstance(viewModelType) as IViewModel;

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

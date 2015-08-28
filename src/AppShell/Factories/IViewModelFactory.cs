using System;
using System.Collections.Generic;

namespace AppShell
{
    public interface IViewModelFactory
    {
        void Initialize();

        IViewModel GetViewModel(Type viewModelType, Dictionary<string, object> data = null);
    }
}

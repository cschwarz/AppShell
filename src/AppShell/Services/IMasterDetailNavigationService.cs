using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("masterDetailNavigationService")]
    public interface IMasterDetailNavigationService : IService
    {
        void PushRoot<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel;
        void PushRoot<TViewModel>(dynamic data) where TViewModel : class, IViewModel;
        void PushRoot(Type viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("pushRoot")]
        void PushRoot(string viewModelType, Dictionary<string, object> data = null);
    }
}

using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("navigationService")]
    public interface INavigationService : IService
    {
        void Push<TViewModel>(Dictionary<string, object> data = null, bool replace = false) where TViewModel : class, IViewModel;
        void Push<TViewModel>(dynamic data, bool replace = false) where TViewModel : class, IViewModel;
        void Push(Type viewModelType, Dictionary<string, object> data = null, bool replace = false);
        [ServiceMethod("push")]
        void Push(string viewModelType, Dictionary<string, object> data = null, bool replace = false);
        [ServiceMethod("pop")]
        void Pop();
        [ServiceMethod("getActive")]
        IViewModel GetActive();
    }
}

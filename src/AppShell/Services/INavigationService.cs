using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("navigationService")]
    public interface INavigationService : IService
    {
        void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel;
        void Push(Type viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("push")]
        void Push(string viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("pop")]
        void Pop();
        [ServiceMethod("getActive")]
        IViewModel GetActive();
        [ServiceMethod("detachActive")]
        void DetachActive();
    }
}

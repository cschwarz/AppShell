using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("navigationService")]
    public interface INavigationService
    {
        void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : class, IViewModel;
        [ServiceMethod("push")]
        void Push(string viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("pop")]
        void Pop();
        [ServiceMethod("detachActive")]
        void DetachActive();
    }
}

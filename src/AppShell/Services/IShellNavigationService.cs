using System;
using System.Collections.Generic;

namespace AppShell
{
    [Service("shellNavigationService")]
    public interface IShellNavigationService : IService
    {
        void Push<TViewModel>(Dictionary<string, object> data = null) where TViewModel : ShellViewModel;
        void Push<TViewModel>(dynamic data) where TViewModel : ShellViewModel;
        void Push(Type viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("push")]
        void Push(string viewModelType, Dictionary<string, object> data = null);
        [ServiceMethod("pop")]
        void Pop();
    }
}

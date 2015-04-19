using System;

namespace AppShell
{
    [Service("navigationService")]
    public interface INavigationService
    {
        void Push<TViewModel>() where TViewModel : class, IViewModel;
        [ServiceMethod("push")]
        void Push(string viewModelType);
        [ServiceMethod("pop")]
        void Pop();
    }
}

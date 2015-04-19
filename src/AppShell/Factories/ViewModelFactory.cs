using System;

namespace AppShell
{
    public class ViewModelFactory : IViewModelFactory
    {
        public IViewModel GetViewModel(Type viewModelType)
        {
            return AppShellCore.Container.GetInstance(viewModelType) as IViewModel;
        }
    }
}

using System;

namespace AppShell
{
    public interface IViewFactory
    {
        void Initialize();
        void Register<TViewModel, TView>();

        object GetView(Type viewModelType);
        object GetView(IViewModel viewModel);
        Type GetViewType(Type viewModelType);
    }
}

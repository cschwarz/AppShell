using System;
using System.Windows;

namespace AppShell.Desktop
{
    public class DesktopViewFactory : ViewFactory
    {
        public DesktopViewFactory(IPlatformProvider platformProvider)
            : base(platformProvider)
        { 
        }

        public override object GetView(Type viewModelType)
        {
            FrameworkElement view = AppShellCore.Container.GetInstance(views[viewModelType]) as FrameworkElement;
            view.DataContext = AppShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            FrameworkElement view = AppShellCore.Container.GetInstance(views[viewModel.GetType()]) as FrameworkElement;
            view.DataContext = viewModel;
            return view;
        }
    }
}

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
            FrameworkElement view = ShellCore.Container.GetInstance(views[viewModelType]) as FrameworkElement;
            view.DataContext = ShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            FrameworkElement view = ShellCore.Container.GetInstance(views[viewModel.GetType()]) as FrameworkElement;
            view.DataContext = viewModel;
            return view;
        }
    }
}

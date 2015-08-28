using System;
using System.Windows;

namespace AppShell.Desktop
{
    public class DesktopViewFactory : ViewFactory
    {
        public DesktopViewFactory(IPlatformProvider platformProvider, IViewResolution viewResolution)
            : base(platformProvider, viewResolution)
        { 
        }

        public override object GetView(Type viewModelType)
        {
            FrameworkElement view = ShellCore.Container.GetInstance(viewMapping[viewModelType]) as FrameworkElement;
            view.DataContext = ShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            FrameworkElement view = ShellCore.Container.GetInstance(viewMapping[viewModel.GetType()]) as FrameworkElement;
            view.DataContext = viewModel;
            return view;
        }
    }
}

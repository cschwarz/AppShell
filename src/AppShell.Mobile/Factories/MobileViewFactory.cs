using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class MobileViewFactory : ViewFactory
    {
        public MobileViewFactory(IPlatformProvider platformProvider)
            : base(platformProvider)
        { 
        }

        public override object GetView(Type viewModelType)
        {
            BindableObject view = AppShellCore.Container.GetInstance(views[viewModelType]) as BindableObject;
            view.BindingContext = AppShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            BindableObject view = AppShellCore.Container.GetInstance(views[viewModel.GetType()]) as BindableObject;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

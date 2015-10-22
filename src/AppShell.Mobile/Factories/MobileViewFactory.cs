using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class MobileViewFactory : ViewFactory
    {
        public MobileViewFactory(IPlatformProvider platformProvider, IViewResolution viewResolution)
            : base(platformProvider, viewResolution)
        {
        }

        public override object GetView(Type viewModelType)
        {
            if (!viewMapping.ContainsKey(viewModelType))
                throw new ViewNotFoundException(viewModelType);

            BindableObject view = ShellCore.Container.GetInstance(viewMapping[viewModelType]) as BindableObject;
            view.BindingContext = ShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();

            if (!viewMapping.ContainsKey(viewModelType))
                throw new ViewNotFoundException(viewModelType);

            BindableObject view = ShellCore.Container.GetInstance(viewMapping[viewModelType]) as BindableObject;
            view.BindingContext = viewModel;
            return view;
        }
    }
}
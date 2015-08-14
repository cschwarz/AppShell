﻿using System;
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
            BindableObject view = ShellCore.Container.GetInstance(views[viewModelType]) as BindableObject;
            view.BindingContext = ShellCore.Container.GetInstance(viewModelType);
            return view;
        }

        public override object GetView(IViewModel viewModel)
        {
            BindableObject view = ShellCore.Container.GetInstance(views[viewModel.GetType()]) as BindableObject;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

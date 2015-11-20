﻿using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore shellCore;

        public ShellApplication()
        {
            shellCore = Activator.CreateInstance<T>();
            
            ConfigurePlatform();

            shellCore.Configure();
            shellCore.Initialize();

            MainPage = ShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Page;
        }

        protected virtual void ConfigurePlatform()
        {
            ShellCore.Container.RegisterSingleton<IViewFactory, MobileViewFactory>();
            ShellCore.Container.RegisterSingleton<IDataTemplateFactory, MobileDataTemplateFactory>();            
        }
    }
}

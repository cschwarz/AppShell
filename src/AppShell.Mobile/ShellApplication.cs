using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore appShellCore;

        public ShellApplication()
        {
            appShellCore = Activator.CreateInstance<T>();
        }

        protected virtual void ConfigurePlatform()
        {
            ShellCore.Container.RegisterSingleton<IViewFactory, MobileViewFactory>();
            ShellCore.Container.RegisterSingleton<IDataTemplateFactory, MobileDataTemplateFactory>();            
        }

        protected override void OnStart()
        {
            base.OnStart();

            ConfigurePlatform();

            appShellCore.Configure();
            appShellCore.Initialize();

            MainPage = ShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Page;            
        }
    }
}

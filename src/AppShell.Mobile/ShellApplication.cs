using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellApplication<T> : Application where T : AppShellCore
    {
        protected AppShellCore appShellCore;

        public ShellApplication()
        {
            appShellCore = Activator.CreateInstance<T>();
        }

        protected virtual void ConfigurePlatform()
        {
            AppShellCore.Container.RegisterSingle<IViewFactory, MobileViewFactory>();
            AppShellCore.Container.RegisterSingle<IDataTemplateFactory, MobileDataTemplateFactory>();            
        }

        protected override void OnStart()
        {
            base.OnStart();

            ConfigurePlatform();

            appShellCore.Configure();
            appShellCore.Initialize();

            MainPage = AppShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Page;            
        }
    }
}

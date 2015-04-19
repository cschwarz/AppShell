using System;
using System.Windows;

namespace AppShell.Desktop
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
            AppShellCore.Container.RegisterSingle<IPlatformProvider, DesktopPlatformProvider>();
            AppShellCore.Container.RegisterSingle<IViewFactory, DesktopViewFactory>();
            AppShellCore.Container.RegisterSingle<IDataTemplateFactory, DesktopDataTemplateFactory>();            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigurePlatform();

            appShellCore.Configure();
            appShellCore.Initialize();
            
            MainWindow = AppShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Window;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            appShellCore.Shutdown();
        }
    }
}

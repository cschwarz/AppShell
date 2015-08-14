using System;
using System.Windows;

namespace AppShell.Desktop
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
            ShellCore.Container.RegisterSingle<IPlatformProvider, DesktopPlatformProvider>();
            ShellCore.Container.RegisterSingle<IViewFactory, DesktopViewFactory>();
            ShellCore.Container.RegisterSingle<IDataTemplateFactory, DesktopDataTemplateFactory>();            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigurePlatform();

            appShellCore.Configure();
            appShellCore.Initialize();
            
            MainWindow = ShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Window;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            appShellCore.Shutdown();
        }
    }
}

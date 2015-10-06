using System;
using System.Windows;

namespace AppShell.Desktop
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore shellCore;

        public ShellApplication()
        {
            shellCore = Activator.CreateInstance<T>();
        }

        protected virtual void ConfigurePlatform()
        {
            ShellCore.Container.RegisterSingleton<IPlatformProvider, DesktopPlatformProvider>();
            ShellCore.Container.RegisterSingleton<IViewFactory, DesktopViewFactory>();
            ShellCore.Container.RegisterSingleton<IDataTemplateFactory, DesktopDataTemplateFactory>();            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShellCore.InitializeContainer();

            ConfigurePlatform();

            shellCore.Configure();
            shellCore.Initialize();
            
            MainWindow = ShellCore.Container.GetInstance<IViewFactory>().GetView(typeof(SplashScreenHostViewModel)) as Window;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            
            shellCore.Shutdown();
            ShellCore.ShutdownContainer();
        }
    }
}

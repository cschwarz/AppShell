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
            shellCore.ShellViewModelPushed += ShellCore_ShellViewModelPushed;
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

            MainWindow = new ShellWindow();

            ShellCore.InitializeContainer();

            ConfigurePlatform();

            shellCore.Configure();
            shellCore.Initialize();

            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            
            shellCore.Shutdown();
            ShellCore.ShutdownContainer();
        }

        private void ShellCore_ShellViewModelPushed(object sender, IViewModel e)
        {
            MainWindow.DataContext = e as ShellViewModel;
            MainWindow.Content = ShellCore.Container.GetInstance<IViewFactory>().GetView(e);
        }
    }
}

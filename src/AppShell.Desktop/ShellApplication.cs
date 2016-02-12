using System;
using System.ComponentModel;
using System.Windows;

namespace AppShell.Desktop
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore shellCore;

        public ShellApplication()
        {
            shellCore = Activator.CreateInstance<T>();
            shellCore.PropertyChanged += ShellCore_PropertyChanged;
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
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            shellCore.Shutdown();
            ShellCore.ShutdownContainer();
        }

        private void ShellCore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveShell")
            {
                Window previousWindow = MainWindow;

                object view = ShellCore.Container.GetInstance<IViewFactory>().GetView(shellCore.ActiveShell);

                if (view is Window)
                {
                    MainWindow = view as Window;
                }
                else
                {
                    MainWindow = new ShellWindow();
                    MainWindow.Content = view;
                }

                if (previousWindow != null)
                    previousWindow.Close();

                MainWindow.DataContext = shellCore.ActiveShell;
                MainWindow.Show();
            }
        }
    }
}

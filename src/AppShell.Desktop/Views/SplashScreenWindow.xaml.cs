using System;
using System.Windows;

namespace AppShell.Desktop
{
    /*
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    [View(typeof(SplashScreenHostViewModel))]
    public partial class SplashScreenWindow : Window
    {
        private bool hasBeenInitialActivated;

        private IServiceDispatcher serviceDispatcher;

        public SplashScreenWindow(IServiceDispatcher serviceDispatcher)
        {
            InitializeComponent();

            this.serviceDispatcher = serviceDispatcher;

            DataContextChanged += SplashScreenWindow_DataContextChanged;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DataContextChanged -= SplashScreenWindow_DataContextChanged;
        }

        private void SplashScreenWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is SplashScreenHostViewModel)
            {
                (DataContext as SplashScreenHostViewModel).ExitRequested += ExitRequested;
                (DataContext as SplashScreenHostViewModel).ShellViewRequested += ShellViewRequested;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            
            Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;
            Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
        }
        
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (hasBeenInitialActivated)
                return;

            hasBeenInitialActivated = true;

            if (DataContext is SplashScreenHostViewModel)
                (DataContext as SplashScreenHostViewModel).ShowSplashScreens();
        }

        private void ExitRequested(object sender, EventArgs e)
        {
            Close();
        }

        private void ShellViewRequested(object sender, EventArgs e)
        {
            IShellConfigurationProvider configurationProvider = ShellCore.Container.GetInstance<IShellConfigurationProvider>();
            TypeConfiguration shellViewModelConfiguration = configurationProvider.GetShellViewModel();
            IViewModel shellViewModel = ShellCore.Container.GetInstance<IViewModelFactory>().GetViewModel(shellViewModelConfiguration.Type, shellViewModelConfiguration.Data);

            foreach (TypeConfiguration viewModelConfiguration in configurationProvider.GetViewModels())
                serviceDispatcher.Dispatch<INavigationService>(n => n.Push(viewModelConfiguration.Type, viewModelConfiguration.Data));
            
            Application.Current.MainWindow = new ShellWindow(shellViewModel as ShellViewModel) { Content = ShellCore.Container.GetInstance<IViewFactory>().GetView(shellViewModel) };
            Application.Current.MainWindow.Show();

            Close();
        }
    }*/
}

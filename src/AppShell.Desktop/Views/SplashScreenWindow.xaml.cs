using System;
using System.Windows;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    [View(typeof(SplashScreenHostViewModel))]
    public partial class SplashScreenWindow : Window
    {
        private bool hasBeenInitialActivated;

        public SplashScreenWindow()
        {
            InitializeComponent();
            
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
            Type shellViewModelType = ShellCore.Container.GetInstance<IShellConfigurationProvider>().GetShellViewModel();
            IViewModel shellViewModel = ShellCore.Container.GetInstance<IViewModelFactory>().GetViewModel(shellViewModelType);

            Application.Current.MainWindow = ShellCore.Container.GetInstance<IViewFactory>().GetView(shellViewModel) as Window;
            Application.Current.MainWindow.Show();

            Close();
        }
    }
}

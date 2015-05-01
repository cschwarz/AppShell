using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Application.Current.MainWindow = AppShellCore.Container.GetInstance<IViewFactory>().GetView(AppShellCore.Container.GetInstance<IShellConfigurationProvider>().GetShellViewModel()) as Window;
            Application.Current.MainWindow.Show();

            Close();
        }
    }
}

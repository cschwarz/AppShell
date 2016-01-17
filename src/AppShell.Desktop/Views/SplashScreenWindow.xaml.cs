using System.Windows;

namespace AppShell.Desktop.Views
{
    [View(typeof(SplashScreenShellViewModel))]
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;
            Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
        }
    }
}
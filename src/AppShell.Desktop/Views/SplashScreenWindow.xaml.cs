using System;
using System.Windows;

namespace AppShell.Desktop.Views
{
    [View(typeof(SplashScreenShellViewModel))]
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();

            DataContextChanged += SplashScreenWindow_DataContextChanged;
        }

        private void SplashScreenWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ShellViewModel oldShellViewModel = e.OldValue as ShellViewModel;

                oldShellViewModel.CloseRequested -= ShellViewModel_CloseRequested;

            }
            if (e.NewValue != null)
            {
                ShellViewModel newShellViewModel = e.NewValue as ShellViewModel;

                newShellViewModel.CloseRequested += ShellViewModel_CloseRequested;
            }
        }

        private void ShellViewModel_CloseRequested(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;
            Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
        }
    }
}
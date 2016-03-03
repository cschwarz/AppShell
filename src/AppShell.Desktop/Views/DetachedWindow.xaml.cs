using System;
using System.Windows;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for DetachedWindow.xaml
    /// </summary>
    public partial class DetachedWindow : Window
    {
        private ShellViewModel shellViewModel;

        public DetachedWindow(ShellViewModel shellViewModel)
        {
            InitializeComponent();

            this.shellViewModel = shellViewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            shellViewModel.CloseDetached(DataContext as IViewModel);
        }
    }
}

using System;
using System.Windows;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for DetachedWindow.xaml
    /// </summary>
    public partial class DetachedWindow : Window
    {
        public DetachedWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (DataContext is IViewModel)
                (DataContext as IViewModel).Dispose();
        }
    }
}

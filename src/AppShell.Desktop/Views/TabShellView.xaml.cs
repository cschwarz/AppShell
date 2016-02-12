using System.Windows.Controls;

namespace AppShell.Desktop.Views
{
    /// <summary>
    /// Interaction logic for TabShellView.xaml
    /// </summary>
    [View(typeof(TabShellViewModel))]
    public partial class TabShellView : UserControl
    {
        public TabShellView()
        {
            InitializeComponent();
        }
    }
}

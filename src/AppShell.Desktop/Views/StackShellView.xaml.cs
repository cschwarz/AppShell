using System.Windows.Controls;

namespace AppShell.Desktop.Views
{
    /// <summary>
    /// Interaction logic for StackShellView.xaml
    /// </summary>
    [View(typeof(StackShellViewModel))]
    public partial class StackShellView : UserControl
    {
        public StackShellView()
        {
            InitializeComponent();
        }
    }
}

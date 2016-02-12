using System.Windows.Controls;

namespace AppShell.Desktop
{
    /// <summary>
    /// Interaction logic for InlineStackShellView.xaml
    /// </summary>
    [View(typeof(InlineStackShellViewModel))]
    public partial class InlineStackShellView : UserControl
    {
        public InlineStackShellView()
        {
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;

namespace AppShell.Samples.Todo.Desktop.Views
{
    [View(typeof(TodoViewModel))]
    public partial class TodoView : UserControl
    {
        public TodoView()
        {
            InitializeComponent();
        }
    }
}

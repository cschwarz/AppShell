using AppShell.Mobile;
using AppShell.Mobile.WinRT;

namespace AppShell.Samples.Todo.Mobile.WinRT
{
    public class TodoShellPage : ShellPage<ShellApplication<TodoShellCore>>
    {
    }

    public sealed partial class MainPage : TodoShellPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Init();            
        }
    }
}

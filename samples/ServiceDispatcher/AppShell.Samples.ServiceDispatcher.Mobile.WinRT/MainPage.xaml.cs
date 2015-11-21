using AppShell.Mobile;
using AppShell.Mobile.WinRT;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AppShell.Samples.ServiceDispatcher.Mobile.WinRT
{
    public class ServiceDispatcherShellPage : ShellPage<ShellApplication<ServiceDispatcherShellCore>>
    {
    }

    public sealed partial class MainPage : ServiceDispatcherShellPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Init();
        }
    }
}

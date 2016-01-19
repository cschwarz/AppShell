using AppShell.Mobile;
using AppShell.Mobile.UWP;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppShell.Samples.ServiceDispatcher.Mobile.UWP
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

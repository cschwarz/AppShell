using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class ViewModel1 : ViewModel
    {
        public Command OpenViewModel2Command { get; private set; }
        public Command ShowMessageCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;
        private IPlatformProvider platformProvider;

        public ViewModel1(IServiceDispatcher serviceDispatcher, IPlatformProvider platformProvider)
        {
            Title = "ViewModel1";

            this.serviceDispatcher = serviceDispatcher;
            this.platformProvider = platformProvider;

            IsLoading = true;
            LoadingText = "Loading for 3 seconds...";
            Task.Delay(3000).ContinueWith(t => IsLoading = false);

            OpenViewModel2Command = new Command(OpenViewModel2);
            ShowMessageCommand = new Command(ShowMessage);
        }

        public void OpenViewModel2()
        {
            serviceDispatcher.Dispatch<INavigationService>(ShellNames.Stack, n => n.Push<ViewModel2>());
        }

        public void ShowMessage()
        {
            platformProvider.ShowMessage("Title", "Sample Message", "OK");
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }
    }
}

using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class ViewModel1 : ViewModel
    {
        public Command OpenViewModel2Command { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel1(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel1";
            HasNavigationBar = false;

            this.serviceDispatcher = serviceDispatcher;

            IsLoading = true;
            LoadingText = "Loading for 3 seconds...";
            Task.Delay(3000).ContinueWith(t => IsLoading = false);

            OpenViewModel2Command = new Command(OpenViewModel2);
        }

        public void OpenViewModel2()
        {
            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<ViewModel2>());
        }
    }
}

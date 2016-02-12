using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class SplashViewModel1 : ViewModel
    {
        private IServiceDispatcher serviceDispatcher;
        private IPlatformProvider platformProvider;

        public SplashViewModel1(IServiceDispatcher serviceDispatcher, IPlatformProvider platformProvider)
        {
            HasNavigationBar = false;

            Title = "SplashViewModel1";

            this.serviceDispatcher = serviceDispatcher;
            this.platformProvider = platformProvider;

            Task.Delay(1500).ContinueWith(t => platformProvider.ExecuteOnUIThread(() => serviceDispatcher.Dispatch<INavigationService>(ShellNames.SplashScreen, n => n.Push<SplashViewModel2>(replace: true))));
        }
    }
}

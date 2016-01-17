using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class SplashViewModel2 : ViewModel
    {
        private IServiceDispatcher serviceDispatcher;
        private IPlatformProvider platformProvider;

        public SplashViewModel2(IServiceDispatcher serviceDispatcher, IPlatformProvider platformProvider)
        {
            HasNavigationBar = false;

            Title = "SplashViewModel2";

            this.serviceDispatcher = serviceDispatcher;
            this.platformProvider = platformProvider;

            Task.Delay(1500).ContinueWith(t => platformProvider.ExecuteOnUIThread(() => serviceDispatcher.Dispatch<INavigationService>(n => n.Push<ViewModel1>(replace: true))));
        }
    }
}

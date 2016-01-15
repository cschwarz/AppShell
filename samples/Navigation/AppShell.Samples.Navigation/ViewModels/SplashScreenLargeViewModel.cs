using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class SplashScreenLargeViewModel : SplashScreenViewModel
    {
        public SplashScreenLargeViewModel(IPlatformProvider platformProvider)
        {
            Task.Delay(1500).ContinueWith((t) => platformProvider.ExecuteOnUIThread(() => Close(true)));
        }
    }
}

using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class SplashScreenSmallViewModel : SplashScreenViewModel
    {
        public SplashScreenSmallViewModel(IPlatformProvider platformProvider)
        {
            Task.Delay(1500).ContinueWith((t) => platformProvider.ExecuteOnUIThread(() => Close(true)));
        }
    }
}

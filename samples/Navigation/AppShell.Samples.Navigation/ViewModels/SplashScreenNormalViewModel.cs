using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShell.Samples.Navigation
{
    public class SplashScreenNormalViewModel : SplashScreenViewModel
    {
        public SplashScreenNormalViewModel(IPlatformProvider platformProvider)
        {
            Task.Delay(1500).ContinueWith((t) => platformProvider.ExecuteOnUIThread(() => Close(true)));
        }
    }
}

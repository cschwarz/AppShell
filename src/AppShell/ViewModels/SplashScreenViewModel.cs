using System;

namespace AppShell
{
    public class SplashScreenViewModel : ViewModel
    {
        public event EventHandler<SplashScreenEventArgs> Closed;

        public void Close(bool result)
        {
            if (Closed != null)
                Closed(this, new SplashScreenEventArgs(result));
        }
    }
}

using System;

namespace AppShell
{
    public class SplashScreenEventArgs : EventArgs
    {
        public bool Result { get; private set; }

        public SplashScreenEventArgs(bool result)
        {
            Result = result;
        }
    }
}

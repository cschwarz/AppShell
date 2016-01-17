namespace AppShell.Samples.Navigation
{
    public class NavigationShellCore : ShellCore
    {
        public override void Run()
        {
            Push<SplashScreenShellViewModel>(new { Name = ShellNames.SplashScreen });
            Container.GetInstance<IServiceDispatcher>().Dispatch<INavigationService>(n => n.Push<SplashViewModel1>());
        }
    }
}

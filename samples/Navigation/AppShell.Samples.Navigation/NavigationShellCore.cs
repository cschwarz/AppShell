namespace AppShell.Samples.Navigation
{
    public class NavigationShellCore : ShellCore
    {
        public override void Run()
        {
            Push<StackShellViewModel>(new { Name = ShellNames.Stack });
            Container.GetInstance<IServiceDispatcher>().Dispatch<INavigationService>(n => n.Push<SplashViewModel1>());
        }
    }
}

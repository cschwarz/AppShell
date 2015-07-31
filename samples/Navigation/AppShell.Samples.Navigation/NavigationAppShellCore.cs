using AppShell;

namespace AppShell.Samples.Navigation
{
    public class NavigationAppShellCore : AppShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingle<IShellConfigurationProvider, NavigationShellConfigurationProvider>();
        }
    }
}

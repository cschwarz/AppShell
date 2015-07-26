using AppShell;

namespace AppShell.Templates
{
    public class __shellname__AppShellCore : AppShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingle<IShellConfigurationProvider, __shellname__ShellConfigurationProvider>();
        }
    }
}

using AppShell;

namespace AppShell.Templates
{
    public class TemplateAppShellCore : AppShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingle<IShellConfigurationProvider, TemplateShellConfigurationProvider>();
        }
    }
}

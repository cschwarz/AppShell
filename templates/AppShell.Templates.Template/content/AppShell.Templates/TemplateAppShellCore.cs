using AppShell;

namespace $safeprojectname$
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

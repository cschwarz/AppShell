using AppShell;

namespace $safeprojectname$
{
    public class $ext_shellname$AppShellCore : AppShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingle<IShellConfigurationProvider, $ext_shellname$ShellConfigurationProvider>();
        }
    }
}

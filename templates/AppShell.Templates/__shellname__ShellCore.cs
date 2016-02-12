namespace AppShell.Templates
{
    public class __shellname__ShellCore : ShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingleton<IShellConfigurationProvider, __shellname__ShellConfigurationProvider>();
        }
    }
}

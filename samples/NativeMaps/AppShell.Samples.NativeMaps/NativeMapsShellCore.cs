
namespace AppShell.Samples.NativeMaps
{
    public class NativeMapsShellCore : ShellCore
    {
        public override void Configure()
        {
            base.Configure();

            Container.RegisterSingleton<IShellConfigurationProvider, NativeMapsShellConfigurationProvider>();
        }
    }
}


namespace AppShell.Mobile.iOS
{
    public class iOSShellApplication<T> : ShellApplication<T> where T : ShellCore
    {
        protected override void ConfigurePlatform()
        {
            base.ConfigurePlatform();

            ShellCore.Container.RegisterSingleton<IPlatformProvider, iOSPlatformProvider>();
        }
    }
}

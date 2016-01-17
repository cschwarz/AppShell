namespace AppShell.Samples.ServiceDispatcher
{
    public class ServiceDispatcherShellCore : ShellCore
    {
        public override void Run()
        {
            Push<StackShellViewModel>(new { Name = "Main" });

            pluginProvider.StartPlugin<SamplePlugin>();

            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<WebBrowserViewModel>(new { EmbeddedHtml = "AppShell.Samples.ServiceDispatcher.ServiceDispatcher.html;AppShell.Samples.ServiceDispatcher" }));
        }
    }
}

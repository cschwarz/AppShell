using AppShell;

namespace AppShell.Templates
{
    public class TemplateShellConfigurationProvider : ShellConfigurationProvider
    {
        public TemplateShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();
        }
    }
}

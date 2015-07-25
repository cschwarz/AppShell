using AppShell;

namespace $safeprojectname$
{
    public class TemplateShellConfigurationProvider : ShellConfigurationProvider
    {
        public TemplateShellConfigurationProvider()
        {
            RegisterShellViewModel<StackShellViewModel>();
        }
    }
}

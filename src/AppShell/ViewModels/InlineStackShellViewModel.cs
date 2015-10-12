namespace AppShell
{
    public class InlineStackShellViewModel : ShellViewModel
    {
        public InlineStackShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(configurationProvider, serviceDispatcher, viewModelFactory)
        {
        }
    }
}

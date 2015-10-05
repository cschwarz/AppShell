using System.Linq;

namespace AppShell
{
    public class TabShellViewModel : ShellViewModel, ITabNavigationService
    {
        public TabShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(configurationProvider, serviceDispatcher, viewModelFactory)
        {
            serviceDispatcher.Subscribe<ITabNavigationService>(this);
        }

        public void Select(string name)
        {
            ActiveItem = Items.FirstOrDefault(v => v.Name == name);
        }
    }
}

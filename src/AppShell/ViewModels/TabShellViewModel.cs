using System.Linq;

namespace AppShell
{
    public class TabShellViewModel : ShellViewModel, ITabNavigationService
    {
        public TabShellViewModel(IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(serviceDispatcher, viewModelFactory)
        {
            serviceDispatcher.Subscribe<ITabNavigationService>(this);
        }

        public override void Dispose()
        {
            base.Dispose();

            serviceDispatcher.Unsubscribe<ITabNavigationService>(this);
        }

        public void Select(string name)
        {
            ActiveItem = Items.FirstOrDefault(v => v.Name == name);
        }
    }
}

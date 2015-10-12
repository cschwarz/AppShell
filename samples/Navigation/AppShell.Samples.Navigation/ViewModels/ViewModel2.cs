using System.Collections.Generic;

namespace AppShell.Samples.Navigation
{
    public class ViewModel2 : ViewModel
    {
        public Command OpenTabViewModelCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel2(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel2";

            this.serviceDispatcher = serviceDispatcher;

            OpenTabViewModelCommand = new Command(OpenTabViewModel);
        }

        public void OpenTabViewModel()
        {
            serviceDispatcher.Dispatch<INavigationService>(n => n.Push<TabShellViewModel>(new Dictionary<string, object>() { { "Name", "TabShell" } }));
            serviceDispatcher.Dispatch<INavigationService>("TabShell", n => n.Push<ViewModel3>());
            serviceDispatcher.Dispatch<INavigationService>("TabShell", n => n.Push<ViewModel4>());
        }        
    }
}

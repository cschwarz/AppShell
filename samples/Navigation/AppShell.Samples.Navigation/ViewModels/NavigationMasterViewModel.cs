namespace AppShell.Samples.Navigation
{
    public class NavigationMasterViewModel : MasterViewModel
    {
        public NavigationMasterViewModel(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Items.Add(new ViewModelMenuItem("View 3", new TypeConfiguration(typeof(ViewModel3))));
            Items.Add(new ViewModelMenuItem("View 4", new TypeConfiguration(typeof(ViewModel4))));
            Items.Add(new ViewModelMenuItem("View 5", new TypeConfiguration(typeof(ViewModel5))));
            Items.Add(new CommandMenuItem("Back", new Command(Back)));
        }

        private void Back()
        {
            serviceDispatcher.Dispatch<IShellNavigationService>(n => n.Push(Shells.Stack));
        }
    }
}

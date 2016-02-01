namespace AppShell.Samples.Navigation
{
    public class NavigationMasterViewModel : MasterViewModel
    {
        public NavigationMasterViewModel(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Items.Add(new ViewModelMenuItem("View 3", new TypeConfiguration(typeof(ViewModel3))) { Icon = "Icon16.png" });
            Items.Add(new ViewModelMenuItem("View 4", new TypeConfiguration(typeof(ViewModel4))) { Icon = "Icon24.png" });
            Items.Add(new ViewModelMenuItem("View 5", new TypeConfiguration(typeof(ViewModel5))) { Icon = "Icon32.png" });
            Items.Add(new CommandMenuItem("Back", new Command(Back)) { Icon = "Icon48.png" });
        }

        private void Back()
        {
            serviceDispatcher.Dispatch<IShellNavigationService>(n => n.Pop());
        }
    }
}

namespace AppShell.Samples.Navigation
{
    public class NavigationMasterViewModel : MasterViewModel
    {
        public NavigationMasterViewModel(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Items.Add(new MenuItem("View 3", new TypeConfiguration(typeof(ViewModel3))));
            Items.Add(new MenuItem("View 4", new TypeConfiguration(typeof(ViewModel4))));
            Items.Add(new MenuItem("View 5", new TypeConfiguration(typeof(ViewModel5))));
        }
    }
}

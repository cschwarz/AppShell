namespace AppShell.Samples.Navigation
{
    public class DetailViewModel : ViewModel
    {
        public Command CloseCommand { get; private set; }
        
        private IServiceDispatcher serviceDispatcher;

        public DetailViewModel(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "DetailViewModel";

            CloseCommand = new Command(Close);

            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Close", Command = CloseCommand });
        }

        private void Close()
        {
            serviceDispatcher.Dispatch<INavigationService>(Shells.MasterDetail, n => n.Pop());
        }
    }
}

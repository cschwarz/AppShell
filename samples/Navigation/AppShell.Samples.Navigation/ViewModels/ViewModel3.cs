namespace AppShell.Samples.Navigation
{
    public class ViewModel3 : ViewModel
    {
        public Command OpenDetailCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel3(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "ViewModel3";

            OpenDetailCommand = new Command(OpenDetail);

            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Detail 1", Order = ToolbarItemOrder.Primary, Command = OpenDetailCommand });
            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Detail 2", Order = ToolbarItemOrder.Primary, Command = OpenDetailCommand });
            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Detail 3", Order = ToolbarItemOrder.Secondary, Command = OpenDetailCommand });
            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Detail 4", Order = ToolbarItemOrder.Secondary, Command = OpenDetailCommand });            
        }

        private void OpenDetail()
        {
            serviceDispatcher.Dispatch<INavigationService>(Shells.MasterDetail, n => n.Push<DetailViewModel>());
        }
    }
}

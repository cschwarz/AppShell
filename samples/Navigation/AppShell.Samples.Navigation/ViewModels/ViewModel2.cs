namespace AppShell.Samples.Navigation
{
    public class ViewModel2 : ViewModel
    {
        public Command OpenMasterDetailViewModelCommand { get; private set; }
        public Command BackCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel2(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel2";

            this.serviceDispatcher = serviceDispatcher;

            OpenMasterDetailViewModelCommand = new Command(OpenMasterDetailViewModel);
            BackCommand = new Command(Back);

            ToolbarItems.Add(new ToolbarItemViewModel() { Title = "Open", Command = OpenMasterDetailViewModelCommand });
        }

        public void OpenMasterDetailViewModel()
        {
            serviceDispatcher.Dispatch<IShellNavigationService>(n => n.Push<MasterDetailShellViewModel>(new { Name = ShellNames.MasterDetail, Master = new NavigationMasterViewModel(serviceDispatcher) }));
            serviceDispatcher.Dispatch<INavigationService>(ShellNames.MasterDetail, n => n.Push<ViewModel3>());
        }

        public void Back()
        {
            serviceDispatcher.Dispatch<INavigationService>(ShellNames.Stack, n => n.Pop());
        }
    }
}

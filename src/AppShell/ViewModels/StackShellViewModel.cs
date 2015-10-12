
namespace AppShell
{
    public class StackShellViewModel : ShellViewModel
    {
        private bool hasNavigationBar;
        public bool HasNavigationBar
        {
            get { return hasNavigationBar; }
            set
            {
                if (hasNavigationBar != value)
                {
                    hasNavigationBar = value;
                    OnPropertyChanged("HasNavigationBar");
                }
            }
        }

        public Command BackCommand { get; private set; }

        public StackShellViewModel(IShellConfigurationProvider configurationProvider, IServiceDispatcher serviceDispatcher, IViewModelFactory viewModelFactory)
            : base(configurationProvider, serviceDispatcher, viewModelFactory)
        {
            HasNavigationBar = true;
            BackCommand = new Command(Back, CanBack);
        }

        public void Back()
        {
            Pop();
        }

        public bool CanBack()
        {
            return ActiveItem != null && ActiveItem.AllowClose;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (BackCommand != null && propertyName == "ActiveItem")
                BackCommand.ChangeCanExecute();
        }
    }
}

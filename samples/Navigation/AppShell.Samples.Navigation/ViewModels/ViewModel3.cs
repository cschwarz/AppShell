namespace AppShell.Samples.Navigation
{
    public class ViewModel3 : ViewModel
    {
        public Command CloseCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel3(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "ViewModel3";

            CloseCommand = new Command(Close);
        }

        public void Close()
        {
            serviceDispatcher.Dispatch<INavigationService>("StackShell", n => n.Pop());
        }
    }
}

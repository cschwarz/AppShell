namespace AppShell.Samples.Navigation
{
    public class ViewModel5 : ViewModel
    {
        public Command CloseCommand { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel5(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "ViewModel5";

            CloseCommand = new Command(Close);
        }

        public void Close()
        {
            serviceDispatcher.Dispatch<INavigationService>("InlineStackShell", n => n.Pop());
        }
    }
}

namespace AppShell.Samples.Navigation
{
    public class ViewModel4 : ViewModel
    {
        public Command OpenViewModel5Command { get; private set; }

        private IServiceDispatcher serviceDispatcher;

        public ViewModel4(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel4";
            HasNavigationBar = false;

            this.serviceDispatcher = serviceDispatcher;

            OpenViewModel5Command = new Command(OpenViewModel5);
        }

        public void OpenViewModel5()
        {
            serviceDispatcher.Dispatch<INavigationService>("InlineStackShell", n => n.Push<ViewModel5>());
        }
    }
}

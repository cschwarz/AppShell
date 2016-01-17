namespace AppShell.Samples.Navigation
{
    public class ViewModel4 : ViewModel
    {
        private IServiceDispatcher serviceDispatcher;

        public ViewModel4(IServiceDispatcher serviceDispatcher)
        {
            Title = "ViewModel4";

            this.serviceDispatcher = serviceDispatcher;
        }
    }
}

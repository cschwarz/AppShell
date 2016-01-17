namespace AppShell.Samples.Navigation
{
    public class ViewModel5 : ViewModel
    {
        private IServiceDispatcher serviceDispatcher;

        public ViewModel5(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;

            Title = "ViewModel5";
        }
    }
}

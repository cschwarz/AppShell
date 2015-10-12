namespace AppShell.Samples.Navigation.Desktop
{
    [ViewModel(Substitute = typeof(ViewModel2))]
    public class DesktopViewModel2 : ViewModel2
    {
        public DesktopViewModel2(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Title = "DesktopViewModel2";
        }
    }
}
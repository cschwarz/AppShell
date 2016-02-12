namespace AppShell.Samples.Navigation.Mobile
{
    [ViewModel(Substitute = typeof(ViewModel2))]
    public class MobileViewModel2 : ViewModel2
    {
        public MobileViewModel2(IServiceDispatcher serviceDispatcher)
            : base(serviceDispatcher)
        {
            Title = "MobileViewModel2";
        }
    }
}

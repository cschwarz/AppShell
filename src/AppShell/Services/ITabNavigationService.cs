namespace AppShell
{
    [Service("tabNavigationService")]
    public interface ITabNavigationService
    {
        [ServiceMethod("select")]
        void Select(string name);
    }
}

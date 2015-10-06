namespace AppShell
{
    [Service("tabNavigationService")]
    public interface ITabNavigationService : IService
    {
        [ServiceMethod("select")]
        void Select(string name);
    }
}

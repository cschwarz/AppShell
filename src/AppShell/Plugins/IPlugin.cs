
namespace AppShell
{
    public interface IPlugin
    {
        string Name { get; set; }

        void Start();
        void Stop();
    }
}

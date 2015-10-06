
namespace AppShell
{
    public class ServicePlugin<T> : IPlugin where T : class, IService
    {
        public string Name { get; set; }

        protected IServiceDispatcher serviceDispatcher;

        public ServicePlugin(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;
        }

        public virtual void Start()
        {
            serviceDispatcher.Subscribe<T>(this as T);
        }

        public virtual void Stop()
        {
            serviceDispatcher.Unsubscribe<T>(this as T);
        }
    }
}

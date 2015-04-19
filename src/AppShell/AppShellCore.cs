using SimpleInjector;

namespace AppShell
{
    public class AppShellCore
    {
        public static Container Container { get; private set; }
                
        static AppShellCore()
        {
            Container = new Container();
        }

        public virtual void Configure()
        {
            Container.RegisterSingle<IServiceDispatcher, ServiceDispatcher>(); 
            Container.RegisterSingle<IViewModelFactory, ViewModelFactory>();
        }

        public virtual void Initialize()
        {
            Container.GetInstance<IViewFactory>().Initialize();
            Container.GetInstance<IServiceDispatcher>().Initialize();
        }
        
        public virtual void Shutdown()
        {
        }
    }
}

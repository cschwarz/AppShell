using System;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore shellCore;

        public ShellApplication()
        {
            shellCore = Activator.CreateInstance<T>();
            shellCore.ShellViewModelPushed += ShellCore_ShellViewModelPushed;

            ConfigurePlatform();

            shellCore.Configure();
            shellCore.Initialize();
        }

        protected virtual void ConfigurePlatform()
        {
            ShellCore.Container.RegisterSingleton<IViewFactory, MobileViewFactory>();
            ShellCore.Container.RegisterSingleton<IDataTemplateFactory, MobileDataTemplateFactory>();
        }

        private void ShellCore_ShellViewModelPushed(object sender, IViewModel e)
        {
            Page page = ShellCore.Container.GetInstance<IViewFactory>().GetView(e) as Page;
            IPageReady pageReady = page as IPageReady;

            if (pageReady == null || pageReady.IsReady)
                MainPage = page;
            else
                pageReady.Ready += PageReady;
        }

        private void PageReady(object sender, Page page)
        {
            if (page is IPageReady)
                (page as IPageReady).Ready -= PageReady;

            MainPage = page;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellApplication<T> : Application where T : ShellCore
    {
        protected ShellCore shellCore;
        protected IDictionary<string, Page> shellViews;

        public ShellApplication()
        {
            shellViews = new Dictionary<string, Page>();

            shellCore = Activator.CreateInstance<T>();
            shellCore.Shells.CollectionChanged += Shells_CollectionChanged;
            shellCore.PropertyChanged += ShellCore_PropertyChanged;

            ConfigurePlatform();

            shellCore.Configure();
            shellCore.Initialize();
        }
                
        protected virtual void ConfigurePlatform()
        {
            ShellCore.Container.RegisterSingleton<IViewFactory, MobileViewFactory>();
            ShellCore.Container.RegisterSingleton<IDataTemplateFactory, MobileDataTemplateFactory>();
        }

        private void Shells_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (IViewModel viewModel in shellCore.Shells)
                    viewModel.Dispose();

                shellViews.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IViewModel viewModel in e.NewItems)
                {
                    Page page = ShellCore.Container.GetInstance<IViewFactory>().GetView(viewModel) as Page;
                    shellViews.Add(viewModel.Name, page);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IViewModel viewModel in e.OldItems)
                {
                    viewModel.Dispose();
                    shellViews.Remove(viewModel.Name);
                }
            }
        }

        private void ShellCore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveShell")
            {
                Page page = shellViews[shellCore.ActiveShell.Name];

                IPageReady pageReady = page as IPageReady;

                if (MainPage == null || pageReady == null || pageReady.IsReady)
                    MainPage = page;
                else
                    pageReady.Ready += PageReady;
            }
        }

        private void PageReady(object sender, Page page)
        {
            if (page is IPageReady)
                (page as IPageReady).Ready -= PageReady;

            MainPage = page;
        }
    }
}

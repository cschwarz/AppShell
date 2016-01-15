
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellViewPage : ContentPage
    {
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create<ShellViewPage, bool>(x => x.HasNavigationBar, true, propertyChanged: OnHasNavigationBarChanged);
        public static readonly BindableProperty ShellToolbarItemsProperty = BindableProperty.Create<ShellViewPage, IEnumerable<ToolbarItemViewModel>>(x => x.ShellToolbarItems, null, propertyChanged: OnShellToolbarItemsChanged);

        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }
        public IEnumerable<ToolbarItemViewModel> ShellToolbarItems { get { return (IEnumerable<ToolbarItemViewModel>)GetValue(ShellToolbarItemsProperty); } set { SetValue(ShellToolbarItemsProperty, value); } }

        private static void OnHasNavigationBarChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            ShellViewPage shellViewPage = (ShellViewPage)bindable;
            NavigationPage.SetHasNavigationBar(shellViewPage, newValue);
        }

        private static void OnShellToolbarItemsChanged(BindableObject bindable, IEnumerable<ToolbarItemViewModel> oldValue, IEnumerable<ToolbarItemViewModel> newValue)
        {
            ShellViewPage shellViewPage = (ShellViewPage)bindable;

            if (oldValue is ObservableCollection<ToolbarItemViewModel>)
                (oldValue as ObservableCollection<ToolbarItemViewModel>).CollectionChanged -= ShellToolbarItems_CollectionChanged;
            if (newValue != null)
            {
                foreach (ToolbarItemViewModel item in newValue)
                    shellViewPage.ToolbarItems.Add(new ToolbarItem() { Text = item.Title, Icon = item.Icon, Order = (Xamarin.Forms.ToolbarItemOrder)item.Order, Priority = item.Priority, Command = item.Command });

                if (newValue is ObservableCollection<ToolbarItemViewModel>)
                    (newValue as ObservableCollection<ToolbarItemViewModel>).CollectionChanged += ShellToolbarItems_CollectionChanged;
            }
        }

        private static void ShellToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        public ShellViewPage()
        {
            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));
            SetBinding(ShellToolbarItemsProperty, new Binding("ToolbarItems"));
        }

        public static Page Create(object content)
        {
            Page page = null;

            if (content is Page)
            {
                page = content as Page;
            }
            else
            {
                View view = content as View;
                page = new ShellViewPage() { Content = view };
                page.BindingContext = view.BindingContext;                
            }

            if (page != null)
            {
                page.SetBinding(TitleProperty, new Binding("Title"));
                page.SetBinding(IconProperty, new Binding("Icon"));
            }

            return page;
        }
    }
}

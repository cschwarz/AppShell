using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellViewPage : ContentPage
    {
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create("HasNavigationBar", typeof(bool), typeof(ShellViewPage), true, propertyChanged: OnHasNavigationBarChanged);
        public static readonly BindableProperty ShellToolbarItemsProperty = BindableProperty.Create("ShellToolbarItems", typeof(IEnumerable<ToolbarItemViewModel>), typeof(ShellViewPage), null, propertyChanged: OnShellToolbarItemsChanged);

        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }
        public IEnumerable<ToolbarItemViewModel> ShellToolbarItems { get { return (IEnumerable<ToolbarItemViewModel>)GetValue(ShellToolbarItemsProperty); } set { SetValue(ShellToolbarItemsProperty, value); } }

        private static void OnHasNavigationBarChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ShellViewPage shellViewPage = (ShellViewPage)bindable;
            bool newHasNavigationBar = (bool)newValue;

            NavigationPage.SetHasNavigationBar(shellViewPage, newHasNavigationBar);
        }

        private static void OnShellToolbarItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ShellViewPage shellViewPage = (ShellViewPage)bindable;
            IEnumerable<IViewModel> oldToolbarItems = oldValue as IEnumerable<IViewModel>;
            IEnumerable<IViewModel> newToolbarItems = newValue as IEnumerable<IViewModel>;

            if (oldToolbarItems is ObservableCollection<ToolbarItemViewModel>)
                (oldToolbarItems as ObservableCollection<ToolbarItemViewModel>).CollectionChanged -= shellViewPage.ShellToolbarItems_CollectionChanged;
            if (newToolbarItems != null)
            {
                foreach (ToolbarItemViewModel item in newToolbarItems)
                    shellViewPage.AddToolbarItem(item);

                if (newToolbarItems is ObservableCollection<ToolbarItemViewModel>)
                    (newToolbarItems as ObservableCollection<ToolbarItemViewModel>).CollectionChanged += shellViewPage.ShellToolbarItems_CollectionChanged;
            }
        }

        private void AddToolbarItem(ToolbarItemViewModel item)
        {
            ToolbarItem nativeItem = new ToolbarItem() { Text = item.Title, Icon = item.Icon, Order = (Xamarin.Forms.ToolbarItemOrder)item.Order, Priority = item.Priority, Command = item.Command };

            item.PropertyChanged += ToolbarItemViewModel_PropertyChanged;

            ToolbarItems.Add(nativeItem);
            toolbarItemMapping.Add(item, nativeItem);
        }

        private void RemoveToolbarItem(ToolbarItemViewModel item)
        {
            item.PropertyChanged -= ToolbarItemViewModel_PropertyChanged;

            ToolbarItems.Remove(toolbarItemMapping[item]);
            toolbarItemMapping.Remove(item);
        }

        private void ToolbarItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ToolbarItemViewModel item = sender as ToolbarItemViewModel;

            switch (e.PropertyName)
            {
                case "Title": toolbarItemMapping[item].Text = item.Title; break;
                case "Icon": toolbarItemMapping[item].Icon = item.Icon; break;
                case "Order": toolbarItemMapping[item].Order = (Xamarin.Forms.ToolbarItemOrder)item.Order; break;
                case "Priority": toolbarItemMapping[item].Priority = item.Priority; break;
                case "Command": toolbarItemMapping[item].Command = item.Command; break;
            }
        }

        private void ShellToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (ToolbarItemViewModel item in toolbarItemMapping.Select(i => i.Key).ToList())
                    RemoveToolbarItem(item);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ToolbarItemViewModel item in e.NewItems)
                    AddToolbarItem(item);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ToolbarItemViewModel item in e.OldItems)
                    RemoveToolbarItem(item);
            }
        }

        private Dictionary<ToolbarItemViewModel, ToolbarItem> toolbarItemMapping;

        public ShellViewPage()
        {
            toolbarItemMapping = new Dictionary<ToolbarItemViewModel, ToolbarItem>();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellViewPage : ContentPage
    {
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create<ShellViewPage, bool>(x => x.HasNavigationBar, true, propertyChanged: OnHasNavigationBarChanged);
        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }

        private static void OnHasNavigationBarChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            ShellViewPage shellViewPage = (ShellViewPage)bindable;
            NavigationPage.SetHasNavigationBar(shellViewPage, newValue);
        }

        public ShellViewPage()
        {
            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));
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

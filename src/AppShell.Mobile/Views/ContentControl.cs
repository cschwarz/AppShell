using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentContextProperty = BindableProperty.Create<ContentControl, object>(x => x.ContentContext, null, propertyChanged: OnContentContextChanged);

        private IViewFactory viewFactory;

        public ContentControl()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();
        }

        private static void OnContentContextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ContentControl contentControl = (ContentControl)bindable;

            if (newValue != null)
            {
                contentControl.Content = contentControl.viewFactory.GetView(newValue as IViewModel) as View;
                if (contentControl.Content != null)
                    contentControl.Content.BindingContext = newValue;
            }
        }

        public object ContentContext
        {
            get { return (object)GetValue(ContentContextProperty); }
            set { SetValue(ContentContextProperty, value); }
        }
    }
}

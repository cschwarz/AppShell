
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentContextProperty = BindableProperty.Create("ContentContext", typeof(object), typeof(ContentControl), null, propertyChanged: OnContentContextChanged);

        private IViewFactory viewFactory;

        public ContentControl()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();
        }

        private static void OnContentContextChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            ContentControl contentControl = (ContentControl)bindableObject;

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

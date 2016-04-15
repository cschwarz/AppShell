
using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [ContentProperty("ShellContent")]
    public partial class ShellView : ContentView
    {
        public static readonly BindableProperty ShellContentProperty = BindableProperty.Create("ShellContent", typeof(View), typeof(ShellView), null, propertyChanged: ShellContentPropertyChanged);
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create("HasNavigationBar", typeof(bool), typeof(ShellView), true, propertyChanged: OnHasNavigationBarChanged);

        public View ShellContent { get { return (View)GetValue(ShellContentProperty); } set { SetValue(ShellContentProperty, value); } }
        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }

        public static void ShellContentPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            ShellView shellView = (ShellView)bindableObject;
            View newView = (View)newValue;

            shellView.ShellContentView.Content = newView;
        }

        private static void OnHasNavigationBarChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            ShellView shellView = (ShellView)bindableObject;
            bool newHasNavigationBar = (bool)newValue;

            NavigationPage.SetHasNavigationBar(shellView, newHasNavigationBar);

            if (!newHasNavigationBar)
                shellView.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
        }

        public ShellView()
        {
            InitializeComponent();

            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));
        }
    }
}

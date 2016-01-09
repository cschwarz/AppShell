
using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [ContentProperty("ShellContent")]
    public partial class ShellView : ContentView
    {
        public static readonly BindableProperty ShellContentProperty = BindableProperty.Create<ShellView, View>(d => d.ShellContent, null, propertyChanged: ShellContentPropertyChanged);
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create<ShellView, bool>(x => x.HasNavigationBar, true, propertyChanged: OnHasNavigationBarChanged);

        public View ShellContent { get { return (View)GetValue(ShellContentProperty); } set { SetValue(ShellContentProperty, value); } }
        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }
        
        public static void ShellContentPropertyChanged(BindableObject d, View oldValue, View newValue)
        {
            ShellView shellView = d as ShellView;
            shellView.ShellContent = newValue;
        }

        private static void OnHasNavigationBarChanged(BindableObject d, bool oldValue, bool newValue)
        {
            ShellView shellView = d as ShellView;
            NavigationPage.SetHasNavigationBar(shellView, newValue);

            if (!newValue)
                shellView.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
        }

        public ShellView()
        {
            InitializeComponent();

            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));
        }
    }
}

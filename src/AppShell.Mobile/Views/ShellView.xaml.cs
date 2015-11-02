
using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [ContentProperty("ShellContent")]
    public partial class ShellView : ContentView
    {
        public static readonly BindableProperty ShellContentProperty = BindableProperty.Create<ShellView, View>(d => d.ShellContent, null, propertyChanged: ShellContentPropertyChanged);

        public View ShellContent { get { return (View)GetValue(ShellContentProperty); } set { SetValue(ShellContentProperty, value); } }

        public static void ShellContentPropertyChanged(BindableObject d, View oldValue, View newValue)
        {
            ShellView shellView = d as ShellView;
            shellView.ShellContentView.Content = newValue;
        }

        public ShellView()
        {
            InitializeComponent();
        }
    }
}

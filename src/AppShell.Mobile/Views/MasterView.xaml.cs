using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [View(typeof(MasterViewModel))]
    public partial class MasterView : ContentView
    {
        public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create<MasterView, View>(d => d.HeaderContent, null, propertyChanged: HeaderContentPropertyChanged);

        public View HeaderContent { get { return (View)GetValue(HeaderContentProperty); } set { SetValue(HeaderContentProperty, value); } }

        public static void HeaderContentPropertyChanged(BindableObject d, View oldValue, View newValue)
        {
            MasterView masterView = d as MasterView;
            masterView.Header.Content = newValue;
        }

        public MasterView()
        {
            InitializeComponent();
        }
    }
}

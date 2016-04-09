using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [View(typeof(MasterViewModel))]
    public partial class MasterView : ContentView
    {
        public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create("HeaderContent", typeof(View), typeof(MasterView), null, propertyChanged: HeaderContentPropertyChanged);

        public View HeaderContent { get { return (View)GetValue(HeaderContentProperty); } set { SetValue(HeaderContentProperty, value); } }

        public static void HeaderContentPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            MasterView masterView = d as MasterView;
            View newView = newValue as View;

            masterView.Header.Content = newView;
        }

        public MasterView()
        {
            InitializeComponent();
        }
    }
}

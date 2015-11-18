
using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [View(typeof(TabShellViewModel))]
    public partial class TabShellPage : TabbedPage
    {
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create<TabShellPage, bool>(x => x.HasNavigationBar, true, propertyChanged: OnHasNavigationBarChanged);
        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }

        private static void OnHasNavigationBarChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            TabShellPage tabShellPage = (TabShellPage)bindable;
            NavigationPage.SetHasNavigationBar(tabShellPage, newValue);
        }

        private IViewFactory viewFactory;

        public TabShellPage()
        {
            InitializeComponent();

            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));
            
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();
        }

        protected override Page CreateDefault(object item)
        {
            Page page = ShellViewPage.Create(viewFactory.GetView(item as IViewModel));

            if (!NavigationPage.GetHasNavigationBar(this))
                page.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            return page;
        }
    }
}

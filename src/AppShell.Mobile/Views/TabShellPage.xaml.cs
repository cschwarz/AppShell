
using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [View(typeof(TabShellViewModel))]
    public partial class TabShellPage : TabbedPage
    {
        public static readonly BindableProperty HasNavigationBarProperty = BindableProperty.Create("HasNavigationBar", typeof(bool), typeof(TabShellPage), true, propertyChanged: OnHasNavigationBarChanged);
        public bool HasNavigationBar { get { return (bool)GetValue(HasNavigationBarProperty); } set { SetValue(HasNavigationBarProperty, value); } }

        private static void OnHasNavigationBarChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            TabShellPage tabShellPage = (TabShellPage)bindableObject;
            bool newHasNavigationBar = (bool)newValue;

            NavigationPage.SetHasNavigationBar(tabShellPage, newHasNavigationBar);
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
            return ShellViewPage.Create(viewFactory.GetView(item as IViewModel));
        }
    }
}

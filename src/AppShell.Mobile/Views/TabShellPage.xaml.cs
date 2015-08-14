using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppShell.Mobile.Views
{
    [View(typeof(TabShellViewModel))]
    public partial class TabShellPage : TabbedPage
    {
        private IViewFactory viewFactory;

        public TabShellPage()
        {
            InitializeComponent();

            this.viewFactory = ShellCore.Container.GetInstance<IViewFactory>();
        }

        protected override Page CreateDefault(object item)
        {
            Page page = viewFactory.GetView(item.GetType()) as Page;

            if (page != null)
                page.SetBinding(TitleProperty, new Binding("Title"));

            return page;
        }
    }
}

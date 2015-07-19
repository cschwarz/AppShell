using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(StackShellViewModel))]
    public class StackShellPage : NavigationPage
    {
        private ShellViewModel shellViewModel;
        private IViewFactory viewFactory;

        public StackShellPage()
            : base(new ContentPage())
        {
            this.viewFactory = AppShellCore.Container.GetInstance<IViewFactory>();
        }

        protected override void OnBindingContextChanged()
        {            
            base.OnBindingContextChanged();

            shellViewModel = BindingContext as ShellViewModel;

            if (shellViewModel.ActiveItem != null)
                PushAsync(viewFactory.GetView(shellViewModel.ActiveItem) as Page);
        }
    }
}

using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(StackShellViewModel))]
    public class StackShellPage : NavigationPage
    {
        private ShellViewModel shellViewModel;
        private IViewFactory viewFactory;

        public StackShellPage()
        {
            this.viewFactory = ShellCore.Container.GetInstance<IViewFactory>();
        }

        protected override void OnBindingContextChanged()
        {            
            base.OnBindingContextChanged();

            shellViewModel = BindingContext as ShellViewModel;

            shellViewModel.ViewModelPushed += ShellViewModel_ViewModelPushed;
            shellViewModel.ViewModelPopped += ShellViewModel_ViewModelPopped;

            if (shellViewModel.ActiveItem != null)
                PushAsync(viewFactory.GetView(shellViewModel.ActiveItem) as Page);
        }

        private void ShellViewModel_ViewModelPushed(object sender, IViewModel e)
        {
            PushAsync(viewFactory.GetView(e) as Page);
        }

        private void ShellViewModel_ViewModelPopped(object sender, IViewModel e)
        {
            PopAsync();
        }
    }
}

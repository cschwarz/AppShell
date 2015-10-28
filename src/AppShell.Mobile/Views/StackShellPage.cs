using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(StackShellViewModel))]
    public class StackShellPage : NavigationPage
    {
        private StackShellViewModel shellViewModel;
        private IViewFactory viewFactory;
        private bool ignorePopEvent;

        public StackShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            Pushed += StackShellPage_Pushed;
            Popped += StackShellPage_Popped;          
        }

        private void StackShellPage_Pushed(object sender, NavigationEventArgs e)
        {
        }

        private void StackShellPage_Popped(object sender, NavigationEventArgs e)
        {
            if (shellViewModel != null)
            {
                ignorePopEvent = true;
                shellViewModel.Pop();
            }
        }
        
        protected override void OnBindingContextChanged()
        {            
            base.OnBindingContextChanged();

            shellViewModel = BindingContext as StackShellViewModel;

            shellViewModel.ViewModelPushed += ShellViewModel_ViewModelPushed;
            shellViewModel.ViewModelPopped += ShellViewModel_ViewModelPopped;

            if (shellViewModel.ActiveItem != null)
                PushAsync(ShellViewPage.Create(viewFactory.GetView(shellViewModel.ActiveItem)));
        }
        
        private void ShellViewModel_ViewModelPushed(object sender, IViewModel e)
        {
            PushAsync(ShellViewPage.Create(viewFactory.GetView(e)));
        }

        private void ShellViewModel_ViewModelPopped(object sender, IViewModel e)
        {
            if (!ignorePopEvent)
                PopAsync();
            else
                ignorePopEvent = false;
        }
    }
}

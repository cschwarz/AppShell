using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(StackShellViewModel))]
    public class StackShellPage : NavigationPage
    {
        private ShellViewModel shellViewModel;
        private IViewFactory viewFactory;
        private bool ignorePopEvent;

        public StackShellPage()
        {
            viewFactory = ShellCore.Container.GetInstance<IViewFactory>();

            Popped += StackShellPage_Popped;
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
            if (!ignorePopEvent)
                PopAsync();
            else
                ignorePopEvent = false;
        }
    }
}

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

            SetBinding(HasNavigationBarProperty, new Binding("HasNavigationBar"));            
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

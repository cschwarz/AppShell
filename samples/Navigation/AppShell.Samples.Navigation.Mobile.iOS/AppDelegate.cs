
using AppShell.Mobile;
using AppShell.Mobile.iOS;
using AppShell.Samples.Navigation.Mobile.Views;
using Foundation;
using UIKit;

namespace AppShell.Samples.Navigation.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : ShellApplicationDelegate<ShellApplication<NavigationShellCore>>
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var views = typeof(View1);
            
            return base.FinishedLaunching(app, options);
        }
    }
}
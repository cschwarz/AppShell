
using AppShell.Mobile;
using AppShell.Mobile.iOS;
using Foundation;
using UIKit;

namespace AppShell.Samples.NativeMaps.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : ShellApplicationDelegate<ShellApplication<NativeMapsShellCore>>
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
            var mapViewModels = typeof(AppShell.NativeMaps.MapViewModel);
            var mapViews = typeof(AppShell.NativeMaps.Mobile.MapView);

            Xamarin.FormsMaps.Init();
            
            return base.FinishedLaunching(app, options);
        }
    }
}
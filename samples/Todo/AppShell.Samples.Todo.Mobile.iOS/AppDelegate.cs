﻿
using AppShell.Data;
using AppShell.Data.Mobile.iOS;
using AppShell.Mobile;
using AppShell.Mobile.iOS;
using AppShell.Samples.Todo.Mobile.Views;
using Foundation;
using UIKit;

namespace AppShell.Samples.Todo.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : ShellApplicationDelegate<ShellApplication<TodoShellCore>>
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
            var views = typeof(TodoPage);

            return base.FinishedLaunching(app, options);
        }

        protected override void ConfigurePlatform()
        {
            base.ConfigurePlatform();

            ShellCore.Container.RegisterSingleton<ISQLiteDatabase, iOSSQLiteDatabase>();
        }
    }
}
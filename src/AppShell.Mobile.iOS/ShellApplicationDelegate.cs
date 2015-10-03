﻿using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppShell.Mobile.iOS
{
    public class ShellApplicationDelegate<T> : FormsApplicationDelegate where T : Application
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            ShellCore.Container.RegisterSingleton<IPlatformProvider, iOSPlatformProvider>();

            LoadApplication(Activator.CreateInstance<T>());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public abstract class MobilePlatformProvider : IPlatformProvider
    {
        public abstract IEnumerable<Assembly> GetAssemblies();
        public abstract Platform GetPlatform();
        public abstract string GetDocumentFolderPath();

        public void ExecuteOnUIThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public void ShowMessage(string title, string message, string cancel)
        {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public void OpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}

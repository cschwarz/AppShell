using AppShell.Mobile;
using AppShell.Mobile.UWP;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ShellWebView), typeof(ShellWebViewRenderer))]

namespace AppShell.Mobile.UWP
{
    public class ShellWebViewRenderer : ViewRenderer<ShellWebView, Windows.UI.Xaml.Controls.WebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ShellWebView> e)
        {
            base.OnElementChanged(e);

            Windows.UI.Xaml.Controls.WebView webView = new Windows.UI.Xaml.Controls.WebView();
            webView.NavigationCompleted += NavigationCompleted;
            webView.ScriptNotify += ScriptNotify;

            SetNativeControl(webView);

            Element.InjectJavaScriptRequested += InjectJavaScriptRequested;

            UpdateSource();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ShellWebView.SourceProperty.PropertyName)
                UpdateSource();
        }

        private void UpdateSource()
        {
            if (Element.Source != null)
            {
                if (Element.Source is HtmlWebViewSource)
                {
                    HtmlWebViewSource htmlSource = Element.Source as HtmlWebViewSource;
                    Control.NavigateToString(htmlSource.Html);
                }
                else if (Element.Source is UrlWebViewSource)
                {
                    UrlWebViewSource urlSource = Element.Source as UrlWebViewSource;
                    Control.Navigate(new Uri(urlSource.Url));
                }
            }
        }

        private void InjectJavaScriptRequested(object sender, string script)
        {
            Control.InvokeScriptAsync("eval", new[] { script });
        }

        private void NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
            Control.InvokeScriptAsync("eval", new[] { "function Native(action, data){window.external.notify(JSON.stringify({ a: action, d: data }));}" });
            Element.OnLoadFinished(sender, EventArgs.Empty);
        }

        private void ScriptNotify(object sender, Windows.UI.Xaml.Controls.NotifyEventArgs e)
        {
            Element.MessageReceived(e.Value);
        }
    }
}

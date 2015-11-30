using Android.Webkit;
using AppShell.Mobile;
using AppShell.Mobile.Android;
using Java.Interop;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShellWebView), typeof(ShellWebViewRenderer))]

namespace AppShell.Mobile.Android
{
    public class ShellWebViewRenderer : ViewRenderer<ShellWebView, global::Android.Webkit.WebView>
    {
        class ShellWebViewClient : WebViewClient
        {
            ShellWebView webView;

            public ShellWebViewClient(ShellWebView webView)
            {
                this.webView = webView;
            }

            public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
            {
                base.OnPageFinished(view, url);

                view.LoadUrl("javascript: function Native(action, data){ ScriptInterface.call(JSON.stringify({ a: action, d: data }));}");
                webView.OnLoadFinished(this, EventArgs.Empty);
            }
        }

        class ScriptInterface : Java.Lang.Object
        {
            ShellWebView webView;

            public ScriptInterface(ShellWebView webView)
            {
                this.webView = webView;
            }
                        
            [Export("call")]
            [JavascriptInterface]
            public void Call(string message)
            {
                webView.MessageReceived(message);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ShellWebView> e)
        {
            base.OnElementChanged(e);

            global::Android.Webkit.WebView webView = new global::Android.Webkit.WebView(Context);
            SetNativeControl(webView);

            Control.Settings.JavaScriptEnabled = true;

            Element.InjectJavaScriptRequested += InjectJavaScriptRequested;
            Control.SetWebViewClient(new ShellWebViewClient(Element));
            Control.AddJavascriptInterface(new ScriptInterface(Element), "ScriptInterface");

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
                    Control.LoadData(htmlSource.Html, "text/html", "UTF-8");
                }
                else if(Element.Source is UrlWebViewSource)
                {
                    UrlWebViewSource urlSource = Element.Source as UrlWebViewSource;
                    Control.LoadUrl(urlSource.Url);
                }
            }
        }

        private void InjectJavaScriptRequested(object sender, string script)
        {
            Control.LoadUrl(string.Format("javascript: {0}", script));
        }
    }
}
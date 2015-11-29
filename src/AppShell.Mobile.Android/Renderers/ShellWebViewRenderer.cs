using Android.Webkit;
using AppShell.Mobile;
using AppShell.Mobile.Android;
using Java.Interop;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShellWebView), typeof(ShellWebViewRenderer))]

namespace AppShell.Mobile.Android
{
    public class ShellWebViewRenderer : WebViewRenderer
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

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            (Element as ShellWebView).InjectJavaScriptRequested += InjectJavaScriptRequested;

            Control.SetWebViewClient(new ShellWebViewClient(Element as ShellWebView));
            Control.AddJavascriptInterface(new ScriptInterface(Element as ShellWebView), "ScriptInterface");
        }

        private void InjectJavaScriptRequested(object sender, string script)
        {
            Control.LoadUrl(string.Format("javascript: {0}", script));
        }
    }
}
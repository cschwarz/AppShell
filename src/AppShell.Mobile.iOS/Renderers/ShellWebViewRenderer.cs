using AppShell.Mobile;
using AppShell.Mobile.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ShellWebView), typeof(ShellWebViewRenderer))]

namespace AppShell.Mobile.iOS
{
    public class ShellWebViewRenderer : ViewRenderer<ShellWebView, WKWebView>, IWKScriptMessageHandler
    {
        private const string ScriptMessageHandlerName = "native";

        private WKUserContentController userController;

        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (Element != null)
                (Element as ShellWebView).OnLoadFinished(webView, EventArgs.Empty);
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.MessageReceived(message.Body.ToString());
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ShellWebView> e)
        {
            base.OnElementChanged(e);

            userController = new WKUserContentController();
            userController.AddUserScript(new WKUserScript(new NSString("function Native(action, data){window.webkit.messageHandlers.native.postMessage(JSON.stringify({ a: action, d: data }));}"), WKUserScriptInjectionTime.AtDocumentEnd, false));
            userController.AddScriptMessageHandler(this, ScriptMessageHandlerName);

            WKWebView webView = new WKWebView(Frame, new WKWebViewConfiguration { UserContentController = userController });
            webView.WeakNavigationDelegate = this;

            SetNativeControl(webView);

            Element.InjectJavaScriptRequested += InjectJavaScriptRequested;

            if (Element.Source != null)
            {
                if (Element.Source is HtmlWebViewSource)
                {
                    HtmlWebViewSource htmlSource = Element.Source as HtmlWebViewSource;
                    webView.LoadHtmlString(htmlSource.Html, new NSUrl(NSBundle.MainBundle.BundlePath, true));
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void InjectJavaScriptRequested(object sender, string script)
        {
            Control.EvaluateJavaScript(script, (r, e) =>
            {
            });
        }
    }
}

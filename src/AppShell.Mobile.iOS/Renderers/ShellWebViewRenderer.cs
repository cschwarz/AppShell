using AppShell.Mobile;
using AppShell.Mobile.iOS;
using Foundation;
using System;
using System.ComponentModel;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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
                    Control.LoadHtmlString(htmlSource.Html, new NSUrl(NSBundle.MainBundle.BundlePath, true));
                }
                else if (Element.Source is UrlWebViewSource)
                {
                    UrlWebViewSource urlSource = Element.Source as UrlWebViewSource;
                    Control.LoadRequest(new NSUrlRequest(new NSUrl(urlSource.Url)));
                }
            }
        }

        private void InjectJavaScriptRequested(object sender, string script)
        {
            Control.EvaluateJavaScript(script, (r, e) => { });
        }
    }
}

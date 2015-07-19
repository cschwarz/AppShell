using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    [View(typeof(WebBrowserViewModel))]
    public class WebBrowserPage : ContentPage
    {
        public static readonly BindableProperty UrlProperty = BindableProperty.Create<WebBrowserPage, string>(d => d.Url, null, propertyChanged: UrlPropertyChanged);
        public static readonly BindableProperty HtmlProperty = BindableProperty.Create<WebBrowserPage, string>(d => d.Html, null, propertyChanged: HtmlPropertyChanged);
        
        public string Url { get { return (string)GetValue(UrlProperty); } set { SetValue(UrlProperty, value); } }
        public string Html { get { return (string)GetValue(HtmlProperty); } set { SetValue(HtmlProperty, value); } }

        public static void UrlPropertyChanged(BindableObject d, string oldValue, string newValue)
        {
            WebBrowserPage webBrowserPage = d as WebBrowserPage;

            if (newValue != null)
                webBrowserPage.webView.Source = new UrlWebViewSource() { Url = newValue };
        }

        public static void HtmlPropertyChanged(BindableObject d, string oldValue, string newValue)
        {
            WebBrowserPage webBrowserPage = d as WebBrowserPage;

            if (newValue != null)
                webBrowserPage.webView.Source = new HtmlWebViewSource() { Html = newValue };
        }

        private WebView webView;

        public WebBrowserPage()
        {
            webView = new WebView();

            Content = webView;

            SetBinding(UrlProperty, new Binding("Url"));
            SetBinding(HtmlProperty, new Binding("Html"));
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Serialization.JsonNET;

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

        private HybridWebView webView;

        public WebBrowserPage()
        {
            webView = new HybridWebView(new XLabs.Serialization.JsonNET.JsonSerializer());
            
            string serviceDispatcherScript = null;

            using (StreamReader streamReader = new StreamReader(typeof(WebBrowserViewModel).GetTypeInfo().Assembly.GetManifestResourceStream("AppShell.ServiceDispatcher.js")))
                serviceDispatcherScript = streamReader.ReadToEnd();

            string services = JsonConvert.SerializeObject(AppShellCore.Container.GetInstance<IServiceDispatcher>().Services);

            webView.LoadFinished += (s, e) =>
            {
                webView.InjectJavaScript(serviceDispatcherScript);
                webView.InjectJavaScript(string.Format("serviceDispatcher.initialize({0});", services));
            };
                        
            webView.RegisterCallback("dispatch", args =>
            {

            });

            Content = webView;       

            SetBinding(UrlProperty, new Binding("Url"));
            SetBinding(HtmlProperty, new Binding("Html"));
        }
    }
}

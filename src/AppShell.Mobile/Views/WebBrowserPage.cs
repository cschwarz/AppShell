using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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
        class DispatchData
        {
            public string ServiceName { get; set; }
            public string InstanceName { get; set; }
            public string MethodName { get; set; }
            public string CallbackId { get; set; }
            public object[] Arguments { get; set; }
        }

        class SubscribeEventData
        {
            public string ServiceName { get; set; }
            public string EventName { get; set; }
            public string CallbackId { get; set; }
        }

        class UnsubscribeEventData
        {
            public string ServiceName { get; set; }            
            public string CallbackId { get; set; }
        }

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
        private ConcurrentDictionary<string, EventRegistration> eventRegistrations;

        public WebBrowserPage(IServiceDispatcher serviceDispatcher, IPlatformProvider platformProvider)
        {
            webView = new HybridWebView(new XLabs.Serialization.JsonNET.JsonSerializer());
            eventRegistrations = new ConcurrentDictionary<string, EventRegistration>();
            
            string serviceDispatcherScript = null;

            using (StreamReader streamReader = new StreamReader(typeof(WebBrowserViewModel).GetTypeInfo().Assembly.GetManifestResourceStream("AppShell.ServiceDispatcher.js")))
                serviceDispatcherScript = streamReader.ReadToEnd();

            string services = JsonConvert.SerializeObject(serviceDispatcher.Services);

            webView.LoadFinished += (s, e) =>
            {
                webView.InjectJavaScript(serviceDispatcherScript);
                webView.InjectJavaScript(string.Format("serviceDispatcher.initialize({0});", services));
            };
                        
            webView.RegisterCallback("dispatch", args =>
            {
                DispatchData dispatchData = JsonConvert.DeserializeObject<DispatchData>(args);
                
                string result = string.IsNullOrEmpty(dispatchData.InstanceName) 
                    ? JsonConvert.SerializeObject(serviceDispatcher.Dispatch(dispatchData.ServiceName, dispatchData.MethodName, dispatchData.Arguments))
                    : JsonConvert.SerializeObject(serviceDispatcher.Dispatch(dispatchData.ServiceName, dispatchData.InstanceName, dispatchData.MethodName, dispatchData.Arguments));

                if (dispatchData.CallbackId != null)
                    platformProvider.ExecuteOnUIThread(() => webView.InjectJavaScript(string.Format("serviceDispatcher._dispatchCallback('{0}', {1});", dispatchData.CallbackId, result)));
            });

            webView.RegisterCallback("subscribeEvent", args =>
            {
                SubscribeEventData subscribeEventData = JsonConvert.DeserializeObject<SubscribeEventData>(args);

                EventRegistration eventRegistration = serviceDispatcher.SubscribeEvent(subscribeEventData.ServiceName, subscribeEventData.EventName, this, (s, e) =>
                {
                    if (subscribeEventData.CallbackId != null)
                        platformProvider.ExecuteOnUIThread(() => webView.InjectJavaScript(string.Format("serviceDispatcher._eventCallback('{0}', '{1}');", subscribeEventData.CallbackId, JsonConvert.SerializeObject(e))));
                });

                eventRegistrations.AddOrUpdate(subscribeEventData.CallbackId, eventRegistration, (k, e) => e);
            });

            webView.RegisterCallback("unsubscribeEvent", args =>
            {
                UnsubscribeEventData unsubscribeEventData = JsonConvert.DeserializeObject<UnsubscribeEventData>(args);

                EventRegistration eventRegistration = null;
                if (eventRegistrations.TryRemove(unsubscribeEventData.CallbackId, out eventRegistration))
                    serviceDispatcher.UnsubscribeEvent(unsubscribeEventData.ServiceName, eventRegistration);
            });

            Content = webView;       

            SetBinding(UrlProperty, new Binding("Url"));
            SetBinding(HtmlProperty, new Binding("Html"));
        }
    }
}

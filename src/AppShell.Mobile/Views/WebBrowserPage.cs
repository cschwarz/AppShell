using Newtonsoft.Json;
using System.Collections.Concurrent;
using Xamarin.Forms;

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

        public static readonly BindableProperty UrlProperty = BindableProperty.Create("Url", typeof(string), typeof(WebBrowserPage), null, propertyChanged: UrlPropertyChanged);
        public static readonly BindableProperty HtmlProperty = BindableProperty.Create("Html", typeof(string), typeof(WebBrowserPage), null, propertyChanged: HtmlPropertyChanged);

        public string Url { get { return (string)GetValue(UrlProperty); } set { SetValue(UrlProperty, value); } }
        public string Html { get { return (string)GetValue(HtmlProperty); } set { SetValue(HtmlProperty, value); } }

        public static void UrlPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            WebBrowserPage webBrowserPage = d as WebBrowserPage;
            string newUrl = (string)newValue;

            if (newUrl != null)
                webBrowserPage.webView.Source = new UrlWebViewSource() { Url = newUrl };
        }

        public static void HtmlPropertyChanged(BindableObject d, object oldValue, object newValue)
        {
            WebBrowserPage webBrowserPage = d as WebBrowserPage;
            string newHtml = (string)newValue;

            if (newHtml != null)
                webBrowserPage.webView.Source = new HtmlWebViewSource() { Html = newHtml };
        }

        private ShellWebView webView;
        private ConcurrentDictionary<string, EventRegistration> eventRegistrations;

        public WebBrowserPage(IServiceDispatcher serviceDispatcher, IPlatformProvider platformProvider)
        {
            webView = new ShellWebView();
            eventRegistrations = new ConcurrentDictionary<string, EventRegistration>();

            webView.RegisterCallback("initialize", args =>
            {
                platformProvider.ExecuteOnUIThread(() => webView.InjectJavaScript(string.Format("serviceDispatcher._initializeCallback({0});", JsonConvert.SerializeObject(serviceDispatcher.Services))));
            });

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

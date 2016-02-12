using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppShell.Desktop.Views
{
    /// <summary>
    /// Interaction logic for WebBrowserView.xaml
    /// </summary>
    [View(typeof(WebBrowserViewModel))]
    public partial class WebBrowserView : UserControl
    {
        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(WebBrowserView), new PropertyMetadata(null, UrlPropertyChanged));
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register("Html", typeof(string), typeof(WebBrowserView), new PropertyMetadata(null, HtmlPropertyChanged));

        public string Url { get { return (string)GetValue(UrlProperty); } set { SetValue(UrlProperty, value); } }
        public string Html { get { return (string)GetValue(HtmlProperty); } set { SetValue(HtmlProperty, value); } }

        public static void UrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebBrowserView webBrowserView = d as WebBrowserView;

            if (e.NewValue != null)
            {
                string url = (string)e.NewValue;
                webBrowserView.WebBrowser.Navigate(url);
            }
        }

        public static void HtmlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebBrowserView webBrowserView = d as WebBrowserView;

            if (e.NewValue != null)
            {
                string html = (string)e.NewValue;
                webBrowserView.WebBrowser.NavigateToString(html);
            }
        }

        public WebBrowserView()
        {
            InitializeComponent();

            SetBinding(UrlProperty, new Binding("Url"));
            SetBinding(HtmlProperty, new Binding("Html"));

            WebBrowser.ObjectForScripting = new ScriptInterface(ShellCore.Container.GetInstance<IServiceDispatcher>(), WebBrowser);
        }
    }

    [ComVisible(true)]
    public class ScriptInterface
    {
        class Message
        {
            [JsonProperty("a")]
            public string Action { get; set; }
            [JsonProperty("d")]
            public string Data { get; set; }
        }

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

        private IServiceDispatcher serviceDispatcher;
        private ConcurrentDictionary<string, EventRegistration> eventRegistrations;

        private WebBrowser webBrowser;

        public ScriptInterface(IServiceDispatcher serviceDispatcher, WebBrowser webBrowser)
        {
            this.serviceDispatcher = serviceDispatcher;
            this.eventRegistrations = new ConcurrentDictionary<string, EventRegistration>();
            this.webBrowser = webBrowser;
        }

        public void Native(string message)
        {
            Message m = JsonConvert.DeserializeObject<Message>(message);

            switch (m.Action)
            {
                case "initialize": Initialize(); break;
                case "dispatch": Dispatch(JsonConvert.DeserializeObject<DispatchData>(m.Data)); break;
                case "subscribeEvent": SubscribeEvent(JsonConvert.DeserializeObject<SubscribeEventData>(m.Data)); break;
                case "unsubscribeEvent": UnsubscribeEvent(JsonConvert.DeserializeObject<UnsubscribeEventData>(m.Data)); break;
            }
        }

        private void Initialize()
        {
            webBrowser.InvokeScript("eval", string.Format("serviceDispatcher._initializeCallback({0});", JsonConvert.SerializeObject(ShellCore.Container.GetInstance<IServiceDispatcher>().Services)));
        }

        private void Dispatch(DispatchData data)
        {
            for (int i = 0; i < data.Arguments.Length; i++)
            {
                if (data.Arguments[i] is JArray)
                    data.Arguments[i] = (data.Arguments[i] as JArray).ToObject<object[]>();
                else if (data.Arguments[i] is JObject)
                    data.Arguments[i] = (data.Arguments[i] as JObject).ToObject<Dictionary<string, object>>();
            }

            string result = string.IsNullOrEmpty(data.InstanceName)
                ? JsonConvert.SerializeObject(serviceDispatcher.Dispatch(data.ServiceName, data.MethodName, data.Arguments))
                : JsonConvert.SerializeObject(serviceDispatcher.Dispatch(data.ServiceName, data.InstanceName, data.MethodName, data.Arguments));

            if (data.CallbackId != null)
                webBrowser.InvokeScript("eval", string.Format("serviceDispatcher._dispatchCallback('{0}', {1})", data.CallbackId, result));
        }

        private void SubscribeEvent(SubscribeEventData data)
        {
            EventRegistration eventRegistration = serviceDispatcher.SubscribeEvent(data.ServiceName, data.EventName, this, (s, e) =>
            {
                if (data.CallbackId != null)
                    Application.Current.Dispatcher.Invoke(() => webBrowser.InvokeScript("eval", string.Format("serviceDispatcher._eventCallback('{0}', '{1}')", data.CallbackId, JsonConvert.SerializeObject(e))));
            });

            eventRegistrations.AddOrUpdate(data.CallbackId, eventRegistration, (k, e) => e);
        }

        private void UnsubscribeEvent(UnsubscribeEventData data)
        {
            EventRegistration eventRegistration = null;
            if (eventRegistrations.TryRemove(data.CallbackId, out eventRegistration))
                serviceDispatcher.UnsubscribeEvent(data.ServiceName, eventRegistration);
        }
    }
}

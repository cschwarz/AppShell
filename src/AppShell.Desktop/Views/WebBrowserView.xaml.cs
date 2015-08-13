using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            WebBrowserView webBrowserView  = d as WebBrowserView;

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

            WebBrowser.ObjectForScripting = new ScriptInterface(AppShellCore.Container.GetInstance<IServiceDispatcher>(), WebBrowser);
            
            string serviceDispatcherScript = null;

            using (StreamReader streamReader = new StreamReader(typeof(WebBrowserViewModel).Assembly.GetManifestResourceStream("AppShell.ServiceDispatcher.js")))
                serviceDispatcherScript = streamReader.ReadToEnd();

            string services = JsonConvert.SerializeObject(AppShellCore.Container.GetInstance<IServiceDispatcher>().Services);

            WebBrowser.LoadCompleted += (s, e) =>
            {
                dynamic document = WebBrowser.Document;
                dynamic head = document.GetElementsByTagName("head")[0];
                dynamic scriptElement = document.CreateElement("script");
                scriptElement.text = string.Concat(serviceDispatcherScript, Environment.NewLine, string.Format("serviceDispatcher.initialize({0});", services));
                head.AppendChild(scriptElement);
            };
        }
    }

    [ComVisible(true)]
    public class ScriptInterface
    {
        private IServiceDispatcher serviceDispatcher;

        private WebBrowser webBrowser;

        public ScriptInterface(IServiceDispatcher serviceDispatcher, WebBrowser webBrowser)
        {
            this.serviceDispatcher = serviceDispatcher;
            this.webBrowser = webBrowser;
        }

        public string Dispatch(string serviceName, string methodName, string callbackId, string arguments)
        {
            object[] parameters = JsonConvert.DeserializeObject<object[]>(arguments);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is JArray)
                    parameters[i] = (parameters[i] as JArray).ToObject<object[]>();
                else if (parameters[i] is JObject)
                    parameters[i] = (parameters[i] as JObject).ToObject<Dictionary<string, object>>();
            }

            string result = JsonConvert.SerializeObject(serviceDispatcher.Dispatch(serviceName, methodName, parameters));

            if (callbackId != null)
                webBrowser.InvokeScript("eval", string.Format("serviceDispatcher._dispatchCallback('{0}', {1})", callbackId, result));

            return null;
        }
                
        public void SubscribeEvent(string serviceName, string eventName, string callbackId)
        {
            serviceDispatcher.SubscribeEvent(serviceName, eventName, this, (s, e) =>
            {
                if (callbackId != null)
                    Application.Current.Dispatcher.Invoke(() => webBrowser.InvokeScript("eval", string.Format("serviceDispatcher._eventCallback('{0}', '{1}')", callbackId, JsonConvert.SerializeObject(e))));
            });
        }

        public void UnsubscribeEvent()
        {
        }
    }
}

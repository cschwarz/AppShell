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

            WebBrowser.ObjectForScripting = AppShellCore.Container.GetInstance<ScriptInterface>();
            
            string serviceDispatcherScript = null;

            using (StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AppShell.Desktop.ServiceDispatcher.js")))
                serviceDispatcherScript = streamReader.ReadToEnd();

            string services = JsonConvert.SerializeObject(AppShellCore.Container.GetInstance<IServiceDispatcher>().Services);

            WebBrowser.LoadCompleted += (s, e) =>
            {
                WebBrowser.InvokeScript("execScript", new object[] { serviceDispatcherScript, "JavaScript" });
                WebBrowser.InvokeScript("execScript", new object[] { string.Format("serviceDispatcher.initialize({0});", services), "JavaScript" });
            };            
        }
    }

    [ComVisible(true)]
    public class ScriptInterface
    {
        private IServiceDispatcher serviceDispatcher;

        public ScriptInterface(IServiceDispatcher serviceDispatcher)
        {
            this.serviceDispatcher = serviceDispatcher;
        }

        public void Dispatch(string serviceName, string methodName, string arguments)
        {
            object[] parameters = JsonConvert.DeserializeObject<Dictionary<int, object>>(arguments).Select(p => p.Value).ToArray();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is JObject)
                    parameters[i] = (parameters[i] as JObject).ToObject<Dictionary<string, object>>();
            }

            serviceDispatcher.Dispatch(serviceName, methodName, parameters);
        }
    }
}
